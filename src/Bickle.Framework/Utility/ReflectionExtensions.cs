namespace Bickle.Utility
{
    public static class ReflectionExtensions
    {
        public static object InvokeWithReflection(this object target, string method, params object[] parameters)
        {
            return target.GetType().GetMethod(method).Invoke(target, parameters);
        }

        public static object GetPropertyWithReflection(this object target, string prop)
        {
            return target.GetType().GetProperty(prop).GetValue(target, null);
        }
    }
}