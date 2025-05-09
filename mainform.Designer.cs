namespace AndroidShellInterface
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtCommand;
        private Button btnRunCommand;
        private TextBox txtOutput;
        private Button btnConnect;
        private Button btnInstallApk;
        private Button btnUninstallApp;
        private TextBox txtPackage;

        private void InitializeComponent()
        {
            this.txtCommand = new TextBox();
            this.btnRunCommand = new Button();
            this.txtOutput = new TextBox();
            this.btnConnect = new Button();
            this.btnInstallApk = new Button();
            this.btnUninstallApp = new Button();
            this.txtPackage = new TextBox();

            // Layout
            this.txtCommand.Location = new System.Drawing.Point(12, 12);
            this.txtCommand.Size = new System.Drawing.Size(300, 22);

            this.btnRunCommand.Location = new System.Drawing.Point(320, 12);
            this.btnRunCommand.Text = "Run Command";
            this.btnRunCommand.Click += new EventHandler(this.btnRunCommand_Click);

            this.txtOutput.Location = new System.Drawing.Point(12, 40);
            this.txtOutput.Multiline = true;
            this.txtOutput.Size = new System.Drawing.Size(460, 180);

            this.btnConnect.Location = new System.Drawing.Point(12, 230);
            this.btnConnect.Text = "Check Device";
            this.btnConnect.Click += new EventHandler(this.btnConnect_Click);

            this.btnInstallApk.Location = new System.Drawing.Point(120, 230);
            this.btnInstallApk.Text = "Install APK";
            this.btnInstallApk.Click += new EventHandler(this.btnInstallApk_Click);

            this.txtPackage.Location = new System.Drawing.Point(12, 260);
            this.txtPackage.Size = new System.Drawing.Size(300, 22);

            this.btnUninstallApp.Location = new System.Drawing.Point(320, 260);
            this.btnUninstallApp.Text = "Uninstall App";
            this.btnUninstallApp.Click += new EventHandler(this.btnUninstallApp_Click);

            this.ClientSize = new System.Drawing.Size(484, 300);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.btnRunCommand);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnInstallApk);
            this.Controls.Add(this.txtPackage);
            this.Controls.Add(this.btnUninstallApp);
            this.Text = "Android Shell Interface";

            private void btnRunCommand_Click(object sender, EventArgs e)
        {
            // Your custom logic here
            MessageBox.Show("Run Command button clicked!");
        }

    }
}
}
