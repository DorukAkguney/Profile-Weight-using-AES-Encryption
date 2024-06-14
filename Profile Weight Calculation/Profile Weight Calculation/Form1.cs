using System;
using System.Windows.Forms;

namespace Profile_Weight_Calculations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form_Closing;
            txtPassword.PasswordChar = '*'; // Parolanın yıldızlı görünmesini sağlar
            chkShowPassword.CheckedChanged += new EventHandler(chkShowPassword_CheckedChanged); // CheckBox olayını ekleyin
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // CheckBox seçiliyse parolanın görünürlüğünü ayarla
            if (chkShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0'; // Parola görünür
            }
            else
            {
                txtPassword.PasswordChar = '*'; // Parola yıldızlı görünür
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen Kullanıcı Adınızı ve Şifreyi Girin");
                return;
            }

            CheckCredentials(username, password);
        }

        private void CheckCredentials(string username, string password)
        {
            if (this.Text == "Admin Girişi" && username == "doruk" && password == "123456")
            {
                OpenForm(new Form2());
            }
            else if (this.Text == "Kullanıcı Girişi" && username == "Doruk" && password == "123456")
            {
                OpenForm(new Form3());
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
            }
        }

        private void OpenForm(Form form)
        {
            this.Hide();
            form.Show();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void btnChange_Click_1(object sender, EventArgs e)
        {
            if (this.Text == "Admin Girişi")
            {
                this.Text = "Kullanıcı Girişi";
                btnChange.Text = "Admin Girişi";
            }
            else
            {
                this.Text = "Admin Girişi";
                btnChange.Text = "Kullanıcı Girişi";
            }
        }
    }
}
