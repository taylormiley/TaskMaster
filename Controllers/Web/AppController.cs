﻿using System;
using Microsoft.AspNet.Mvc;
using TaskMaster.Services;
using TaskMaster.ViewModels;
using TaskMaster.Models;
using System.Linq;
using Microsoft.AspNet.Authorization;

namespace TaskMaster.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private ITaskRepository _repository;

        public AppController(IMailService service, ITaskRepository repository)
        {
            _mailService = service;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Quests()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("", "Could not send email, configuration problem.");
                }

                if (_mailService.SendMail(email,
                  email,
                  $"Contact Page from {model.Name} ({model.Email})",
                  model.Message))
                {
                    ModelState.Clear();

                    ViewBag.Message = "Mail Sent. Thanks!";

                }
            }

            return View();
        }
    }
}
