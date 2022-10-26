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
       // private readonly IMapper _mapper;

        public ContactController(IConfiguration Configuration, IContactServices contactServices) //IMapper mapper)
        {
            _configuration = Configuration;
            _contactServices = contactServices;
            //_mapper = mapper;
        }

        public IActionResult Index()
        {
            var contacts = _contactServices.GetList();
            return View(contacts);
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
        public IActionResult Edit(int id) //prikaz
        {
            ContactDto newContact = new ContactDto();
            if (id > 0)
            {
                newContact = _contactServices.GetById(id);
            }
            
            return View(newContact);
        }
        [HttpPost]
        public IActionResult Edit(ContactDto contact, int id) //azuriranje kreiranje
        {
            try
            {
                _contactServices.Edit(contact);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

       /* public IActionResult Create()
        {
            return View();
        }
       */

       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactEditViewModel contact) //tu ide taj json 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactEditViewModel addContact = new ContactEditViewModel(); //sql procedura s ifom ako je id > 0 update inace create

                    if (_contactServices.Create(contact))
                    {
                        //umjesto IactionResult vrati se json
                        ModelState.Clear();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        } */

        [HttpPost]
        public IActionResult Delete(int id) //json umjesto IActionResult
        {
            try
            {
                if(_contactServices.DeleteContact(id))
                {
                    
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
    
}
