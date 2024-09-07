using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/color")]
    public class ColorController : ControllerBase
    {
        [HttpGet("GetColors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<ColorDTO>> GetAllColors()
        {
            var ColorList = Color.Colors();
            if (ColorList == null)
                return NotFound("No Color Found.");

            else
                return Ok(ColorList);
        }



        [HttpGet("ByColorID/{id}", Name = "GetColorByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ColorDTO> GetColorByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var color = Color.Find(id);
            if (color == null)
                return NotFound("No Data Found.");

            return Ok(color.CDTO);
        }



        [HttpGet("{name}", Name = "GetColorByName")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ColorDTO>? GetColorByName(string name)
        {

            if (name.Length < 1 || string.IsNullOrEmpty(name.Trim()))
                return BadRequest($"Not Acceppted Name: ({name})");


            var color = Color.Find(name);
            if (color == null)
                return NotFound($"No Color With Name: ({name}) Found.");

            return Ok(color.CDTO);
        }



        [HttpPost("AddColor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<ColorDTO> AddNewColor(ColorDTO newColorDTO)
        {
            if (newColorDTO == null || string.IsNullOrEmpty(newColorDTO.ColorName))
                return BadRequest("Invalid color data.");

            if (Color.IsExists(newColorDTO.ColorName))
                return BadRequest($"Color '{newColorDTO.ColorName}' already exists.");

            Color color = new(new ColorDTO(newColorDTO.ColorID, newColorDTO.ColorName));
            color.Save();
            newColorDTO.ColorID = color.ColorID;

            return CreatedAtRoute("GetColorByID", new { id = newColorDTO.ColorID }, newColorDTO);
        }




        [HttpPut("{id}", Name = "UpdateColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ColorDTO> UpdateColor(int id, ColorDTO updatedColor)
        {

            if (id < 1 || updatedColor == null || string.IsNullOrEmpty(updatedColor.ColorName.Trim()))
                return BadRequest("Invalid color data.");


            Color? Color = Color.Find(id);
            if (Color == null)
                return NotFound($"Color with ID {id} not found.");


            if (Color.IsExists(updatedColor.ColorName))
                return BadRequest($"Color '{Color.ColorName}' already exists.");


            Color.ColorName = updatedColor.ColorName;
            Color.Save();

            return Ok(Color.CDTO);
        }




        [HttpDelete("{id}", Name = "DeleteColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteColor(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Color? color = Color.Find(id);
            if (color == null)
                return NotFound($"Color with {id} not found.");

            color.Delete();
            return Ok($"Color with {id} has been deleted");
        }
    }
}
