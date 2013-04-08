namespace midiplayer
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
         this.button1 = new System.Windows.Forms.CheckBox();
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.listBox1 = new System.Windows.Forms.ListBox();
         this.progressBar1 = new System.Windows.Forms.ProgressBar();
         this.trackBar1 = new System.Windows.Forms.TrackBar();
         this.timer2 = new System.Windows.Forms.Timer(this.components);
         this.button2 = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.button3 = new System.Windows.Forms.Button();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(13, 2);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 0;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         this.button1.CheckedChanged += new System.EventHandler(this.button1_CheckedChanged);
         // 
         // timer1
         // 
         this.timer1.Interval = 300;
         this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
         // 
         // listBox1
         // 
         this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.listBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.listBox1.FormattingEnabled = true;
         this.listBox1.ItemHeight = 14;
         this.listBox1.Location = new System.Drawing.Point(12, 287);
         this.listBox1.Name = "listBox1";
         this.listBox1.Size = new System.Drawing.Size(631, 116);
         this.listBox1.TabIndex = 1;
         // 
         // progressBar1
         // 
         this.progressBar1.Location = new System.Drawing.Point(12, 258);
         this.progressBar1.Name = "progressBar1";
         this.progressBar1.Size = new System.Drawing.Size(630, 23);
         this.progressBar1.TabIndex = 2;
         // 
         // trackBar1
         // 
         this.trackBar1.Location = new System.Drawing.Point(12, 58);
         this.trackBar1.Maximum = 1000;
         this.trackBar1.Name = "trackBar1";
         this.trackBar1.Size = new System.Drawing.Size(630, 45);
         this.trackBar1.TabIndex = 3;
         this.trackBar1.TickFrequency = 10;
         this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
         // 
         // timer2
         // 
         this.timer2.Interval = 1;
         this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
         // 
         // button2
         // 
         this.button2.Location = new System.Drawing.Point(130, 13);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(75, 23);
         this.button2.TabIndex = 4;
         this.button2.Text = "button2";
         this.button2.UseVisualStyleBackColor = true;
         this.button2.Click += new System.EventHandler(this.button2_Click);
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.button3, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 1);
         this.tableLayoutPanel1.Location = new System.Drawing.Point(72, 122);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.61644F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.38356F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(375, 73);
         this.tableLayoutPanel1.TabIndex = 5;
         // 
         // textBox1
         // 
         this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.textBox1.Location = new System.Drawing.Point(3, 3);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(181, 20);
         this.textBox1.TabIndex = 0;
         // 
         // button3
         // 
         this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.button3.Location = new System.Drawing.Point(187, 0);
         this.button3.Margin = new System.Windows.Forms.Padding(0);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(188, 26);
         this.button3.TabIndex = 1;
         this.button3.Text = "button3";
         this.button3.UseVisualStyleBackColor = true;
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.checkBox1.Location = new System.Drawing.Point(3, 29);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(181, 41);
         this.checkBox1.TabIndex = 2;
         this.checkBox1.Text = "checkBox1";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(655, 410);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Controls.Add(this.button2);
         this.Controls.Add(this.trackBar1);
         this.Controls.Add(this.progressBar1);
         this.Controls.Add(this.listBox1);
         this.Controls.Add(this.button1);
         this.DoubleBuffered = true;
         this.Name = "Form1";
         this.Text = "Form1";
         this.Load += new System.EventHandler(this.Form1_Load);
         this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox button1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Timer timer2;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.CheckBox checkBox1;
	}
}

