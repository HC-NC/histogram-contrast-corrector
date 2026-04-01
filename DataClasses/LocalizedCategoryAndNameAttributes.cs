using System.ComponentModel;
using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector.DataClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceKey)
            : base(GetMessageFromResource(resourceKey)) { }

        private static string GetMessageFromResource(string resourceKey)
        {
            var value = Resources.ResourceManager.GetString(resourceKey);
            return value ?? resourceKey;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string resourceKey)
            : base(GetMessageFromResource(resourceKey)) { }

        private static string GetMessageFromResource(string resourceKey)
        {
            var value = Resources.ResourceManager.GetString(resourceKey);
            return value ?? resourceKey;
        }
    }
}
