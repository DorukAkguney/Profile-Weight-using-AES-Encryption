using System;
using System.IO;
using System.Windows.Forms;
using static Profile_Weight_Calculations.Program;

namespace Profile_Weight_Calculations
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form_Closing;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string filePath = txtBrowse.Text.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Lütfen bir dosya seçin.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int lineCount = 0;

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 2 && double.TryParse(parts[1], out double value))
                    {
                        GlobalData.Dataset[parts[0]] = value;
                        lineCount++;

                        // İlerleme çubuğunu güncelle
                        prgData.Value = (int)((double)lineCount / lines.Length * 100);
                    }
                }

                MessageBox.Show($"Dosya Okuma Tamamlandı. Eklenen Veri Sayısı: {lineCount}");
                prgData.Value = 0; // Tamamlandıktan sonra ilerleme çubuğunu sıfırla
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya okuma hatası: {ex.Message}");
                prgData.Value = 0; // Hata durumunda ilerleme çubuğunu sıfırla
            }
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            OpenForm(new Form1());
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBrowse.Text = openFileDialog.FileName;
                }
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
    }
}
