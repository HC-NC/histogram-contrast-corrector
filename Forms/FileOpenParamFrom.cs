

namespace Histogram_Contrast_Corrector
{
    public partial class FileOpenParamFrom : Form
    {
        public FileOpenParamFrom(string name, string path)
        {
            InitializeComponent();

            nameTextBox.Text = name;
            pathTextBox.Text = path;
        }

        public bool IgnoreZero => ignoreZeroCheckBox.Checked;
    }
}
