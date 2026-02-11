namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    internal sealed class InterpolatedStringHandlerAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    internal sealed class InterpolatedStringHandlerArgumentAttribute : Attribute
    {
        public InterpolatedStringHandlerArgumentAttribute(string argument) { }
        public InterpolatedStringHandlerArgumentAttribute(params string[] arguments) { }
    }
}
