using BMBusiness;
using BMData;
using Microsoft.AspNetCore.Mvc;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/currincy")]

    public class CurrincyController : ControllerBase
    {

        [HttpGet("GetCurrincies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<CurrencyDTO>> GetAllCurrincies()
        {
            var currencyList = Currency.CurrencyList();
            if (currencyList == null)
                return NotFound("No Currency Found.");

            else
                return Ok(currencyList);
        }




        [HttpGet("getByID/{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<CurrencyDTO> GetCurrencyByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var currency = Currency.Find(id);
            if (currency == null)
                return NotFound("No Data Found.");

            return Ok(currency.CDTO);
        }



        [HttpGet("getByCode/{code}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<CurrencyDTO>>? GetCurrencyByCode(string code)
        {

            if (code.Length != 3 || !code.All(char.IsLetter))
                return BadRequest($"Not Acceppted CODE: ({code})");

            
            var currencyList = Currency.Find(code);
            if (currencyList == null)
                return NotFound("No Data Found.");

            return Ok(currencyList);
        }



        [HttpPut("updateByID/{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<CurrencyDTO> UpdateCurrency(int id, decimal newRate)
        {

            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var currency = Currency.Find(id);
            if (currency == null)
                return NotFound($"currency With ID {id} Not Found.");

            // Updare the currency rate.
            currency.Rate = newRate;

            return (currency.UpdateRate()) ? Ok(currency.CDTO) : BadRequest($"An Error Occurred"); ;
        }
    }
}
