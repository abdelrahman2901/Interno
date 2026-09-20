using Polly;

namespace E_Commerce_Inern_Project.Core.Policies
{
    public interface IPollyPolicy
    {
        //public IAsyncPolicy GetRetryPolicy();
        //public IAsyncPolicy GetCircuiteBreakerPolicy();
        public IAsyncPolicy GetPolicy();
    }
}
