using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockBalanceUpdateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StockBalanceUpdateController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<StockBalanceUpdateModel> GetAll()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            IEnumerable<StockBalanceUpdateModel> objList = _db.StockBalanceUpdates.Where(x => x.AdminId == currentUser).Include(x => x.stockBalanceUpdateDetails);

            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.StockBalanceUpdates.Where(x => x.AdminId == currentUser && x.Id==Id).Include(x => x.stockBalanceUpdateDetails);


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

       [HttpPost]

        public IActionResult PostStockBalanceUpdate([FromBody] StockBalanceUpdateModel stock)
        {
            string connectionString = @"User ID=postgres;Password=rita20;Server=localhost;Port=5432;Database=SalesRegister";
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                SqlCommand command = new SqlCommand(
                "SELECT ProductCode, Product, Measure, Quantity, AdminId " +
                "FROM StockBalances;", sourceConnection);
                SqlDataReader reader = command.ExecuteReader();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connectionString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = "StockBalanceUpdates";
                    bulkCopy.ColumnMappings.Add("ProductCode", "ProductCode");
                    bulkCopy.ColumnMappings.Add("Product", "Product");
                    bulkCopy.ColumnMappings.Add("Measure", "Measure");
                    bulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                    bulkCopy.ColumnMappings.Add("AdminId", "AdminId");
                    bulkCopy.ColumnMappings.Add("Date", DateTime.Now.ToString());

                    bulkCopy.WriteToServer(reader);
                    reader.Close();
                }
            }

            return Ok();
        }


    }
}
