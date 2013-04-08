namespace midiplayer
{
   partial class PatternEditor
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
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.listView1 = new WindowsApplication1.SMK_EditListView();
         this.button1 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.Location = new System.Drawing.Point(0, 0);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(45, 17);
         this.checkBox1.TabIndex = 1;
         this.checkBox1.Text = "play";
         this.checkBox1.UseVisualStyleBackColor = true;
         this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
         // 
         // timer1
         // 
         this.timer1.Interval = 500;
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // listView1
         // 
         this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.listView1.FullRowSelect = true;
         this.listView1.GridLines = true;
         this.listView1.Location = new System.Drawing.Point(0, 79);
         this.listView1.Name = "listView1";
         this.listView1.Size = new System.Drawing.Size(611, 273);
         this.listView1.TabIndex = 0;
         this.listView1.UseCompatibleStateImageBehavior = false;
         this.listView1.View = System.Windows.Forms.View.Details;
         this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(223, 0);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(136, 25);
         this.button1.TabIndex = 2;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // PatternEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(611, 352);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.listView1);
         this.Controls.Add(this.checkBox1);
         this.Name = "PatternEditor";
         this.Text = "Form2";
         this.Load += new System.EventHandler(this.PatternEditor_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.CheckBox checkBox1;
      private System.Windows.Forms.Timer timer1;
      private WindowsApplication1.SMK_EditListView listView1;
      private System.Windows.Forms.Button button1;
   }
}