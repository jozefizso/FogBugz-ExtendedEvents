using System;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    // Add extension methods support to .NET 2.0 runtime
    [AttributeUsageAttribute(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class ExtensionAttribute : Attribute
    {
    }
}
