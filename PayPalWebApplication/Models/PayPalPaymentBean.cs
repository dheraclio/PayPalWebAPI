using PayPal.Api;

namespace PayPalWebApplication.Models
{
    public class PayPalPaymentBean: PaymentBean
    {
        //Payment session identification
        public string guid { get; set;  }

        //Paypal payer identification         
        public string payerId { get; set; }

        //URLs in case of canceling or confirmation of payment
        public RedirectUrls redirect_urls { get; set; }
    }
}
