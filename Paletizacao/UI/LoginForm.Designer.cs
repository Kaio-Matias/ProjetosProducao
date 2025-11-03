using System.Drawing;
using System.Windows.Forms;
using Paletizacao.UI.Controls;

namespace Paletizacao.UI
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlMain;
        private PictureBox pictureBoxLogo;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private RoundedTextBox txtMatricula;
        private RoundedButton btnEntrar;
        private LinkLabel linkLabelRegistreSe;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.linkLabelRegistreSe = new System.Windows.Forms.LinkLabel();
            this.btnEntrar = new Paletizacao.UI.Controls.RoundedButton();
            this.txtMatricula = new Paletizacao.UI.Controls.RoundedTextBox();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.linkLabelRegistreSe);
            this.pnlMain.Controls.Add(this.btnEntrar);
            this.pnlMain.Controls.Add(this.txtMatricula);
            this.pnlMain.Controls.Add(this.lblSubtitulo);
            this.pnlMain.Controls.Add(this.lblTitulo);
            this.pnlMain.Controls.Add(this.pictureBoxLogo);
            this.pnlMain.Location = new System.Drawing.Point(194, 76);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(400, 480);
            this.pnlMain.TabIndex = 0;
            // 
            // linkLabelRegistreSe
            // 
            // MUDANÇA DE COR
            this.linkLabelRegistreSe.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(97)))), ((int)(((byte)(10)))));
            this.linkLabelRegistreSe.AutoSize = true;
            this.linkLabelRegistreSe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.linkLabelRegistreSe.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.linkLabelRegistreSe.Location = new System.Drawing.Point(94, 436);
            this.linkLabelRegistreSe.Name = "linkLabelRegistreSe";
            this.linkLabelRegistreSe.Size = new System.Drawing.Size(212, 15);
            this.linkLabelRegistreSe.TabIndex = 2;
            this.linkLabelRegistreSe.TabStop = true;
            this.linkLabelRegistreSe.Text = "Não tem uma conta? Clique para registrar";
            this.linkLabelRegistreSe.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRegistreSe_LinkClicked);
            // 
            // btnEntrar
            // 
            // MUDANÇA DE COR
            this.btnEntrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(97)))), ((int)(((byte)(10)))));
            this.btnEntrar.FlatAppearance.BorderSize = 0;
            this.btnEntrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnEntrar.ForeColor = System.Drawing.Color.White;
            this.btnEntrar.Location = new System.Drawing.Point(50, 360);
            this.btnEntrar.Name = "btnEntrar";
            this.btnEntrar.Size = new System.Drawing.Size(300, 50);
            this.btnEntrar.TabIndex = 1;
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.UseVisualStyleBackColor = false;
            this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);
            // 
            // txtMatricula
            // 
            this.txtMatricula.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtMatricula.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMatricula.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.txtMatricula.Location = new System.Drawing.Point(50, 280);
            this.txtMatricula.Name = "txtMatricula";
            this.txtMatricula.Size = new System.Drawing.Size(300, 36);
            this.txtMatricula.TabIndex = 0;
            this.txtMatricula.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSubtitulo.Location = new System.Drawing.Point(0, 230);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(400, 23);
            this.lblSubtitulo.TabIndex = 2;
            this.lblSubtitulo.Text = "Insira sua matrícula para continuar";
            this.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.lblTitulo.Location = new System.Drawing.Point(0, 180);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(400, 40);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Controle de Produção";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.ImageLocation = "https://lirp.cdn-website.com/6d19be99/dms3rep/multi/opt/Logo-Valedourado-390w.png";
            this.pictureBoxLogo.Location = new System.Drawing.Point(100, 30);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(200, 120);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnEntrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(784, 611);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(800, 650);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login - Controle de Paletização";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
        }
    }
}