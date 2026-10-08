using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Timer = System.Windows.Forms.Timer;

namespace WallpaperSwitcher
{
    internal class Backend
    {
        public List<DataEntry> wallpaperSchedule;
        private Timer scheduleTimer;
        private string currentWallpaper = "";
        public Backend() 
        {
            wallpaperSchedule = new List<DataEntry>();
            SetupTimer();
            ScheduleTimer_Tick(this, new EventArgs());
        }
        private void SetupTimer()
        {
            scheduleTimer = new Timer();
            scheduleTimer.Interval = 60000;
            scheduleTimer.Tick += ScheduleTimer_Tick;
            scheduleTimer.Start();
        }

        private void ScheduleTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            string wallpaperToSet = null;
            wallpaperSchedule.Sort((a,b) => a.StartTime.CompareTo(b.StartTime));
            foreach (DataEntry entry in wallpaperSchedule)
            {
                if (IsTimeInRange(now, entry.StartTime, entry.EndTime))
                {
                    wallpaperToSet = entry.ImagePath;
                    break;
                }
            }
            if (wallpaperToSet == null && wallpaperSchedule.Any())
            {
                wallpaperToSet = wallpaperSchedule.First().ImagePath;
            }
            if (wallpaperToSet != currentWallpaper && wallpaperToSet != null)
            {
                WallpaperEngine.SetWallpaper(wallpaperToSet);
                currentWallpaper = wallpaperToSet;
            }
        }
        private string GetConfigFilePath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "WallPaperScheduler");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            return Path.Combine(appFolder, "schedule.json");
        }

        public List<ScheduleEntry> ConvertDictionaryClassSchedule()
        {
            List<ScheduleEntry> result = new List<ScheduleEntry>();
            foreach (DataEntry entry in wallpaperSchedule)
            {
                result.Add(new ScheduleEntry
                {
                    StartTime = entry.StartTime.ToString(@"hh\:mm"),
                    EndTime = entry.EndTime.ToString(@"hh\:mm"),
                    ImagePath = entry.ImagePath
                });
            }
            return result;
        }

        private bool IsTimeInRange(TimeSpan now, TimeSpan start, TimeSpan end)
        {
            if (start <= end)
            {
                return now >= start && now <= end;
            }
            else
            {
                return now >= start || now <= end;
            }
        }

        public bool AddToSchedule(TimeSpan start, TimeSpan end, string imagePath)
        {
            DataEntry master = new DataEntry()
            {
                StartTime = start,
                EndTime = end,
                ImagePath = imagePath
            };
            foreach (DataEntry entry in wallpaperSchedule)
            {
                if (DoEntriesOverlap(master, entry))
                {
                    return false;
                }
            }
            wallpaperSchedule.Add(master);
            return true;
        }

        public bool DoEntriesOverlap(DataEntry a, DataEntry b)
        {
            var blocks1 = GetActiveBlocks(a.StartTime, a.EndTime);
            var blocks2 = GetActiveBlocks(b.StartTime, b.EndTime);

            foreach (var blockA in blocks1)
            {
                foreach (var blockB in blocks2)
                {
                    if (blockA.start < blockB.end && blockA.end > blockB.start)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<(TimeSpan start, TimeSpan end)> GetActiveBlocks(TimeSpan start, TimeSpan end)
        {
            if (start < end)
            {
                return new List<(TimeSpan start, TimeSpan end)> { (start, end) };
            } else
            {
                return new List<(TimeSpan start, TimeSpan end)>
                {
                    (start, TimeSpan.FromHours(24)),
                    (TimeSpan.Zero, end)
                };
            }
        }

        public void SaveSchedule()
        {
            var listToSave = ConvertDictionaryClassSchedule();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(listToSave, options);

            File.WriteAllText(GetConfigFilePath(), jsonString);
        }

        public void LoadSchedule()
        {
            string filePath = GetConfigFilePath();

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(filePath);
                    var loadedList = JsonSerializer.Deserialize<List<ScheduleEntry>>(jsonString);
                    wallpaperSchedule = new List<DataEntry>();
                    foreach (var entry in loadedList)
                    {
                        if (TimeSpan.TryParse(entry.StartTime, out TimeSpan start) && TimeSpan.TryParse(entry.EndTime, out TimeSpan end) && File.Exists(entry.ImagePath))
                        {
                            DataEntry data = new DataEntry()
                            {
                                StartTime = start,
                                EndTime = end,
                                ImagePath = entry.ImagePath
                            };
                            wallpaperSchedule.Add(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading schedule: {ex.Message}");
                }
            }
        }
    }
}
