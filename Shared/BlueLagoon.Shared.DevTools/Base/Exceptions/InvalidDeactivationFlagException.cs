namespace BlueLagoon.Shared.DevTools.Base.Exceptions;

public sealed class InvalidDeactivationFlagException(string EntityName) : Exception
{
    public override string Message => $"Rekord typu {EntityName} nie może być dezaktywowany ponieważ jest obecnie nieaktywny.";
}
