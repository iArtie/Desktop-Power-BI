using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Presentacion
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser navegador;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            navegador = new ChromiumWebBrowser("")
            {
                Dock = DockStyle.Fill
            };

            pnlPower.Width = Width - 20;
            pnlPower.Height = Height - 40;
            pnlPower.Top = 40;
            pnlPower.Left = 0;
            pnlPower.Controls.Add(navegador);
            navegador.Load("https://app.powerbi.com/view?r=eyJrIjoiOTkwYTZjYTMtODdiZS00MWEwLWJjZjYtMGFmYWNmZDFkZTY0IiwidCI6ImUxMTlmY2ZmLTRmMzUtNDMzOC04MzQzLTc2ZDQ1OTg5NGI2YiIsImMiOjR9");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
          
         
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Estás a punto de cerrar sesión. ¿Estás seguro de que deseas continuar?", "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                FrmLogin login = new FrmLogin();
                login.Show();
                Hide();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
