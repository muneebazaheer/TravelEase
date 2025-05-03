using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelEaseForms
{
    partial class SP_Registration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SP_Registration));
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            btnRegister = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panelBody = new Panel();
            panelHeader = new Panel();
            pictureBox1 = new PictureBox();
            panelBody.SuspendLayout();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Montserrat Medium", 10.2F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(51, 51, 51);
            label3.Location = new Point(46, 20);
            label3.Name = "label3";
            label3.Size = new Size(64, 27);
            label3.TabIndex = 2;
            label3.Text = "Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Montserrat Medium", 10.2F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(51, 51, 51);
            label4.Location = new Point(46, 67);
            label4.Name = "label4";
            label4.Size = new Size(61, 27);
            label4.TabIndex = 3;
            label4.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Montserrat Medium", 10.2F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(51, 51, 51);
            label5.Location = new Point(46, 111);
            label5.Name = "label5";
            label5.Size = new Size(83, 27);
            label5.TabIndex = 4;
            label5.Text = "Address";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Montserrat Medium", 10.2F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(51, 51, 51);
            label6.Location = new Point(46, 162);
            label6.Name = "label6";
            label6.Size = new Size(95, 27);
            label6.TabIndex = 5;
            label6.Text = "City Code";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Montserrat Medium", 10.2F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(51, 51, 51);
            label7.Location = new Point(46, 210);
            label7.Name = "label7";
            label7.Size = new Size(81, 27);
            label7.TabIndex = 6;
            label7.Text = "Contact";
            // 
            // btnRegister
            // 
            btnRegister.Font = new Font("Montserrat Medium", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = Color.FromArgb(51, 51, 51);
            btnRegister.Location = new Point(208, 261);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(94, 29);
            btnRegister.TabIndex = 7;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(166, 20);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(272, 27);
            textBox1.TabIndex = 8;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(166, 67);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(272, 27);
            textBox2.TabIndex = 9;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(166, 210);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(272, 27);
            textBox3.TabIndex = 10;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(166, 111);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(272, 27);
            textBox4.TabIndex = 11;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(166, 162);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(272, 27);
            textBox5.TabIndex = 12;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Montserrat ExtraBold", 16.2F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(51, 51, 51);
            label2.Location = new Point(177, 56);
            label2.Name = "label2";
            label2.Size = new Size(504, 44);
            label2.TabIndex = 1;
            label2.Text = "Join as a Service Provider today!";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Montserrat ExtraBold", 16.2F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(51, 51, 51);
            label1.Location = new Point(117, 12);
            label1.Name = "label1";
            label1.Size = new Size(645, 44);
            label1.TabIndex = 0;
            label1.Text = "Be the gateway to unforgettable journeys";
            label1.Click += label1_Click_1;
            // 
            // panelBody
            // 
            panelBody.Controls.Add(textBox5);
            panelBody.Controls.Add(textBox4);
            panelBody.Controls.Add(textBox3);
            panelBody.Controls.Add(textBox2);
            panelBody.Controls.Add(textBox1);
            panelBody.Controls.Add(btnRegister);
            panelBody.Controls.Add(label7);
            panelBody.Controls.Add(label6);
            panelBody.Controls.Add(label5);
            panelBody.Controls.Add(label4);
            panelBody.Controls.Add(label3);
            panelBody.Location = new Point(157, 130);
            panelBody.Name = "panelBody";
            panelBody.Size = new Size(483, 313);
            panelBody.TabIndex = 13;
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(pictureBox1);
            panelHeader.Controls.Add(label2);
            panelHeader.Controls.Add(label1);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(800, 125);
            panelHeader.TabIndex = 14;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(108, 105);
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 241, 230);
            ClientSize = new Size(800, 450);
            Controls.Add(panelHeader);
            Controls.Add(panelBody);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            panelBody.ResumeLayout(false);
            panelBody.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Button btnRegister;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Label label2;
        private Label label1;
        private Panel panelBody;
        private Panel panelHeader;
        private PictureBox pictureBox1;
    }
}
