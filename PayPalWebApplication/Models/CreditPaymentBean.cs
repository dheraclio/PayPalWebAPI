using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;

namespace PayPalWebApplication.Models
{
    public class CreditPaymentBean
    {
        //Item for which you are taking payment
        public Item item { get; set; }
        
        //Credit card         
        public CreditCard creditCard { get; set; }
               
        // Specify your total payment amount and assign the details object
        public Amount amount { get; set; }

        //Description about the payment amount
        public String description { get; set; }

        //Your invoice number which you are generating"
        public String invoice_number { get; set; }         
    }
}
