using AutoMapper;
using IronBarCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.HelperClass;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db
            )
        {
            _db = db;
        }


        [HttpGet]
     //   [AllowAnonymous]
        public ActionResult<ProductsModel> GetAll()
        {
            IEnumerable<ProductsModel> objList = _db.Products;
            return Ok(objList);
        }

        [HttpGet("getProduct")]
        //   [AllowAnonymous]
        public ActionResult<ProductsModel> GetProduct()
        {
            var objList = _db.Products.Select(x => x.Product).Distinct().ToList();
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


        [HttpGet("productname")]
    //    [AllowAnonymous]

        public ActionResult GetMeasure(string Name)
         {
            if (Name == null)
            {
                return NotFound();
            }
            var obj = _db.Products.Where(x => x.Product == Name).Select(x => new { x.Measure, x.UnitPrice }).ToList();


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

       
        //[HttpGet("filter")]
        //public async Task<ActionResult<List<ProductsModelDTO>>> Filter([FromQuery] FilterProducts filterProducts)
        //{
        //    var productQuerable = _db.Products.AsQueryable();

        //    if (!string.IsNullOrEmpty(filterProducts.Product))
        //    {
        //        productQuerable = productQuerable.Where(x => x.Product.Contains(filterProducts.Product));

        //    }
        //   // await HttpContext.InsertParameterPaginationInHeader(productQuerable);
        //    var products =  productQuerable.OrderBy(x => x.Product).ToList();
        //    return Ok(products);
        //  //  return Mapper.Map<List<ProductsModelDTO>>(products);
        //}

        [HttpPost]

        public ActionResult Post([FromBody] ProductsModelDTO productsModel)
        {
            if (ModelState.IsValid)
            {
                var product = new ProductsModel
                {
                    ProductCode = productsModel.ProductCode,
                    Product = (productsModel.Product).ToUpper(),
                    UnitPrice = productsModel.UnitPrice,
                    Measure = productsModel.Measure
                };





                // product.BarcodeImage = await _fileStorageService.SaveFile(containerName, productsModel.BarcodeImage);
                //BarcodeResult[] Results = BarcodeReader.QuicklyReadAllBarcodes("MultipleBarcodes.png");
                //// Work with the results
                //foreach (BarcodeResult Result in Results)
                //{
                //    string Value = Result.Value;
                //    Bitmap Img = Result.BarcodeImage;
                //    BarcodeEncoding BarcodeType = Result.BarcodeType;
                //    byte[] Binary = Result.BinaryValue;
                //    Console.WriteLine(Result.Value);
                //    return Ok(Value);


                //};


                //  BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode("GetStarted.png");
                //BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode("barcode.jpg");
                //if (Result != null)
                //{
                //    productsModel.BarcodeImage= Result.BinaryValue;
                //}

                if (product == null)
                {
                    return BadRequest();
                }
                



                _db.Products.Add(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut("{id:int}")]

        public ActionResult Put(int Id, [FromBody] ProductsModel productsModel)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(Id);
                product.Product = (productsModel.Product).ToUpper();
                product.UnitPrice = productsModel.UnitPrice;
                product.Measure = productsModel.Measure;
                product.ProductCode = productsModel.ProductCode;

                _db.Products.Update(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        
        [HttpDelete("{id:int}")]

        public ActionResult Delete(int Id)
        {
            var product = _db.Products.Find(Id);

            _db.Products.Remove(product);
            _db.SaveChanges();
            return Ok();
        }



    }
}
