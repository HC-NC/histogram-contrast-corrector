using System.Reflection;
using Histogram_Contrast_Corrector.Properties;

namespace Histogram_Contrast_Corrector
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            this.Text = string.Format(Resources.AboutTitle, AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;

            this.labelVersion.Text = string.Format(Resources.AboutVersion, AssemblyFullVersion);
            this.textBoxDescription.Text = AssemblyDescription;

            this.labelCopyright.Visible = false;
            this.labelCompanyName.Visible = false;
        }

        #region Методы доступа к атрибутам сборки

        public string AssemblyTitle
        {
            get
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>();

                if (attr != null && !string.IsNullOrEmpty(attr.Title))
                {
                    return attr.Title;
                }

                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            }
        }

        public string AssemblyFullVersion
        {
            get
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>();

                if (attr != null && !string.IsNullOrEmpty(attr.Version))
                {
                    return attr.Version;
                }

                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";
            }
        }

        public string AssemblyDescription
        {
            get
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>();

                if (attr != null && !string.IsNullOrEmpty(attr.Description))
                {
                    return attr.Description;
                }

                // Локализованное дефолтное описание
                return Resources.AboutDefaultDesc;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>();

                if (attr != null && !string.IsNullOrEmpty(attr.Product))
                {
                    return attr.Product;
                }

                return "Histogram Contrast Corrector";
            }
        }

        #endregion
    }
}
