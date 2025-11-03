using System.Drawing;
using System.Windows.Forms;
using Paletizacao.UI.Controls;

namespace Paletizacao.UI
{
    partial class PaletizacaoForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlMain;
        private GroupBox grpProduto;
        private RoundedTextBox txtProduto;
        private GroupBox grpCodigoBarras;
        private RoundedTextBox txtCodigoDeBarras;
        private CheckBox chkBloqueio;
        private DataGridView dgvPaletes;
        private RoundedButton btnVoltar;
        private Label lblTotalProduzido;
        private CheckBox chkUltimoPalete;
        private NumericUpDown numQtdeUltimoPalete;
        private CheckedListBox chklistBloqueio;
        private Label lblInfoOp;
        private TableLayoutPanel tableLayoutPanelTop;
        private Panel panelOptions;
        private Panel panelHeader;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.dgvPaletes = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.grpCodigoBarras = new System.Windows.Forms.GroupBox();
            this.txtCodigoDeBarras = new Paletizacao.UI.Controls.RoundedTextBox();
            this.grpProduto = new System.Windows.Forms.GroupBox();
            this.txtProduto = new Paletizacao.UI.Controls.RoundedTextBox();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.chklistBloqueio = new System.Windows.Forms.CheckedListBox();
            this.numQtdeUltimoPalete = new System.Windows.Forms.NumericUpDown();
            this.chkUltimoPalete = new System.Windows.Forms.CheckBox();
            this.chkBloqueio = new System.Windows.Forms.CheckBox();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTotalProduzido = new System.Windows.Forms.Label();
            this.lblInfoOp = new System.Windows.Forms.Label();
            this.btnVoltar = new Paletizacao.UI.Controls.RoundedButton();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaletes)).BeginInit();
            this.tableLayoutPanelTop.SuspendLayout();
            this.grpCodigoBarras.SuspendLayout();
            this.grpProduto.SuspendLayout();
            this.panelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtdeUltimoPalete)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.dgvPaletes);
            this.pnlMain.Controls.Add(this.tableLayoutPanelTop);
            this.pnlMain.Controls.Add(this.panelHeader);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(20, 20);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1144, 661);
            this.pnlMain.TabIndex = 0;
            // 
            // dgvPaletes
            // 
            this.dgvPaletes.AllowUserToAddRows = false;
            this.dgvPaletes.AllowUserToDeleteRows = false;
            this.dgvPaletes.AllowUserToResizeRows = false;
            this.dgvPaletes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPaletes.BackgroundColor = System.Drawing.Color.White;
            this.dgvPaletes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPaletes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPaletes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            // MUDANÇA DE COR
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(97)))), ((int)(((byte)(10)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(97)))), ((int)(((byte)(10)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPaletes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPaletes.ColumnHeadersHeight = 40;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            // MUDANÇA DE COR
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(239)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPaletes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPaletes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPaletes.EnableHeadersVisualStyles = false;
            this.dgvPaletes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvPaletes.Location = new System.Drawing.Point(0, 190);
            this.dgvPaletes.MultiSelect = false;
            this.dgvPaletes.Name = "dgvPaletes";
            this.dgvPaletes.ReadOnly = true;
            this.dgvPaletes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPaletes.RowHeadersVisible = false;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(239)))), ((int)(((byte)(226)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvPaletes.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPaletes.RowTemplate.Height = 35;
            this.dgvPaletes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPaletes.Size = new System.Drawing.Size(1144, 471);
            this.dgvPaletes.TabIndex = 2;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.ColumnCount = 3;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanelTop.Controls.Add(this.grpCodigoBarras, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.grpProduto, 2, 0);
            this.tableLayoutPanelTop.Controls.Add(this.panelOptions, 1, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 60);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 1;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(1144, 130);
            this.tableLayoutPanelTop.TabIndex = 1;
            // 
            // grpCodigoBarras
            // 
            this.grpCodigoBarras.Controls.Add(this.txtCodigoDeBarras);
            this.grpCodigoBarras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpCodigoBarras.Location = new System.Drawing.Point(3, 3);
            this.grpCodigoBarras.Name = "grpCodigoBarras";
            this.grpCodigoBarras.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.grpCodigoBarras.Size = new System.Drawing.Size(394, 124);
            this.grpCodigoBarras.TabIndex = 0;
            this.grpCodigoBarras.TabStop = false;
            this.grpCodigoBarras.Text = "Leitura do Código de Barras";
            // 
            // txtCodigoDeBarras
            // 
            this.txtCodigoDeBarras.BackColor = System.Drawing.Color.White;
            this.txtCodigoDeBarras.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoDeBarras.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCodigoDeBarras.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.txtCodigoDeBarras.Location = new System.Drawing.Point(10, 28);
            this.txtCodigoDeBarras.Name = "txtCodigoDeBarras";
            this.txtCodigoDeBarras.Size = new System.Drawing.Size(374, 40);
            this.txtCodigoDeBarras.TabIndex = 0;
            this.txtCodigoDeBarras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCodigoDeBarras_KeyPress);
            // 
            // grpProduto
            // 
            this.grpProduto.Controls.Add(this.txtProduto);
            this.grpProduto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProduto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpProduto.Location = new System.Drawing.Point(746, 3);
            this.grpProduto.Name = "grpProduto";
            this.grpProduto.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.grpProduto.Size = new System.Drawing.Size(395, 124);
            this.grpProduto.TabIndex = 2;
            this.grpProduto.TabStop = false;
            this.grpProduto.Text = "Produto Registrado";
            // 
            // txtProduto
            // 
            this.txtProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.txtProduto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProduto.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtProduto.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.txtProduto.Location = new System.Drawing.Point(10, 28);
            this.txtProduto.Name = "txtProduto";
            this.txtProduto.ReadOnly = true;
            this.txtProduto.Size = new System.Drawing.Size(375, 40);
            this.txtProduto.TabIndex = 0;
            // 
            // panelOptions
            // 
            this.panelOptions.Controls.Add(this.chklistBloqueio);
            this.panelOptions.Controls.Add(this.numQtdeUltimoPalete);
            this.panelOptions.Controls.Add(this.chkUltimoPalete);
            this.panelOptions.Controls.Add(this.chkBloqueio);
            this.panelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptions.Location = new System.Drawing.Point(403, 3);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(337, 124);
            this.panelOptions.TabIndex = 1;
            // 
            // chklistBloqueio
            // 
            this.chklistBloqueio.BackColor = System.Drawing.Color.White;
            this.chklistBloqueio.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chklistBloqueio.CheckOnClick = true;
            this.chklistBloqueio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chklistBloqueio.FormattingEnabled = true;
            this.chklistBloqueio.Location = new System.Drawing.Point(180, 5);
            this.chklistBloqueio.Name = "chklistBloqueio";
            this.chklistBloqueio.Size = new System.Drawing.Size(150, 108);
            this.chklistBloqueio.TabIndex = 3;
            this.chklistBloqueio.Visible = false;
            // 
            // numQtdeUltimoPalete
            // 
            this.numQtdeUltimoPalete.Enabled = false;
            this.numQtdeUltimoPalete.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.numQtdeUltimoPalete.Location = new System.Drawing.Point(15, 85);
            this.numQtdeUltimoPalete.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numQtdeUltimoPalete.Name = "numQtdeUltimoPalete";
            this.numQtdeUltimoPalete.Size = new System.Drawing.Size(120, 27);
            this.numQtdeUltimoPalete.TabIndex = 1;
            // 
            // chkUltimoPalete
            // 
            this.chkUltimoPalete.AutoSize = true;
            this.chkUltimoPalete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.chkUltimoPalete.Location = new System.Drawing.Point(15, 55);
            this.chkUltimoPalete.Name = "chkUltimoPalete";
            this.chkUltimoPalete.Size = new System.Drawing.Size(130, 25);
            this.chkUltimoPalete.TabIndex = 0;
            this.chkUltimoPalete.Text = "Palete Final?";
            this.chkUltimoPalete.UseVisualStyleBackColor = true;
            // 
            // chkBloqueio
            // 
            this.chkBloqueio.AutoSize = true;
            this.chkBloqueio.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.chkBloqueio.Location = new System.Drawing.Point(15, 15);
            this.chkBloqueio.Name = "chkBloqueio";
            this.chkBloqueio.Size = new System.Drawing.Size(147, 25);
            this.chkBloqueio.TabIndex = 2;
            this.chkBloqueio.Text = "Bloquear Palete";
            this.chkBloqueio.UseVisualStyleBackColor = true;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.lblTotalProduzido);
            this.panelHeader.Controls.Add(this.lblInfoOp);
            this.panelHeader.Controls.Add(this.btnVoltar);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1144, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTotalProduzido
            // 
            this.lblTotalProduzido.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalProduzido.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalProduzido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTotalProduzido.Location = new System.Drawing.Point(794, 0);
            this.lblTotalProduzido.Name = "lblTotalProduzido";
            this.lblTotalProduzido.Size = new System.Drawing.Size(350, 60);
            this.lblTotalProduzido.TabIndex = 2;
            this.lblTotalProduzido.Text = "Total Produzido: 0";
            this.lblTotalProduzido.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInfoOp
            // 
            this.lblInfoOp.AutoSize = true;
            this.lblInfoOp.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            // MUDANÇA DE COR
            this.lblInfoOp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(97)))), ((int)(((byte)(10)))));
            this.lblInfoOp.Location = new System.Drawing.Point(145, 14);
            this.lblInfoOp.Name = "lblInfoOp";
            this.lblInfoOp.Size = new System.Drawing.Size(262, 32);
            this.lblInfoOp.TabIndex = 1;
            this.lblInfoOp.Text = "OP: 12345 - PRODUTO";
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnVoltar.FlatAppearance.BorderSize = 0;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnVoltar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnVoltar.Location = new System.Drawing.Point(0, 10);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(120, 40);
            this.btnVoltar.TabIndex = 0;
            this.btnVoltar.Text = "<< Voltar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            // 
            // PaletizacaoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 701);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(960, 600);
            this.Name = "PaletizacaoForm";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controle de Paletização";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaletes)).EndInit();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.grpCodigoBarras.ResumeLayout(false);
            this.grpProduto.ResumeLayout(false);
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQtdeUltimoPalete)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}