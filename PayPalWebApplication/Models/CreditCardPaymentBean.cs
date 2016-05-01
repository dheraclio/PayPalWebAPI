using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPal.Api;

namespace PayPalWebApplication.Models
{
    public class CreditCardPaymentBean: PaymentBean  
    {        
        //Credit card         
        public CreditCard creditCard { get; set; }

        
    }
}
