﻿namespace test
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
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.SuspendLayout();
         // 
         // textBox1
         // 
         this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox1.Location = new System.Drawing.Point(0, 0);
         this.textBox1.Multiline = true;
         this.textBox1.Name = "textBox1";
         this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.textBox1.Size = new System.Drawing.Size(1471, 722);
         this.textBox1.TabIndex = 0;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.Transparent;
         this.panel1.Location = new System.Drawing.Point(507, 586);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(161, 83);
         this.panel1.TabIndex = 1;
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.Transparent;
         this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel2.Location = new System.Drawing.Point(759, 586);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(528, 98);
         this.panel2.TabIndex = 2;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1471, 722);
         this.Controls.Add(this.textBox1);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "Form1";
         this.Text = "Form1";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
         this.Load += new System.EventHandler(this.Form1_Load);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Panel panel2;
   }
}

