using System;
using System.Windows.Forms;
using static Profile_Weight_Calculations.Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Profile_Weight_Calculations
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            FillComboBox();
            this.FormClosing += Form_Closing;
        }

        private void FillComboBox()
        {
            foreach (var entry in GlobalData.Dataset)
            {
                cmbBox.Items.Add(entry.Key);
            }
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            OpenForm(new Form1());
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            if (cmbBox.SelectedItem == null)
            {
                MessageBox.Show("Lütfen malzeme değerini seçin.");
                return;
            }

            string selectedValue = cmbBox.SelectedItem.ToString();
            double weight = GlobalData.Dataset[selectedValue];

            try
            {
                double quantity = Convert.ToDouble(txtQuantity.Text);
                double length = Convert.ToDouble(txtLength.Text);

                double result = quantity * length * weight / 1000;
                lblResult.Text = $"Sonuç: {result} kg.";
            }
            catch (FormatException)
            {
                MessageBox.Show("Geçersiz uzunluk veya adet değeri.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
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
