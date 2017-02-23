namespace LSystemsPlants
{
    partial class MainForm
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
            this.portraitControl = new OpenTK.GLControl();
            this.btRegenerate = new System.Windows.Forms.Button();
            this.tbRule0 = new System.Windows.Forms.TextBox();
            this.tbAxiom = new System.Windows.Forms.TextBox();
            this.tbIterations = new System.Windows.Forms.TextBox();
            this.tbDefaultDelta = new System.Windows.Forms.TextBox();
            this.tbDefaultStep = new System.Windows.Forms.TextBox();
            this.tbDeltaChange = new System.Windows.Forms.TextBox();
            this.tbStepChange = new System.Windows.Forms.TextBox();
            this.lbRule0 = new System.Windows.Forms.Label();
            this.lbAxiom = new System.Windows.Forms.Label();
            this.lbIterations = new System.Windows.Forms.Label();
            this.lbDEfaultDelta = new System.Windows.Forms.Label();
            this.lbDefaultStep = new System.Windows.Forms.Label();
            this.lbDeltaChange = new System.Windows.Forms.Label();
            this.lbStepChange = new System.Windows.Forms.Label();
            this.lbRule1 = new System.Windows.Forms.Label();
            this.tbRule1 = new System.Windows.Forms.TextBox();
            this.lbRule2 = new System.Windows.Forms.Label();
            this.tbRule2 = new System.Windows.Forms.TextBox();
            this.lbRule3 = new System.Windows.Forms.Label();
            this.tbRule3 = new System.Windows.Forms.TextBox();
            this.lbRule4 = new System.Windows.Forms.Label();
            this.tbRule4 = new System.Windows.Forms.TextBox();
            this.btSimple = new System.Windows.Forms.Button();
            this.btSetKoch = new System.Windows.Forms.Button();
            this.btSquares = new System.Windows.Forms.Button();
            this.btTriangles = new System.Windows.Forms.Button();
            this.btDrago = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portraitControl
            // 
            this.portraitControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portraitControl.BackColor = System.Drawing.Color.Black;
            this.portraitControl.Location = new System.Drawing.Point(12, 12);
            this.portraitControl.Name = "portraitControl";
            this.portraitControl.Size = new System.Drawing.Size(1541, 960);
            this.portraitControl.TabIndex = 1;
            this.portraitControl.VSync = true;
            // 
            // btRegenerate
            // 
            this.btRegenerate.Location = new System.Drawing.Point(1604, 343);
            this.btRegenerate.Name = "btRegenerate";
            this.btRegenerate.Size = new System.Drawing.Size(284, 23);
            this.btRegenerate.TabIndex = 2;
            this.btRegenerate.Text = "UpdateModel";
            this.btRegenerate.UseVisualStyleBackColor = true;
            this.btRegenerate.Click += new System.EventHandler(this.btRegenerate_Click);
            // 
            // tbRule0
            // 
            this.tbRule0.Location = new System.Drawing.Point(1678, 44);
            this.tbRule0.Name = "tbRule0";
            this.tbRule0.Size = new System.Drawing.Size(210, 20);
            this.tbRule0.TabIndex = 3;
            // 
            // tbAxiom
            // 
            this.tbAxiom.Location = new System.Drawing.Point(1678, 18);
            this.tbAxiom.Name = "tbAxiom";
            this.tbAxiom.Size = new System.Drawing.Size(210, 20);
            this.tbAxiom.TabIndex = 4;
            // 
            // tbIterations
            // 
            this.tbIterations.Location = new System.Drawing.Point(1678, 186);
            this.tbIterations.Name = "tbIterations";
            this.tbIterations.Size = new System.Drawing.Size(210, 20);
            this.tbIterations.TabIndex = 5;
            // 
            // tbDefaultDelta
            // 
            this.tbDefaultDelta.Location = new System.Drawing.Point(1678, 214);
            this.tbDefaultDelta.Name = "tbDefaultDelta";
            this.tbDefaultDelta.Size = new System.Drawing.Size(210, 20);
            this.tbDefaultDelta.TabIndex = 6;
            // 
            // tbDefaultStep
            // 
            this.tbDefaultStep.Location = new System.Drawing.Point(1678, 242);
            this.tbDefaultStep.Name = "tbDefaultStep";
            this.tbDefaultStep.Size = new System.Drawing.Size(210, 20);
            this.tbDefaultStep.TabIndex = 7;
            // 
            // tbDeltaChange
            // 
            this.tbDeltaChange.Location = new System.Drawing.Point(1678, 270);
            this.tbDeltaChange.Name = "tbDeltaChange";
            this.tbDeltaChange.Size = new System.Drawing.Size(210, 20);
            this.tbDeltaChange.TabIndex = 8;
            // 
            // tbStepChange
            // 
            this.tbStepChange.Location = new System.Drawing.Point(1678, 298);
            this.tbStepChange.Name = "tbStepChange";
            this.tbStepChange.Size = new System.Drawing.Size(210, 20);
            this.tbStepChange.TabIndex = 9;
            // 
            // lbRule0
            // 
            this.lbRule0.AutoSize = true;
            this.lbRule0.Location = new System.Drawing.Point(1601, 44);
            this.lbRule0.Name = "lbRule0";
            this.lbRule0.Size = new System.Drawing.Size(38, 13);
            this.lbRule0.TabIndex = 10;
            this.lbRule0.Text = "Rule 0";
            // 
            // lbAxiom
            // 
            this.lbAxiom.AutoSize = true;
            this.lbAxiom.Location = new System.Drawing.Point(1601, 18);
            this.lbAxiom.Name = "lbAxiom";
            this.lbAxiom.Size = new System.Drawing.Size(35, 13);
            this.lbAxiom.TabIndex = 11;
            this.lbAxiom.Text = "Axiom";
            // 
            // lbIterations
            // 
            this.lbIterations.AutoSize = true;
            this.lbIterations.Location = new System.Drawing.Point(1601, 186);
            this.lbIterations.Name = "lbIterations";
            this.lbIterations.Size = new System.Drawing.Size(50, 13);
            this.lbIterations.TabIndex = 12;
            this.lbIterations.Text = "Iterations";
            // 
            // lbDEfaultDelta
            // 
            this.lbDEfaultDelta.AutoSize = true;
            this.lbDEfaultDelta.Location = new System.Drawing.Point(1601, 214);
            this.lbDEfaultDelta.Name = "lbDEfaultDelta";
            this.lbDEfaultDelta.Size = new System.Drawing.Size(32, 13);
            this.lbDEfaultDelta.TabIndex = 13;
            this.lbDEfaultDelta.Text = "Delta";
            // 
            // lbDefaultStep
            // 
            this.lbDefaultStep.AutoSize = true;
            this.lbDefaultStep.Location = new System.Drawing.Point(1601, 242);
            this.lbDefaultStep.Name = "lbDefaultStep";
            this.lbDefaultStep.Size = new System.Drawing.Size(29, 13);
            this.lbDefaultStep.TabIndex = 14;
            this.lbDefaultStep.Text = "Step";
            // 
            // lbDeltaChange
            // 
            this.lbDeltaChange.AutoSize = true;
            this.lbDeltaChange.Location = new System.Drawing.Point(1601, 270);
            this.lbDeltaChange.Name = "lbDeltaChange";
            this.lbDeltaChange.Size = new System.Drawing.Size(71, 13);
            this.lbDeltaChange.TabIndex = 15;
            this.lbDeltaChange.Text = "Delta change";
            // 
            // lbStepChange
            // 
            this.lbStepChange.AutoSize = true;
            this.lbStepChange.Location = new System.Drawing.Point(1601, 298);
            this.lbStepChange.Name = "lbStepChange";
            this.lbStepChange.Size = new System.Drawing.Size(66, 13);
            this.lbStepChange.TabIndex = 16;
            this.lbStepChange.Text = "StepChange";
            // 
            // lbRule1
            // 
            this.lbRule1.AutoSize = true;
            this.lbRule1.Location = new System.Drawing.Point(1601, 70);
            this.lbRule1.Name = "lbRule1";
            this.lbRule1.Size = new System.Drawing.Size(38, 13);
            this.lbRule1.TabIndex = 18;
            this.lbRule1.Text = "Rule 1";
            // 
            // tbRule1
            // 
            this.tbRule1.Location = new System.Drawing.Point(1678, 70);
            this.tbRule1.Name = "tbRule1";
            this.tbRule1.Size = new System.Drawing.Size(210, 20);
            this.tbRule1.TabIndex = 17;
            // 
            // lbRule2
            // 
            this.lbRule2.AutoSize = true;
            this.lbRule2.Location = new System.Drawing.Point(1601, 96);
            this.lbRule2.Name = "lbRule2";
            this.lbRule2.Size = new System.Drawing.Size(38, 13);
            this.lbRule2.TabIndex = 20;
            this.lbRule2.Text = "Rule 2";
            // 
            // tbRule2
            // 
            this.tbRule2.Location = new System.Drawing.Point(1678, 96);
            this.tbRule2.Name = "tbRule2";
            this.tbRule2.Size = new System.Drawing.Size(210, 20);
            this.tbRule2.TabIndex = 19;
            // 
            // lbRule3
            // 
            this.lbRule3.AutoSize = true;
            this.lbRule3.Location = new System.Drawing.Point(1601, 122);
            this.lbRule3.Name = "lbRule3";
            this.lbRule3.Size = new System.Drawing.Size(38, 13);
            this.lbRule3.TabIndex = 22;
            this.lbRule3.Text = "Rule 3";
            // 
            // tbRule3
            // 
            this.tbRule3.Location = new System.Drawing.Point(1678, 122);
            this.tbRule3.Name = "tbRule3";
            this.tbRule3.Size = new System.Drawing.Size(210, 20);
            this.tbRule3.TabIndex = 21;
            // 
            // lbRule4
            // 
            this.lbRule4.AutoSize = true;
            this.lbRule4.Location = new System.Drawing.Point(1601, 148);
            this.lbRule4.Name = "lbRule4";
            this.lbRule4.Size = new System.Drawing.Size(38, 13);
            this.lbRule4.TabIndex = 24;
            this.lbRule4.Text = "Rule 4";
            // 
            // tbRule4
            // 
            this.tbRule4.Location = new System.Drawing.Point(1678, 148);
            this.tbRule4.Name = "tbRule4";
            this.tbRule4.Size = new System.Drawing.Size(210, 20);
            this.tbRule4.TabIndex = 23;
            // 
            // btSimple
            // 
            this.btSimple.Location = new System.Drawing.Point(1604, 401);
            this.btSimple.Name = "btSimple";
            this.btSimple.Size = new System.Drawing.Size(284, 23);
            this.btSimple.TabIndex = 25;
            this.btSimple.Text = "Tree";
            this.btSimple.UseVisualStyleBackColor = true;
            this.btSimple.Click += new System.EventHandler(this.btSimple_Click);
            // 
            // btSetKoch
            // 
            this.btSetKoch.Location = new System.Drawing.Point(1604, 430);
            this.btSetKoch.Name = "btSetKoch";
            this.btSetKoch.Size = new System.Drawing.Size(284, 23);
            this.btSetKoch.TabIndex = 26;
            this.btSetKoch.Text = "Koch";
            this.btSetKoch.UseVisualStyleBackColor = true;
            this.btSetKoch.Click += new System.EventHandler(this.btSetKoch_Click);
            // 
            // btSquares
            // 
            this.btSquares.Location = new System.Drawing.Point(1604, 459);
            this.btSquares.Name = "btSquares";
            this.btSquares.Size = new System.Drawing.Size(284, 23);
            this.btSquares.TabIndex = 27;
            this.btSquares.Text = "Squares";
            this.btSquares.UseVisualStyleBackColor = true;
            this.btSquares.Click += new System.EventHandler(this.btSquares_Click);
            // 
            // btTriangles
            // 
            this.btTriangles.Location = new System.Drawing.Point(1604, 488);
            this.btTriangles.Name = "btTriangles";
            this.btTriangles.Size = new System.Drawing.Size(284, 23);
            this.btTriangles.TabIndex = 28;
            this.btTriangles.Text = "Triangles";
            this.btTriangles.UseVisualStyleBackColor = true;
            this.btTriangles.Click += new System.EventHandler(this.btTrinagles_Click);
            // 
            // btDrago
            // 
            this.btDrago.Location = new System.Drawing.Point(1604, 517);
            this.btDrago.Name = "btDrago";
            this.btDrago.Size = new System.Drawing.Size(284, 23);
            this.btDrago.TabIndex = 29;
            this.btDrago.Text = "Drago";
            this.btDrago.UseVisualStyleBackColor = true;
            this.btDrago.Click += new System.EventHandler(this.btDrago_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1900, 1000);
            this.Controls.Add(this.btDrago);
            this.Controls.Add(this.btTriangles);
            this.Controls.Add(this.btSquares);
            this.Controls.Add(this.btSetKoch);
            this.Controls.Add(this.btSimple);
            this.Controls.Add(this.lbRule4);
            this.Controls.Add(this.tbRule4);
            this.Controls.Add(this.lbRule3);
            this.Controls.Add(this.tbRule3);
            this.Controls.Add(this.lbRule2);
            this.Controls.Add(this.tbRule2);
            this.Controls.Add(this.lbRule1);
            this.Controls.Add(this.tbRule1);
            this.Controls.Add(this.lbStepChange);
            this.Controls.Add(this.lbDeltaChange);
            this.Controls.Add(this.lbDefaultStep);
            this.Controls.Add(this.lbDEfaultDelta);
            this.Controls.Add(this.lbIterations);
            this.Controls.Add(this.lbAxiom);
            this.Controls.Add(this.lbRule0);
            this.Controls.Add(this.tbStepChange);
            this.Controls.Add(this.tbDeltaChange);
            this.Controls.Add(this.tbDefaultStep);
            this.Controls.Add(this.tbDefaultDelta);
            this.Controls.Add(this.tbIterations);
            this.Controls.Add(this.tbAxiom);
            this.Controls.Add(this.tbRule0);
            this.Controls.Add(this.btRegenerate);
            this.Controls.Add(this.portraitControl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "L Systems";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl portraitControl;
        private System.Windows.Forms.Button btRegenerate;
        private System.Windows.Forms.TextBox tbRule0;
        private System.Windows.Forms.TextBox tbAxiom;
        private System.Windows.Forms.TextBox tbIterations;
        private System.Windows.Forms.TextBox tbDefaultDelta;
        private System.Windows.Forms.TextBox tbDefaultStep;
        private System.Windows.Forms.TextBox tbDeltaChange;
        private System.Windows.Forms.TextBox tbStepChange;
        private System.Windows.Forms.Label lbRule0;
        private System.Windows.Forms.Label lbAxiom;
        private System.Windows.Forms.Label lbIterations;
        private System.Windows.Forms.Label lbDEfaultDelta;
        private System.Windows.Forms.Label lbDefaultStep;
        private System.Windows.Forms.Label lbDeltaChange;
        private System.Windows.Forms.Label lbStepChange;
        private System.Windows.Forms.Label lbRule1;
        private System.Windows.Forms.TextBox tbRule1;
        private System.Windows.Forms.Label lbRule2;
        private System.Windows.Forms.TextBox tbRule2;
        private System.Windows.Forms.Label lbRule3;
        private System.Windows.Forms.TextBox tbRule3;
        private System.Windows.Forms.Label lbRule4;
        private System.Windows.Forms.TextBox tbRule4;
        private System.Windows.Forms.Button btSimple;
        private System.Windows.Forms.Button btSetKoch;
        private System.Windows.Forms.Button btSquares;
        private System.Windows.Forms.Button btTriangles;
        private System.Windows.Forms.Button btDrago;
    }
}

