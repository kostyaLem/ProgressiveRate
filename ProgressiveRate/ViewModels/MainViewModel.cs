using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel
    {
        public CargoStorageRecord SelectedRecord { get; set; }

        public Dictionary<string, string> ColumnHeaderNames { get; } = new Dictionary<string, string>();
        public ObservableCollection<CargoStorageRecord> Records { get; set; } = new ObservableCollection<CargoStorageRecord>();

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames);

            Records = new ObservableCollection<CargoStorageRecord>()
            {
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
            };
        }

        private void AddColumnName()
        {

        }
    }
}
