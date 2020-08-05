using System;

namespace ProgressiveRate.Models
{
    /// <summary>
    /// Груз
    /// </summary>
    public class Cargo
    {
        // Название груза
        public string Name { get; set; }

        // Дата прихода
        public DateTime DateOfArrival { get; set; }

        // Дата ухода
        public DateTime? DateOfLeaving { get; set; }
    }
}
