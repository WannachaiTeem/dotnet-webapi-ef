using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_webapi_ef.Data;
using dotnet_webapi_ef.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_webapi_ef.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        // Attribute context is private and readonly
        // _name => only use in this class => this.xxxx
        private readonly ApplicationDBContext _context;
        public CustomerController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(){
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var customers = _context.Customers.Find(id);
            if(customers == null){
                return NotFound();
            }
            return Ok(customers.ToCustomerDTO());
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName([FromRoute] string name){
            var customers = _context.Customers.Where(c=> c.Fullname.Contains(name));
            if(customers == null){
                return NotFound();
            }
            return Ok(customers);
        }
    }
}