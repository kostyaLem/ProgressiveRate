using System;
using System.ComponentModel;

namespace ProgressiveRate.Models
{
    /// <summary>
    /// Груз
    /// </summary>
    public class Cargo
    {
        [DisplayName("Груз")]
        public string Name { get; set; }

        [DisplayName("Дата прихода на склад")]
        public DateTime? DateOfArrival { get; set; }

        [DisplayName("Дата ухода со склада")]
        public DateTime? DateOfLeaving { get; set; }
    }
}
