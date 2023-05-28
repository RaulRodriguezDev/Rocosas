using Braintree;

namespace Rocosa.Services.BrainTree
{
    public interface IBrainTreeGates
    {
        IBraintreeGateway CreateGateWay();
        IBraintreeGateway GetGateWay();
    }
}
