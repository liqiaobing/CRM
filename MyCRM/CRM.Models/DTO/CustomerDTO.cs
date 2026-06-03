using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.DTO
{
    public class CustomerDTO
    {
        public string? OwnerName { get; set; }
        public string? ActivityName { get; set; }
        public string? FullName { get; set; }
        public string? Appellation { get; set; }
        public string? Phone { get; set; }
        public string? Weixin { get; set; }
        public string? Qq { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? Job { get; set; }
        public Decimal? YearIncome { get; set; }
        public string? Address { get; set; }
        public string? NeedLoan { get; set; }
        public string? ProductName { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }
        public DateTime? NextContactTime { get; set; }

    }
}
