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

        public const string TEMP_DATA_STEP_NUMBER_KEY = "stepNumber";
        private const string SESSION_STORAGE_RESULTS_KEY = "results";

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
            if(model.StepNumber > 0)
            {
                //AddModelToSession(model);
                UpdateModelInSession(model);           
            }

            // storing data in TempData because of PRG pattern
            TempData[TEMP_DATA_STEP_NUMBER_KEY] = model.StepNumber;

            return RedirectToAction("NextStep");
        }

        [HttpGet]
        public IActionResult NextStep()
        {
            // reading data from TempData because of PRG pattern
            var stepNumber = Convert.ToInt16(TempData[TEMP_DATA_STEP_NUMBER_KEY]);

            if(stepNumber >= _settings.StepsCount)
            {
                return RedirectToAction("Finish");
            }

            var model = new StepModel()
            {
                StepNumber = stepNumber + 1,
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

            _sessionStorage.SetObjectAsJson(SESSION_STORAGE_RESULTS_KEY, data);
        }

        void UpdateModelInSession(StepModel model){

            var data = GetRegisteredSteps();

            var registeredModel = data
                .Where(x => x.StepNumber == model.StepNumber)
                .Single();

            registeredModel.ElapsedTime = model.ElapsedTime;
            registeredModel.SameColor = model.SameColor;

            _sessionStorage.SetObjectAsJson(SESSION_STORAGE_RESULTS_KEY, data);
        }
    }
}
