using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Data;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using Rocosa.Models.ViewModels;

namespace Rocosa.Controllers
{
    [Authorize(Roles =WebConstants.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository,IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _productRepository= productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll(includeProperties: "Category,ApplicationType");
            return View(products);
        }

        // Get
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoriesDropDown = _dbContext.Category.Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString(),
            //});

            //ViewBag.Categories = categoriesDropDown;

            //Product product = new Product();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                Categories = _productRepository.GetAll(WebConstants.CategoryName),
                ApplicationTypeList = _productRepository.GetAll(WebConstants.ApplicationTypeName)
            };

            if(id==null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _productRepository.GetById(id.GetValueOrDefault());

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if(productViewModel.Product.Id==0)
                {
                    string upload = webRootPath + WebConstants.ImgRoute;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = fileName + extension;
                    _productRepository.Add(productViewModel.Product);

                }
                else
                {
                    var objProduct = _productRepository.GetFirst(p => p.Id == productViewModel.Product.Id,isTracking: false);

                    if(files.Count >0)
                    {
                        string upload = webRootPath + WebConstants.ImgRoute;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var previousFile = Path.Combine(upload, objProduct.ImageUrl);

                        if (System.IO.File.Exists(previousFile))
                        {
                            System.IO.File.Delete(previousFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.ImageUrl= fileName + extension;
                    }

                    else
                    {
                        productViewModel.Product.ImageUrl = objProduct.ImageUrl;
                    }
                    _productRepository.Update(productViewModel.Product);
                }
                _productRepository.Record();
                return RedirectToAction("Index");
            }

            productViewModel.Categories = _productRepository.GetAll(WebConstants.CategoryName);
            productViewModel.ApplicationTypeList = _productRepository.GetAll(WebConstants.ApplicationTypeName);

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Product product = _productRepository.GetFirst(p => p.Id == id, includeProperties: "Category,ApplicationType");

            if(product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product product)
        {
            if (product == null)
                return NotFound();

            string upload = _webHostEnvironment.WebRootPath + WebConstants.ImgRoute;
            var previousFile = Path.Combine(upload, product.ImageUrl);

            if(System.IO.File.Exists(previousFile))
            {
                System.IO.File.Delete(previousFile);
            }

            _productRepository.Remove(product);
            _productRepository.Record();

            return RedirectToAction("Index");
        }
    }
}
