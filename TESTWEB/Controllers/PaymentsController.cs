using Braintree;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TESTWEB.Models;

namespace TESTWEB.Controllers
{
    [RoutePrefix("payments")]
    public class PaymentsController : ApiController
    {
        [HttpPost]
        [Route("braintree")]
        public BraintreeResponse BrainTreeRequest(BraintreeRequest brainTreeRequest)
        {
            var response = new BraintreeResponse();

            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "wyr3nqgq36d5bw9m",
                PublicKey = "xbt6sjtrdyppp3vn",
                PrivateKey = "3c8f2740815399f8f7433c9410eabe63"
            };

            TransactionRequest transactionRequest = new TransactionRequest
            {
                Amount = brainTreeRequest.Amount,
                PaymentMethodNonce = brainTreeRequest.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            try
            {
                Result<Transaction> saleResult = gateway.Transaction.Sale(transactionRequest);

                if (saleResult.IsSuccess())
                {
                    Transaction transaction = saleResult.Target;
                    response.Message = "Success";
                    response.TransactionId = transaction.Id;
                }
                else if (saleResult.Transaction != null)
                {
                    Transaction transaction = saleResult.Transaction;

                    response.Message = "Error processing transaction";
                    response.Success = false;
                    response.TransactionError = new BraintreeErrorProcessingTransaction(
                        transaction.Status.ToString(),
                        transaction.ProcessorResponseCode,
                        transaction.ProcessorResponseText);
                }
                else
                {
                    response.Success = false;
                    response.ValidationErrors = new List<BraintreeValidationError>();

                    foreach (ValidationError resError in saleResult.Errors.DeepAll())
                    {
                        response.ValidationErrors.Add(new BraintreeValidationError(
                            resError.Attribute,
                            resError.Code.ToString(),
                            resError.Message));
                    }
                }
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }
    }
}
