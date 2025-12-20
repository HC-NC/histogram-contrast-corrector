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
            acceptButton = new Button();
            cancelButton = new Button();
            ignoreZeroCheckBox = new CheckBox();
            nameTextBox = new TextBox();
            pathTextBox = new TextBox();
            SuspendLayout();
            // 
            // acceptButton
            // 
            acceptButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            acceptButton.DialogResult = DialogResult.OK;
            acceptButton.Location = new Point(254, 138);
            acceptButton.Name = "acceptButton";
            acceptButton.Size = new Size(112, 34);
            acceptButton.TabIndex = 0;
            acceptButton.Text = "Accept";
            acceptButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(136, 138);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(112, 34);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // ignoreZeroCheckBox
            // 
            ignoreZeroCheckBox.AutoSize = true;
            ignoreZeroCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            ignoreZeroCheckBox.Checked = true;
            ignoreZeroCheckBox.CheckState = CheckState.Checked;
            ignoreZeroCheckBox.Location = new Point(12, 86);
            ignoreZeroCheckBox.Name = "ignoreZeroCheckBox";
            ignoreZeroCheckBox.Size = new Size(129, 29);
            ignoreZeroCheckBox.TabIndex = 2;
            ignoreZeroCheckBox.Text = "Ignore zero";
            ignoreZeroCheckBox.UseVisualStyleBackColor = true;
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameTextBox.Location = new Point(12, 12);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.ReadOnly = true;
            nameTextBox.Size = new Size(354, 31);
            nameTextBox.TabIndex = 3;
            // 
            // pathTextBox
            // 
            pathTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pathTextBox.Location = new Point(12, 49);
            pathTextBox.Name = "pathTextBox";
            pathTextBox.ReadOnly = true;
            pathTextBox.Size = new Size(354, 31);
            pathTextBox.TabIndex = 4;
            // 
            // FileOpenParamFrom
            // 
            AcceptButton = acceptButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(378, 184);
            Controls.Add(pathTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(ignoreZeroCheckBox);
            Controls.Add(cancelButton);
            Controls.Add(acceptButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FileOpenParamFrom";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "FileOpenParamFrom";
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