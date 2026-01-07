namespace Common.Object
{
    public static class ObjectExtensions
    {
        public static bool TryParse<Type>(this object input, out Type result) where Type : class
        {
            var passedObject = input;

            if (passedObject is Type)
            {
                result = (Type)passedObject;

                return true;
            }

            result = default(Type);
            return false;
        }
    }
}
