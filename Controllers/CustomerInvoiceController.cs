﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class CustomerInvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        UserManager<StaffModel> _userManager;
        public CustomerInvoiceController(ApplicationDbContext db, UserManager<StaffModel> userManager
            )
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerInvoiceModel>> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<CustomerInvoiceModel> objList = _db.CustomerInvoice.Where(x => x.AdminId == currentUser.CreatedById);
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(string Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (Id == null)
            {
                return NotFound();
            }
            var obj = _db.CustomerInvoice.Where(x => x.AdminId == currentUser.CreatedById && x.Id == Id).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

       

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var customer = _db.CustomerInvoice.Where(x => x.AdminId == currentUser.CreatedById && x.Id == Id).FirstOrDefault();

            _db.CustomerInvoice.Remove(customer);
            _db.SaveChanges();
            return Ok();

        }
    }
}
