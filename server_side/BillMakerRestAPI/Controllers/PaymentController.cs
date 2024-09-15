using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;


namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {

        [HttpGet("ByPaymentID/{id}", Name = "GetPaymentByID")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PaymentDTO> GetPaymentByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var payment = Payment.Find(id);
            if (payment == null)
                return NotFound("No Data Found.");


            return Ok(payment.PDTO);
        }


        [HttpPost("AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaymentDTO> AddNewPayment(PaymentDTO newPaymentDTO)
        {
            if (newPaymentDTO.AmountPaid < 1)
                return BadRequest("Invalid payment data.");

            Payment payment = new(new PaymentDTO
                 (
                 newPaymentDTO.PaymentID,
                 newPaymentDTO.CustomerID,
                 newPaymentDTO.BillID,
                 newPaymentDTO.CarrencyID,
                 newPaymentDTO.MethodID,
                 newPaymentDTO.CurrencyRate,
                 newPaymentDTO.AmountPaid,
                 newPaymentDTO.RefindAmount,
                 newPaymentDTO.Date,
                 newPaymentDTO.Note
                 ));


            if (!payment.Save())
                return BadRequest("While Adding a new payment an error occur");

            newPaymentDTO.BillID = payment.BillID;

            return CreatedAtRoute("GetPaymentByID", new { id = newPaymentDTO.PaymentID }, newPaymentDTO);
        }



        [HttpPut("UpdatePayment/{paymentID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PaymentDTO> UpdatePayment(int paymentID, PaymentDTO updatedPayment)
        {
            if (paymentID < 1)
                return BadRequest($"({paymentID}) Not Accepted ID.");

            Payment? paymen = Payment.Find(paymentID);
            if (paymen == null)
                return NotFound("No Data found.");

            paymen.CustomerID = updatedPayment.CustomerID;
            paymen.BillID = updatedPayment.BillID;
            paymen.CarrencyID = updatedPayment.CarrencyID;
            paymen.MethodID = updatedPayment.MethodID;
            paymen.CurrencyRate = updatedPayment.CurrencyRate;
            paymen.AmountPaid = updatedPayment.AmountPaid;
            paymen.CurrencyRate = updatedPayment.CurrencyRate;
            paymen.RefindAmount = updatedPayment.RefindAmount;
            paymen.Date = updatedPayment.Date;
            paymen.Note = updatedPayment.Note;

            if (!paymen.Save())
                return BadRequest("While Save an error occur.");

            return Ok(paymen.PDTO);
        }



        [HttpDelete("Delete/{paymentID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePayment(int paymentID)
        {
            if (paymentID < 1)
                return BadRequest($"Not Accepted ID {paymentID}.");

            Payment? payment = Payment.Find(paymentID);
            if (payment == null)
                return NotFound("No data found.");

            if (!payment.Delete())
                return BadRequest("While deleting payment an error ouccr.");

            return Ok($"Payment With ID {paymentID} delted successfully.");
        }
    }
}
