namespace BlueLagoon.Shared.DevTools.Base.Exceptions;

public sealed class InvalidActivationFlagException(string EntityName) : Exception
{
    public override string Message => $"Rekord typu {EntityName} nie może być aktywawany ponieważ jest obecnie aktywny.";
}