namespace rotozoomer
{
   partial class Form1
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
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.trackBar1 = new System.Windows.Forms.TrackBar();
         this.trackBar2 = new System.Windows.Forms.TrackBar();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
         this.SuspendLayout();
         // 
         // timer1
         // 
         this.timer1.Enabled = true;
         this.timer1.Interval = 10;
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // textBox1
         // 
         this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox1.Location = new System.Drawing.Point(12, 182);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(637, 177);
         this.textBox1.TabIndex = 0;
         // 
         // trackBar1
         // 
         this.trackBar1.Location = new System.Drawing.Point(391, 134);
         this.trackBar1.Name = "trackBar1";
         this.trackBar1.Size = new System.Drawing.Size(236, 42);
         this.trackBar1.TabIndex = 1;
         this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
         // 
         // trackBar2
         // 
         this.trackBar2.Location = new System.Drawing.Point(391, 86);
         this.trackBar2.Maximum = 255;
         this.trackBar2.Name = "trackBar2";
         this.trackBar2.Size = new System.Drawing.Size(236, 42);
         this.trackBar2.TabIndex = 2;
         this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.Location = new System.Drawing.Point(420, 51);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(53, 17);
         this.checkBox1.TabIndex = 3;
         this.checkBox1.Text = "rotate";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(661, 371);
         this.Controls.Add(this.checkBox1);
         this.Controls.Add(this.trackBar2);
         this.Controls.Add(this.trackBar1);
         this.Controls.Add(this.textBox1);
         this.DoubleBuffered = true;
         this.Name = "Form1";
         this.Text = "Form1";
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.TrackBar trackBar1;
      private System.Windows.Forms.TrackBar trackBar2;
      private System.Windows.Forms.CheckBox checkBox1;
   }
}

