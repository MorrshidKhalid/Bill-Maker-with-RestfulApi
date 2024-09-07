using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {

        [HttpGet("GetStorages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<List<StorageDTO>> GetAllStorages()
        {
            var storageList = Storage.Storages();
            if (storageList == null)
                return NotFound("No Color Found.");

            else
                return Ok(storageList);
        }



        [HttpGet("ByStorageID/{id}", Name = "GetStorageByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<StorageDTO> GetStorageByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var storage = Storage.Find(id);
            if (storage == null)
                return NotFound("No Data Found.");

            return Ok(storage.SDTO);
        }



        [HttpGet("{capacity}", Name = "GetStorageByCapacity")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<StorageDTO>? GetStorageByCapacity(string capacity)
        {

            if (string.IsNullOrEmpty(capacity.Trim()))
                return BadRequest($"Not Acceppted capacity: ({capacity})");


            var storage = Storage.Find(capacity);
            if (storage == null)
                return NotFound($"No Storage With Capacity: ({capacity}) Found.");

            return Ok(storage.SDTO);
        }



        [HttpPost("AddStorage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<StorageDTO> AddNewStorage(StorageDTO newStorageDTO)
        {
            if (newStorageDTO == null || string.IsNullOrEmpty(newStorageDTO.Capacity.Trim()))
                return BadRequest("Invalid storage data.");

            if (Color.IsExists(newStorageDTO.Capacity))
                return BadRequest($"Capacity '{newStorageDTO.Capacity}' already exists.");

            Storage storage = new(new StorageDTO(newStorageDTO.StorageID, newStorageDTO.Capacity));
            storage.Save();
            newStorageDTO.StorageID = storage.StorageID;

            return CreatedAtRoute("GetStorageByID", new { id = newStorageDTO.StorageID }, newStorageDTO);
        }




        [HttpPut("{id}", Name = "UpdateStorage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<StorageDTO> UpdateStorage(int id, StorageDTO updatedStorage)
        {

            if (id < 1 || updatedStorage == null || string.IsNullOrEmpty(updatedStorage.Capacity.Trim()))
                return BadRequest("Invalid storage data.");


            Storage? storage = Storage.Find(id);
            if (storage == null)
                return NotFound($"Storage with ID {id} not found.");


            if (Storage.IsExists(updatedStorage.Capacity))
                return BadRequest($"Capacity '{storage.Capacity}' already exists.");


            storage.Capacity = updatedStorage.Capacity;
            storage.Save();

            return Ok(storage.SDTO);
        }




        [HttpDelete("{id}", Name = "DeleteStorage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteStorage(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Storage? storage = Storage.Find(id);
            if (storage == null)
                return NotFound($"Storage with {id} not found.");

            storage.Delete();
            return Ok($"Storage with {id} has been deleted");
        }
    }
}
