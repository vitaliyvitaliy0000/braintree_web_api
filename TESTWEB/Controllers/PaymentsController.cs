using Braintree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.Http;
using TESTWEB.Models;

namespace TESTWEB.Controllers
{
    [RoutePrefix("payments")]
    public class PaymentsController : ApiController
    {
        [HttpPost]
        [Route("uploadimage")]
        public string UploadImage(UploadImageRequest request)
        {
            var imgByteArray = Base64Decode(request.Base64Image);

            using (var canvas = new Bitmap(new MemoryStream(imgByteArray)))
            {
                float onePercentOfWidth = canvas.Width * 1.0F / 100;
                float onePercentOfHeight = canvas.Height * 1.0F / 100;

                int width = Convert.ToInt32(request.WidthPercent * onePercentOfWidth);
                int height = Convert.ToInt32(request.HeightPercent * onePercentOfHeight);

                int x = 0;
                int y = 0;
                if (request.HorizontalClipAlign == HorizontalClipAlign.Center)
                {
                    x = canvas.Width / 2 - width / 2;
                }
                else if (request.HorizontalClipAlign == HorizontalClipAlign.End)
                {
                    x = canvas.Width - width;
                }

                if (request.VerticalClipAlign == VerticalClipAlign.Center)
                {
                    y = canvas.Height / 2 - height / 2;
                }
                else if (request.VerticalClipAlign == VerticalClipAlign.End)
                {
                    y = canvas.Height - height;
                }

                var cropRect = new Rectangle(x, y, width, height);
                using (var newCanvas = canvas.Clone(cropRect, canvas.PixelFormat))
                {
                    var converter = new ImageConverter();
                    var imgByteArr = (byte[])converter.ConvertTo(newCanvas, typeof(byte[]));
                    var base64Image = Base64Encode(imgByteArr);

                    return base64Image;
                }
            }
        }

        private byte[] Base64Decode(string dataStr)
        {
            return Convert.FromBase64String(dataStr);
        }

        private string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

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
