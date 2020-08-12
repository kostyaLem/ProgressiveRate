using DevExpress.Mvvm.Native;
using ProgressiveRate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProgressiveRate.Services
{
    public class CargoManager : ICargoManager
    {
        private class PeriodDate
        {
            public PeriodDate(DateTime startDate, DateTime endDate, int rate)
            {
                StartDate = startDate;
                EndDate = endDate;
                Rate = rate;
            }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int Rate { get; set; }
        }

        public List<CargoStorageRecord> GenerateReport(Cargo[] cargos, StorageRate[] rates, DateTime beginReportDate, DateTime endReportDate)
        {
            cargos = ValidateCargos(cargos);
            rates = ValidateRates(rates);

            var records = new List<CargoStorageRecord>();
            foreach (var cargo in cargos)
            {
                DateTime endCalcDate;

                if (cargo.DateOfLeaving.HasValue)
                    endCalcDate = cargo.DateOfLeaving > endReportDate ? endReportDate : cargo.DateOfLeaving.Value;
                else
                    endCalcDate = endReportDate;

                endCalcDate = SetToNight(endCalcDate);

                var dates = new List<PeriodDate>();
                var tempStartCalcDate = cargo.DateOfArrival.Value;
                var tempEndCalcDate = endCalcDate;
                for (int i = 0; i < rates.Length; i++)
                {
                    int currentRateDays;

                    if (rates[i].EndOfPeriod.HasValue)
                        currentRateDays = GetDeltaPeriod(rates[i].EndOfPeriod.Value, rates[i].StartOfPeriod.Value);
                    else
                        currentRateDays = GetTimeDifference(endCalcDate, tempStartCalcDate);

                    tempEndCalcDate = SetToNight(tempStartCalcDate.AddDays(currentRateDays - 1));

                    dates.Add(new PeriodDate(tempStartCalcDate, tempEndCalcDate, rates[i].Rate.Value));

                    tempStartCalcDate = SetToNewDay(tempStartCalcDate.AddDays(currentRateDays));
                }

                var startCalcDate = cargo.DateOfArrival.Value <= beginReportDate ? beginReportDate : cargo.DateOfArrival.Value;

                TruncateStartDate(dates, startCalcDate, endCalcDate);

                foreach (var period in dates)
                {
                    records.Add(new CargoStorageRecord()
                    {
                        Name = cargo.Name,
                        DateOfArrival = cargo.DateOfArrival.Value,
                        DateOfLeaving = cargo.DateOfLeaving,
                        StartOfCalc = period.StartDate,
                        EndOfCalc = period.EndDate,
                        StorageDaysCount = GetTimeDifference(period.EndDate, period.StartDate),
                        Rate = period.Rate,
                        Note = $"Период №{dates.IndexOf(period) + 1}"
                    });
                }
            }

            return records;
        }

        private void TruncateStartDate(List<PeriodDate> periods, DateTime startDate, DateTime endDate)
        {
            for (int i = 0; i < periods.Count; i++)
            {
                if (periods.Count > 0)
                {
                    if (IsBetween(periods[i].StartDate, periods[i].EndDate, startDate))
                    {
                        periods[i].StartDate = startDate;
                        break;
                    }
                    else
                    {
                        periods.RemoveAt(i);
                        i--;
                    }
                }
            }

            for (int i = periods.Count - 1; i >= 0; i--)
            {
                if (periods.Count > 0)
                {
                    if (IsBetween(periods[i].StartDate, periods[i].EndDate, endDate))
                    {
                        periods[i].EndDate = endDate;
                        return;
                    }
                    else
                    {
                        periods.RemoveAt(i);
                        i = periods.Count;
                    }
                }
            }
        }

        private DateTime SetToNight(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
        }

        private DateTime SetToNewDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        private int GetDeltaPeriod(int endOf, int startOf)
        {
            if (startOf == 0)
                return endOf;
            else
                return endOf - startOf + 1;
        }

        private bool IsBetween(DateTime d1, DateTime d2, DateTime value)
        {
            return value >= d1 && value <= d2;
        }

        private int GetTimeDifference(DateTime d1, DateTime d2)
        {
            int result = default;
            if (d1 > d2)
                result = (d1 - d2).Days;
            else
                result = (d2 - d1).Days;

            return result != 0 ? result + 1 : 1;
        }

        #region Validates
        private Cargo[] ValidateCargos(Cargo[] cargos)
        {
            if (cargos is null || cargos.Length == 0)
                throw new Exception("Пустая таблица грузов");

            var cargoNames = cargos.Select(x => x.Name).ToList();

            if (cargoNames.Any(string.IsNullOrEmpty))
                throw new Exception("Среди наименований грузов имеется пустое значение");

            var duplicates = cargoNames.Except(cargoNames.Distinct());
            if (duplicates.Count() != 0)
                throw new Exception($"Среди наименований грузов имеются дубликаты: [{string.Join(",", duplicates)}");

            foreach (var cargo in cargos)
            {
                if (!cargo.DateOfArrival.HasValue)
                    throw new Exception($"Пустая дата прихода для груза [{cargo.Name}]");

                if (cargo.DateOfArrival > cargo.DateOfLeaving)
                    throw new Exception($"Неправильная дата прихода для груза [{cargo.Name}]");
            }

            return cargos;
        }

        private StorageRate[] ValidateRates(StorageRate[] rates)
        {
            if (rates is null || rates.Length == 0)
                throw new Exception("Пустая таблица ставок");

            if (rates.Any(x => !x.Number.HasValue))
                throw new Exception("Среди ставок имеется ставка с пустым номером");

            if (rates.Any(x => !x.Rate.HasValue))
                throw new Exception("Среди ставок имеется ставка с пустым значением ставки");

            var duplicates = rates.Except(rates.Distinct());
            if (duplicates.Count() != 0)
                throw new Exception($"Среди номеров имеются дубликаты: [{string.Join(",", duplicates)}");

            rates = rates.OrderBy(x => x.Number).ToArray();

            if (rates.Last().EndOfPeriod.HasValue)
                throw new Exception($"Не указана ставка для бесконечного периода");

            for (int i = 0; i < rates.Length - 1; i++)
            {
                if (rates[i + 1].StartOfPeriod - 1 != rates[i].EndOfPeriod)
                    throw new Exception($"Указана неправильная последовательность хранения " +
                                        $"между ставками №{rates[i].Number} и №{rates[i + 1].Number}");
            }

            return rates;
        }
        #endregion
    }
}
