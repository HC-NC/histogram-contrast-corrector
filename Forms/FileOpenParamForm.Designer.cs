namespace Histogram_Contrast_Corrector
{
    partial class FileOpenParamForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileOpenParamForm));
            acceptButton = new Button();
            cancelButton = new Button();
            ignoreZeroCheckBox = new CheckBox();
            nameTextBox = new TextBox();
            pathTextBox = new TextBox();
            SuspendLayout();
            // 
            // acceptButton
            // 
            resources.ApplyResources(acceptButton, "acceptButton");
            acceptButton.DialogResult = DialogResult.OK;
            acceptButton.Name = "acceptButton";
            acceptButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // ignoreZeroCheckBox
            // 
            resources.ApplyResources(ignoreZeroCheckBox, "ignoreZeroCheckBox");
            ignoreZeroCheckBox.Checked = true;
            ignoreZeroCheckBox.CheckState = CheckState.Checked;
            ignoreZeroCheckBox.Name = "ignoreZeroCheckBox";
            ignoreZeroCheckBox.UseVisualStyleBackColor = true;
            // 
            // nameTextBox
            // 
            resources.ApplyResources(nameTextBox, "nameTextBox");
            nameTextBox.Name = "nameTextBox";
            nameTextBox.ReadOnly = true;
            // 
            // pathTextBox
            // 
            resources.ApplyResources(pathTextBox, "pathTextBox");
            pathTextBox.Name = "pathTextBox";
            pathTextBox.ReadOnly = true;
            // 
            // FileOpenParamForm
            // 
            AcceptButton = acceptButton;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            Controls.Add(pathTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(ignoreZeroCheckBox);
            Controls.Add(cancelButton);
            Controls.Add(acceptButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FileOpenParamForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button acceptButton;
        private Button cancelButton;
        private CheckBox ignoreZeroCheckBox;
        private TextBox nameTextBox;
        private TextBox pathTextBox;
    }
}