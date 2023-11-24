namespace FootParser
{
    partial class FootParserForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            exitButton = new Button();
            parseTeamsButton = new Button();
            parseRefereesButton = new Button();
            parsePlayersButton = new Button();
            parseCoachesButton = new Button();
            leagueComboBox = new ComboBox();
            seasonComboBox = new ComboBox();
            addSeasonsToDbButton = new Button();
            SuspendLayout();
            // 
            // exitButton
            // 
            exitButton.Location = new Point(522, 434);
            exitButton.Margin = new Padding(4, 3, 4, 3);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(88, 27);
            exitButton.TabIndex = 0;
            exitButton.Text = "Выход";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            // 
            // parseTeamsButton
            // 
            parseTeamsButton.Location = new Point(159, 190);
            parseTeamsButton.Margin = new Padding(4, 3, 4, 3);
            parseTeamsButton.Name = "parseTeamsButton";
            parseTeamsButton.Size = new Size(90, 47);
            parseTeamsButton.TabIndex = 1;
            parseTeamsButton.Text = "Распарсить команды";
            parseTeamsButton.UseVisualStyleBackColor = true;
            parseTeamsButton.Click += ParseTeamsButton_Click;
            // 
            // parseRefereesButton
            // 
            parseRefereesButton.Location = new Point(310, 190);
            parseRefereesButton.Margin = new Padding(4, 3, 4, 3);
            parseRefereesButton.Name = "parseRefereesButton";
            parseRefereesButton.Size = new Size(90, 47);
            parseRefereesButton.TabIndex = 2;
            parseRefereesButton.Text = "Распарсить судей";
            parseRefereesButton.UseVisualStyleBackColor = true;
            parseRefereesButton.Click += ParseRefereesButton_Click;
            // 
            // parsePlayersButton
            // 
            parsePlayersButton.Location = new Point(470, 190);
            parsePlayersButton.Margin = new Padding(4, 3, 4, 3);
            parsePlayersButton.Name = "parsePlayersButton";
            parsePlayersButton.Size = new Size(90, 47);
            parsePlayersButton.TabIndex = 3;
            parsePlayersButton.Text = "Распарсить игроков";
            parsePlayersButton.UseVisualStyleBackColor = true;
            parsePlayersButton.Click += ParsePlayersButton_Click;
            // 
            // parseCoachesButton
            // 
            parseCoachesButton.Location = new Point(630, 190);
            parseCoachesButton.Margin = new Padding(4, 3, 4, 3);
            parseCoachesButton.Name = "parseCoachesButton";
            parseCoachesButton.Size = new Size(90, 47);
            parseCoachesButton.TabIndex = 4;
            parseCoachesButton.Text = "Распарсить тренеров";
            parseCoachesButton.UseVisualStyleBackColor = true;
            parseCoachesButton.Click += ParseCoachesButton_Click;
            // 
            // leagueComboBox
            // 
            leagueComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            leagueComboBox.FormattingEnabled = true;
            leagueComboBox.Location = new Point(159, 88);
            leagueComboBox.Margin = new Padding(4, 3, 4, 3);
            leagueComboBox.Name = "leagueComboBox";
            leagueComboBox.Size = new Size(140, 23);
            leagueComboBox.TabIndex = 5;
            leagueComboBox.SelectedIndexChanged += LeagueComboBox_SelectedIndexChanged;
            // 
            // seasonComboBox
            // 
            seasonComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            seasonComboBox.FormattingEnabled = true;
            seasonComboBox.Location = new Point(470, 88);
            seasonComboBox.Margin = new Padding(4, 3, 4, 3);
            seasonComboBox.Name = "seasonComboBox";
            seasonComboBox.Size = new Size(140, 23);
            seasonComboBox.TabIndex = 6;
            seasonComboBox.SelectedIndexChanged += SeasonComboBox_SelectedIndexChanged;
            // 
            // addSeasonsToDbButton
            // 
            addSeasonsToDbButton.Location = new Point(338, 322);
            addSeasonsToDbButton.Margin = new Padding(4, 3, 4, 3);
            addSeasonsToDbButton.Name = "addSeasonsToDbButton";
            addSeasonsToDbButton.Size = new Size(195, 27);
            addSeasonsToDbButton.TabIndex = 7;
            addSeasonsToDbButton.Text = "Добавить сезоны в базу данных";
            addSeasonsToDbButton.UseVisualStyleBackColor = true;
            addSeasonsToDbButton.Click += AddSeasonsToDbButton_Click;
            // 
            // FootParserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(addSeasonsToDbButton);
            Controls.Add(seasonComboBox);
            Controls.Add(leagueComboBox);
            Controls.Add(parseCoachesButton);
            Controls.Add(parsePlayersButton);
            Controls.Add(parseRefereesButton);
            Controls.Add(parseTeamsButton);
            Controls.Add(exitButton);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FootParserForm";
            ShowIcon = false;
            Text = "SiteParser";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button parseTeamsButton;
        private System.Windows.Forms.Button parseRefereesButton;
        private System.Windows.Forms.Button parsePlayersButton;
        private System.Windows.Forms.Button parseCoachesButton;
        private System.Windows.Forms.ComboBox leagueComboBox;
        private System.Windows.Forms.ComboBox seasonComboBox;
        private Button addSeasonsToDbButton;
    }
}