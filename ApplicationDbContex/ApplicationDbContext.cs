using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.ApplicationDbContex
{
    public class ApplicationDbContext : IdentityDbContext<StaffModel>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProductsModel> Products { get; set; }
        public DbSet<DailyRecordsModel> DailyRecords { get; set; }
        public DbSet<TotalModel> Totals { get; set; }
        public DbSet<CompanyModel> CompanyName { get; set; }
        public DbSet<RolesModel> Department { get; set; }
        public DbSet<StockBalanceModel> StockBalances { get; set; }
        public DbSet<StockBalanceUpdateModel> StockBalanceUpdates { get; set; }
        public DbSet<CustomerInvoiceModel> CustomerInvoice { get; set; }

    }
}
