using AutoMapper;
using IronBarCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<StaffModel> _userManager;
        public ProductsController(ApplicationDbContext db, UserManager<StaffModel> userManager
            )
        {
            _db = db;
            _userManager = userManager;
        }


        [HttpGet]
        //   [AllowAnonymous]
        public async Task<ActionResult<ProductsModel>> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            IEnumerable<ProductsModel> objList = _db.Products.Where(x => x.AdminId == currentUser.Id);
            return Ok(objList);
        }

        [HttpGet("getProduct")]
        //   [AllowAnonymous]
        public async Task<ActionResult<ProductsModel>> GetProduct()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            //var objList = _db.Products.Select(x => x.ProductName).Distinct().ToList();
            var objList = _db.Products.Where(x => x.AdminId == currentUser.Id).Select(x => x.ProductName).ToList();
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(string Id)
        {
            if ( Id == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var obj = _db.Products.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpGet("productname")]
        //    [AllowAnonymous]
        public async Task<ActionResult> GetMeasure(string Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (Id == null)
            {
                return NotFound();
            }
            var obj = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();
            var objMeasures = obj.ProductMeasures.Select(x => new { x.Measure, x.UnitPrice }).ToList();

            if (objMeasures == null)
            {
                return NotFound();
            }
            return Ok(objMeasures);
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

        public async Task<ActionResult> Post([FromBody] ProductsModelDTO productsModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueId = System.Guid.NewGuid().ToString();
                var currentUser = await _userManager.GetUserAsync(User);
                var product = new ProductsModel
                {
                    Id= uniqueId,
                    ProductCode = productsModel.ProductCode,
                    ProductName = (productsModel.ProductName).ToUpper(),
                    AdminId = currentUser.Id
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

        [HttpPut("{id}")]

        public async Task<ActionResult> Put(string Id, [FromBody] ProductsModel productsModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();
                product.ProductName = (productsModel.ProductName).ToUpper();
                product.ProductCode = productsModel.ProductCode;

                _db.Products.Update(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut("productMeasure")]

        public async Task<ActionResult> PutMeasure(string Id, [FromBody] List<ProductMeasureDTO> productMeasure)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                string uniqueId = System.Guid.NewGuid().ToString();
                var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();
                var prodMeasure = new ProductMeasureModel();
                var productMeasures = new List<ProductMeasureModel>();
                foreach(var item in productMeasure)
                {
                    prodMeasure.CostPrice = item.CostPrice;
                    prodMeasure.Id = uniqueId;
                    prodMeasure.Measure = item.Measure;
                    prodMeasure.ProductId = product.Id;
                    prodMeasure.QtyPerMeasure = item.QtyPerMeasure;
                    prodMeasure.UnitPrice = item.UnitPrice;

                    product.ProductMeasures.Add(prodMeasure);
                }

                _db.Products.UpdateRange(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete("deletemeasure")]

        public async Task<ActionResult> DeleteMeasure(string ProductId, string MeasureId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var product = _db.Products.Where(x => x.Id == ProductId && x.AdminId == currentUser.Id).FirstOrDefault();
            var productMeasure = product.ProductMeasures.Where(x => x.Id != MeasureId).ToList();
            product.ProductMeasures = productMeasure;
            _db.Products.UpdateRange(product);
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(string Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();

            _db.Products.Remove(product);
            _db.SaveChanges();
            return Ok();
        }



    }
}
