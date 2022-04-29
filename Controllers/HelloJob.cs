using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Quartz;
using SalesRegister.ApplicationDbContex;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    public class HelloJob : IJob
    {
        
        public async Task Execute(IJobExecutionContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(@"User ID=postgres;Password=rita20;Server=localhost;Port=5432;Database=SalesRegister");
            using var _db = new ApplicationDbContext(optionsBuilder.Options);
            var stockBalancesUpdate = new StockBalanceUpdateModel();
            stockBalancesUpdate.Date = DateTime.Now;
            stockBalancesUpdate.stockBalanceUpdateDetails = new List<StockBalanceUpdateDetailsModel>();
            var stockBalances = _db.StockBalances.ToList();
            //stockBalancesUpdate.AdminId =
            foreach (var stock in stockBalances)
            {
                var stockBalanceUpdate = new StockBalanceUpdateDetailsModel
                {
                    AdminId = stock.AdminId,
                    Measure = stock.Measure,
                    Product = stock.Product,
                    ProductCode = stock.ProductCode,
                    Quantity = stock.Quantity,
                    Date = stockBalancesUpdate.Date
                };
                stockBalancesUpdate.stockBalanceUpdateDetails.Add(stockBalanceUpdate);
            }
            await _db.StockBalanceUpdates.AddAsync(stockBalancesUpdate);
            _db.SaveChanges();

        }
    }
}
