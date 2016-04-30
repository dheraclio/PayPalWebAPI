using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPalWebApplication.Controllers;
using System.Web.Mvc;
using PayPalWebApplication.Models;
using PayPal.Api;

namespace PayPalWebApplication.Tests.Controllers
{
    [TestClass]
    public class PayPalControllerTest
    {       
        [TestMethod]
        public void ExecuteCreditCardPaymentWithContext()
        {
            CreditPaymentBean payment = new CreditPaymentBean();
            payment.description = "Description about the payment amount.";
            payment.invoice_number = "your invoice number which you are generating";

            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            payment.item = new Item();
            payment.item.name = "Demo Item";
            payment.item.currency = "USD";
            payment.item.price = "5";
            payment.item.quantity = "1";
            payment.item.sku = "sku";

            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            payment.creditCard = new CreditCard();

            payment.creditCard.billing_address = new Address();
            payment.creditCard.billing_address.city = "NewYork";
            payment.creditCard.billing_address.country_code = "US";
            payment.creditCard.billing_address.line1 = "23rd street kew gardens";
            payment.creditCard.billing_address.postal_code = "43210";
            payment.creditCard.billing_address.state = "NY";

            payment.creditCard.cvv2 = "874";  //card cvv2 number
            payment.creditCard.expire_month = 1; //card expire date
            payment.creditCard.expire_year = 2020; //card expire year
            payment.creditCard.first_name = "Aman";
            payment.creditCard.last_name = "Thakur";
            payment.creditCard.number = "1234567890123456"; //enter your credit card number here
            payment.creditCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify your total payment amount and assign the details object
            payment.amount = new Amount();
            payment.amount.currency = "USD";
            payment.amount.total = "7";
            payment.amount.details = new Details();
            payment.amount.details.shipping = "1";
            payment.amount.details.subtotal = "5";
            payment.amount.details.tax = "1";

            
            APIContext apiContext = Configuration.GetAPIContext();
            PayPalController controller = new PayPalController();
            ActionResult result = controller.ExecuteCreditCardPayment(apiContext,payment);

            // Assert
            Assert.IsNotNull(result, "Null result");
        }

        [TestMethod]
        public void ExecuteCreditCardPaymentDefaultContext()
        {                       
            CreditPaymentBean payment = new CreditPaymentBean();
            payment.description = "Description about the payment amount.";
            payment.invoice_number = "your invoice number which you are generating";

            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            payment.item = new Item();
            payment.item.name = "Demo Item";
            payment.item.currency = "USD";
            payment.item.price = "5";
            payment.item.quantity = "1";
            payment.item.sku = "sku";

            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            payment.creditCard = new CreditCard();

            payment.creditCard.billing_address = new Address();
            payment.creditCard.billing_address.city = "NewYork";
            payment.creditCard.billing_address.country_code = "US";
            payment.creditCard.billing_address.line1 = "23rd street kew gardens";
            payment.creditCard.billing_address.postal_code = "43210";
            payment.creditCard.billing_address.state = "NY";

            payment.creditCard.cvv2 = "874";  //card cvv2 number
            payment.creditCard.expire_month = 1; //card expire date
            payment.creditCard.expire_year = 2020; //card expire year
            payment.creditCard.first_name = "Aman";
            payment.creditCard.last_name = "Thakur";
            payment.creditCard.number = "1234567890123456"; //enter your credit card number here
            payment.creditCard.type = "visa"; //credit card type here paypal allows 4 types

            // Specify your total payment amount and assign the details object
            payment.amount = new Amount();
            payment.amount.currency = "USD";
            payment.amount.total = "7";
            payment.amount.details = new Details();
            payment.amount.details.shipping = "1";
            payment.amount.details.subtotal = "5";
            payment.amount.details.tax = "1";
            
            PayPalController controller = new PayPalController();
            ActionResult result = controller.ExecuteCreditCardPayment(payment);

            // Assert
            Assert.IsNotNull(result, "Null result");
        }

        [TestMethod]
        public void ExecutePayPalPaymentDefaultContext()
        {
            Assert.Fail("Not implemented");
        }
        
    }
}
