using Polly;

namespace E_Commerce_Inerno.Core.Policies
{
    public interface IPollyPolicy
    {
        IAsyncPolicy GetPolicy();
    }
}
