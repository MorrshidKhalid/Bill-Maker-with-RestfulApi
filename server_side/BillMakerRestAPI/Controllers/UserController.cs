using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserDTO>> GetAllUsers()
        {
            var customersDTOs = BMUser.Users();

            if (customersDTOs.Count == 0)
                return NotFound("No data found");

            return Ok(customersDTOs);
        }



        [HttpGet("ByUserID/{id}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var user = BMUser.Find(id);
            if (user == null)
                return NotFound("No Data Found.");

            return Ok(user.UDTO);
        }




        [HttpGet("ByUserUsername/{username}", Name = "GetUsernameByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest($"Not Acceppted data.");

            var user = BMUser.Find(username);
            if (user == null)
                return NotFound("No Data Found.");

            return Ok(user.UDTO);
        }



        [HttpPost("AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDTO> AddNewUser(UserDTO newUserDTO)
        {
            if (newUserDTO == null || string.IsNullOrEmpty(newUserDTO.UserName))
                return BadRequest("Invalid user data.");

            if (BMUser.IsExists(newUserDTO.UserName))
                return BadRequest($"User {newUserDTO.UserName} is already exists.");

            BMUser user = new(new UserDTO
                (
                newUserDTO.UserID,
                newUserDTO.UserName,
                newUserDTO.Password,
                newUserDTO.IsActive,
                newUserDTO.Permission,
                newUserDTO.PersonID,
                newUserDTO.FirstName,
                newUserDTO.SecondName,
                newUserDTO.LastName,
                newUserDTO.Email,
                newUserDTO.Gender,
                newUserDTO.Address,
                newUserDTO.Phone
                ));

            if (!user.Save())
                return BadRequest("While Adding a new user an error occur");

            newUserDTO.UserID = user.UserID;

            return CreatedAtRoute("GetUserByID", new { id = newUserDTO.UserID }, newUserDTO);
        }




        [HttpPut("UpdateUser/{id}", Name = "UpdateUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserInfoDTO> UpdateUserInfo(int id, UserInfoDTO updatedUserDTO)
        {
            if (id < 1 || updatedUserDTO == null || string.IsNullOrEmpty(updatedUserDTO.UserName.Trim()))
                return BadRequest("Invalid data.");

            BMUser? user = BMUser.Find(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");


            if (BMUser.IsExists(updatedUserDTO.UserName))
                return BadRequest($"User {updatedUserDTO.UserName} is already exists.");


            user.UserName = updatedUserDTO.UserName;
            user.Password = updatedUserDTO.Password;
            user.IsActive = updatedUserDTO.IsActive;
            user.Permission= updatedUserDTO.Permission;

            if (!user.Save())
                return BadRequest("An Error Occur.");

            return Ok(user.UDTO);
        }



        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCustomer(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            BMUser? user = BMUser.Find(id);
            if (user == null)
                return NotFound($"User with {id} not found.");

            if (!user.Delete())
                return BadRequest($"An error occur. Make suer this user has no related data.");

            return Ok($"User with ID: {id} has been deleted");
        }
    }
}