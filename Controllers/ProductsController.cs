using AutoMapper;
using IronBarCode;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.HelperClass;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
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
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();

            IEnumerable<ProductsModel> objList = _db.Products.Where(x => x.AdminId == currentUser).Include(y => y.ProductMeasures);

            return Ok(objList);
        }


        [HttpGet("updatedproducts")]
        //   [AllowAnonymous]
        public async Task<ActionResult<ProductsModel>> GetAllWithPrice()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();

            IEnumerable<ProductsModel> objList = _db.Products.Where(x => x.AdminId == currentUser).Include(y => y.ProductMeasures.Where(y => y.UnitPrice != 0));

            return Ok(objList);
        }

        //[HttpGet("getProduct")]
        ////   [AllowAnonymous]
        //public async Task<ActionResult<ProductsModel>> GetProduct()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    //var objList = _db.Products.Select(x => x.ProductName).Distinct().ToList();
        //    var objList = _db.Products.Where(x => x.AdminId == currentUser.Id).Select(x => x.ProductName).ToList();
        //    return Ok(objList);
        //}


        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(string Id)
        {
            if ( Id == null)
            {
                return NotFound();
            }
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();

            var obj = _db.Products.Where(x => x.AdminId == currentUser && x.Id == Id).Include(y => y.ProductMeasures).FirstOrDefault();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        //[HttpGet("productname")]
        ////    [AllowAnonymous]
        //public async Task<ActionResult> GetMeasure(string Id)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }
        //    var obj = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();
        //    var objMeasures = obj.ProductMeasures.Select(x => new { x.Measure, x.UnitPrice }).ToList();

        //    if (objMeasures == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(objMeasures);
        //}



        [HttpPost]

        public async Task<ActionResult> Post([FromBody] ProductsModelDTO productsModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueId = System.Guid.NewGuid().ToString();
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();

                var produtName = (productsModel.ProductName).ToUpper().Substring(0, 3);
                var rnd = new Random();
                int num = rnd.Next(50);
                var product = new ProductsModel
                {
                    Id = uniqueId,
                    ProductCode = produtName + num,
                    ProductName = (productsModel.ProductName).ToUpper(),
                    AdminId = currentUser,
                    ProductMeasures = new List<ProductMeasureModel>()
                };
                foreach (var item in productsModel.ProductMeasures)
                {
                    string measureUniqueId = System.Guid.NewGuid().ToString();
                    var prodMeasure = new ProductMeasureModel
                    {
                        Id = measureUniqueId,
                        CostPrice = item.CostPrice,
                        Measure = item.Measure,
                        ProductId = product.Id,
                        QtyPerMeasure = item.QtyPerMeasure,
                        UnitPrice = item.UnitPrice,
                    };

                    product.ProductMeasures.Add(prodMeasure);
                }
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

        public async Task<ActionResult> Put(string Id, [FromBody] ProductsModelDTO productsModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();

                var productName = (productsModel.ProductName).ToUpper().Substring(0, 3);
                var rnd = new Random();
                int num = rnd.Next(50);
                var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser).Include(y => y.ProductMeasures).FirstOrDefault();
                product.ProductName = (productsModel.ProductName).ToUpper();
                product.ProductCode = productName + num;
                var productMeasureExsit = product.ProductMeasures;
                foreach(var child in product.ProductMeasures)
                {
                    _db.ProductMeasureModel.Remove(child);
                }
                product.ProductMeasures = new List<ProductMeasureModel>();
                foreach (var item in productsModel.ProductMeasures)
                {
                    string measureUniqueId = System.Guid.NewGuid().ToString();
                    var productQty = productMeasureExsit.Find(x => x.Measure == item.Measure);
                    if (productQty == null)
                    {
                        var productMeasure = new ProductMeasureModel
                        {
                            Id = measureUniqueId,
                            ProductId = product.Id,
                            CostPrice = item.CostPrice,
                            Measure = item.Measure,
                            QtyPerMeasure = item.QtyPerMeasure,
                            UnitPrice = item.UnitPrice,
                        };
                        product.ProductMeasures.Add(productMeasure);
                    }
                    else
                    {
                        var productMeasure = new ProductMeasureModel
                        {
                            Id = measureUniqueId,
                            ProductId = product.Id,
                            CostPrice = item.CostPrice,
                            Measure = item.Measure,
                            QtyPerMeasure = item.QtyPerMeasure,
                            UnitPrice = item.UnitPrice,
                            Quantity = productQty.Quantity
                        };
                        product.ProductMeasures.Add(productMeasure);
                    }

                }
                    //    else
                    //    {
                    //        string measureUniqueId = System.Guid.NewGuid().ToString();
                    //        var prodMeasure = new ProductMeasureModel
                    //        {
                    //            Id = measureUniqueId,
                    //            CostPrice = item.CostPrice,
                    //            Measure = item.Measure,
                    //            ProductId = product.Id,
                    //            QtyPerMeasure = item.QtyPerMeasure,
                    //            UnitPrice = item.UnitPrice
                    //        };

                    //        product.ProductMeasures.Add(prodMeasure);
                    //    }
                    //}

                    _db.Products.Update(product);
                _db.SaveChanges();
            }
            return Ok();
        }

        //[HttpPut("productMeasure")]

        //public async Task<ActionResult> PutMeasure(string Id, [FromBody] List<ProductMeasureDTO> productMeasure)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var currentUser = await _userManager.GetUserAsync(User);
        //        string uniqueId = System.Guid.NewGuid().ToString();
        //        var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser.Id).FirstOrDefault();
        //        var productMeasures = new List<ProductMeasureModel>();
        //        foreach(var item in productMeasure)
        //        {
        //            var prodMeasure = new ProductMeasureModel
        //            {
        //                CostPrice = item.CostPrice,
        //                Id = uniqueId,
        //                Measure = item.Measure,
        //                ProductId = product.Id,
        //                QtyPerMeasure = item.QtyPerMeasure,
        //                UnitPrice = item.UnitPrice
        //            };

        //            product.ProductMeasures.Add(prodMeasure);
        //        }

        //        _db.Products.UpdateRange(product);
        //        _db.SaveChanges();
        //    }
        //    return Ok();
        //}

        [HttpDelete("deletemeasure/{id}")]

        public async Task<ActionResult> DeleteMeasure(string ProductId, string MeasureId)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();

            var product = _db.Products.Where(x => x.Id == ProductId && x.AdminId == currentUser).FirstOrDefault();
            var productMeasure = product.ProductMeasures.Where(x => x.Id != MeasureId).ToList();
            product.ProductMeasures = productMeasure;
            _db.Products.UpdateRange(product);
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete(string Id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
            var product = _db.Products.Where(x => x.Id == Id && x.AdminId == currentUser).Include(y => y.ProductMeasures).FirstOrDefault();

            _db.Products.Remove(product);
            _db.SaveChanges();
            return Ok();
        }



    }
}
