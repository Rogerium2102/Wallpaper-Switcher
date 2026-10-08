using System;
using System.Collections.Generic;
using System.Text;

namespace WallpaperSwitcher
{
    internal class ScheduleEntry
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ImagePath { get; set; }

        public ScheduleEntry() { }
    }
}
