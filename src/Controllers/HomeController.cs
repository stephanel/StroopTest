using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StroopTest.Configuration;
using StroopTest.Interfaces;
using StroopTest.Models;

namespace StroopTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionStorage _sessionStorage;
        private readonly IColorProvider _colorProvider;
        private readonly StroopTestSettings _settings;

        private const string TEMP_DATA_STEP_NUMBER_KEY = "stepNumber";
        private const string SESSION_STORAGE_RESULTS_KEY = "results";
        private const int NUMBER_OF_EACH = 20;

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

        private List<StepModel> GenerateRandomsColorsModels(Func<ColorsModel> funcToRandomColorsModel, ref int stepNumber)
        {
            var models = new List<StepModel>();

            for (var i = 1; i <= NUMBER_OF_EACH; i++)
            {
                var model = new StepModel()
                {
                    StepNumber = stepNumber,
                    Colors = funcToRandomColorsModel()
                };

                models.Add(model);

                stepNumber++;
            }

            return models;
        }
        public IActionResult Start()
        {
            var models = new List<StepModel>();

            int stepNumber = 1;

            var congruentModels = GenerateRandomsColorsModels(() => _colorProvider.GetCongruentColor(), ref stepNumber);
            var incongruentModels = GenerateRandomsColorsModels(() => _colorProvider.GetIncongruentColor(), ref stepNumber);

            models.AddRange(congruentModels);
            models.AddRange(incongruentModels);

            // random sort
            var rnd = new Random();
            models = models
                .OrderBy(item => rnd.Next())
                .Select((item,index) => {
                    item.StepNumber = index + 1;
                    return item;
                })
                .ToList();

            AddModelsToSession(models);

            return RedirectToAction("NextStep");
        }

        [HttpGet]
        public IActionResult NextStep()
        {
            // reading data from TempData because of PRG pattern
            var stepNumber = Convert.ToInt16(TempData[TEMP_DATA_STEP_NUMBER_KEY]);

            if(stepNumber >= NUMBER_OF_EACH * 2)    // * 3 => congruents, incongruents and neutrals
            {
                return RedirectToAction("Finish");
            }

            var model = GetModelFromSession(stepNumber + 1);

            return View(model);
        }

        [HttpPost]
        public IActionResult GoToNextStep(StepModel model)
        {
            UpdateModelInSession(model);           

            // storing data in TempData because of PRG pattern
            TempData[TEMP_DATA_STEP_NUMBER_KEY] = model.StepNumber;

            return RedirectToAction("NextStep");
        }

        public IActionResult Finish()
        {
            var stepModels = GetRegisteredSteps();

            var model = new ResultsModel()
            {
                CongruentWords = new ResultsSetModel()
                {
                    Results = stepModels.Where(x => x.IsCongruent).ToList()
                },
                IncongruentWords = new ResultsSetModel()
                {
                    Results = stepModels.Where(x => x.IsIncongruent).ToList()
                }
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        List<StepModel> GetRegisteredSteps()
        {
            return _sessionStorage.GetObjectFromJson<List<StepModel>>(SESSION_STORAGE_RESULTS_KEY);
        }
       
        private void AddModelsToSession(List<StepModel> models)
        {
            var data = GetRegisteredSteps();
            foreach (var model in models)
            {
                data.Add(model);
            }
            _sessionStorage.SetObjectAsJson(SESSION_STORAGE_RESULTS_KEY, data);
        }

        private void UpdateModelInSession(StepModel model){

            var data = GetRegisteredSteps();

            var registeredModel = data
                .Where(x => x.StepNumber == model.StepNumber)
                .Single();

            registeredModel.ElapsedTime = model.ElapsedTime;
            registeredModel.SameColor = model.SameColor;

            _sessionStorage.SetObjectAsJson(SESSION_STORAGE_RESULTS_KEY, data);
        }

        private StepModel GetModelFromSession(int stepNumber)
        {
            var data = GetRegisteredSteps();

            return data
                .Where(x => x.StepNumber == stepNumber)
                .Single();
        }
    }
}
