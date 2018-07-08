using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StroopTest.Configuration;
using StroopTest.Extensions;
using StroopTest.Interfaces;
using StroopTest.Models;

namespace StroopTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionStorage _sessionStorage;
        private readonly IColorProvider _colorProvider;
        private readonly StroopTestSettings _settings;

        public HomeController(ISessionStorage sessionStorage, IColorProvider colorProvider, StroopTestSettings settings)
        {
            _sessionStorage = sessionStorage;
            _colorProvider = colorProvider;
            _settings = settings;
        }
        
        public IActionResult Index()
        {
            _sessionStorage.Clear();

            return View();
        }

        [HttpPost]
        public IActionResult GoToNextStep(StepModel model)
        {
            if(model.Step > 0)
            {
                //AddModelToSession(model);
                UpdateModelInSession(model);           
            }

            // storing data in TempData because of PRG pattern
            TempData["step"] = model.Step;

            return RedirectToAction("NextStep");
        }

        [HttpGet]
        public IActionResult NextStep()
        {
            // reading data from TempData because of PRG pattern
            var step = Convert.ToInt16(TempData["step"]);

            if(step >= _settings.StepsCount)
            {
                return RedirectToAction("Finish");
            }

            var model = new StepModel()
            {
                Step = step + 1,
                Colors = _colorProvider.GetRandomColor()
            };

            AddModelToSession(model);

            return View(model);
        }

        public IActionResult Finish()
        {
            var model = GetRegisteredSteps();

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        List<StepModel> GetRegisteredSteps()
        {
            return _sessionStorage.GetObjectFromJson<List<StepModel>>("results");
        }

        void AddModelToSession(StepModel model){

            var data = GetRegisteredSteps();

            data.Add(model);

            _sessionStorage.SetObjectAsJson("results", data);
        }

        void UpdateModelInSession(StepModel model){

            var data = GetRegisteredSteps();

            var registeredModel = data
                .Where(x => x.Step == model.Step)
                .Single();

            registeredModel.ElapsedTime = model.ElapsedTime;
            registeredModel.SameColor = model.SameColor;

            _sessionStorage.SetObjectAsJson("results", data);
        }
    }
}
