using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Presentacion
{
    public partial class FrmAdmin : Form
    {
        static List<User> users = new List<User>();
        private void SaveUsersToFile()
        {
            string filePath = "users.json"; // Ruta del archivo donde se guardarán los usuarios

            // Serializar la lista de usuarios a formato JSON
            string json = JsonConvert.SerializeObject(users);

            // Guardar el JSON en el archivo
            File.WriteAllText(filePath, json);
        }

        private void LoadUsersFromFile()
        {
            string filePath = "users.json"; // Ruta del archivo donde se guardarán los usuarios

            if (File.Exists(filePath))
            {
                // Leer el contenido del archivo
                string json = File.ReadAllText(filePath);

                // Deserializar el JSON a una lista de usuarios
                users = JsonConvert.DeserializeObject<List<User>>(json);

                // Actualizar la fuente de datos del dataGridView
                dtbUsers.DataSource = users;
            }
        }

        public FrmAdmin()
        {
            InitializeComponent();
        }
        public void clearTextBox()
        {
            txtUser.Text = string.Empty;
            txtPass.Text = string.Empty;
        }
        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            LoadUsersFromFile();
           
        }

        public class User
        {
            public string Username { get; }
            public string Password { get; }

            public User(string username, string password)
            {
                Username = username;
                Password = password;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "Admin" || txtPass.Text == "123456")
            {
                MessageBox.Show("No puedes registrar el usuario administrador");
            }
            else
            {
                if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass.Text))
                {
                    string username = txtUser.Text.Trim();

                    // Verificar si el usuario ya existe
                    bool userExists = users.Any(u => u.Username == username);

                    if (userExists)
                    {
                        MessageBox.Show("El usuario ya existe.");
                    }
                    else
                    {
                        users.Add(new User(username, txtPass.Text));
                        MessageBox.Show("Usuario registrado correctamente");
                        dtbUsers.DataSource = null; // Desvincula la fuente de datos actual
                        dtbUsers.DataSource = users; // Vuelve a asignar la lista de usuarios como fuente de datos
                        clearTextBox();
                    }
                }
                else
                {
                    MessageBox.Show("Campos vacíos, por favor complete");
                }
            }
           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dtbUsers.SelectedRows.Count > 0)
            {
                var selectedRow = dtbUsers.SelectedRows[0];
                var user = selectedRow.DataBoundItem as User;
                users.Remove(user);
                dtbUsers.DataSource = null; // Desvincula la fuente de datos actual
                dtbUsers.DataSource = users; // Vuelve a asignar la lista de usuarios como fuente de datos
                MessageBox.Show("Usuario eliminado correctamente");
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminar");
            }
        }

        private void FrmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Estás a punto de salir del modo administrador. ¿Estás seguro de que deseas continuar?", "Confirmar cierre de sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SaveUsersToFile();
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
