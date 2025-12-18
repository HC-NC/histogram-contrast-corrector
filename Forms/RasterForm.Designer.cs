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
            Label label2;
            Label label3;
            GroupBox groupBox1;
            Label label6;
            Label label5;
            Label label4;
            Label label7;
            blueComboBox = new ComboBox();
            greenComboBox = new ComboBox();
            redComboBox = new ComboBox();
            pathTextBox = new TextBox();
            xSizeTextBox = new TextBox();
            ySizeTextBox = new TextBox();
            ignoreZeroCheckBox = new CheckBox();
            acceptButton = new Button();
            cancelButton = new Button();
            interpolationComboBox = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            groupBox1 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label7 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(50, 25);
            label1.TabIndex = 0;
            label1.Text = "Path:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(63, 25);
            label2.TabIndex = 2;
            label2.Text = "X Size:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(247, 53);
            label3.Name = "label3";
            label3.Size = new Size(58, 25);
            label3.TabIndex = 3;
            label3.Text = "Y Size";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(interpolationComboBox);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(blueComboBox);
            groupBox1.Controls.Add(greenComboBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(redComboBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new Point(12, 121);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(459, 193);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Display settings";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 111);
            label6.Name = "label6";
            label6.Size = new Size(49, 25);
            label6.TabIndex = 5;
            label6.Text = "Blue:";
            // 
            // blueComboBox
            // 
            blueComboBox.FormattingEnabled = true;
            blueComboBox.Location = new Point(74, 108);
            blueComboBox.Name = "blueComboBox";
            blueComboBox.Size = new Size(379, 33);
            blueComboBox.TabIndex = 4;
            // 
            // greenComboBox
            // 
            greenComboBox.FormattingEnabled = true;
            greenComboBox.Location = new Point(74, 69);
            greenComboBox.Name = "greenComboBox";
            greenComboBox.Size = new Size(379, 33);
            greenComboBox.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 72);
            label5.Name = "label5";
            label5.Size = new Size(62, 25);
            label5.TabIndex = 2;
            label5.Text = "Green:";
            // 
            // redComboBox
            // 
            redComboBox.FormattingEnabled = true;
            redComboBox.Location = new Point(74, 30);
            redComboBox.Name = "redComboBox";
            redComboBox.Size = new Size(379, 33);
            redComboBox.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 33);
            label4.Name = "label4";
            label4.Size = new Size(46, 25);
            label4.TabIndex = 0;
            label4.Text = "Red:";
            // 
            // pathTextBox
            // 
            pathTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pathTextBox.Location = new Point(81, 12);
            pathTextBox.Name = "pathTextBox";
            pathTextBox.ReadOnly = true;
            pathTextBox.Size = new Size(390, 31);
            pathTextBox.TabIndex = 1;
            // 
            // xSizeTextBox
            // 
            xSizeTextBox.Location = new Point(81, 49);
            xSizeTextBox.Name = "xSizeTextBox";
            xSizeTextBox.ReadOnly = true;
            xSizeTextBox.Size = new Size(160, 31);
            xSizeTextBox.TabIndex = 4;
            // 
            // ySizeTextBox
            // 
            ySizeTextBox.Location = new Point(311, 49);
            ySizeTextBox.Name = "ySizeTextBox";
            ySizeTextBox.ReadOnly = true;
            ySizeTextBox.Size = new Size(160, 31);
            ySizeTextBox.TabIndex = 5;
            // 
            // ignoreZeroCheckBox
            // 
            ignoreZeroCheckBox.AutoSize = true;
            ignoreZeroCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            ignoreZeroCheckBox.Enabled = false;
            ignoreZeroCheckBox.Location = new Point(12, 86);
            ignoreZeroCheckBox.Name = "ignoreZeroCheckBox";
            ignoreZeroCheckBox.Size = new Size(133, 29);
            ignoreZeroCheckBox.TabIndex = 7;
            ignoreZeroCheckBox.Text = "Ignore zero:";
            ignoreZeroCheckBox.UseVisualStyleBackColor = true;
            // 
            // acceptButton
            // 
            acceptButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            acceptButton.DialogResult = DialogResult.OK;
            acceptButton.Location = new Point(359, 318);
            acceptButton.Name = "acceptButton";
            acceptButton.Size = new Size(112, 34);
            acceptButton.TabIndex = 9;
            acceptButton.Text = "Accept";
            acceptButton.UseVisualStyleBackColor = true;
            acceptButton.Click += acceptButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(241, 318);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(112, 34);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 150);
            label7.Name = "label7";
            label7.Size = new Size(118, 25);
            label7.TabIndex = 6;
            label7.Text = "Interpolation:";
            // 
            // interpolationComboBox
            // 
            interpolationComboBox.FormattingEnabled = true;
            interpolationComboBox.Location = new Point(130, 147);
            interpolationComboBox.Name = "interpolationComboBox";
            interpolationComboBox.Size = new Size(323, 33);
            interpolationComboBox.TabIndex = 7;
            // 
            // RasterForm
            // 
            AcceptButton = acceptButton;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new Size(483, 364);
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
            StartPosition = FormStartPosition.CenterParent;
            Text = "DatasetForm";
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