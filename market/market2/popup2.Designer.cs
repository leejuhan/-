namespace market2
{
    partial class popup2
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
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox18
            // 
            this.textBox18.Font = new System.Drawing.Font("굴림", 18F);
            this.textBox18.Location = new System.Drawing.Point(0, -1);
            this.textBox18.Multiline = true;
            this.textBox18.Name = "textBox18";
            this.textBox18.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox18.Size = new System.Drawing.Size(624, 612);
            this.textBox18.TabIndex = 64;
            this.textBox18.TextChanged += new System.EventHandler(this.textBox18_TextChanged);
            // 
            // popup2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 610);
            this.Controls.Add(this.textBox18);
            this.Name = "popup2";
            this.Text = "확대하면";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox18;
    }
}