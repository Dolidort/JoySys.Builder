namespace JoySys.Builder
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnRunBuilder = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblCurrentStepTitle = new System.Windows.Forms.Label();
            this.lblCurrentStepValue = new System.Windows.Forms.Label();
            this.progressBarBuild = new System.Windows.Forms.ProgressBar();
            this.btnReloadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.btnOpenConfigFolder = new System.Windows.Forms.Button();
            this.btnBrowseProject = new System.Windows.Forms.Button();
            this.btnTestConfig = new System.Windows.Forms.Button();
            this.btnTestSql = new System.Windows.Forms.Button();
            this.grpConfiguration = new System.Windows.Forms.GroupBox();
            this.tblConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.lblDbfFileValue = new System.Windows.Forms.Label();
            this.lblDatabaseTitle = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblRequiredDbf = new System.Windows.Forms.Label();
            this.txtSqlServer = new System.Windows.Forms.TextBox();
            this.lblSqlServerTitle = new System.Windows.Forms.Label();
            this.lblConfigPathTitle = new System.Windows.Forms.Label();
            this.txtConfigPath = new System.Windows.Forms.TextBox();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.lblDbfStatus = new System.Windows.Forms.Label();
            this.lblDbfStatusTitle = new System.Windows.Forms.Label();
            this.grpValidation = new System.Windows.Forms.GroupBox();
            this.txtValidation = new System.Windows.Forms.RichTextBox();
            this.lblValidationHelp = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.lblRunHelp = new System.Windows.Forms.Label();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.grpInstructions = new System.Windows.Forms.GroupBox();
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.lblProjPath = new System.Windows.Forms.Label();
            this.grpConfiguration.SuspendLayout();
            this.tblConfiguration.SuspendLayout();
            this.grpValidation.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.grpInstructions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunBuilder
            // 
            this.btnRunBuilder.Location = new System.Drawing.Point(88, 206);
            this.btnRunBuilder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunBuilder.Name = "btnRunBuilder";
            this.btnRunBuilder.Size = new System.Drawing.Size(253, 51);
            this.btnRunBuilder.TabIndex = 0;
            this.btnRunBuilder.Text = "Run Builder";
            this.btnRunBuilder.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(7, 46);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1170, 174);
            this.txtLog.TabIndex = 1;
            this.txtLog.WordWrap = false;
            // 
            // lblCurrentStepTitle
            // 
            this.lblCurrentStepTitle.AutoSize = true;
            this.lblCurrentStepTitle.Location = new System.Drawing.Point(132, 265);
            this.lblCurrentStepTitle.Name = "lblCurrentStepTitle";
            this.lblCurrentStepTitle.Size = new System.Drawing.Size(101, 20);
            this.lblCurrentStepTitle.TabIndex = 2;
            this.lblCurrentStepTitle.Text = "Current step:";
            // 
            // lblCurrentStepValue
            // 
            this.lblCurrentStepValue.AutoSize = true;
            this.lblCurrentStepValue.Location = new System.Drawing.Point(230, 265);
            this.lblCurrentStepValue.Name = "lblCurrentStepValue";
            this.lblCurrentStepValue.Size = new System.Drawing.Size(55, 20);
            this.lblCurrentStepValue.TabIndex = 3;
            this.lblCurrentStepValue.Text = "Ready";
            // 
            // progressBarBuild
            // 
            this.progressBarBuild.Location = new System.Drawing.Point(7, 294);
            this.progressBarBuild.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBarBuild.Name = "progressBarBuild";
            this.progressBarBuild.Size = new System.Drawing.Size(434, 29);
            this.progressBarBuild.TabIndex = 4;
            this.progressBarBuild.Visible = false;
            // 
            // btnReloadConfig
            // 
            this.btnReloadConfig.Location = new System.Drawing.Point(230, 84);
            this.btnReloadConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReloadConfig.Name = "btnReloadConfig";
            this.btnReloadConfig.Size = new System.Drawing.Size(170, 41);
            this.btnReloadConfig.TabIndex = 10;
            this.btnReloadConfig.Text = "Reload Config";
            this.btnReloadConfig.UseVisualStyleBackColor = true;
            this.btnReloadConfig.Click += new System.EventHandler(this.btnReloadConfig_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnSaveConfig.Location = new System.Drawing.Point(33, 84);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(170, 41);
            this.btnSaveConfig.TabIndex = 11;
            this.btnSaveConfig.Text = "Save Config";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnOpenConfig.Location = new System.Drawing.Point(33, 132);
            this.btnOpenConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(170, 41);
            this.btnOpenConfig.TabIndex = 12;
            this.btnOpenConfig.Text = "Open Config";
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // btnOpenConfigFolder
            // 
            this.btnOpenConfigFolder.Location = new System.Drawing.Point(244, 228);
            this.btnOpenConfigFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenConfigFolder.Name = "btnOpenConfigFolder";
            this.btnOpenConfigFolder.Size = new System.Drawing.Size(198, 34);
            this.btnOpenConfigFolder.TabIndex = 13;
            this.btnOpenConfigFolder.Text = "Open Config Folder";
            this.btnOpenConfigFolder.UseVisualStyleBackColor = true;
            // 
            // btnBrowseProject
            // 
            this.btnBrowseProject.Location = new System.Drawing.Point(11, 228);
            this.btnBrowseProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowseProject.Name = "btnBrowseProject";
            this.btnBrowseProject.Size = new System.Drawing.Size(226, 34);
            this.btnBrowseProject.TabIndex = 14;
            this.btnBrowseProject.Text = "Browse SCADA Folder...";
            this.btnBrowseProject.UseVisualStyleBackColor = true;
            // 
            // btnTestConfig
            // 
            this.btnTestConfig.Location = new System.Drawing.Point(33, 35);
            this.btnTestConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTestConfig.Name = "btnTestConfig";
            this.btnTestConfig.Size = new System.Drawing.Size(170, 41);
            this.btnTestConfig.TabIndex = 15;
            this.btnTestConfig.Text = "Check Configuration";
            this.btnTestConfig.UseVisualStyleBackColor = true;
            this.btnTestConfig.Click += new System.EventHandler(this.btnTestConfig_Click);
            // 
            // btnTestSql
            // 
            this.btnTestSql.Location = new System.Drawing.Point(230, 35);
            this.btnTestSql.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTestSql.Name = "btnTestSql";
            this.btnTestSql.Size = new System.Drawing.Size(170, 41);
            this.btnTestSql.TabIndex = 16;
            this.btnTestSql.Text = "Test SQL";
            this.btnTestSql.UseVisualStyleBackColor = true;
            this.btnTestSql.Click += new System.EventHandler(this.btnTestSql_Click);
            // 
            // grpConfiguration
            // 
            this.grpConfiguration.Controls.Add(this.tblConfiguration);
            this.grpConfiguration.Controls.Add(this.btnOpenConfigFolder);
            this.grpConfiguration.Controls.Add(this.btnBrowseProject);
            this.grpConfiguration.Location = new System.Drawing.Point(14, 90);
            this.grpConfiguration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpConfiguration.Name = "grpConfiguration";
            this.grpConfiguration.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpConfiguration.Size = new System.Drawing.Size(709, 274);
            this.grpConfiguration.TabIndex = 18;
            this.grpConfiguration.TabStop = false;
            this.grpConfiguration.Text = "Configuration";
            this.grpConfiguration.Enter += new System.EventHandler(this.grpConfiguration_Enter);
            // 
            // tblConfiguration
            // 
            this.tblConfiguration.ColumnCount = 2;
            this.tblConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.84868F));
            this.tblConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.15132F));
            this.tblConfiguration.Controls.Add(this.lblProjPath, 0, 1);
            this.tblConfiguration.Controls.Add(this.lblDbfFileValue, 1, 2);
            this.tblConfiguration.Controls.Add(this.lblDatabaseTitle, 0, 5);
            this.tblConfiguration.Controls.Add(this.txtDatabase, 1, 5);
            this.tblConfiguration.Controls.Add(this.lblRequiredDbf, 0, 2);
            this.tblConfiguration.Controls.Add(this.txtSqlServer, 1, 4);
            this.tblConfiguration.Controls.Add(this.lblSqlServerTitle, 0, 4);
            this.tblConfiguration.Controls.Add(this.lblConfigPathTitle, 0, 0);
            this.tblConfiguration.Controls.Add(this.txtConfigPath, 1, 0);
            this.tblConfiguration.Controls.Add(this.txtProjectPath, 1, 1);
            this.tblConfiguration.Controls.Add(this.lblDbfStatus, 1, 3);
            this.tblConfiguration.Controls.Add(this.lblDbfStatusTitle, 0, 3);
            this.tblConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblConfiguration.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tblConfiguration.Location = new System.Drawing.Point(3, 23);
            this.tblConfiguration.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tblConfiguration.Name = "tblConfiguration";
            this.tblConfiguration.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.tblConfiguration.RowCount = 6;
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66944F));
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tblConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.tblConfiguration.Size = new System.Drawing.Size(703, 247);
            this.tblConfiguration.TabIndex = 1;
            // 
            // lblDbfFileValue
            // 
            this.lblDbfFileValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDbfFileValue.AutoSize = true;
            this.lblDbfFileValue.Location = new System.Drawing.Point(175, 92);
            this.lblDbfFileValue.Name = "lblDbfFileValue";
            this.lblDbfFileValue.Size = new System.Drawing.Size(100, 20);
            this.lblDbfFileValue.TabIndex = 23;
            this.lblDbfFileValue.Text = "variable.DBF";
            this.lblDbfFileValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDatabaseTitle
            // 
            this.lblDatabaseTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDatabaseTitle.AutoSize = true;
            this.lblDatabaseTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDatabaseTitle.Location = new System.Drawing.Point(12, 206);
            this.lblDatabaseTitle.Name = "lblDatabaseTitle";
            this.lblDatabaseTitle.Size = new System.Drawing.Size(83, 20);
            this.lblDatabaseTitle.TabIndex = 20;
            this.lblDatabaseTitle.Text = "Database:";
            this.lblDatabaseTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatabase.Location = new System.Drawing.Point(175, 199);
            this.txtDatabase.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(516, 26);
            this.txtDatabase.TabIndex = 8;
            this.txtDatabase.Text = "txtDatabase";
            // 
            // lblRequiredDbf
            // 
            this.lblRequiredDbf.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRequiredDbf.AutoSize = true;
            this.lblRequiredDbf.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblRequiredDbf.Location = new System.Drawing.Point(12, 92);
            this.lblRequiredDbf.Name = "lblRequiredDbf";
            this.lblRequiredDbf.Size = new System.Drawing.Size(139, 20);
            this.lblRequiredDbf.TabIndex = 21;
            this.lblRequiredDbf.Text = "Required DBF file:";
            this.lblRequiredDbf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSqlServer
            // 
            this.txtSqlServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSqlServer.Location = new System.Drawing.Point(175, 162);
            this.txtSqlServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSqlServer.Name = "txtSqlServer";
            this.txtSqlServer.Size = new System.Drawing.Size(516, 26);
            this.txtSqlServer.TabIndex = 7;
            this.txtSqlServer.Text = "txtSqlServer";
            // 
            // lblSqlServerTitle
            // 
            this.lblSqlServerTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSqlServerTitle.AutoSize = true;
            this.lblSqlServerTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSqlServerTitle.Location = new System.Drawing.Point(12, 166);
            this.lblSqlServerTitle.Name = "lblSqlServerTitle";
            this.lblSqlServerTitle.Size = new System.Drawing.Size(95, 20);
            this.lblSqlServerTitle.TabIndex = 19;
            this.lblSqlServerTitle.Text = "SQL Server:";
            this.lblSqlServerTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConfigPathTitle
            // 
            this.lblConfigPathTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblConfigPathTitle.AutoSize = true;
            this.lblConfigPathTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblConfigPathTitle.Location = new System.Drawing.Point(12, 18);
            this.lblConfigPathTitle.Name = "lblConfigPathTitle";
            this.lblConfigPathTitle.Size = new System.Drawing.Size(83, 20);
            this.lblConfigPathTitle.TabIndex = 17;
            this.lblConfigPathTitle.Text = "Config file:\n";
            this.lblConfigPathTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConfigPath.Location = new System.Drawing.Point(175, 14);
            this.txtConfigPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.ReadOnly = true;
            this.txtConfigPath.Size = new System.Drawing.Size(516, 26);
            this.txtConfigPath.TabIndex = 5;
            this.txtConfigPath.Text = "txtConfigPath";
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProjectPath.Location = new System.Drawing.Point(175, 51);
            this.txtProjectPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(516, 26);
            this.txtProjectPath.TabIndex = 6;
            this.txtProjectPath.Text = "txtProjectPath";
            // 
            // lblDbfStatus
            // 
            this.lblDbfStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDbfStatus.AutoSize = true;
            this.lblDbfStatus.Location = new System.Drawing.Point(175, 129);
            this.lblDbfStatus.Name = "lblDbfStatus";
            this.lblDbfStatus.Size = new System.Drawing.Size(97, 20);
            this.lblDbfStatus.TabIndex = 9;
            this.lblDbfStatus.Text = "lblDbfStatus";
            this.lblDbfStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDbfStatusTitle
            // 
            this.lblDbfStatusTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDbfStatusTitle.AutoSize = true;
            this.lblDbfStatusTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDbfStatusTitle.Location = new System.Drawing.Point(12, 129);
            this.lblDbfStatusTitle.Name = "lblDbfStatusTitle";
            this.lblDbfStatusTitle.Size = new System.Drawing.Size(94, 20);
            this.lblDbfStatusTitle.TabIndex = 22;
            this.lblDbfStatusTitle.Text = "DBF status:";
            this.lblDbfStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpValidation
            // 
            this.grpValidation.Controls.Add(this.txtValidation);
            this.grpValidation.Controls.Add(this.lblValidationHelp);
            this.grpValidation.Location = new System.Drawing.Point(14, 382);
            this.grpValidation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpValidation.Name = "grpValidation";
            this.grpValidation.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpValidation.Size = new System.Drawing.Size(709, 292);
            this.grpValidation.TabIndex = 19;
            this.grpValidation.TabStop = false;
            this.grpValidation.Text = "Configuration Check";
            // 
            // txtValidation
            // 
            this.txtValidation.BackColor = System.Drawing.Color.White;
            this.txtValidation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValidation.DetectUrls = false;
            this.txtValidation.Location = new System.Drawing.Point(7, 46);
            this.txtValidation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtValidation.Name = "txtValidation";
            this.txtValidation.ReadOnly = true;
            this.txtValidation.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtValidation.Size = new System.Drawing.Size(695, 238);
            this.txtValidation.TabIndex = 25;
            this.txtValidation.Text = "";
            // 
            // lblValidationHelp
            // 
            this.lblValidationHelp.AutoSize = true;
            this.lblValidationHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValidationHelp.Location = new System.Drawing.Point(3, 23);
            this.lblValidationHelp.Name = "lblValidationHelp";
            this.lblValidationHelp.Size = new System.Drawing.Size(530, 20);
            this.lblValidationHelp.TabIndex = 18;
            this.lblValidationHelp.Text = "This section shows missing files, invalid paths, and configuration problems.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTitle.Location = new System.Drawing.Point(14, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(205, 38);
            this.lblTitle.TabIndex = 20;
            this.lblTitle.Text = "JoySys Builder";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblSubtitle.Location = new System.Drawing.Point(14, 50);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(660, 25);
            this.lblSubtitle.TabIndex = 21;
            this.lblSubtitle.Text = "Creates or updates the JoySys database using the SCADA project variable.DBF file." +
    "";
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.lblRunHelp);
            this.grpActions.Controls.Add(this.btnTestSql);
            this.grpActions.Controls.Add(this.btnTestConfig);
            this.grpActions.Controls.Add(this.btnSaveConfig);
            this.grpActions.Controls.Add(this.btnReloadConfig);
            this.grpActions.Controls.Add(this.btnOpenConfig);
            this.grpActions.Controls.Add(this.btnRunBuilder);
            this.grpActions.Controls.Add(this.progressBarBuild);
            this.grpActions.Controls.Add(this.lblCurrentStepTitle);
            this.grpActions.Controls.Add(this.lblCurrentStepValue);
            this.grpActions.Location = new System.Drawing.Point(754, 382);
            this.grpActions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpActions.Name = "grpActions";
            this.grpActions.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpActions.Size = new System.Drawing.Size(448, 330);
            this.grpActions.TabIndex = 22;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            // 
            // lblRunHelp
            // 
            this.lblRunHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRunHelp.AutoSize = true;
            this.lblRunHelp.Location = new System.Drawing.Point(29, 182);
            this.lblRunHelp.Name = "lblRunHelp";
            this.lblRunHelp.Size = new System.Drawing.Size(413, 20);
            this.lblRunHelp.TabIndex = 17;
            this.lblRunHelp.Text = "Run the builder after configuration and SQL checks pass.";
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.lblLog);
            this.grpLog.Controls.Add(this.txtLog);
            this.grpLog.Location = new System.Drawing.Point(11, 720);
            this.grpLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpLog.Name = "grpLog";
            this.grpLog.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpLog.Size = new System.Drawing.Size(1184, 194);
            this.grpLog.TabIndex = 23;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Log";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLog.Location = new System.Drawing.Point(3, 23);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(538, 20);
            this.lblLog.TabIndex = 2;
            this.lblLog.Text = "Detailed operation log. Use this when troubleshooting installation problems.";
            // 
            // grpInstructions
            // 
            this.grpInstructions.Controls.Add(this.txtInstructions);
            this.grpInstructions.Location = new System.Drawing.Point(754, 90);
            this.grpInstructions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInstructions.Name = "grpInstructions";
            this.grpInstructions.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInstructions.Size = new System.Drawing.Size(448, 274);
            this.grpInstructions.TabIndex = 24;
            this.grpInstructions.TabStop = false;
            this.grpInstructions.Text = "Setup Requirements";
            // 
            // txtInstructions
            // 
            this.txtInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInstructions.Location = new System.Drawing.Point(3, 23);
            this.txtInstructions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.ReadOnly = true;
            this.txtInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInstructions.Size = new System.Drawing.Size(442, 247);
            this.txtInstructions.TabIndex = 0;
            // 
            // lblProjPath
            // 
            this.lblProjPath.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProjPath.AutoSize = true;
            this.lblProjPath.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblProjPath.Location = new System.Drawing.Point(12, 55);
            this.lblProjPath.Name = "lblProjPath";
            this.lblProjPath.Size = new System.Drawing.Size(155, 20);
            this.lblProjPath.TabIndex = 24;
            this.lblProjPath.Text = "Scada project folder:\n";
            this.lblProjPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 931);
            this.Controls.Add(this.grpInstructions);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpActions);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpValidation);
            this.Controls.Add(this.grpConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1122, 924);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JoySys builder";
            this.grpConfiguration.ResumeLayout(false);
            this.tblConfiguration.ResumeLayout(false);
            this.tblConfiguration.PerformLayout();
            this.grpValidation.ResumeLayout(false);
            this.grpValidation.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpActions.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.grpLog.PerformLayout();
            this.grpInstructions.ResumeLayout(false);
            this.grpInstructions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunBuilder;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblCurrentStepTitle;
        private System.Windows.Forms.Label lblCurrentStepValue;
        private System.Windows.Forms.ProgressBar progressBarBuild;
        private System.Windows.Forms.Button btnReloadConfig;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.Button btnOpenConfigFolder;
        private System.Windows.Forms.Button btnBrowseProject;
        private System.Windows.Forms.Button btnTestConfig;
        private System.Windows.Forms.Button btnTestSql;
        private System.Windows.Forms.GroupBox grpConfiguration;
        private System.Windows.Forms.GroupBox grpValidation;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblValidationHelp;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Label lblRunHelp;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.GroupBox grpInstructions;
        private System.Windows.Forms.TextBox txtInstructions;
        private System.Windows.Forms.RichTextBox txtValidation;
        private System.Windows.Forms.TableLayoutPanel tblConfiguration;
        private System.Windows.Forms.Label lblDbfFileValue;
        private System.Windows.Forms.Label lblDatabaseTitle;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblRequiredDbf;
        private System.Windows.Forms.TextBox txtSqlServer;
        private System.Windows.Forms.Label lblSqlServerTitle;
        private System.Windows.Forms.Label lblConfigPathTitle;
        private System.Windows.Forms.TextBox txtConfigPath;
        private System.Windows.Forms.TextBox txtProjectPath;
        private System.Windows.Forms.Label lblDbfStatus;
        private System.Windows.Forms.Label lblDbfStatusTitle;
        private System.Windows.Forms.Label lblProjPath;
    }
}