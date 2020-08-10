using DevExpress.Mvvm.Native;
using ProgressiveRate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProgressiveRate.Services
{
    public class CargoManager
    {
        public List<CargoStorageRecord> GenerateReport(Cargo[] cargos, StorageRate[] rates, DateTime beginReportDate, DateTime endReportDate)
        {
            cargos = ValidateCargos(cargos);
            rates = ValidateRates(rates);

            cargos = cargos.Where(c => c.DateOfArrival >= beginReportDate).ToArray();

            var records = new List<CargoStorageRecord>();
            foreach (var cargo in cargos)
            {
                DateTime startCalcDate = cargo.DateOfArrival.Value;
                DateTime endCalcDate;

                if (cargo.DateOfLeaving.HasValue)
                    endCalcDate = cargo.DateOfLeaving > endReportDate ? endReportDate : cargo.DateOfLeaving.Value;
                else
                    endCalcDate = endReportDate;

                endCalcDate = endCalcDate.Add(new TimeSpan(23, 59, 0));

                for (int i = 0; i < rates.Length; i++)
                {
                    int currentRateDays;

                    if (rates[i].EndOfPeriod.HasValue)
                        currentRateDays = GetDeltaPeriod(rates[i].EndOfPeriod.Value, rates[i].StartOfPeriod.Value);
                    else
                        currentRateDays = GetTimeDifference(endCalcDate, startCalcDate);

                    var tempEndCalcDate = SetToNight(startCalcDate.AddDays(currentRateDays - 1));

                    if (tempEndCalcDate > endCalcDate)
                    {
                        tempEndCalcDate = endCalcDate;
                        currentRateDays = GetTimeDifference(endCalcDate, startCalcDate);
                    }

                    records.Add(new CargoStorageRecord()
                    {
                        Name = cargo.Name,
                        DateOfArrival = cargo.DateOfArrival.Value,
                        DateOfLeaving = cargo.DateOfLeaving,
                        StartOfCalc = startCalcDate,
                        EndOfCalc = tempEndCalcDate,
                        StorageDaysCount = currentRateDays,
                        Rate = rates[i].Rate.Value,
                        Note = $"Период №{i + 1}"
                    });

                    startCalcDate = SetToNewDay(tempEndCalcDate.AddDays(1));

                    if (tempEndCalcDate == endCalcDate)
                        break;
                }
            }

            return records;
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

        private int GetTimeDifference(DateTime d1, DateTime d2)
        {
            int result = default;
            if (d1 > d2)
                result = (d1 - d2).Days;
            else
                result = (d2 - d1).Days;

            return result != 0 ? result + 1 : 1;
        }

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
    }
}
