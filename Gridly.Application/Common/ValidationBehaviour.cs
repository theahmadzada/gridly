using ErrorOr;
using FluentValidation;
using MediatR;

namespace Loot.Application.Common;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr 
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehaviour(IValidator<TRequest> validator)
    {
        _validator = validator;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validator is null) return await next(cancellationToken);
        
        var result = await _validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid) return await next(cancellationToken);
        
        var errors = result.Errors.ConvertAll(validationFailure => Error.Validation(
            validationFailure.ErrorCode, 
            validationFailure.ErrorMessage));

        return (dynamic)errors;
    }
}