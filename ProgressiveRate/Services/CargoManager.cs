using DevExpress.Mvvm.Native;
using ProgressiveRate.Models;
using System;
using System.Data;
using System.Linq;

namespace ProgressiveRate.Services
{
    public class CargoManager
    {
        public CargoStorageRecord[] GenerateReport(Cargo[] cargos, StorageRate[] rates)
        {
            cargos = ValidateCargos(cargos);
            rates = ValidateRates(rates);

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
