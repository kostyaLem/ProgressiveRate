using System;

namespace ProgressiveRate.Models
{
    /// <summary>
    /// Ставка хранения груза
    /// </summary>
    public class StorageRate
    {
        // Номер тарифа
        public int Number { get; set; }

        // Начало периода
        public DateTime StartOfPeriod { get; set; }

        // Окончание периода
        public DateTime? EndOfPeriod { get; set; }

        // Ставка
        public double Rate { get; set; }
    }
}
