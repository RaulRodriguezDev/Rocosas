using System.Reflection.Metadata;

namespace Rocosa
{
    public class WebConstants
    {
        public static string ImgRoute = @"\img\products\";
        public static string CartShopSession = "CartShopSession";
        public static string SessionOrderId = "SessionOrder";

        public const string AdminRole = "Admin";
        public static string ClientRole = "Client";

        public const string CategoryName = "Category";
        public const string ApplicationTypeName = "ApplicationType";

        public const string Successful = "Successful";
        public const string Error = "Error";

        public const string PendingStatus = "Pending";
        public const string ApprovedStatus = "Approved";
        public const string InProcessStatus = "In Process";
        public const string SentStatus = "Sent";
        public const string CanceledStatus = "Canceled";
        public const string ReturnedStatus = "Returned";
    }
}
