using GalaSoft.MvvmLight.Command;
using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using ProgressiveRate.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private CancellationTokenSource _tkn;

        private readonly ExcelService _excelService = new ExcelService();

        public int ProcessValue { get; set; }


        public CargoStorageRecord SelectedRecord { get; set; }

        public Dictionary<string, string> ColumnHeaderNames { get; } = new Dictionary<string, string>();
        public ObservableCollection<CargoStorageRecord> Records { get; set; } = new ObservableCollection<CargoStorageRecord>();

        public ICommand GenerateReport { get; set; }
        public ICommand OpenExcelFileCommand { get; set; }

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames);

            GenerateReport = new RelayCommand(Run, () => true);
            OpenExcelFileCommand = new RelayCommand(Sho, () => true);
        }

        private object _sync = new object();

        private async void Run()
        {
            _tkn = new CancellationTokenSource();

            _excelService

        }

        private async void Sho()
        {

            for (int i = 0; i < 1000; i++)
            {
                lock (_sync)
                {
                    ProcessValue++;
                    OnPropertyChanged(nameof(ProcessValue));
                }

                await Task.Delay(200);
            }
        }
    }
}
