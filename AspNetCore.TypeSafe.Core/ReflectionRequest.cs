using System.Collections.Generic;

namespace AspnetCore.TypeSafe.Core
{
    public class ReflectionRequest : ITypeSafeRequest
    {
        public ReflectionRequest()
        {
        }

        public ReflectionRequest(string methodToInvoke, List<object> parameters)
        {
            MethodToInvoke = methodToInvoke;
            Parameters = parameters;
        }

        public string MethodToInvoke { get; set; }

        public List<object> Parameters { get; set; }

        public TypeSafeRequestWrapper Wrap()
        {
            return new TypeSafeRequestWrapper(this);
        }
    }
}