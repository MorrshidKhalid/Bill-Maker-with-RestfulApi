using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/business")]
    public class BusinessController : ControllerBase
    {
        [HttpGet("GetBusinesses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<BusinessDTO>> GetAllBusinesses()
        {
            var businessList = Business.Businesses();
            if (businessList == null)
                return NotFound("No Business Found.");

            else
                return Ok(businessList);
        }



        [HttpGet("ByBusinessID/{id}", Name = "GetBusinessByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<BusinessDTO> GetBusinessByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var business = Business.Find(id);
            if (business == null)
                return NotFound("No Data Found.");

            return Ok(business.BDTO);
        }



        [HttpGet("{name}", Name = "GetBusinessByName")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<BusinessDTO>? GetBusinessByName(string name)
        {

            if (name.Length < 1 || string.IsNullOrEmpty(name.Trim()))
                return BadRequest($"Not Acceppted Name: ({name})");


            var business = Business.Find(name);
            if (business == null)
                return NotFound($"No Business With Name: ({name}) Found.");

            return Ok(business.BDTO);
        }



        [HttpPost("AddBusiness")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BusinessDTO> AddNewBusiness(BusinessDTO newBusinessDTO)
        {
            if (newBusinessDTO == null || string.IsNullOrEmpty(newBusinessDTO.Name))
                return BadRequest("Invalid business data.");

            if (Brand.IsExists(newBusinessDTO.Name))
                return BadRequest($"Business '{newBusinessDTO.Name}' already exists.");

            Business business = new(new BusinessDTO(newBusinessDTO.BusinessID, newBusinessDTO.Name));

            if (!business.Save())
                return BadRequest("An Error occur.");

            newBusinessDTO.BusinessID = business.BusinessID;

            return CreatedAtRoute("GetBusinessByID", new { id = newBusinessDTO.BusinessID }, newBusinessDTO);
        }




        [HttpPut("{id}", Name = "UpdateBusiness")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BusinessDTO> UpdateBusiness(int id, BusinessDTO updatedBusiness)
        {

            if (id < 1 || updatedBusiness == null || string.IsNullOrEmpty(updatedBusiness.Name.Trim()))
                return BadRequest("Invalid business data.");


            Business? business = Business.Find(id);
            if (business == null)
                return NotFound($"Business with ID {id} not found.");


            if (Business.IsExists(updatedBusiness.Name))
                return BadRequest($"Business '{business.Name}' already exists.");


            business.Name = updatedBusiness.Name;

            if (!business.Save())
                return BadRequest("An Error occur.");

            return Ok(business.BDTO);
        }




        [HttpDelete("{id}", Name = "DeleteBusiness")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBusiness(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Business? business = Business.Find(id);
            if (business == null)
                return NotFound($"Business with {id} not found.");

            if (!business.Delete())
                return BadRequest($"Make sure your business not relited with any other data.");

            return Ok($"Business with {id} has been deleted");
        }
    }
}
