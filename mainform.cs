using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AndroidShellInterface
{
    public partial class MainForm : Form
    {
        AndroidManager manager;

        public MainForm()
        {
            InitializeComponent();
            manager = new AndroidManager();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            txtOutput.Text = manager.CheckDevice();
        }

        private void btnRunCommand_Click(object sender, EventArgs e)
        {
            txtOutput.Text = manager.ExecuteCommand(txtCommand.Text);
        }

        private void btnInstallApk_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "APK Files|*.apk";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtOutput.Text = manager.InstallApk(dlg.FileName);
                }
            }
        }

        private void btnUninstallApp_Click(object sender, EventArgs e)
        {
            txtOutput.Text = manager.UninstallApp(txtPackage.Text);
        }
    }
}
