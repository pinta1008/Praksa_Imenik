using Contacts.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Options;
using Contacts.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace Contacts.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IContactServices _contactServices;
        private readonly IMapper _mapper;

        public ContactController(IConfiguration Configuration, IContactServices contactServices,IMapper mapper)
        {
            _configuration = Configuration;
            _contactServices = contactServices;
            _mapper = mapper;
        }

        public IActionResult Index(int pg = 1)
        {
            var contacts = _contactServices.GetList();
            const int pageSize = 2;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = contacts.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = contacts.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ContactDto newContact = new ContactDto();
            if (id > 0)
            {
                newContact = _contactServices.GetById(id);
            }
            
            return View(newContact);
        }
        [HttpPost]
        public IActionResult Edit(ContactEditViewModel VMcontact, int id)
        {
            ContactDto contactDto = new ContactDto();
            _mapper.Map(VMcontact, contactDto);
            try
            {
                _contactServices.Edit(contactDto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            ContactDto newContact = new ContactDto();
            if (id > 0)
            {
                newContact = _contactServices.GetById(id);
            }

            return View(newContact);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _contactServices.DeleteContact(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




    }
    
}
