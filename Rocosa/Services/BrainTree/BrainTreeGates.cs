using Braintree;
using Microsoft.Extensions.Options;

namespace Rocosa.Services.BrainTree
{
    public class BrainTreeGates: IBrainTreeGates
    {
        public BrainTreeSettings _options { get; set; }
        private IBraintreeGateway brainTreeGateWay { get; set; }

        public BrainTreeGates(IOptions<BrainTreeSettings> options)
        {
            _options = options.Value;
        }

        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(_options.Environment, _options.MerchantId, _options.PublicKey, _options.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if(brainTreeGateWay == null)
            {
                brainTreeGateWay = CreateGateWay();
            }
            return brainTreeGateWay;
        }
    }
}
