namespace midiplayer
{
   partial class Form2
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
         this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.cacasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.contextMenuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // contextMenuStrip1
         // 
         this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cacasToolStripMenuItem});
         this.contextMenuStrip1.Name = "contextMenuStrip1";
         this.contextMenuStrip1.Size = new System.Drawing.Size(104, 26);
         // 
         // cacasToolStripMenuItem
         // 
         this.cacasToolStripMenuItem.Name = "cacasToolStripMenuItem";
         this.cacasToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
         this.cacasToolStripMenuItem.Text = "cacas";
         // 
         // Form2
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 262);
         this.Name = "Form2";
         this.Text = "Form2";
         this.Load += new System.EventHandler(this.Form2_Load);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form2_Paint);
         this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseClick);
         this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseDown);
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
         this.contextMenuStrip1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
      private System.Windows.Forms.ToolStripMenuItem cacasToolStripMenuItem;


   }
}