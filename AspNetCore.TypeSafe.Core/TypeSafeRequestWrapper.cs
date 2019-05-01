namespace AspnetCore.TypeSafe.Core
{
    // hack: Necessary for json (de)serialization
    public class TypeSafeRequestWrapper
    {
        public TypeSafeRequestWrapper()
        {
        }

        public TypeSafeRequestWrapper(ITypeSafeRequest request)
        {
            Request = request;
        }

        public ITypeSafeRequest Request { get; set; }
    }
}