using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WallpaperSwitcher
{
    public partial class WallpaperSwitcher : Form
    {
        private NotifyIcon trayIcon;
        private Backend backendManager;
        private BindingList<ScheduleEntry> uiScheduleList;
        private DateTimePicker dtPicker;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public WallpaperSwitcher()
        {
            InitializeComponent();
            SetupSystemTray();
            backendManager = new Backend();
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(30, 30, 30);
            TopPanel.MouseDown += TopPanel_MouseDown;
            SetupForm();
            backendManager.LoadSchedule();
            SetupScheduleGrid();
            SetupTimePicker();
        }

        private void SetupTimePicker()
        {
            dtPicker = new DateTimePicker();
            dtPicker.Format = DateTimePickerFormat.Time;
            dtPicker.ShowUpDown = true;
            dtPicker.Visible = false;

            dtPicker.ValueChanged += DTPicker_ValueChanged;

            dtPicker.Leave += (s, e) => dtPicker.Visible = false;
            ScheduleView.Scroll += (s, e) => dtPicker.Visible = false;

            ScheduleView.Controls.Add(dtPicker);
        }

        private void DTPicker_ValueChanged(object sender, EventArgs e)
        {
            if (ScheduleView.CurrentCell != null)
            {
                ScheduleView.CurrentCell.Value = dtPicker.Value.ToString("HH:mm");
            }
        }

        private void SetupScheduleGrid()
        {
            uiScheduleList = new BindingList<ScheduleEntry>(backendManager.ConvertDictionaryClassSchedule());
            ScheduleView.DataSource = uiScheduleList;
            ScheduleView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetStartup(bool enableStartup)
        {
            string appName = "WallpaperSwitcher";
            string appPath = Application.ExecutablePath;

            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (enableStartup)
                {
                    rk.SetValue(appName, appPath);
                }
                else
                {
                    rk.DeleteValue(appName, false);
                }
            }
        }

        private void SetupForm()
        {
            StyleModernButton(TerminateButton);
            StyleModernButton(HideButton);
            StyleModernButton(MinimiseButton);
            StyleModernButton(SaveButton);
            StyleModernGrid(ScheduleView);
        }

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); ;
            }
        }

        private void StyleModernButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.BackColor = Color.FromArgb(45, 45, 48);
            btn.ForeColor = Color.White;

            btn.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            btn.Cursor = Cursors.Hand;

            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 66);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 122, 204);
        }

        private void StyleModernGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.FromArgb(37, 37, 38);
            grid.GridColor = Color.FromArgb(50, 50, 50);
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AllowUserToResizeRows = false;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204); // Don't highlight header on click
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            grid.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            grid.DefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;

            grid.RowTemplate.Height = 35;
        }

        private List<int> ValidateSchedule()
        {
            List<int> result = new List<int>();
            for (int i = 0; i < uiScheduleList.Count; i++)
            {
                if (!isEntryValid(uiScheduleList[i]))
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private bool isEntryValid(ScheduleEntry entry)
        {
            return TimeSpan.TryParse(entry.StartTime, out var res) && TimeSpan.TryParse(entry.EndTime, out var res2) && File.Exists(entry.ImagePath);
        }

        private void ScheduleGrid_Add(object sender, DataGridViewCellEventArgs e)
        {
            int index = uiScheduleList.Count - 1;
            ScheduleEntry entry = uiScheduleList[index];

            if (TimeSpan.TryParse(entry.StartTime, out TimeSpan start) && TimeSpan.TryParse(entry.EndTime, out TimeSpan end) && File.Exists(entry.ImagePath))
            {
                if (!backendManager.AddToSchedule(start, end, entry.ImagePath))
                {
                    MessageBox.Show("Failed to add event to schedule!", "FAILURE");
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            List<int> scheduleValidResult = ValidateSchedule();
            if (scheduleValidResult.Count == 0)
            {
                backendManager.SaveSchedule();
                MessageBox.Show("Schedule saved successfully!", "Success");
            }
            else
            {
                string assembleIndex = "";
                foreach (int i in scheduleValidResult)
                {
                    assembleIndex = i + " ";
                }
                MessageBox.Show($"Mistake at lines " + assembleIndex + ".");
            }
        }

        private void CloseButoon_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void TerminateButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void SetupSystemTray()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Text = "Wallpaper Scheduler";
            trayIcon.Visible = true;

            trayIcon.DoubleClick += (s, e) =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };

            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add(
                "Exit",
                null,
                (s, e) => Application.Exit()
            );
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void ScheduleView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = ScheduleView.Columns[e.ColumnIndex].DataPropertyName;
                if (columnName == "ImagePath")
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Title = "Select Wallpaper Image";
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                        openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                        openFileDialog.FilterIndex = 1;
                        openFileDialog.RestoreDirectory = true;

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            uiScheduleList[e.RowIndex].ImagePath = openFileDialog.FileName;
                            ScheduleView.InvalidateRow(e.RowIndex);
                        }
                    }
                }
                else if (columnName == "StartTime" || columnName == "EndTime")
                {
                    Rectangle cellRectangle = ScheduleView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtPicker.Location = cellRectangle.Location;
                    dtPicker.Width = cellRectangle.Width;
                    dtPicker.Height = cellRectangle.Height;

                    string cellValue = ScheduleView[e.ColumnIndex, e.RowIndex].Value?.ToString();
                    if (TimeSpan.TryParse(cellValue, out TimeSpan time))
                    {
                        dtPicker.Value = DateTime.Today.Add(time);
                    }
                    else
                    {
                        dtPicker.Value = DateTime.Now;
                    }
                    dtPicker.Visible = true;
                    dtPicker.Focus();
                }
                else
                {
                    dtPicker.Visible = false;
                }
            }
        }

        private void ScheduleView_SelectionChanged(object sender, EventArgs e)
        {
            if (ScheduleView.CurrentRow != null && ScheduleView.CurrentRow.Index >= 0)
            {
                var entry = ScheduleView.CurrentRow.DataBoundItem as ScheduleEntry;

                if (entry != null && File.Exists(entry.ImagePath))
                {
                    ImageBox.ImageLocation = entry.ImagePath;
                }
                else
                {
                    ImageBox.ImageLocation = null;
                }
            }
            else
            {
                ImageBox.ImageLocation = null;
            }
        }

        private void StartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SetStartup(StartupCheckbox.Checked);
        }
    }
}