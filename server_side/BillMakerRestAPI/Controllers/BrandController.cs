using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/brand")]
    public class BrandController : ControllerBase
    {
        [HttpGet("GetBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<BrandDTO>> GetAllBrands()
        {
            var brandList = Brand.Brands();
            if (brandList == null)
                return NotFound("No Brand Found.");

            else
                return Ok(brandList);
        }



        [HttpGet("ByBrandID/{id}", Name = "GetBrandByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<BrandDTO> GetBrandByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var brand = Brand.Find(id);
            if (brand == null)
                return NotFound("No Data Found.");

            return Ok(brand.BDTO);
        }



        [HttpGet("{name}", Name = "GetBrandByName")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<BrandDTO>? GetBrandByName(string name)
        {

            if (name.Length < 1 || string.IsNullOrEmpty(name.Trim()))
                return BadRequest($"Not Acceppted Name: ({name})");


            var brand = Brand.Find(name);
            if (brand == null)
                return NotFound($"No Brand With Name: ({name}) Found.");

            return Ok(brand.BDTO);
        }



        [HttpPost("AddBrand")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BrandDTO> AddNewBrand(BrandDTO newBrandDTO)
        {
            if (newBrandDTO == null || string.IsNullOrEmpty(newBrandDTO.BrandName))
                return BadRequest("Invalid brand data.");

            if (Brand.IsExists(newBrandDTO.BrandName))
                return BadRequest($"Brand '{newBrandDTO.BrandName}' already exists.");

            Brand brand = new(new BrandDTO(newBrandDTO.BrandID, newBrandDTO.BrandName));
            brand.Save();
            newBrandDTO.BrandID = brand.BrandID;

            return CreatedAtRoute("GetBrandByID", new { id = newBrandDTO.BrandID}, newBrandDTO);
        }




        [HttpPut("{id}", Name = "UpdateBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BrandDTO> UpdateBrand(int id, BrandDTO updatedBrand)
        {

            if (id < 1 || updatedBrand == null || string.IsNullOrEmpty(updatedBrand.BrandName.Trim()))
                return BadRequest("Invalid brand data.");


            Brand? brand = Brand.Find(id);
            if (brand == null)
                return NotFound($"Brand with ID {id} not found.");


            if (Brand.IsExists(updatedBrand.BrandName))
                return BadRequest($"Brand '{brand.BrandName}' already exists.");


            brand.BrandName = updatedBrand.BrandName;
            brand.Save();

            return Ok(brand.BDTO);
        }




        [HttpDelete("{id}", Name = "DeleteBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBrand(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Brand? brand = Brand.Find(id);
            if (brand == null)
                return NotFound($"Brand with {id} not found.");

            brand.Delete();
            return Ok($"Brand with {id} has been deleted");
        }
    }
}
