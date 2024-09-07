using Microsoft.AspNetCore.Mvc;
using BMBusiness;
using BMData;

namespace BillMakerRestAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProductsNormal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ProductDTO>>? GetAllProducts()
        {
            var producsList = Product.Producs();
            if (producsList.Count == 0 || producsList == null)
                return NotFound("No Producs Found.");

            else
                return Ok(producsList);
        }



        [HttpGet("GetProductsDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ProductDetailsDTO>> GetAllProducsWithDetails()
        {
            var producsList = Product.AllProducsDetails();
            if (producsList.Count == 0 || producsList == null)
                return NotFound("No Producs Found.");

            else
                return Ok(producsList);
        }


        [HttpGet("ProductCCUFilter/{condition}/{currency}", Name = "GetProductByCCUFilter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProductDetailsDTO>> GetProducsCCUFilter(bool condition, string currencyCode)
        {
            if (currencyCode.Length != 3)
                return BadRequest($"Not Accepted Currency CODE {currencyCode}");

            var producsList = Product.ProducsByFilter(condition, currencyCode);
            if (producsList.Count == 0)
                return NotFound("No Producs Found.");

            else
                return Ok(producsList);
        }




        [HttpGet("ProductBetweenFilter/{firtPrice}/{secondPrice}", Name = "GetProductBetween")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProductDetailsDTO>> GetProducsBetweenFilter(decimal firtPrice, decimal secondPrice)
        {
            if (firtPrice > secondPrice)
                (secondPrice, firtPrice) = (firtPrice, secondPrice);


            if (firtPrice < 1 || secondPrice < 1)
                return BadRequest("Not Accepted Data");

            var producsList = Product.ProductsBetweenPrices(firtPrice, secondPrice);
            if (producsList.Count == 0)
                return NotFound("No Producs Found.");

            else
                return Ok(producsList);
        }



        [HttpGet("ProductByBrand/{brand}", Name = "GetProductByBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProductDetailsDTO>> GetProducsByBrand(string brand)
        {
            if (string.IsNullOrEmpty(brand))
                return BadRequest($"Not Accepted Data");

            var producsList = Product.ProductsByBrand(brand);
            if (producsList.Count == 0)
                return NotFound("No Producs Found.");

            else
                return Ok(producsList);
        }




        [HttpGet("ByProductID/{id}", Name = "GetProductByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ProductDetailsDTO> GetProductByID(int id)
        {
            if (id < 1)
                return BadRequest($"Not Acceppted ID: ({id})");

            var product = Product.Find(id);
            if (product == null)
                return NotFound("No Data Found.");

            return Ok(product.PDTO);
        }



        [HttpPut("UpdateProduct/{id}", Name = "UpdateProductInfo")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<ProductDTO> UpdateProduct(int id, ProductDTO updatedProduct)
        {

            if (id < 1 || updatedProduct == null || string.IsNullOrEmpty(updatedProduct.Model.Trim()))
                return BadRequest("Invalid product data.");

            Product? product = Product.Find(id);

            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            product.BrandID = updatedProduct.BrandID;
            product.Model = updatedProduct.Model;
            product.ColorID = updatedProduct.ColorID;
            product.StorageID = updatedProduct.StorageID;
            product.Condition = updatedProduct.Condition;
            product.TermID = updatedProduct.TermID;
            product.CurrencyID = updatedProduct.CurrencyID;
            product.PurchaPrice = updatedProduct.PurchaPrice;
            product.PurchaPrice = updatedProduct.PurchaPrice;
            product.ImagePath = updatedProduct.ImagePath;
            product.Quality = updatedProduct.Quality;

            if (!product.Save())
                return BadRequest("An Error Occur.");

            return Ok(product.PDTO);
        }




        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> AddNewBrand(ProductDTO newProductDTO)
        {
            if (newProductDTO == null || string.IsNullOrEmpty(newProductDTO.Model))
                return BadRequest("Invalid product data.");


            Product product = new(new ProductDTO
                (
                newProductDTO.ProductID,
                newProductDTO.BrandID,
                newProductDTO.Model,
                newProductDTO.ColorID,
                newProductDTO.StorageID,
                newProductDTO.Condition,
                newProductDTO.TermID,
                newProductDTO.CurrencyID,
                newProductDTO.PurchaPrice,
                newProductDTO.ImagePath,
                newProductDTO.Quality
                ));
            if (!product.Save())
                return BadRequest("An Error Occur");

            newProductDTO.ProductID = product.ProductID;

            return CreatedAtRoute("GetProductByID", new { id = newProductDTO.ProductID }, newProductDTO);
        }




        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProduct(int id)
        {
            if (id < 1)
                return BadRequest($"{id} Not Valid ID.");

            Product? product = Product.Find(id);
            if (product == null)
                return NotFound($"Product with {id} not found.");

            product.Delete();
            return Ok($"Product with {id} has been deleted");
        }
    }
}