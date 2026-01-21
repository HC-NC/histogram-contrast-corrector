namespace Histogram_Contrast_Corrector
{
    partial class RasterForm
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
            Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterForm));
            Label label2;
            Label label3;
            GroupBox groupBox1;
            Label label7;
            Label label6;
            Label label5;
            Label label4;
            interpolationComboBox = new ComboBox();
            blueComboBox = new ComboBox();
            greenComboBox = new ComboBox();
            redComboBox = new ComboBox();
            pathTextBox = new TextBox();
            xSizeTextBox = new TextBox();
            ySizeTextBox = new TextBox();
            ignoreZeroCheckBox = new CheckBox();
            acceptButton = new Button();
            cancelButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(interpolationComboBox);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(blueComboBox);
            groupBox1.Controls.Add(greenComboBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(redComboBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // interpolationComboBox
            // 
            resources.ApplyResources(interpolationComboBox, "interpolationComboBox");
            interpolationComboBox.FormattingEnabled = true;
            interpolationComboBox.Name = "interpolationComboBox";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // blueComboBox
            // 
            resources.ApplyResources(blueComboBox, "blueComboBox");
            blueComboBox.FormattingEnabled = true;
            blueComboBox.Name = "blueComboBox";
            // 
            // greenComboBox
            // 
            resources.ApplyResources(greenComboBox, "greenComboBox");
            greenComboBox.FormattingEnabled = true;
            greenComboBox.Name = "greenComboBox";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // redComboBox
            // 
            resources.ApplyResources(redComboBox, "redComboBox");
            redComboBox.FormattingEnabled = true;
            redComboBox.Name = "redComboBox";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // pathTextBox
            // 
            resources.ApplyResources(pathTextBox, "pathTextBox");
            pathTextBox.Name = "pathTextBox";
            pathTextBox.ReadOnly = true;
            // 
            // xSizeTextBox
            // 
            resources.ApplyResources(xSizeTextBox, "xSizeTextBox");
            xSizeTextBox.Name = "xSizeTextBox";
            xSizeTextBox.ReadOnly = true;
            // 
            // ySizeTextBox
            // 
            resources.ApplyResources(ySizeTextBox, "ySizeTextBox");
            ySizeTextBox.Name = "ySizeTextBox";
            ySizeTextBox.ReadOnly = true;
            // 
            // ignoreZeroCheckBox
            // 
            resources.ApplyResources(ignoreZeroCheckBox, "ignoreZeroCheckBox");
            ignoreZeroCheckBox.Name = "ignoreZeroCheckBox";
            ignoreZeroCheckBox.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            resources.ApplyResources(acceptButton, "acceptButton");
            acceptButton.DialogResult = DialogResult.OK;
            acceptButton.Name = "acceptButton";
            acceptButton.UseVisualStyleBackColor = true;
            acceptButton.Click += acceptButton_Click;
            // 
            // cancelButton
            // 
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // RasterForm
            // 
            AcceptButton = acceptButton;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            Controls.Add(cancelButton);
            Controls.Add(acceptButton);
            Controls.Add(groupBox1);
            Controls.Add(ignoreZeroCheckBox);
            Controls.Add(pathTextBox);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(xSizeTextBox);
            Controls.Add(ySizeTextBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "RasterForm";
            ShowIcon = false;
            Load += RasterForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox pathTextBox;
        private Label label2;
        private Label label3;
        private TextBox xSizeTextBox;
        private TextBox ySizeTextBox;
        private CheckBox ignoreZeroCheckBox;
        private Label label4;
        private ComboBox blueComboBox;
        private ComboBox greenComboBox;
        private ComboBox redComboBox;
        private Button acceptButton;
        private Button cancelButton;
        private ComboBox interpolationComboBox;
    }
}