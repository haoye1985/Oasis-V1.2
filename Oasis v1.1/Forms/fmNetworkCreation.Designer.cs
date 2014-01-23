namespace Oasis_v1._1
{
    partial class fmNetworkCreation
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNetworkID = new System.Windows.Forms.TextBox();
            this.lblNetworkID = new System.Windows.Forms.Label();
            this.cbNetworkType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNetworkName = new System.Windows.Forms.Label();
            this.txtNetworkName = new System.Windows.Forms.TextBox();
            this.btnNetworkCreate = new System.Windows.Forms.Button();
            this.btnNetworkOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbTorlerance = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMapMode = new System.Windows.Forms.Label();
            this.CheckBoxMap = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNetworkID);
            this.groupBox1.Controls.Add(this.lblNetworkID);
            this.groupBox1.Controls.Add(this.cbNetworkType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblNetworkName);
            this.groupBox1.Controls.Add(this.txtNetworkName);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 197);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network Creation";
            // 
            // txtNetworkID
            // 
            this.txtNetworkID.Location = new System.Drawing.Point(195, 43);
            this.txtNetworkID.Name = "txtNetworkID";
            this.txtNetworkID.Size = new System.Drawing.Size(154, 28);
            this.txtNetworkID.TabIndex = 6;
            // 
            // lblNetworkID
            // 
            this.lblNetworkID.AutoSize = true;
            this.lblNetworkID.Location = new System.Drawing.Point(33, 46);
            this.lblNetworkID.Name = "lblNetworkID";
            this.lblNetworkID.Size = new System.Drawing.Size(103, 21);
            this.lblNetworkID.TabIndex = 5;
            this.lblNetworkID.Text = "Network ID:";
            // 
            // cbNetworkType
            // 
            this.cbNetworkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNetworkType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbNetworkType.FormattingEnabled = true;
            this.cbNetworkType.Items.AddRange(new object[] {
            "Road",
            "Rail",
            "Tube",
            "Other"});
            this.cbNetworkType.Location = new System.Drawing.Point(195, 128);
            this.cbNetworkType.Name = "cbNetworkType";
            this.cbNetworkType.Size = new System.Drawing.Size(154, 29);
            this.cbNetworkType.TabIndex = 4;
            this.cbNetworkType.SelectedIndexChanged += new System.EventHandler(this.cbNetworkType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Network Type:";
            // 
            // lblNetworkName
            // 
            this.lblNetworkName.AutoSize = true;
            this.lblNetworkName.Location = new System.Drawing.Point(33, 87);
            this.lblNetworkName.Name = "lblNetworkName";
            this.lblNetworkName.Size = new System.Drawing.Size(138, 21);
            this.lblNetworkName.TabIndex = 1;
            this.lblNetworkName.Text = "Network Name: ";
            // 
            // txtNetworkName
            // 
            this.txtNetworkName.Location = new System.Drawing.Point(195, 84);
            this.txtNetworkName.Name = "txtNetworkName";
            this.txtNetworkName.Size = new System.Drawing.Size(154, 28);
            this.txtNetworkName.TabIndex = 0;
            // 
            // btnNetworkCreate
            // 
            this.btnNetworkCreate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNetworkCreate.Location = new System.Drawing.Point(13, 403);
            this.btnNetworkCreate.Name = "btnNetworkCreate";
            this.btnNetworkCreate.Size = new System.Drawing.Size(194, 36);
            this.btnNetworkCreate.TabIndex = 1;
            this.btnNetworkCreate.Text = "Create New Network";
            this.btnNetworkCreate.UseVisualStyleBackColor = true;
            this.btnNetworkCreate.Click += new System.EventHandler(this.btnNetworkOK_Click);
            // 
            // btnNetworkOK
            // 
            this.btnNetworkOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNetworkOK.Location = new System.Drawing.Point(303, 403);
            this.btnNetworkOK.Name = "btnNetworkOK";
            this.btnNetworkOK.Size = new System.Drawing.Size(98, 36);
            this.btnNetworkOK.TabIndex = 2;
            this.btnNetworkOK.Text = "OK";
            this.btnNetworkOK.UseVisualStyleBackColor = true;
            this.btnNetworkOK.Click += new System.EventHandler(this.btnNetworkCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CheckBoxMap);
            this.groupBox2.Controls.Add(this.lblMapMode);
            this.groupBox2.Controls.Add(this.cbTorlerance);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 145);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parameter Setting";
            // 
            // cbTorlerance
            // 
            this.cbTorlerance.FormattingEnabled = true;
            this.cbTorlerance.Items.AddRange(new object[] {
            "5(small)",
            "10(medium)",
            "20(large)"});
            this.cbTorlerance.Location = new System.Drawing.Point(194, 38);
            this.cbTorlerance.Name = "cbTorlerance";
            this.cbTorlerance.Size = new System.Drawing.Size(154, 29);
            this.cbTorlerance.TabIndex = 1;
            this.cbTorlerance.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Snap Tolerance";
            // 
            // lblMapMode
            // 
            this.lblMapMode.AutoSize = true;
            this.lblMapMode.Location = new System.Drawing.Point(36, 97);
            this.lblMapMode.Name = "lblMapMode";
            this.lblMapMode.Size = new System.Drawing.Size(100, 21);
            this.lblMapMode.TabIndex = 2;
            this.lblMapMode.Text = "Map Mode:";
            // 
            // CheckBoxMap
            // 
            this.CheckBoxMap.AutoSize = true;
            this.CheckBoxMap.Location = new System.Drawing.Point(194, 97);
            this.CheckBoxMap.Name = "CheckBoxMap";
            this.CheckBoxMap.Size = new System.Drawing.Size(22, 21);
            this.CheckBoxMap.TabIndex = 3;
            this.CheckBoxMap.UseVisualStyleBackColor = true;
            this.CheckBoxMap.CheckedChanged += new System.EventHandler(this.CheckBoxMap_CheckedChanged);
            // 
            // fmNetworkCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 460);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNetworkCreate);
            this.Controls.Add(this.btnNetworkOK);
            this.Name = "fmNetworkCreation";
            this.Text = "NetworkCreation";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbNetworkType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNetworkName;
        private System.Windows.Forms.TextBox txtNetworkName;
        private System.Windows.Forms.Button btnNetworkCreate;
        private System.Windows.Forms.Button btnNetworkOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbTorlerance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNetworkID;
        private System.Windows.Forms.Label lblNetworkID;
        private System.Windows.Forms.CheckBox CheckBoxMap;
        private System.Windows.Forms.Label lblMapMode;
    }
}