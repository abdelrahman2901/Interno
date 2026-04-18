using FluentValidation;
using MediatR;
using System.Reflection;

namespace Microservice_Audit.Application.Validation.ValidationPipeLine
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _Validator;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            _Validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_Validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResult = await Task.WhenAll(_Validator.Select(v => v.ValidateAsync(context, cancellationToken)));
                if (validationResult != null)
                {
                    var ErrorMessages = validationResult.Where(r => r != null).SelectMany(r => r.Errors).Select(e => e.ErrorMessage).ToList();

                    if (ErrorMessages.Any())
                    {
                        var returnResponse = typeof(TResponse).GetMethod("BadRequest", BindingFlags.Public | BindingFlags.Static);
                        return (TResponse)returnResponse.Invoke(null, new object[] { string.Join(Environment.NewLine, ErrorMessages) });
                    }
                }
            }

            return await next(cancellationToken);
        }
    }
}
