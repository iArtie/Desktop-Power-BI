using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Presentacion.FrmAdmin;

namespace Presentacion
{
    public partial class FrmLogin : Form
    {
        int cont = 6;
        public FrmLogin()
        {
            InitializeComponent();
        }
        BackgroundWorker bg = new BackgroundWorker();
        public void clearTextBox()
        {
            txtUser.Text = string.Empty;
            txtPass.Text = string.Empty;
        }
        private void bg_DoWork(object sender, EventArgs e)
        {
            int progreso = 0, porciento = 0;


            for (int i = 0; i <= 100; i++)
            {
                progreso++;
                Thread.Sleep(50);
                bg.ReportProgress(i);
            }
        }

        private void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbLogin.Value = e.ProgressPercentage;
            pgbLogin.Style = ProgressBarStyle.Continuous;


            if (e.ProgressPercentage > 100)
            {
                lblPer.Text = "100%";
                pgbLogin.Value = pgbLogin.Maximum;
            }
            else
            {
                lblPer.Text = Convert.ToString(e.ProgressPercentage) + "%";
                pgbLogin.Value = e.ProgressPercentage;
            }


        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Form1 main = new Form1();
            main.Show();
            Hide();

        }
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            pgbLogin.Visible = false;
            lblPer.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (txtUser.Text == String.Empty || txtPass.Text == String.Empty)
            {
                MessageBox.Show("Campos vacíos");
            }
            else
            {
                if (txtUser.Text == "Admin" && txtPass.Text == "123456")
                {
                    MessageBox.Show("Has ingresado al modo administrador");
                    FrmAdmin admin = new FrmAdmin();
                    admin.Show();
                    Hide();
                }
                string filePath = "users.json"; // Ruta del archivo JSON donde se almacenan los usuarios

                // Leer el contenido del archivo
                string json = File.ReadAllText(filePath);

                // Deserializar el JSON en una lista de usuarios
                List<User> userList = JsonConvert.DeserializeObject<List<User>>(json);

                string username = txtUser.Text.Trim();
                string password = txtPass.Text;

                // Verificar si las credenciales coinciden con algún usuario en la lista
                User user = userList.FirstOrDefault(u => u.Username == username && u.Password == password);
               
                    if (user != null)
                    {
                       
                    bg.WorkerReportsProgress = true;
                    bg.ProgressChanged += bg_ProgressChanged;
                    bg.DoWork += bg_DoWork;
                    bg.RunWorkerCompleted += bg_RunWorkerCompleted;
                    bg.RunWorkerAsync();
                    lblPer.Visible = true;
                    pgbLogin.Visible = true;
                    
                    txtUser.Enabled = false;
                    txtPass.Enabled = false;
                    btnAccept.Enabled = false;
                    btnExit.Enabled = false;
                    lblPer.Visible = true;
                    MessageBox.Show("Has accedido correctamente");
                }
                    else 
                    {
                            Cursor.Current = Cursors.Default;
                            --cont;
                            label4.Visible = true;
                            lblTrys.Text = cont.ToString();
                            //MessageBox.Show("Error: usuario o contraseña incorrecta", cont + " Intentos restantes");
                            if (cont == 0)
                            {
                                MessageBox.Show("Ha excedido la cantidad de intentos, por favor inténtelo más tarde");
                                cont = 5;
                                btnAccept.Enabled = false;
                                btnExit.Enabled = false;
                                Thread.Sleep(3000);
                                btnAccept.Enabled = true;
                                btnExit.Enabled = true;
                                lblTrys.Text = cont.ToString();
                            }
                        
                    }
                   
                    
                
               

                clearTextBox();
            }
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
