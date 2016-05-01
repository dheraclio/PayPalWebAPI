using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Api;
using PayPalWebApplication.Controllers;
using PayPalWebApplication.Models;
using System.Web.Mvc;

namespace PayPalWebApplication.Tests.Controllers
{
    [TestClass]
    public class PayPalControllerTest
    {      
        
        [TestMethod]
        public void PaymentWithCreditCard()
        {
            PayPalController controller = new PayPalController();
            
            ActionResult result = controller.PaymentWithCreditCard();
            Assert.IsNotNull(result, "Null payment result");
        }
        
        [TestMethod]
        public void ExecutePayPalPayment()
        {            
            PayPalController controller = new PayPalController();
                        
            ActionResult result = controller.PaymentWithPaypal();
            Assert.IsNotNull(result, "Null payment result");
        }
        
    }
}
