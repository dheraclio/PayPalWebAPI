using System;
using System.Collections.Generic;
using PayPal.Api;
using PayPalWebApplication.Models;

namespace PayPalWebApplication.Factories
{
    class ModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CreditCardPaymentBean createSampleCreditCardPayment()
        {
            return new CreditCardPaymentBean()
            {
                description = "Description about the payment amount.",
                invoice_number = "your invoice number which you are generating",

                //create and item for which you are taking payment
                //if you need to add more items in the list
                //Then you will need to create multiple item objects or use some loop to instantiate object
                item = new Item()
                {
                    name = "Demo Item",
                    currency = "USD",
                    price = "5",
                    quantity = "1",
                    sku = "sku"
                },

                // Specify your total payment amount and assign the details object
                amount = new Amount()
                {
                    currency = "USD",
                    total = "7",
                    details = new Details()
                    {
                        shipping = "1",
                        subtotal = "5",
                        tax = "1"
                    }
                },

                //Now Create an object of credit card and add above details to it
                //Please replace your credit card details over here which you got from paypal
                creditCard = new CreditCard()
                {
                    cvv2 = "874",  //card cvv2 number
                    expire_month = 01, //card expire date //mm
                    expire_year = 2020, //card expire year  //yy
                    first_name = "John",
                    last_name = "Doe",
                    //Samples CC numbers at http://www.paypalobjects.com/en_US/vhelp/paypalmanager_help/credit_card_numbers.htm
                    //Visa credit card
                    number = "4012888888881881", //enter your credit card number here
                    type = "visa", //credit card type here paypal allows 4 types

                    billing_address = new Address()
                    {
                        city = "NewYork",
                        country_code = "US",
                        line1 = "23rd street kew gardens",
                        postal_code = "43210",
                        state = "NY"
                    }
                }
            };
        }

        /// <summary>
        /// Creates sample paypal payment
        /// Paypal requires currency to match user account.
        /// For currency codes see https://developer.paypal.com/docs/classic/api/currency_codes/
        /// </summary>
        /// <returns></returns>
        public static PayPalPaymentBean createSamplePayPalPayment(string guid, string redirectUrl)
        {   
            return new PayPalPaymentBean()
            {
                guid = guid,
                description = "Transaction description.",
                invoice_number = "your invoice number",

                // similar as we did for credit card, do here and create amount object
                amount = new Amount()
                {
                    currency = "BRL",
                    total = "7", // Total must be equal to sum of shipping, tax and subtotal.

                    // similar as we did for credit card, do here and create details object
                    details = new Details()
                    {
                        tax = "1",
                        shipping = "1",
                        subtotal = "5"
                    }
                },                
                //similar to credit card create itemlist and add item objects to it
                item = new Item()
                {
                    name = "Item Name",
                    currency = "BRL",
                    price = "5",
                    quantity = "1",
                    sku = "sku"
                },
                // Configure Redirect Urls here with RedirectUrls object
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = redirectUrl,
                    return_url = redirectUrl
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Payment translateToPayPalPayment(CreditCardPaymentBean creditPayment)
        {
            //Sync transaction object to bean data
            Transaction transaction = new Transaction();
            transaction.amount = creditPayment.amount;
            transaction.description = creditPayment.description;
            transaction.invoice_number = creditPayment.invoice_number;

            //Create itemlist, add item objects to it and add it to transaction
            transaction.item_list = new ItemList() { items = new List<Item>() { creditPayment.item } };

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = creditPayment.creditCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            // finally create the payment object and assign the payer object & transaction list to it
            return new Payment()
            {
                intent = "sale",
                payer = payr,
                transactions = new List<Transaction>() { transaction }
            };
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Payment translateToPayPalPayment(PayPalPaymentBean payment)
        {
            //Sync transaction object to bean data
            Transaction transaction = new Transaction()
            {
                amount = payment.amount,
                description = payment.description,
                invoice_number = payment.invoice_number,
                item_list = new ItemList() { items = new List<Item>() { payment.item } }
            };
            
            return new Payment()
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                redirect_urls = payment.redirect_urls,
                transactions = new List<Transaction>() { transaction }                
            };
        }
    }
}
