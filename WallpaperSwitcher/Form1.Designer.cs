namespace WallpaperSwitcher
{
    partial class WallpaperSwitcher
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog = new OpenFileDialog();
            TopPanel = new Panel();
            StartupCheckbox = new CheckBox();
            ImageBox = new PictureBox();
            SaveButton = new Button();
            ScheduleView = new DataGridView();
            HideButton = new Button();
            MinimiseButton = new Button();
            TerminateButton = new Button();
            TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImageBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScheduleView).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = ".png|";
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(StartupCheckbox);
            TopPanel.Controls.Add(ImageBox);
            TopPanel.Controls.Add(SaveButton);
            TopPanel.Controls.Add(ScheduleView);
            TopPanel.Controls.Add(HideButton);
            TopPanel.Controls.Add(MinimiseButton);
            TopPanel.Controls.Add(TerminateButton);
            TopPanel.Location = new Point(12, 12);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(776, 426);
            TopPanel.TabIndex = 0;
            // 
            // StartupCheckbox
            // 
            StartupCheckbox.AutoSize = true;
            StartupCheckbox.ForeColor = SystemColors.HighlightText;
            StartupCheckbox.Location = new Point(582, 347);
            StartupCheckbox.Name = "StartupCheckbox";
            StartupCheckbox.Size = new Size(183, 36);
            StartupCheckbox.TabIndex = 7;
            StartupCheckbox.Text = "Startup App?";
            StartupCheckbox.UseVisualStyleBackColor = true;
            StartupCheckbox.CheckedChanged += StartupCheckbox_CheckedChanged;
            // 
            // ImageBox
            // 
            ImageBox.Location = new Point(541, 93);
            ImageBox.Name = "ImageBox";
            ImageBox.Size = new Size(200, 230);
            ImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            ImageBox.TabIndex = 6;
            ImageBox.TabStop = false;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(356, 341);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(150, 46);
            SaveButton.TabIndex = 5;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // ScheduleView
            // 
            ScheduleView.AllowDrop = true;
            ScheduleView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ScheduleView.Location = new Point(26, 23);
            ScheduleView.Name = "ScheduleView";
            ScheduleView.RowHeadersWidth = 82;
            ScheduleView.Size = new Size(480, 300);
            ScheduleView.TabIndex = 3;
            ScheduleView.CellDoubleClick += ScheduleView_CellDoubleClick;
            ScheduleView.CellEndEdit += ScheduleGrid_Add;
            ScheduleView.SelectionChanged += ScheduleView_SelectionChanged;
            // 
            // HideButton
            // 
            HideButton.Location = new Point(623, 14);
            HideButton.Name = "HideButton";
            HideButton.Size = new Size(68, 46);
            HideButton.TabIndex = 2;
            HideButton.Text = "T";
            HideButton.UseVisualStyleBackColor = true;
            HideButton.Click += CloseButoon_Click;
            // 
            // MinimiseButton
            // 
            MinimiseButton.Location = new Point(549, 14);
            MinimiseButton.Name = "MinimiseButton";
            MinimiseButton.Size = new Size(68, 46);
            MinimiseButton.TabIndex = 1;
            MinimiseButton.Text = "_";
            MinimiseButton.UseVisualStyleBackColor = true;
            MinimiseButton.Click += MinimizeButton_Click;
            // 
            // TerminateButton
            // 
            TerminateButton.Location = new Point(693, 14);
            TerminateButton.Name = "TerminateButton";
            TerminateButton.Size = new Size(68, 46);
            TerminateButton.TabIndex = 0;
            TerminateButton.Text = "X";
            TerminateButton.UseVisualStyleBackColor = true;
            TerminateButton.Click += TerminateButton_Click;
            // 
            // WallpaperSwitcher
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TopPanel);
            Name = "WallpaperSwitcher";
            Text = "Wallpaper Switcher";
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImageBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScheduleView).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private OpenFileDialog openFileDialog;
        private Panel TopPanel;
        private Button TerminateButton;
        private Button HideButton;
        private Button MinimiseButton;
        private DataGridView ScheduleView;
        private Button SaveButton;
        private PictureBox ImageBox;
        private CheckBox StartupCheckbox;
    }
}
