namespace NetGraphWinForms
{
    partial class ToolWindowEdges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolWindowEdges));
            this.bdxEditEdge = new System.Windows.Forms.GroupBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.nudMetricTo = new System.Windows.Forms.NumericUpDown();
            this.nudMetricFrom = new System.Windows.Forms.NumericUpDown();
            this.btnEditEdge = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxApexTo = new System.Windows.Forms.ComboBox();
            this.cbxApexFrom = new System.Windows.Forms.ComboBox();
            this.bdxEditEdge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMetricTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMetricFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // bdxEditEdge
            // 
            this.bdxEditEdge.Controls.Add(this.btnEdit);
            this.bdxEditEdge.Controls.Add(this.btnDelete);
            this.bdxEditEdge.Controls.Add(this.nudMetricTo);
            this.bdxEditEdge.Controls.Add(this.nudMetricFrom);
            this.bdxEditEdge.Controls.Add(this.btnEditEdge);
            this.bdxEditEdge.Controls.Add(this.label3);
            this.bdxEditEdge.Controls.Add(this.label2);
            this.bdxEditEdge.Controls.Add(this.cbxApexTo);
            this.bdxEditEdge.Controls.Add(this.cbxApexFrom);
            this.bdxEditEdge.Location = new System.Drawing.Point(12, 12);
            this.bdxEditEdge.Name = "bdxEditEdge";
            this.bdxEditEdge.Size = new System.Drawing.Size(178, 137);
            this.bdxEditEdge.TabIndex = 13;
            this.bdxEditEdge.TabStop = false;
            this.bdxEditEdge.Text = "Создать ребро";
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.Location = new System.Drawing.Point(69, 95);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(32, 32);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(107, 95);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(32, 32);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // nudMetricTo
            // 
            this.nudMetricTo.Location = new System.Drawing.Point(60, 69);
            this.nudMetricTo.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMetricTo.Name = "nudMetricTo";
            this.nudMetricTo.Size = new System.Drawing.Size(57, 20);
            this.nudMetricTo.TabIndex = 9;
            this.nudMetricTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMetricTo.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudMetricFrom
            // 
            this.nudMetricFrom.Location = new System.Drawing.Point(60, 19);
            this.nudMetricFrom.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMetricFrom.Name = "nudMetricFrom";
            this.nudMetricFrom.Size = new System.Drawing.Size(57, 20);
            this.nudMetricFrom.TabIndex = 8;
            this.nudMetricFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMetricFrom.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMetricFrom.ValueChanged += new System.EventHandler(this.nudMetricFrom_ValueChanged);
            // 
            // btnEditEdge
            // 
            this.btnEditEdge.Image = ((System.Drawing.Image)(resources.GetObject("btnEditEdge.Image")));
            this.btnEditEdge.Location = new System.Drawing.Point(31, 95);
            this.btnEditEdge.Name = "btnEditEdge";
            this.btnEditEdge.Size = new System.Drawing.Size(32, 32);
            this.btnEditEdge.TabIndex = 6;
            this.btnEditEdge.UseVisualStyleBackColor = true;
            this.btnEditEdge.Click += new System.EventHandler(this.btnEditEdge_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "<--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "-->";
            // 
            // cbxApexTo
            // 
            this.cbxApexTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxApexTo.FormattingEnabled = true;
            this.cbxApexTo.Location = new System.Drawing.Point(123, 45);
            this.cbxApexTo.Name = "cbxApexTo";
            this.cbxApexTo.Size = new System.Drawing.Size(48, 21);
            this.cbxApexTo.TabIndex = 1;
            this.cbxApexTo.SelectedIndexChanged += new System.EventHandler(this.cbxApexFrom_SelectedIndexChanged);
            // 
            // cbxApexFrom
            // 
            this.cbxApexFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxApexFrom.FormattingEnabled = true;
            this.cbxApexFrom.Location = new System.Drawing.Point(6, 45);
            this.cbxApexFrom.Name = "cbxApexFrom";
            this.cbxApexFrom.Size = new System.Drawing.Size(48, 21);
            this.cbxApexFrom.TabIndex = 0;
            this.cbxApexFrom.SelectedIndexChanged += new System.EventHandler(this.cbxApexFrom_SelectedIndexChanged);
            // 
            // ToolWindowEdges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(200, 155);
            this.Controls.Add(this.bdxEditEdge);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ToolWindowEdges";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование рёбер";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolWindowEdges_FormClosing);
            this.Shown += new System.EventHandler(this.ToolWindowEdges_Shown);
            this.bdxEditEdge.ResumeLayout(false);
            this.bdxEditEdge.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMetricTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMetricFrom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox bdxEditEdge;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.NumericUpDown nudMetricTo;
        private System.Windows.Forms.NumericUpDown nudMetricFrom;
        private System.Windows.Forms.Button btnEditEdge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxApexTo;
        private System.Windows.Forms.ComboBox cbxApexFrom;
    }
}