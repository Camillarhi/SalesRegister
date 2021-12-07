using IronBarCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<ProductsModel> GetAll()
        {
            IEnumerable<ProductsModel> objList = _db.Products;
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0||Id==null)
            {
                return NotFound();
            }
            var obj = _db.Products.Find(Id);


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpPost]

        public ActionResult Post([FromBody] ProductsModelDTO productsModel)
        {
            if (ModelState.IsValid)
            {
                //  BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode("GetStarted.png");

                var product = new ProductsModel()
                {
                    Product = productsModel.Product,
                    UnitPrice = productsModel.UnitPrice
                };



                _db.Products.Add(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]

        public ActionResult Put(int Id, [FromBody] ProductsModel productsModel)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(Id);
                product.Product = productsModel.Product;
                product.UnitPrice = productsModel.UnitPrice;

                _db.Products.Update(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        
        [HttpDelete]

        public ActionResult Delete(int Id)
        {
            var product = _db.Products.Find(Id);

            _db.Products.Remove(product);
            _db.SaveChanges();
            return Ok();
        }



    }
}
