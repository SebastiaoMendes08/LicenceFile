using System;
using System.Windows.Forms;

namespace FileLicence
{
    public partial class frm_main : Form
    {
        
        FunctionService functionService = new FunctionService();

        public frm_main()
        {

          InitializeComponent();
 
        }

        private void btn_filtar_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_path.Text))
            {
                MessageBox.Show("Selecione o Caminho da licença", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            functionService.Save(date_.Value, txt_path.Text); 
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            functionService.StartService();
        }

        private void btn_filtar_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_path.Clear();
                txt_path.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_install_Click_1(object sender, EventArgs e)
        {
            functionService.InstallService();
        }

        private void btn_uninstall_Click_1(object sender, EventArgs e)
        {
            functionService.UninstallService();
        }

        private void btn_start_Click_1(object sender, EventArgs e)
        {
            functionService.StartService();
        }
    }
}
