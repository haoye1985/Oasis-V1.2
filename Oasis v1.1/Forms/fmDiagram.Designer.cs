namespace Oasis_v1._1
{
    partial class fmDiagram
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
            this.pDiagram1 = new Pan.Diagrammer.pDiagram();
            this.SuspendLayout();
            // 
            // pDiagram1
            // 
            this.pDiagram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDiagram1.EditToolsEnabled = true;
            this.pDiagram1.Location = new System.Drawing.Point(0, 0);
            this.pDiagram1.Margin = new System.Windows.Forms.Padding(4);
            this.pDiagram1.Name = "pDiagram1";
            this.pDiagram1.Size = new System.Drawing.Size(906, 671);
            this.pDiagram1.TabIndex = 0;
            // 
            // fmDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 671);
            this.Controls.Add(this.pDiagram1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "fmDiagram";
            this.Text = "Diagram";
            this.ResumeLayout(false);

        }

        #endregion

        public Pan.Diagrammer.pDiagram pDiagram1;
    }
}