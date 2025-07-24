namespace Space.Application.Behaviours;


public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IHttpContextAccessor httpContextAccessor)
    {
        _validators = validators;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        List<ValidationFailure> failures = new();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(validationResult.Errors);
        }

        if (failures.Count != 0)
            throw new ValidationException("Validation error(s) occurred. See the 'errors' property for details.", failures);

        return await next(cancellationToken);
    }
}
