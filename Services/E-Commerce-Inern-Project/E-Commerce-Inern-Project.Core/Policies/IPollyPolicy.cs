using Polly;

namespace E_Commerce_Inern_Project.Core.Policies
{
    public interface IPollyPolicy
    {
        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
        public IAsyncPolicy<HttpResponseMessage> GetCircuiteBreakerPolicy();
    }
}
