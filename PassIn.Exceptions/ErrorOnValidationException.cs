namespace PassIn.Exceptions;
public class ErrorOnValidationException : PassInException
{
    public ErrorOnValidationException(string messsage) : base(messsage)
    {
    }
}
