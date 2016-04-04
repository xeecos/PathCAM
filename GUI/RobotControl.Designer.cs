namespace GUI
{
    partial class RobotControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.steppersEnabledBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.pause_resume_button = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBackY = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonRightY = new System.Windows.Forms.Button();
            this.buttonLeftY = new System.Windows.Forms.Button();
            this.buttonLeftY10 = new System.Windows.Forms.Button();
            this.buttonRightY10 = new System.Windows.Forms.Button();
            this.buttonRightZ10 = new System.Windows.Forms.Button();
            this.buttonLeftZ10 = new System.Windows.Forms.Button();
            this.buttonLeftZ = new System.Windows.Forms.Button();
            this.buttonRightZ = new System.Windows.Forms.Button();
            this.buttonBackZ = new System.Windows.Forms.Button();
            this.buttonRightX10 = new System.Windows.Forms.Button();
            this.buttonLeftX10 = new System.Windows.Forms.Button();
            this.buttonLeftX = new System.Windows.Forms.Button();
            this.buttonRightX = new System.Windows.Forms.Button();
            this.buttonBackX = new System.Windows.Forms.Button();
            this.buttonHomePosition = new System.Windows.Forms.Button();
            this.checkBoxMotor = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // steppersEnabledBox
            // 
            this.steppersEnabledBox.AutoCheck = false;
            this.steppersEnabledBox.AutoSize = true;
            this.steppersEnabledBox.Enabled = false;
            this.steppersEnabledBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.steppersEnabledBox.Location = new System.Drawing.Point(500, 105);
            this.steppersEnabledBox.Name = "steppersEnabledBox";
            this.steppersEnabledBox.Size = new System.Drawing.Size(93, 16);
            this.steppersEnabledBox.TabIndex = 3;
            this.steppersEnabledBox.Text = "使能步进电机";
            this.steppersEnabledBox.UseVisualStyleBackColor = true;
            this.steppersEnabledBox.Click += new System.EventHandler(this.steppersEnabledBox_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Enabled = false;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(100, 45);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "停止";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // pause_resume_button
            // 
            this.pause_resume_button.Enabled = false;
            this.pause_resume_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pause_resume_button.Location = new System.Drawing.Point(6, 45);
            this.pause_resume_button.Name = "pause_resume_button";
            this.pause_resume_button.Size = new System.Drawing.Size(75, 23);
            this.pause_resume_button.TabIndex = 1;
            this.pause_resume_button.Text = "暂停";
            this.pause_resume_button.UseVisualStyleBackColor = true;
            this.pause_resume_button.Click += new System.EventHandler(this.pause_resume_button_Click);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runButton.Location = new System.Drawing.Point(100, 15);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "运行";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(6, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 79;
            this.button4.Text = "串口";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 12);
            this.label1.TabIndex = 80;
            this.label1.Text = "------------------------Robot------------------------";
            // 
            // buttonBackY
            // 
            this.buttonBackY.Enabled = false;
            this.buttonBackY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackY.Location = new System.Drawing.Point(356, 45);
            this.buttonBackY.Name = "buttonBackY";
            this.buttonBackY.Size = new System.Drawing.Size(75, 23);
            this.buttonBackY.TabIndex = 81;
            this.buttonBackY.Text = "Y 回到零点";
            this.buttonBackY.UseVisualStyleBackColor = true;
            this.buttonBackY.Click += new System.EventHandler(this.buttonBackY_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 3;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDown1.Location = new System.Drawing.Point(65, 77);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(110, 21);
            this.numericUpDown1.TabIndex = 82;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 83;
            this.label2.Text = "Z Offset";
            // 
            // buttonRightY
            // 
            this.buttonRightY.Enabled = false;
            this.buttonRightY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightY.Location = new System.Drawing.Point(437, 45);
            this.buttonRightY.Name = "buttonRightY";
            this.buttonRightY.Size = new System.Drawing.Size(75, 23);
            this.buttonRightY.TabIndex = 84;
            this.buttonRightY.Text = "Y +1";
            this.buttonRightY.UseVisualStyleBackColor = true;
            this.buttonRightY.Click += new System.EventHandler(this.buttonRightY_Click);
            // 
            // buttonLeftY
            // 
            this.buttonLeftY.Enabled = false;
            this.buttonLeftY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftY.Location = new System.Drawing.Point(275, 45);
            this.buttonLeftY.Name = "buttonLeftY";
            this.buttonLeftY.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftY.TabIndex = 85;
            this.buttonLeftY.Text = "Y -1";
            this.buttonLeftY.UseVisualStyleBackColor = true;
            this.buttonLeftY.Click += new System.EventHandler(this.buttonLeftY_Click);
            // 
            // buttonLeftY10
            // 
            this.buttonLeftY10.Enabled = false;
            this.buttonLeftY10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftY10.Location = new System.Drawing.Point(194, 45);
            this.buttonLeftY10.Name = "buttonLeftY10";
            this.buttonLeftY10.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftY10.TabIndex = 86;
            this.buttonLeftY10.Text = "Y -10";
            this.buttonLeftY10.UseVisualStyleBackColor = true;
            this.buttonLeftY10.Click += new System.EventHandler(this.buttonLeftY10_Click);
            // 
            // buttonRightY10
            // 
            this.buttonRightY10.Enabled = false;
            this.buttonRightY10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightY10.Location = new System.Drawing.Point(518, 45);
            this.buttonRightY10.Name = "buttonRightY10";
            this.buttonRightY10.Size = new System.Drawing.Size(75, 23);
            this.buttonRightY10.TabIndex = 87;
            this.buttonRightY10.Text = "Y +10";
            this.buttonRightY10.UseVisualStyleBackColor = true;
            this.buttonRightY10.Click += new System.EventHandler(this.buttonRightY10_Click);
            // 
            // buttonRightZ10
            // 
            this.buttonRightZ10.Enabled = false;
            this.buttonRightZ10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightZ10.Location = new System.Drawing.Point(518, 74);
            this.buttonRightZ10.Name = "buttonRightZ10";
            this.buttonRightZ10.Size = new System.Drawing.Size(75, 23);
            this.buttonRightZ10.TabIndex = 92;
            this.buttonRightZ10.Text = "Z +5";
            this.buttonRightZ10.UseVisualStyleBackColor = true;
            this.buttonRightZ10.Click += new System.EventHandler(this.buttonRightZ10_Click);
            // 
            // buttonLeftZ10
            // 
            this.buttonLeftZ10.Enabled = false;
            this.buttonLeftZ10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftZ10.Location = new System.Drawing.Point(194, 74);
            this.buttonLeftZ10.Name = "buttonLeftZ10";
            this.buttonLeftZ10.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftZ10.TabIndex = 91;
            this.buttonLeftZ10.Text = "Z -5";
            this.buttonLeftZ10.UseVisualStyleBackColor = true;
            this.buttonLeftZ10.Click += new System.EventHandler(this.buttonLeftZ10_Click);
            // 
            // buttonLeftZ
            // 
            this.buttonLeftZ.Enabled = false;
            this.buttonLeftZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftZ.Location = new System.Drawing.Point(275, 74);
            this.buttonLeftZ.Name = "buttonLeftZ";
            this.buttonLeftZ.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftZ.TabIndex = 90;
            this.buttonLeftZ.Text = "Z -0.2";
            this.buttonLeftZ.UseVisualStyleBackColor = true;
            this.buttonLeftZ.Click += new System.EventHandler(this.buttonLeftZ_Click);
            // 
            // buttonRightZ
            // 
            this.buttonRightZ.Enabled = false;
            this.buttonRightZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightZ.Location = new System.Drawing.Point(437, 74);
            this.buttonRightZ.Name = "buttonRightZ";
            this.buttonRightZ.Size = new System.Drawing.Size(75, 23);
            this.buttonRightZ.TabIndex = 89;
            this.buttonRightZ.Text = "Z +0.2";
            this.buttonRightZ.UseVisualStyleBackColor = true;
            this.buttonRightZ.Click += new System.EventHandler(this.buttonRightZ_Click);
            // 
            // buttonBackZ
            // 
            this.buttonBackZ.Enabled = false;
            this.buttonBackZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackZ.Location = new System.Drawing.Point(356, 74);
            this.buttonBackZ.Name = "buttonBackZ";
            this.buttonBackZ.Size = new System.Drawing.Size(75, 23);
            this.buttonBackZ.TabIndex = 88;
            this.buttonBackZ.Text = "Z 回到零点";
            this.buttonBackZ.UseVisualStyleBackColor = true;
            this.buttonBackZ.Click += new System.EventHandler(this.buttonBackZ_Click);
            // 
            // buttonRightX10
            // 
            this.buttonRightX10.Enabled = false;
            this.buttonRightX10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightX10.Location = new System.Drawing.Point(518, 15);
            this.buttonRightX10.Name = "buttonRightX10";
            this.buttonRightX10.Size = new System.Drawing.Size(75, 23);
            this.buttonRightX10.TabIndex = 97;
            this.buttonRightX10.Text = "X +10";
            this.buttonRightX10.UseVisualStyleBackColor = true;
            this.buttonRightX10.Click += new System.EventHandler(this.buttonRightX10_Click);
            // 
            // buttonLeftX10
            // 
            this.buttonLeftX10.Enabled = false;
            this.buttonLeftX10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftX10.Location = new System.Drawing.Point(194, 15);
            this.buttonLeftX10.Name = "buttonLeftX10";
            this.buttonLeftX10.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftX10.TabIndex = 96;
            this.buttonLeftX10.Text = "X -10";
            this.buttonLeftX10.UseVisualStyleBackColor = true;
            this.buttonLeftX10.Click += new System.EventHandler(this.buttonLeftX10_Click);
            // 
            // buttonLeftX
            // 
            this.buttonLeftX.Enabled = false;
            this.buttonLeftX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLeftX.Location = new System.Drawing.Point(275, 15);
            this.buttonLeftX.Name = "buttonLeftX";
            this.buttonLeftX.Size = new System.Drawing.Size(75, 23);
            this.buttonLeftX.TabIndex = 95;
            this.buttonLeftX.Text = "X -1";
            this.buttonLeftX.UseVisualStyleBackColor = true;
            this.buttonLeftX.Click += new System.EventHandler(this.buttonLeftX_Click);
            // 
            // buttonRightX
            // 
            this.buttonRightX.Enabled = false;
            this.buttonRightX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRightX.Location = new System.Drawing.Point(437, 15);
            this.buttonRightX.Name = "buttonRightX";
            this.buttonRightX.Size = new System.Drawing.Size(75, 23);
            this.buttonRightX.TabIndex = 94;
            this.buttonRightX.Text = "X +1";
            this.buttonRightX.UseVisualStyleBackColor = true;
            this.buttonRightX.Click += new System.EventHandler(this.buttonRightX_Click);
            // 
            // buttonBackX
            // 
            this.buttonBackX.Enabled = false;
            this.buttonBackX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackX.Location = new System.Drawing.Point(356, 15);
            this.buttonBackX.Name = "buttonBackX";
            this.buttonBackX.Size = new System.Drawing.Size(75, 23);
            this.buttonBackX.TabIndex = 93;
            this.buttonBackX.Text = "X 回到零点";
            this.buttonBackX.UseVisualStyleBackColor = true;
            this.buttonBackX.Click += new System.EventHandler(this.buttonBackX_Click);
            // 
            // buttonHomePosition
            // 
            this.buttonHomePosition.Enabled = false;
            this.buttonHomePosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHomePosition.Location = new System.Drawing.Point(194, 103);
            this.buttonHomePosition.Name = "buttonHomePosition";
            this.buttonHomePosition.Size = new System.Drawing.Size(156, 23);
            this.buttonHomePosition.TabIndex = 98;
            this.buttonHomePosition.Text = "设置零点";
            this.buttonHomePosition.UseVisualStyleBackColor = true;
            this.buttonHomePosition.Click += new System.EventHandler(this.buttonHomePosition_Click);
            // 
            // checkBoxMotor
            // 
            this.checkBoxMotor.AutoCheck = false;
            this.checkBoxMotor.AutoSize = true;
            this.checkBoxMotor.Enabled = false;
            this.checkBoxMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMotor.Location = new System.Drawing.Point(388, 106);
            this.checkBoxMotor.Name = "checkBoxMotor";
            this.checkBoxMotor.Size = new System.Drawing.Size(69, 16);
            this.checkBoxMotor.TabIndex = 99;
            this.checkBoxMotor.Text = "使能电机";
            this.checkBoxMotor.UseVisualStyleBackColor = true;
            this.checkBoxMotor.Click += new System.EventHandler(this.checkBoxMotor_Click);
            // 
            // RobotControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.checkBoxMotor);
            this.Controls.Add(this.buttonHomePosition);
            this.Controls.Add(this.buttonRightX10);
            this.Controls.Add(this.buttonLeftX10);
            this.Controls.Add(this.buttonLeftX);
            this.Controls.Add(this.buttonRightX);
            this.Controls.Add(this.buttonBackX);
            this.Controls.Add(this.buttonRightZ10);
            this.Controls.Add(this.buttonLeftZ10);
            this.Controls.Add(this.buttonLeftZ);
            this.Controls.Add(this.buttonRightZ);
            this.Controls.Add(this.buttonBackZ);
            this.Controls.Add(this.buttonRightY10);
            this.Controls.Add(this.buttonLeftY10);
            this.Controls.Add(this.buttonLeftY);
            this.Controls.Add(this.buttonRightY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonBackY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.steppersEnabledBox);
            this.Controls.Add(this.pause_resume_button);
            this.Controls.Add(this.cancelButton);
            this.Name = "RobotControl";
            this.Size = new System.Drawing.Size(606, 134);
            this.Load += new System.EventHandler(this.RobotControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox steppersEnabledBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button pause_resume_button;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBackY;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonRightY;
        private System.Windows.Forms.Button buttonLeftY;
        private System.Windows.Forms.Button buttonLeftY10;
        private System.Windows.Forms.Button buttonRightY10;
        private System.Windows.Forms.Button buttonRightZ10;
        private System.Windows.Forms.Button buttonLeftZ10;
        private System.Windows.Forms.Button buttonLeftZ;
        private System.Windows.Forms.Button buttonRightZ;
        private System.Windows.Forms.Button buttonBackZ;
        private System.Windows.Forms.Button buttonRightX10;
        private System.Windows.Forms.Button buttonLeftX10;
        private System.Windows.Forms.Button buttonLeftX;
        private System.Windows.Forms.Button buttonRightX;
        private System.Windows.Forms.Button buttonBackX;
        private System.Windows.Forms.Button buttonHomePosition;
        private System.Windows.Forms.CheckBox checkBoxMotor;
    }
}
