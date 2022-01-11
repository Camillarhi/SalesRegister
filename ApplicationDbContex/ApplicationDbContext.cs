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
        public DbSet<ProductBalanceModel> ProductBalances { get; set; }
        public DbSet<ProductBalanceUpdateModel> ProductBalanceUpdates { get; set; }
        public DbSet<CustomerInvoiceModel> CustomerInvoice { get; set; }
        public DbSet<CustomerInvoiceDetailModel> CustomerInvoiceDetails { get; set; }

    }
}
