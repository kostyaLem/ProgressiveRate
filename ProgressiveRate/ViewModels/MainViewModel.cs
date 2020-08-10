using DevExpress.Mvvm;
using ProgressiveRate.Helpers;
using ProgressiveRate.Models;
using ProgressiveRate.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ProgressiveRate.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private DateTime _startOfDate = new DateTime(2017, 10, 1);
        private DateTime _endOfDate = new DateTime(2017, 10, 15);
        #endregion

        private CancellationTokenSource _tkn;

        CargoManager _cargoManager = new CargoManager();
        IExcelReader _excelReader = new ExcelReader();
        ICustomDialogService _dialogService = new CustomDialogService();

        public DateTime StartOfDate { get { return _startOfDate; } set { SetValue(ref _startOfDate, value); } }
        public DateTime EndOfDate { get { return _endOfDate; } set { SetValue(ref _endOfDate, value); } }
        public string SelectedFileName { get { return GetValue<string>(); } set { SetValue(value); } }

        public bool IsWaiting { get { return GetValue<bool>(); } set { SetValue(value); } }
        public double ProcessScore { get { return GetValue<double>(); } set { SetValue(value); } }

        public Dictionary<string, string> ColumnHeaderNames { get; }
        public ObservableCollection<CargoStorageRecord> Records { get; private set; }

        #region Commands
        public ICommand GenerateReportCommand { get; set; }
        public ICommand ClearSelectedFileCommand { get; set; }
        public ICommand OpenExcelFileCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            DisplayNameHelper.FillNames(typeof(CargoStorageRecord), ColumnHeaderNames = new Dictionary<string, string>());
            Records = new ObservableCollection<CargoStorageRecord>();

            _excelReader.FileProcessed += (s, value) => ProcessScore = value;

            GenerateReportCommand = new DelegateCommand(Run, () => StartOfDate < EndOfDate && !string.IsNullOrEmpty(SelectedFileName));
            ClearSelectedFileCommand = new DelegateCommand(() => SelectedFileName = string.Empty, () => !string.IsNullOrEmpty(SelectedFileName));
            OpenExcelFileCommand = new DelegateCommand(SelectFile);
            CancelCommand = new DelegateCommand(() => _tkn.Cancel());
        }

        private async void Run()
        {
            IsWaiting = true;
            _tkn = new CancellationTokenSource();

            try
            {
                var cargosDataTable = await _excelReader.ReadTableAsync(SelectedFileName, "Груз", 3, _tkn.Token);
                var cargos = DataTableMapper.MapTo<Cargo>(cargosDataTable);

                var ratesDataTable = await _excelReader.ReadTableAsync(SelectedFileName, "Тариф", 4, _tkn.Token);
                var rates = DataTableMapper.MapTo<StorageRate>(ratesDataTable);

                var records = _cargoManager.GenerateReport(cargos, rates, StartOfDate, EndOfDate);
                Records = new ObservableCollection<CargoStorageRecord>(records);
                RaisePropertyChanged(nameof(Records));

                _dialogService.ShowMessage("Справка", "Отчет успешно сформирован", MessageBoxImage.Information);
            }
            catch (OperationCanceledException)
            {
                Records.Clear();
            }
            catch (Exception e)
            {
                _dialogService.ShowMessage("Ошибка формирования отчета", e.Message, MessageBoxImage.Error);
            }
            finally
            {
                IsWaiting = false;
            }
        }

        private void SelectFile()
        {
            if (_dialogService.OpenFileDialog(FileExtensions.ExcelExtensions))
            {
                SelectedFileName = _dialogService.FilePath;
            }
        }
    }
}