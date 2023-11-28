namespace Tetris
{
    partial class TetrisForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TetrisForm));
            gb_Info = new GroupBox();
            lb_score = new Label();
            gb_Info.SuspendLayout();
            SuspendLayout();
            // 
            // gb_Info
            // 
            gb_Info.Controls.Add(lb_score);
            gb_Info.Location = new Point(197, 12);
            gb_Info.Name = "gb_Info";
            gb_Info.Size = new Size(100, 50);
            gb_Info.TabIndex = 0;
            gb_Info.TabStop = false;
            gb_Info.Text = "Score";
            // 
            // lb_score
            // 
            lb_score.AutoSize = true;
            lb_score.Location = new Point(10, 22);
            lb_score.Name = "lb_score";
            lb_score.RightToLeft = RightToLeft.Yes;
            lb_score.Size = new Size(0, 15);
            lb_score.TabIndex = 0;
            lb_score.TextAlign = ContentAlignment.TopRight;
            // 
            // TetrisForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(309, 461);
            Controls.Add(gb_Info);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TetrisForm";
            Text = "Tetris";
            gb_Info.ResumeLayout(false);
            gb_Info.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gb_Info;
        private Label lb_score;
    }
}