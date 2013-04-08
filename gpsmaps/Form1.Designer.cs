namespace gpsmaps
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
         this.trackBar1 = new System.Windows.Forms.TrackBar();
         this.label1 = new System.Windows.Forms.Label();
         this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
         this.label2 = new System.Windows.Forms.Label();
         this.Logging = new System.Windows.Forms.CheckBox();
         this.button1 = new System.Windows.Forms.Button();
         this.graph1 = new gpsmaps.Graph();
         this.pictureBox1 = new gpsmaps.PictureBoxEx();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
         this.SuspendLayout();
         // 
         // trackBar1
         // 
         this.trackBar1.Location = new System.Drawing.Point(26, 16);
         this.trackBar1.Maximum = 18;
         this.trackBar1.Name = "trackBar1";
         this.trackBar1.Size = new System.Drawing.Size(307, 45);
         this.trackBar1.TabIndex = 0;
         this.trackBar1.Value = 14;
         this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(381, 28);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(35, 13);
         this.label1.TabIndex = 3;
         this.label1.Text = "label1";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(507, 28);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(35, 13);
         this.label2.TabIndex = 4;
         this.label2.Text = "label2";
         // 
         // Logging
         // 
         this.Logging.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
         this.Logging.AutoSize = true;
         this.Logging.Location = new System.Drawing.Point(602, 24);
         this.Logging.Name = "Logging";
         this.Logging.Size = new System.Drawing.Size(64, 17);
         this.Logging.TabIndex = 5;
         this.Logging.Text = "Logging";
         this.Logging.UseVisualStyleBackColor = true;
         this.Logging.CheckedChanged += new System.EventHandler(this.Logging_CheckedChanged);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(684, 20);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 6;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // graph1
         // 
         this.graph1.Location = new System.Drawing.Point(12, 418);
         this.graph1.Name = "graph1";
         this.graph1.Size = new System.Drawing.Size(913, 156);
         this.graph1.TabIndex = 7;
         this.graph1.GPSMove += new gpsmaps.Graph.GPSMoveHandler(this.graph1_GPSMove_1);
         // 
         // pictureBox1
         // 
         this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.pictureBox1.Location = new System.Drawing.Point(12, 67);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(913, 324);
         this.pictureBox1.TabIndex = 2;
         this.pictureBox1.TabStop = false;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(937, 586);
         this.Controls.Add(this.graph1);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.Logging);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(this.trackBar1);
         this.Name = "Form1";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

     }

      #endregion

      private System.Windows.Forms.TrackBar trackBar1;
      private System.Windows.Forms.Label label1;
      private System.IO.Ports.SerialPort serialPort1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.CheckBox Logging;
      private System.Windows.Forms.Button button1;
      private PictureBoxEx pictureBox1;
      private Graph graph1;
   }
}

