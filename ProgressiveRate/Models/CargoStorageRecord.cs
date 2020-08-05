using System;
using System.ComponentModel;

namespace ProgressiveRate.Models
{
    /// <summary>
    /// Данные о хранении груза
    /// </summary>
    public class CargoStorageRecord
    {
        [DisplayName("Груз")]
        public string Name { get; set; }

        [DisplayName("Дата прихода на склад")]
        public DateTime DateOfArrival { get; set; }

        [DisplayName("Дата ухода со склада")]
        public DateTime? DateOfLeaving { get; set; }

        [DisplayName("Начало расчета")]
        public DateTime StartOfCalc { get; set; }

        [DisplayName("Окончание расчета")]
        public DateTime EndOfCalc { get; set; }

        [DisplayName("Кол-во дней хранения")]
        public int StorageDaysCount { get; set; }

        [DisplayName("Ставка")]
        public double Rate { get; set; }

        [DisplayName("Примечание")]
        public string Note { get; set; }
    }
}