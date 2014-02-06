namespace Oasis_v1._1
{
    partial class fmConsole
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
            this.messages1 = new Pan.Utilities.Messages();
            this.SuspendLayout();
            // 
            // messages1
            // 
            this.messages1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messages1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messages1.ForeColor = System.Drawing.Color.Blue;
            this.messages1.Location = new System.Drawing.Point(0, 0);
            this.messages1.Name = "messages1";
            this.messages1.Size = new System.Drawing.Size(946, 681);
            this.messages1.TabIndex = 0;
            // 
            // fmConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 681);
            this.Controls.Add(this.messages1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "fmConsole";
            this.Text = "Console";
            this.ResumeLayout(false);

        }

        #endregion

        private Pan.Utilities.Messages messages1;
    }
}