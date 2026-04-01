using System.Reflection;

namespace Histogram_Contrast_Corrector
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            this.Text = string.Format("О программе {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;

            this.labelVersion.Text = string.Format("Версия: {0}", AssemblyFullVersion);

            this.textBoxDescription.Text = AssemblyDescription;

            this.labelCopyright.Visible = false;
            this.labelCompanyName.Visible = false;
        }

        #region Методы доступа к атрибутам сборки

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (!string.IsNullOrEmpty(titleAttribute.Title))
                    {
                        return titleAttribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            }
        }

        public string AssemblyFullVersion
        {
            get
            {
                var attribute = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyFileVersionAttribute>();

                if (attribute != null && !string.IsNullOrEmpty(attribute.Version))
                {
                    return attribute.Version;
                }

                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "Программа предназначена для коррекции контраста космических снимков и изображений на основе гистограммных методов.";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "Histogram Contrast Corrector";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        #endregion
    }
}
