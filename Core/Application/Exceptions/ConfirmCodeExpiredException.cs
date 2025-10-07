namespace Yu.Application.Exceptions;

public class ConfirmCodeExpiredException : AutheticationException
{
    public ConfirmCodeExpiredException() : base("Confirm code has expired. Please request a new one.") { }

    public ConfirmCodeExpiredException(string? message) : base(message)
    {
    }

    public ConfirmCodeExpiredException(string? message, Exception? innerException) : base(message, innerException) { }
}
