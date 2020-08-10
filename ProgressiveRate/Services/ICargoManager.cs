using ProgressiveRate.Models;
using System;
using System.Collections.Generic;

namespace ProgressiveRate.Services
{
    public interface ICargoManager
    {
        List<CargoStorageRecord> GenerateReport(Cargo[] cargos, StorageRate[] rates, DateTime beginReportDate, DateTime endReportDate);
    }
}