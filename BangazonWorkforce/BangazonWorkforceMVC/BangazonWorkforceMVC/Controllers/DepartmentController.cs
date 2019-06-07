using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BangazonWorkforceMVC.Repositories;
using BangazonWorkforceMVC.Models.View;

using BangazonWorkforceMVC.Models;


namespace BangazonWorkforceMVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
        {
            DepartmentRepository.SetConfig(config);
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Department
        public ActionResult Index()
        {

            List<DepartmentEmployeeViewModel> departments = DepartmentRepository.GetDepartments();
            return View(departments);

        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            Department department = DepartmentRepository.GetDepartmentDetails(id);
            ViewData["Title"] = department.Name;
            return View(department);
        }



        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
            }
                return RedirectToAction(nameof(Index));}
        public async Task<ActionResult> Create(Department department)
        {
            try
            {
                DepartmentRepository.CreateDepartment(department);
                return RedirectToAction(nameof(Index));


            }
            catch
            {
                return View();
            }
        }
            // GET: Department/Edit/5
            public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}