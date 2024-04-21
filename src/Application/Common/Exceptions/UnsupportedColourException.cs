using System.Runtime.Serialization;

namespace MyCoffeeShop.Application.Common.Exceptions;

public class UnsupportedColourException : Exception
{
    public UnsupportedColourException()
    {
    }

    public UnsupportedColourException(string code)
        : base($"Colour '{code}' is unsupported.")
    {
    }

    public UnsupportedColourException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}
