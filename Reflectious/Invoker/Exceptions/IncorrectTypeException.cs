using System;

namespace Firestorm
{
    public class IncorrectTypeException : Exception
    {
    }
    
    public class IncorrectPropertyTypeException : IncorrectTypeException
    {
    }
    
    public class IncorrectInstanceTypeException : IncorrectTypeException
    {
    }
}