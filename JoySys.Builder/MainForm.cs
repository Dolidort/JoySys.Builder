using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JoySys.Builder
{
    public partial class MainForm : Form
    {

        private BuilderConfigService _configService;
        private BuilderConfig _config;
        public MainForm()
        {
            InitializeComponent();

            Text = "JoySys Builder";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 550);

            txtLog.Font = new Font("Consolas", 9);
            txtLog.BackColor = Color.White;
            txtLog.ReadOnly = true;

            progressBarBuild.Minimum = 0;
            progressBarBuild.Maximum = 100;
            progressBarBuild.Value = 0;

            txtValidation.Clear();
            txtLog.Clear();

            txtInstructions.Text =
                "Required setup:" + Environment.NewLine +
                "1. SQL Server or SQL Server Express must be installed." + Environment.NewLine +
                "2. The SQL instance should match the SQL Server field, for example .\\SQLEXPRESS or (localdb)\\MSSQLLocalDB." + Environment.NewLine +
                "3. The Windows user must have permission to create/open the database." + Environment.NewLine +
                "4. Select the SCADA project folder that contains variable.DBF." + Environment.NewLine +
                "5. Click Test Configuration before running the builder." + Environment.NewLine +
                "6. Click Test SQL to verify database access.";

            lblCurrentStepValue.Text = "Ready";
            btnRunBuilder.Click += btnRunBuilder_Click;
            _configService = new BuilderConfigService();
            btnOpenConfigFolder.Click += btnOpenConfigFolder_Click;
            LoadConfigToScreen();
        }

        private void SetStatus(string step, int progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetStatus(step, progress)));
                return;
            }

            lblCurrentStepValue.Text = step;
            progressBarBuild.Value = Math.Max(
                progressBarBuild.Minimum,
                Math.Min(progress, progressBarBuild.Maximum));
        }

        private void AppendLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new Action(() => AppendLog(message)));
                return;
            }

            txtLog.AppendText(
                DateTime.Now.ToString("HH:mm:ss") +
                "  " +
                message +
                Environment.NewLine);
        }

        private void LoadConfigToScreen()
        {
            try
            {
                _config = _configService.LoadOrCreate();

                txtConfigPath.Text = _config.ConfigPath;
                txtProjectPath.Text = _config.ProjectPath;
                txtSqlServer.Text = _config.SqlServer;
                txtDatabase.Text = _config.Database;

                RefreshConfigStatus();

                SetStatus("Ready", 0);
                AppendLog("Configuration loaded.");
            }
            catch (Exception ex)
            {
                SetStatus("Configuration load failed", 0);
                AppendLog("Failed to load configuration: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private async void btnRunBuilder_Click(object sender, EventArgs e)
        {
            SetActionsEnabled(false);
            txtLog.Clear();

            try
            {
                SaveScreenToConfigObject();
                _configService.Save(_config);
                RefreshConfigStatus();

                if (!IsConfigurationValid())
                {
                    SetStatus("Configuration has problems", 0);
                    AppendLog("Builder was not started because configuration has problems.");

                    MessageBox.Show(
                        "Configuration has problems. Fix them before running the builder.",
                        "JoySys Builder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                SetStatus("Starting builder", 5);
                AppendLog("Starting JoySys Builder...");

                BuildResult result = await Task.Run(() =>
                {
                    BuilderEngine engine = new BuilderEngine();

                    return engine.Run(message =>
                    {
                        AppendLog(message);
                    });
                });

                if (result.Status == BuildStatus.Success)
                {
                    SetStatus("Completed", 100);
                    AppendLog(result.Message);

                    MessageBox.Show(
                        result.Message,
                        "JoySys Builder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else if (result.Status == BuildStatus.Cancelled)
                {
                    SetStatus("Cancelled", 0);
                    AppendLog(result.Message);

                    MessageBox.Show(
                        result.Message,
                        "JoySys Builder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    SetStatus("Failed", progressBarBuild.Value);
                    AppendLog(result.Message);

                    MessageBox.Show(
                        result.Message,
                        "Builder Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Failed", progressBarBuild.Value);
                AppendLog("ERROR: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "Builder Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetActionsEnabled(true);
            }
        }

        private void RefreshConfigStatus()
        {
            if (_config == null)
                return;

            txtValidation.Clear();

            AddValidationLine(
                _config.ConfigExists,
                "Config file found: " + _config.ConfigPath,
                "Config file was not found: " + _config.ConfigPath);

            AddValidationLine(
                _config.ProjectPathExists,
                "SCADA project folder found: " + _config.ProjectPath,
                "SCADA project folder was not found: " + _config.ProjectPath);

            AddValidationLine(
                _config.DbfExists,
                "Required DBF file found: " + _config.DbfFilePath,
                "Required DBF file was not found: " + _config.DbfFilePath);

            if (_config.DbfExists)
            {
                var info = new FileInfo(_config.DbfFilePath);

                AddValidationLine(
                    true,
                    "DBF size: " + info.Length + " bytes",
                    "");

                lblDbfStatus.Text = "Found: variable.DBF";
                lblDbfStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblDbfStatus.Text = "Missing: variable.DBF was not found";
                lblDbfStatus.ForeColor = Color.DarkRed;
            }
        }

        private void AddValidationLine(bool ok, string successText, string failText)
        {
            if (txtValidation.InvokeRequired)
            {
                txtValidation.BeginInvoke(new Action(() => AddValidationLine(ok, successText, failText)));
                return;
            }

            string text = ok ? successText : failText;

            txtValidation.SelectionStart = txtValidation.TextLength;
            txtValidation.SelectionLength = 0;

            txtValidation.SelectionColor = ok ? Color.DarkGreen : Color.DarkRed;

            txtValidation.AppendText(
                (ok ? "✓ " : "✗ ") +
                text +
                Environment.NewLine);

            txtValidation.SelectionColor = txtValidation.ForeColor;
        }
        private void btnBrowseProject_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description =
                    "Select SCADA Project Folder. The folder must contain variable.DBF.";

                if (Directory.Exists(txtProjectPath.Text))
                    dialog.SelectedPath = txtProjectPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtProjectPath.Text = dialog.SelectedPath;
                    SaveScreenToConfigObject();
                    RefreshConfigStatus();
                }
            }
        }

        private bool IsConfigurationValid()
        {
            SaveScreenToConfigObject();

            return
                _config.ConfigExists &&
                _config.ProjectPathExists &&
                _config.DbfExists &&
                !string.IsNullOrWhiteSpace(_config.SqlServer) &&
                !string.IsNullOrWhiteSpace(_config.Database);
        }

        private void SaveScreenToConfigObject()
        {
            if (_config == null)
                _config = new BuilderConfig();

            _config.ConfigPath = txtConfigPath.Text;
            _config.ProjectPath = txtProjectPath.Text;
            _config.SqlServer = txtSqlServer.Text;
            _config.Database = txtDatabase.Text;
        }

        private void btnOpenConfigFolder_Click(object sender, EventArgs e)
        {
            try
            {
                SaveScreenToConfigObject();

                string folder = Path.GetDirectoryName(_config.ConfigPath);

                if (!Directory.Exists(folder))
                {
                    MessageBox.Show("Config folder does not exist.");
                    return;
                }

                Process.Start("explorer.exe", folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Open Folder Error");
            }
        }

        private void grpConfiguration_Enter(object sender, EventArgs e)
        {

        }



        private void btnTestConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SaveScreenToConfigObject();
                RefreshConfigStatus();

                bool ok = IsConfigurationValid();

                if (ok)
                {
                    SetStatus("Configuration OK", 100);
                    AppendLog("Configuration OK.");

                    MessageBox.Show(
                        "Configuration looks valid.",
                        "JoySys Builder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    SetStatus("Configuration has problems", 0);
                    AppendLog("Configuration has problems. See Configuration Check section.");

                    MessageBox.Show(
                        "Configuration has problems. See Configuration Check section.",
                        "JoySys Builder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Configuration check failed", 0);
                AppendLog("Configuration check failed: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "Configuration Check Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SaveScreenToConfigObject();
                _configService.Save(_config);

                RefreshConfigStatus();

                SetStatus("Configuration saved", 100);
                AppendLog("Configuration saved.");

                MessageBox.Show(
                    "Configuration saved.",
                    "JoySys Builder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("Save configuration failed", 0);
                AppendLog("Failed to save configuration: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "Save Configuration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnReloadConfig_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Reload configuration from config.ini? Unsaved changes on screen will be lost.",
                "Reload Configuration",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            LoadConfigToScreen();
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            try
            {
                SaveScreenToConfigObject();

                if (string.IsNullOrWhiteSpace(_config.ConfigPath) || !File.Exists(_config.ConfigPath))
                {
                    MessageBox.Show(
                        "Config file does not exist.",
                        "Open Config",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Process.Start("notepad.exe", _config.ConfigPath);

                AppendLog("Opened config file.");
            }
            catch (Exception ex)
            {
                AppendLog("Failed to open config file: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "Open Config Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnTestSql_Click(object sender, EventArgs e)
        {
            btnTestSql.Enabled = false;

            try
            {
                SaveScreenToConfigObject();

                if (string.IsNullOrWhiteSpace(_config.SqlServer))
                    throw new InvalidOperationException("SQL Server is empty.");

                if (string.IsNullOrWhiteSpace(_config.Database))
                    throw new InvalidOperationException("Database name is empty.");

                SetStatus("Testing SQL connection", 20);
                AppendLog("Testing SQL connection...");

                await Task.Run(() =>
                {
                    SqlDataAccess.EnsureDatabaseExists(
                        _config.SqlServer,
                        _config.Database);

                    string connectionString =
                        SqlDataAccess.BuildConnectionString(
                            _config.SqlServer,
                            _config.Database);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                    }
                });

                SetStatus("SQL connection OK", 100);
                AppendLog("SQL connection OK.");

                MessageBox.Show(
                    "SQL connection OK.",
                    "JoySys Builder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("SQL connection failed", 0);
                AppendLog("SQL connection failed: " + ex.Message);

                MessageBox.Show(
                    ex.ToString(),
                    "SQL Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnTestSql.Enabled = true;
            }
        }

        private void SetActionsEnabled(bool enabled)
        {
            btnTestConfig.Enabled = enabled;
            btnTestSql.Enabled = enabled;
            btnSaveConfig.Enabled = enabled;
            btnReloadConfig.Enabled = enabled;
            btnOpenConfig.Enabled = enabled;
            btnRunBuilder.Enabled = enabled;
            btnBrowseProject.Enabled = enabled;
        }
    }
}
