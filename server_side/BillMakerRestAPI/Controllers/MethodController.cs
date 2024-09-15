using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/method")]
    public class MethodController : ControllerBase
    {
        [HttpGet("GetMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<MethodDTO>> GetAllMethods()
        {
            var methodList = Method.Methods();
            if (methodList == null)
                return NotFound("No Method Found.");

            else
                return Ok(methodList);
        }



        [HttpGet("ByMethodID/{id}", Name = "GetMethodByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<MethodDTO> GetMethodByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var method = Method.Find(id);
            if (method == null)
                return NotFound("No Data Found.");

            return Ok(method.MDTO);
        }



        [HttpPost("AddMethod")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MethodDTO> AddNewMethod(MethodDTO newMethodDTO)
        {
            if (newMethodDTO == null || string.IsNullOrEmpty(newMethodDTO.MethodName))
                return BadRequest("Invalid method data.");

            if (Method.IsExists(newMethodDTO.MethodName))
                return BadRequest($"Method '{newMethodDTO.MethodName}' already exists.");

            Method method = new(new MethodDTO(newMethodDTO.MethodID, newMethodDTO.MethodName));
            method.Save();
            newMethodDTO.MethodID = method.MethodID;

            return CreatedAtRoute("GetMethodByID", new { id = newMethodDTO.MethodID}, newMethodDTO);
        }




        [HttpPut("{id}", Name = "UpdateMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MethodDTO> UpdateMethod(int id, MethodDTO updatedMethod)
        {

            if (id < 1 || updatedMethod == null || string.IsNullOrEmpty(updatedMethod.MethodName.Trim()))
                return BadRequest("Invalid method data.");


            Method? method = Method.Find(id);
            if (method == null)
                return NotFound($"Method with ID {id} not found.");


            if (Method.IsExists(updatedMethod.MethodName))
                return BadRequest($"Method '{method.MethodName}' already exists.");


            method.MethodName = updatedMethod.MethodName;
            method.Save();

            return Ok(method.MDTO);
        }




        [HttpDelete("{id}", Name = "DeleteMethod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteMethod(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Method? method = Method.Find(id);
            if (method == null)
                return NotFound($"Method with {id} not found.");

            method.Delete();
            return Ok($"Method with {id} has been deleted");
        }

    }
}
