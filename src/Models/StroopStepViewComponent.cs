using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StroopTest.Models
{
    public class StroopStepViewComponent : ViewComponent
    {
        public StroopStepViewComponent()
        { }

        public IViewComponentResult Invoke(StepModel model)
        {            
            return View(model);
        }
    }
}