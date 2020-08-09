using System;
using System.ComponentModel;

namespace ProgressiveRate.Models
{
    /// <summary>
    /// Ставка хранения груза
    /// </summary>
    public class StorageRate
    {
        [DisplayName("№")]
        public int? Number { get; set; }

        [DisplayName("Начало периода")]
        public int? StartOfPeriod { get; set; }

        [DisplayName("Окончание периода")]
        public int? EndOfPeriod { get; set; }

        [DisplayName("Ставка")]
        public double? Rate { get; set; }
    }
}
