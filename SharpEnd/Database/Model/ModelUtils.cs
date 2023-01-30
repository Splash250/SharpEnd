using System.Reflection;

namespace SharpEnd.Model
{
    internal static class ModelUtils
    {
        public static List<string> GetPropertyStrings(Model model)
        {
            List<string> props = new List<string>();
            foreach (var property in model.GetType().GetProperties())
            {
                props.Add(property.Name);
            }
            return props;
        }
        public static PropertyInfo[] GetProperties(Model model)
        {
            return model.GetType().GetProperties();
        }

    }
}
