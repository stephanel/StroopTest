using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace StroopTest.Models
{
    public class StroopStepViewComponent : ViewComponent
    {
        public StroopStepViewComponent()
        { }

        public async Task<IViewComponentResult> InvokeAsync(StepModel model)
        {            
            return View(model);
        }
    }
}