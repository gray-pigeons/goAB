﻿
namespace goABClient
{
    partial class LoginPage
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.loginGroupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_login = new System.Windows.Forms.Button();
            this.txtBox_password = new System.Windows.Forms.TextBox();
            this.txtBox_username = new System.Windows.Forms.TextBox();
            this.loginGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginGroupBox1
            // 
            this.loginGroupBox1.Controls.Add(this.label2);
            this.loginGroupBox1.Controls.Add(this.label1);
            this.loginGroupBox1.Controls.Add(this.btn_login);
            this.loginGroupBox1.Controls.Add(this.txtBox_password);
            this.loginGroupBox1.Controls.Add(this.txtBox_username);
            this.loginGroupBox1.Location = new System.Drawing.Point(621, 124);
            this.loginGroupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loginGroupBox1.Name = "loginGroupBox1";
            this.loginGroupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loginGroupBox1.Size = new System.Drawing.Size(293, 245);
            this.loginGroupBox1.TabIndex = 0;
            this.loginGroupBox1.TabStop = false;
            this.loginGroupBox1.Text = "登录界面";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "密  码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户名：";
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(105, 182);
            this.btn_login.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(100, 29);
            this.btn_login.TabIndex = 2;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.Login_Click);
            // 
            // txtBox_password
            // 
            this.txtBox_password.Location = new System.Drawing.Point(88, 115);
            this.txtBox_password.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBox_password.Name = "txtBox_password";
            this.txtBox_password.Size = new System.Drawing.Size(159, 25);
            this.txtBox_password.TabIndex = 1;
            // 
            // txtBox_username
            // 
            this.txtBox_username.Location = new System.Drawing.Point(88, 56);
            this.txtBox_username.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBox_username.Name = "txtBox_username";
            this.txtBox_username.Size = new System.Drawing.Size(159, 25);
            this.txtBox_username.TabIndex = 0;
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.loginGroupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LoginPage";
            this.Text = "goABClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.loginGroupBox1.ResumeLayout(false);
            this.loginGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox loginGroupBox1;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox txtBox_password;
        private System.Windows.Forms.TextBox txtBox_username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

