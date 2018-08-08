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
    public class StroopTestController : Controller
    {
        private readonly ISessionStorage _sessionStorage;
        private readonly IColorProvider _colorProvider;
        private readonly StroopTestSettings _settings;
        
        public const string SESSION_PHASE_NUMBER_KEY = "phaseNumber";
        public const string SESSION_STEP_NUMBER_KEY = "stepNumber";
        private const string SESSION_STORAGE_RESULTS_KEY = "results";
        private const int NUMBER_OF_EACH = 20;

        public StroopTestController(ISessionStorage sessionStorage, IColorProvider colorProvider, StroopTestSettings settings)
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

        private TestPhaseModel GenerateRandomsColorsModels(Func<ColorsModel> funcToRandomColorsModel, int phaseNumber)
        {
            var phase = new TestPhaseModel()
            {
                PhaseNumber = phaseNumber
            };

            phase.StepModels = new List<StepModel>();

            for (var i = 1; i <= NUMBER_OF_EACH; i++)
            {
                var model = new StepModel()
                {
                    PhaseNumber = phaseNumber,
                    StepNumber = i,
                    Colors = funcToRandomColorsModel()
                };

                phase.StepModels.Add(model);
            }

            return phase;
        }

        public IActionResult Start()
        {
            var models = new List<TestPhaseModel>();

            var congruentModels = GenerateRandomsColorsModels(() => _colorProvider.GetCongruentColor(), phaseNumber: 1);

            var incongruentModels = GenerateRandomsColorsModels(() => _colorProvider.GetIncongruentColor(), phaseNumber: 2);

            models.Add(congruentModels);
            models.Add(incongruentModels);

            AddModelsToSession(models);

            return RedirectToAction("StartTestPhase", new { phaseNumber = 1 });
        }

        [HttpGet]
        public IActionResult StartTestPhase(int phaseNumber)
        {
            _sessionStorage.SetString(SESSION_PHASE_NUMBER_KEY, phaseNumber.ToString());
            _sessionStorage.SetString(SESSION_STEP_NUMBER_KEY, 0.ToString());

            // TODO: isolate invite messages
            if (phaseNumber == 1)
            {
                ViewBag.PhaseTitle = "PHASE 1";
                ViewBag.PhaseInfos = "CONGRUENT WORDS";
                ViewBag.PhaseNumber = 1;
            }
            else
            {
                ViewBag.PhaseTitle = "PHASE 2";
                ViewBag.PhaseInfos = "INCONGRUENT WORDS";
                ViewBag.PhaseNumber = 2;
            }

            return View();
        }

        [HttpGet]
        public IActionResult NextStep()
        {
            // reading data from TempData because of PRG pattern
            var phaseNumber = Convert.ToInt16(_sessionStorage.GetString(SESSION_PHASE_NUMBER_KEY));
            var stepNumber = Convert.ToInt16(_sessionStorage.GetString(SESSION_STEP_NUMBER_KEY));

            if(stepNumber >= NUMBER_OF_EACH)    // * 3 => congruents, incongruents and neutrals
            {
                if(phaseNumber < 2)
                {
                    return RedirectToAction("StartTestPhase", new { phaseNumber = 2 });
                }

                return RedirectToAction("Finish");
            }

            var model = GetModelFromSession(phaseNumber, stepNumber + 1);

            return View(model);
        }

        [HttpPost]
        public IActionResult GoToNextStep(StepModel model)
        {
            UpdateModelInSession(model);

            // storing data in TempData because of PRG pattern
            _sessionStorage.SetString(SESSION_STEP_NUMBER_KEY, model.StepNumber.ToString());

            return RedirectToAction("NextStep");
        }

        public IActionResult Finish()
        {
            var stepModels = GetRegisteredSteps();

            var model = new ResultsModel()
            {
                CongruentWords = new ResultsSetModel()
                {
                    Results = stepModels.First(phase => phase.PhaseNumber == 1).StepModels
                },
                IncongruentWords = new ResultsSetModel()
                {
                    Results = stepModels.First(phase => phase.PhaseNumber == 2).StepModels
                }
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        List<TestPhaseModel> GetRegisteredSteps()
        {
            return _sessionStorage.GetObjectFromJson<List<TestPhaseModel>>(SESSION_STORAGE_RESULTS_KEY);
        }
       
        private void AddModelsToSession(List<TestPhaseModel> models)
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
                .Where(phase => phase.PhaseNumber == model.PhaseNumber)
                .SelectMany(phase => phase.StepModels)
                .First(x => x.StepNumber == model.StepNumber);

            registeredModel.ElapsedTime = model.ElapsedTime;
            registeredModel.SameColor = model.SameColor;

            _sessionStorage.SetObjectAsJson(SESSION_STORAGE_RESULTS_KEY, data);
        }

        private StepModel GetModelFromSession(int phaseNumber, int stepNumber)
        {
            var data = GetRegisteredSteps();

            return data
                .Where(phase => phase.PhaseNumber == phaseNumber)
                .SelectMany(phase => phase.StepModels)
                .First(x => x.StepNumber == stepNumber);
        }
    }
}
