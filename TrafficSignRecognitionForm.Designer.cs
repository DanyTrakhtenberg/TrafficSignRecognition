namespace TrafficSignRecognition
{
   partial class TrafficSignRecognitionForm
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
          this.splitContainer1 = new System.Windows.Forms.SplitContainer();
          this.panel1 = new System.Windows.Forms.Panel();
          this.label1 = new System.Windows.Forms.Label();
          this.imageBox1 = new Emgu.CV.UI.ImageBox();
          this.panel2 = new System.Windows.Forms.Panel();
          this.button2 = new System.Windows.Forms.Button();
          this.processTimeLabel = new System.Windows.Forms.Label();
          this.informationLabel = new System.Windows.Forms.Label();
          this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
          this.splitContainer1.Panel1.SuspendLayout();
          this.splitContainer1.Panel2.SuspendLayout();
          this.splitContainer1.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
          this.panel2.SuspendLayout();
          this.SuspendLayout();
          // 
          // splitContainer1
          // 
          this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splitContainer1.Location = new System.Drawing.Point(0, 0);
          this.splitContainer1.Name = "splitContainer1";
          // 
          // splitContainer1.Panel1
          // 
          this.splitContainer1.Panel1.Controls.Add(this.panel1);
          // 
          // splitContainer1.Panel2
          // 
          this.splitContainer1.Panel2.Controls.Add(this.label1);
          this.splitContainer1.Panel2.Controls.Add(this.imageBox1);
          this.splitContainer1.Panel2.Controls.Add(this.panel2);
          this.splitContainer1.Size = new System.Drawing.Size(786, 515);
          this.splitContainer1.SplitterDistance = 179;
          this.splitContainer1.TabIndex = 0;
          // 
          // panel1
          // 
          this.panel1.AutoScroll = true;
          this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panel1.Location = new System.Drawing.Point(0, 0);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(179, 515);
          this.panel1.TabIndex = 0;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.BackColor = System.Drawing.Color.AliceBlue;
          this.label1.Location = new System.Drawing.Point(8, 496);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(391, 13);
          this.label1.TabIndex = 5;
          this.label1.Text = "Sytem to recognize traffic signs. Made by Noam On and Dany Trahtenberg, 2012.";
          // 
          // imageBox1
          // 
          this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
          this.imageBox1.Location = new System.Drawing.Point(3, 42);
          this.imageBox1.Name = "imageBox1";
          this.imageBox1.Size = new System.Drawing.Size(597, 451);
          this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
          this.imageBox1.TabIndex = 4;
          this.imageBox1.TabStop = false;
          // 
          // panel2
          // 
          this.panel2.Controls.Add(this.button2);
          this.panel2.Controls.Add(this.processTimeLabel);
          this.panel2.Controls.Add(this.informationLabel);
          this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
          this.panel2.Location = new System.Drawing.Point(0, 0);
          this.panel2.Name = "panel2";
          this.panel2.Size = new System.Drawing.Size(603, 81);
          this.panel2.TabIndex = 3;
          // 
          // button2
          // 
          this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
          this.button2.Location = new System.Drawing.Point(0, 3);
          this.button2.Name = "button2";
          this.button2.Size = new System.Drawing.Size(600, 37);
          this.button2.TabIndex = 5;
          this.button2.Text = "Record";
          this.button2.UseVisualStyleBackColor = true;
          this.button2.Click += new System.EventHandler(this.button2_Click);
          // 
          // processTimeLabel
          // 
          this.processTimeLabel.AutoSize = true;
          this.processTimeLabel.Location = new System.Drawing.Point(33, 55);
          this.processTimeLabel.Name = "processTimeLabel";
          this.processTimeLabel.Size = new System.Drawing.Size(0, 13);
          this.processTimeLabel.TabIndex = 4;
          // 
          // informationLabel
          // 
          this.informationLabel.AutoSize = true;
          this.informationLabel.Location = new System.Drawing.Point(27, 55);
          this.informationLabel.Name = "informationLabel";
          this.informationLabel.Size = new System.Drawing.Size(0, 13);
          this.informationLabel.TabIndex = 3;
          // 
          // openFileDialog1
          // 
          this.openFileDialog1.FileName = "openFileDialog1";
          // 
          // TrafficSignRecognitionForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(786, 515);
          this.Controls.Add(this.splitContainer1);
          this.Name = "TrafficSignRecognitionForm";
          this.Text = "Traffic Sign Recognition";
          this.splitContainer1.Panel1.ResumeLayout(false);
          this.splitContainer1.Panel2.ResumeLayout(false);
          this.splitContainer1.Panel2.PerformLayout();
          this.splitContainer1.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
          this.panel2.ResumeLayout(false);
          this.panel2.PerformLayout();
          this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.Panel panel1;
      private Emgu.CV.UI.ImageBox imageBox1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.OpenFileDialog openFileDialog1;
      private System.Windows.Forms.Label informationLabel;
      private System.Windows.Forms.Label processTimeLabel;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Label label1;
   }
}