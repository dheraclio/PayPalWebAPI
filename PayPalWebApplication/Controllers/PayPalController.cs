using log4net;
using PayPal.Api;
using PayPalWebApplication.Models;
using PayPalWebApplication.Factories;
using System;
using System.Web.Mvc;

namespace PayPalWebApplication.Controllers
{
    public class PayPalController : Controller
    {
        // Logs output statements, errors, debug info to a text file    
        private static ILog logger = LogManager.GetLogger(typeof(PayPalController));

        // Static constructor for setting the readonly static members.
        static PayPalController()
        {
            // Load the log4net configuration settings from Web.config or App.config    
            log4net.Config.XmlConfigurator.Configure();
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Function for running sample CreditCardPayment
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentWithCreditCard()
        {
            //Create sample payment in local model
            CreditCardPaymentBean samplePayment = ModelFactory.createSampleCreditCardPayment();
            
            //Translate to paypal payment
            Payment translatedPayment = ModelFactory.translateToPayPalPayment(samplePayment);

            //Executes payment
            return ExecutePayment(translatedPayment);
        }
                
        /// <summary>
        /// Payment with PayPal account takes two steps.
        /// First get the payerId and payment identification
        /// Second perform payment confirmation
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentWithPaypal()
        {
            //Recover payerId from request to determine first or second step
            string payerId = Request.Params["PayerID"];
            try
            {
                if (string.IsNullOrEmpty(payerId))
                {
                    //Return address
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";
                                        
                    //Payment identification to be stored at the session
                    string guid = Convert.ToString((new Random()).Next(100000));

                    string redirectUrl = baseURI + "guid=" + guid;

                    //Create sample payment
                    PayPalPaymentBean samplePayment = ModelFactory.createSamplePayPalPayment(guid,redirectUrl);
                    
                    return RequestPayPalPayment(samplePayment);
                }
                else
                {
                    string guid = Session[Request.Params["guid"]] as string;
                    if (string.IsNullOrEmpty(guid))
                    {
                        logger.Info("guid not found in session after payment request");
                        throw new Exception("guid not found in session after payment request");
                    }

                    APIContext apiContext = Configuration.GetAPIContext();
                    if (apiContext == null)
                    {
                        logger.Info("Paypal API context not found");
                        throw new Exception("Paypal API context not found");
                    }
                                        
                    var paymentExecution = new PaymentExecution() { payer_id = payerId };
                    Payment payment = new Payment() { id = guid };

                    Payment executedPayment = payment.Execute(apiContext, paymentExecution);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                    return View("SuccessView");
                }            
            }
            catch (Exception ex)
            {
                logger.Error("Error" + ex.Message);
                return View("FailureView");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseURI"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult RequestPayPalPayment(PayPalPaymentBean payment)
        {
            APIContext apiContext = Configuration.GetAPIContext();
            if (apiContext == null)
            {
                logger.Info("Paypal API context not found");
                throw new Exception("Paypal API context not found");
            }

            Payment translatedPayment = ModelFactory.translateToPayPalPayment(payment);

            //Fills PayerID on Request  
            Payment executedPayment = translatedPayment.Create(apiContext);

            //get links returned from paypal in response to Create function call
            var links = executedPayment.links.GetEnumerator();

            string paypalRedirectUrl = null;

            while (links.MoveNext())
            {
                Links lnk = links.Current;

                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                {
                    //saving the payapalredirect URL to which user will be redirected for payment
                    paypalRedirectUrl = lnk.href;
                }
            }

            // saving the paymentID in the key guid
            Session.Add(payment.guid, executedPayment.id);

            return Redirect(paypalRedirectUrl);
        }
        
        /// <summary>
        /// Executes payment over Paypal REST API.
        /// </summary>
        /// <param name="payment">payment method bean</param>        
        /// <returns>Payment execution result</returns>
        public ActionResult ExecutePayment(Payment payment)
        {
            APIContext apiContext = Configuration.GetAPIContext();
            if (apiContext == null)
            {
                logger.Info("Paypal API context not found");
                throw new Exception("Paypal API context not found");
            }           

            try
            {
                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. 
                Payment resultPayment = payment.Create(apiContext);

                //if the createdPayment.state is "approved" it means the payment was successful else not
                if (resultPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }

                return View("SuccessView");
            }
            catch (PayPal.PayPalException ex)
            {
                logger.Debug("Error: " + ex.Message, ex);
                return View("FailureView", ex);
            }
        }
    }
}

    