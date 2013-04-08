namespace WindowsFormsApplication1
{
   partial class Form1 : FormDownloader
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
         this.urlBox = new System.Windows.Forms.TextBox();
         this.progressBar1 = new System.Windows.Forms.ProgressBar();
         this.button1 = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.progressBar2 = new System.Windows.Forms.ProgressBar();
         this.label2 = new System.Windows.Forms.Label();
         this.button2 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // urlBox
         // 
         this.urlBox.Location = new System.Drawing.Point(12, 12);
         this.urlBox.Name = "urlBox";
         this.urlBox.Size = new System.Drawing.Size(232, 20);
         this.urlBox.TabIndex = 0;
         this.urlBox.Text = "http://mschnlnine.vo.llnwd.net/d1/pdc08/PPTX/BB01.pptx";
         // 
         // progressBar1
         // 
         this.progressBar1.Location = new System.Drawing.Point(12, 76);
         this.progressBar1.Name = "progressBar1";
         this.progressBar1.Size = new System.Drawing.Size(268, 29);
         this.progressBar1.TabIndex = 1;
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(250, 12);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(30, 20);
         this.button1.TabIndex = 2;
         this.button1.Text = "go";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 60);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(35, 13);
         this.label1.TabIndex = 3;
         this.label1.Text = "label1";
         // 
         // progressBar2
         // 
         this.progressBar2.Location = new System.Drawing.Point(89, 175);
         this.progressBar2.Name = "progressBar2";
         this.progressBar2.Size = new System.Drawing.Size(190, 31);
         this.progressBar2.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(89, 156);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(35, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "label2";
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(15, 230);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(90, 31);
         this.button2.TabIndex = 6;
         this.button2.Text = "button2";
         this.button2.UseVisualStyleBackColor = true;
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(292, 273);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.progressBar2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.progressBar1);
         this.Controls.Add(this.urlBox);
         this.Name = "Form1";
         this.Text = "Form1";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox urlBox;
      private System.Windows.Forms.ProgressBar progressBar1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ProgressBar progressBar2;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button button2;
   }
}

