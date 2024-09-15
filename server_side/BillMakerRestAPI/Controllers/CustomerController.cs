using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
     
        [HttpGet("GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<CustomerDTO>> GetAllCustomers()
        {
            var customersDTOs = Customer.AllCustomers();
            if (customersDTOs.Count == 0)
                return NotFound("No data found");

            return Ok(customersDTOs);
        }



        [HttpGet("ByCustomerID/{id}", Name = "GetCustomerByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<CustomerDTO> GetCustomerByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var customer = Customer.Find(id);
            if (customer == null)
                return NotFound("No Data Found.");

            return Ok(customer.CDTO);
        }
  


       
        [HttpPost("AddNewCustomer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CustomerDTO> AddNewCustomer(CustomerDTO newCustomerDTO)
        {
            if (newCustomerDTO == null || string.IsNullOrEmpty(newCustomerDTO.FirstName))
                return BadRequest("Invalid customer data.");


            Customer customer = new(new CustomerDTO
                (
                newCustomerDTO.CustomerID,
                newCustomerDTO.PersonID,
                newCustomerDTO.FirstName,
                newCustomerDTO.SecondName,
                newCustomerDTO.LastName,
                newCustomerDTO.Email,
                newCustomerDTO.Gender,
                newCustomerDTO.Address,
                newCustomerDTO.Phone
                ));

            if (!customer.Save())
                return BadRequest("While Adding a new customer an error occur");

            newCustomerDTO.CustomerID = customer.CustomerID;

            return CreatedAtRoute("GetCustomerByID", new { id = newCustomerDTO.CustomerID }, newCustomerDTO);
        }



        [HttpPut("UpdateCustomer/{id}", Name = "UpdateCustomerInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> UpdateCustomerInfo(int id, CustomerDTO updatedCustomerDTO)
        {
            if (id < 1 || updatedCustomerDTO == null || string.IsNullOrEmpty(updatedCustomerDTO.FirstName.Trim()))
                return BadRequest("Invalid data.");

            Customer? customer = Customer.Find(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            customer.FirstName = updatedCustomerDTO.FirstName;
            customer.SecondName = updatedCustomerDTO.SecondName;
            customer.LastName = updatedCustomerDTO.LastName;
            customer.Email = updatedCustomerDTO.Email;
            customer.Gender = updatedCustomerDTO.Gender;
            customer.Phone = updatedCustomerDTO.Phone;
            customer.Address = updatedCustomerDTO.Address;
            
            if(!customer.Save())
                return BadRequest("An Error Occur.");

            return Ok(customer.CDTO);
        }



        [HttpGet("CountCustomers/{gendor}", Name = "GetTotalCustomersByGendor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CountCustomersByGendor(bool gendor)
        {
            int count = Customer.CountCustomersByGender(gendor);

            if (count == 0)
                return NotFound("No Data");

            return Ok(count);
        }




        [HttpDelete("{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCustomer(int id)
        {
            if (id < 1) 
                return BadRequest($"{id} Not Valid ID.");

            Customer? customer = Customer.Find(id);
            if (customer == null)
                return NotFound($"Customer with {id} not found.");

            if(!customer.Delete())
                return BadRequest($"An error occur. Make this customer has no related data.");

            return Ok($"Customer with ID: {id} has been deleted");
        }

    }
}
