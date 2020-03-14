using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;
using System.Diagnostics;

namespace MyFinance.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            /* 
             *A forma de apresentação abaixo é equivalente a ViewData. Na ViewData está resumido. 
             * HomeModel objhomemodel = HomeModel();
             string nome = objhomemodel.LerNumeroUsuario();
             ViewData["Nome"] = nome;
              */

            ViewData["Nome"] = new HomeModel().LerNumeroUsuario();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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

        public IActionResult Menu()
        {
            return View(); // O retorno de View será sempre o que é inserido no método, neste caso o MENU é a View em questão
        }
        public IActionResult Ajuda()
        {
            return View(); // O retorno de View será sempre o que é inserido no método, neste caso o MENU é a View em questão
        }
    }
}
