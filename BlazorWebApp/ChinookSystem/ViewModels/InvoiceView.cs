using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.ViewModels
{
    public class InvoiceView
    {
        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "Name is too long")]
        public string SalePerson { get; set; }

        [Required]
        public string PaymentType { get; set; }
    }
}
