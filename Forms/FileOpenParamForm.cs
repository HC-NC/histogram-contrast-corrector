

namespace Histogram_Contrast_Corrector
{
    public partial class FileOpenParamForm : Form
    {
        public FileOpenParamForm(string name, string path)
        {
            InitializeComponent();

            nameTextBox.Text = name;
            pathTextBox.Text = path;
        }

        public bool IgnoreZero => ignoreZeroCheckBox.Checked;
    }
}
