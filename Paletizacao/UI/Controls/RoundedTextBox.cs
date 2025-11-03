using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Paletizacao.UI.Controls
{
    public class RoundedTextBox : TextBox
    {
        private int borderRadius = 15;
        private int borderSize = 0; // Sem borda por defeito
        private Color borderColor = Color.Gray;

        public RoundedTextBox()
        {
            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 12F);
        }

        // Importa a função da API do Windows para definir as margens
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref Rectangle lParam);

        // Define as margens internas quando o controlo é criado ou redimensionado
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetMargins();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetMargins();
        }

        private void SetMargins()
        {
            // Define uma margem de 10 pixels à esquerda e à direita.
            Rectangle margins = new Rectangle(10, 4, 10, 4);
            SendMessage(this.Handle, 0xd3, 3, ref margins);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
            path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
            path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseAllFigures();

            this.Region = new Region(path);

            if (borderSize > 0)
            {
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    g.DrawPath(pen, path);
                }
            }
        }
    }
}
