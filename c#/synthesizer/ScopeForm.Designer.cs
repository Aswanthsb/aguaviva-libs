namespace midiplayer
{
   partial class ScopeForm
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
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.button3 = new System.Windows.Forms.Button();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.checkBox2 = new System.Windows.Forms.CheckBox();
         this.graphControl1 = new XOscillo.GraphControl();
         this.checkBox3 = new System.Windows.Forms.CheckBox();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(0, 126);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(17, 21);
         this.button1.TabIndex = 0;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(0, 195);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(17, 20);
         this.button2.TabIndex = 1;
         this.button2.Text = "button2";
         this.button2.UseVisualStyleBackColor = true;
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // button3
         // 
         this.button3.Location = new System.Drawing.Point(234, 10);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(44, 23);
         this.button3.TabIndex = 2;
         this.button3.Text = "Step";
         this.button3.UseVisualStyleBackColor = true;
         this.button3.Click += new System.EventHandler(this.button3_Click);
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.Location = new System.Drawing.Point(130, 14);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(55, 17);
         this.checkBox1.TabIndex = 3;
         this.checkBox1.Text = "trigger";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // checkBox2
         // 
         this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
         this.checkBox2.AutoSize = true;
         this.checkBox2.Location = new System.Drawing.Point(191, 10);
         this.checkBox2.Name = "checkBox2";
         this.checkBox2.Size = new System.Drawing.Size(37, 23);
         this.checkBox2.TabIndex = 4;
         this.checkBox2.Text = "Play";
         this.checkBox2.UseVisualStyleBackColor = true;
         this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
         // 
         // graphControl1
         // 
         this.graphControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.graphControl1.Location = new System.Drawing.Point(36, 39);
         this.graphControl1.Name = "graphControl1";
         this.graphControl1.Size = new System.Drawing.Size(694, 303);
         this.graphControl1.TabIndex = 5;
         // 
         // checkBox3
         // 
         this.checkBox3.AutoSize = true;
         this.checkBox3.Location = new System.Drawing.Point(297, 14);
         this.checkBox3.Name = "checkBox3";
         this.checkBox3.Size = new System.Drawing.Size(35, 17);
         this.checkBox3.TabIndex = 6;
         this.checkBox3.Text = "fft";
         this.checkBox3.UseVisualStyleBackColor = true;
         this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
         // 
         // Scope
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(738, 354);
         this.Controls.Add(this.checkBox3);
         this.Controls.Add(this.graphControl1);
         this.Controls.Add(this.checkBox2);
         this.Controls.Add(this.checkBox1);
         this.Controls.Add(this.button3);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.DoubleBuffered = true;
         this.Name = "Scope";
         this.Text = "Scope";
         this.Load += new System.EventHandler(this.Scope_Load);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Scope_Paint);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.CheckBox checkBox1;
      private System.Windows.Forms.CheckBox checkBox2;
      private XOscillo.GraphControl graphControl1;
      private System.Windows.Forms.CheckBox checkBox3;
   }
}