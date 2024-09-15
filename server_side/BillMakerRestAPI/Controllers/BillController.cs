using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{

    [ApiController]
    [Route("api/bill")]
    public class BillController : ControllerBase
    {

        [HttpGet("GetBillsWithDetails")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<BillDetailsDTO>> GetAllBills()
        {
            List<BillDetailsDTO> bills = Bill.BillsWithDetails();
            if (bills.Count == 0)
                return NotFound("No data found.");

            return Ok(bills);
        }



        [HttpGet("GetBillsByBusiness/{businessID}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<BillDetailsDTO>> GetAllBillsByBusiness(int businessID)
        {
            if (businessID < 1)
                return BadRequest($"({businessID}) Not Accepted ID.");

            List<BillDetailsDTO> bills = Bill.BillsByBusiness(businessID);
            if (bills.Count == 0)
                return NotFound("No data found.");

            return Ok(bills);
        }



        [HttpGet("GetBillsByBusinessAndDate/{businessID}/{start}/{end}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<BillDetailsDTO>> GetAllBillsByBusinessAndDate(int businessID, DateTime start, DateTime end)
        {
            if (businessID < 1)
                return BadRequest($"({businessID}) Not Accepted ID.");

            List<BillDetailsDTO> bills = Bill.BillsByBusinessBetweenDate(businessID, start, end);
            if (bills.Count == 0)
                return NotFound("No data found.");

            return Ok(bills);
        }




        [HttpGet("GetTotalSelles/{businessID}/{currencyID}/{start}/{end}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<decimal> GetAllBillsByBusinessAndDate(int businessID, int currencyID, DateTime start, DateTime end)
        {
            if (businessID < 1)
                return BadRequest($"({businessID}) Not Accepted ID.");

            if (currencyID < 1)
                return BadRequest($"({currencyID}) Not Accepted ID.");

            decimal total = Bill.TotalSelles(businessID, currencyID, start, end);
            if (total == 0)
                return NotFound("No data found.");

            if (total == -1)
                return BadRequest("Error Ocuur.");

            return Ok(total);
        }





        [HttpGet("ByBillID/{id}", Name = "GetBillByID")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BillDTO> GetBillByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var bill = Bill.Find(id);
            if (bill == null)
                return NotFound("No Data Found.");


            return Ok(bill.BDTO);
        }


        [HttpPost("AddNewBill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BillDTO> AddNewBill(BillDTO newBillDTO)
        {
            if (newBillDTO.Amount < 1)
                return BadRequest("Invalid bill data.");

            Bill bill = new(new BillDTO
                 (
                 newBillDTO.BillID,
                 newBillDTO.FromBusinessID,
                 newBillDTO.ToCustomerID,
                 newBillDTO.Amount,
                 newBillDTO.Discount,
                 newBillDTO.DateCreated,
                 newBillDTO.BillStatus,
                 newBillDTO.CurrencyRate,
                 newBillDTO.CurrencyID,
                 newBillDTO.CreatedByUserID,
                 newBillDTO.BodyList
                 ));


            if (!bill.Save())
                return BadRequest("While Adding a new bill an error occur");

            newBillDTO.BillID = bill.BillID;

            return CreatedAtRoute("GetBillByID", new { id = newBillDTO.BillID }, newBillDTO);
        }



        [HttpDelete("Delete/{billID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBill(int billID)
        {
            if (billID < 1)
                return BadRequest($"Not Accepted ID {billID}.");

            Bill? bill = Bill.Find(billID);
            if (bill == null)
                return NotFound("No data found.");

            if (!bill.Delete())
                return BadRequest("While deleting bill an error ouccr.");

            return Ok($"Bill With ID {billID} delted successfully.");
        }



        [HttpPut("UpdateBill/{billID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BillDTO> UpdateBill(int billID, BillDTO updatedBill)
        {
            if (billID < 1)
                return BadRequest($"({billID}) Not Accepted ID.");

            Bill? bill = Bill.Find(billID);
            if (bill == null)
                return NotFound("No Data found.");
            bill.FromBusinessID = updatedBill.FromBusinessID;
            bill.ToCustomerID= updatedBill.ToCustomerID;
            bill.BillStatus = updatedBill.BillStatus;
            bill.Amount = updatedBill.Amount;
            bill.Discount = updatedBill.Discount;
            bill.DateCreated = updatedBill.DateCreated;
            bill.CurrencyRate = updatedBill.CurrencyRate;
            bill.CurrencyID = updatedBill.CurrencyID;
            bill.BodyList = updatedBill.BodyList;

            if (!bill.Save())
                return BadRequest("While Save an error occur.");

            return Ok(bill.BDTO);
        }

    }
}
