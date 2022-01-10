using AutoMapper;
using IronBarCode;
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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "Product";

        public ProductsController(ApplicationDbContext db , IMapper mapper,IFileStorageService fileStorageService
            )
        {
            _fileStorageService = fileStorageService;
            _mapper = mapper;
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
                var product = new ProductsModel();

                product.ProductCode = productsModel.ProductCode;
                product.Product = (productsModel.Product).ToUpper();
                product.UnitPrice = productsModel.UnitPrice;
                product.Measure = productsModel.Measure;
                

               


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
