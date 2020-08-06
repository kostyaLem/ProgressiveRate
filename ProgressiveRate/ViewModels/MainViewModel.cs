using GalaSoft.MvvmLight.Command;
using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using ProgressiveRate.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICustomDialogService _dialogService = new CustomDialogService();

        public CargoStorageRecord SelectedRecord { get; set; }

        public Dictionary<string, string> ColumnHeaderNames { get; } = new Dictionary<string, string>();
        public ObservableCollection<CargoStorageRecord> Records { get; set; } = new ObservableCollection<CargoStorageRecord>();

        public ICommand OpenExcelFileCommand { get; set; }

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames);

            OpenExcelFileCommand = new RelayCommand(() => _dialogService.ShowMessage("alert", "some", System.Windows.MessageBoxImage.Information), () => true);

            Records = new ObservableCollection<CargoStorageRecord>()
            {
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
                new CargoStorageRecord() { Note = " qwe" },
            };
        }
    }
}
