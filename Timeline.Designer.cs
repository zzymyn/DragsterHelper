namespace DragsterHelper
{
    partial class Timeline
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
            this.components = new System.ComponentModel.Container();
            this.m_Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // m_Timer
            // 
            this.m_Timer.Enabled = true;
            this.m_Timer.Interval = 1;
            this.m_Timer.Tick += new System.EventHandler(this.m_Timer_Tick);
            // 
            // Timeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 793);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Timeline";
            this.Text = "Timeline";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Timeline_FormClosing);
            this.Load += new System.EventHandler(this.Timeline_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Timeline_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer m_Timer;
    }
}

