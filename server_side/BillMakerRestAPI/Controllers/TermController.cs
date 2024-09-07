using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/term")]
    public class TermController : ControllerBase
    {
        [HttpGet("GetTerms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<TermDTO>> GetAllTerms()
        {
            var termList = Term.Terms();
            if (termList == null)
                return NotFound("No Term Found.");

            else
                return Ok(termList);
        }



        [HttpGet("ByTermID/{id}", Name = "GetTermByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<TermDTO> GetTermByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var term = Term.Find(id);
            if (term == null)
                return NotFound("No Data Found.");

            return Ok(term.TDTO);
        }




        [HttpPost("AddTerm")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<TermDTO> AddNewTerm(TermDTO newTermDTO)
        {
            if (newTermDTO == null || string.IsNullOrEmpty(newTermDTO.Description.Trim()))
                return BadRequest("Invalid term data.");


            Term term = new(new TermDTO(newTermDTO.TermID, newTermDTO.Description));
            term.Save();
            newTermDTO.TermID = term.TermID;

            return CreatedAtRoute("GetTermByID", new { id = newTermDTO.TermID}, newTermDTO);
        }




        [HttpPut("{id}", Name = "UpdateTerm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<TermDTO> UpdateTerm(int id, TermDTO updatedTerm)
        {

            if (id < 1 || updatedTerm == null || string.IsNullOrEmpty(updatedTerm.Description.Trim()))
                return BadRequest("Invalid term data.");


            Term? term = Term.Find(id);
            if (term == null)
                return NotFound($"Term with ID {id} not found.");


            term.Description = updatedTerm.Description;
            term.Save();

            return Ok(term.TDTO);
        }




        [HttpDelete("{id}", Name = "DeleteTerm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteTerm(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Term? term = Term.Find(id);
            if (term == null)
                return NotFound($"Term with {id} not found.");

            term.Delete();
            return Ok($"Term with {id} has been deleted");
        }
    }
}
