using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class ContaController : Controller
    {
        //classe da injeção de dependência

        IHttpContextAccessor HttpContextAccessor;


        //injeção de dependência
        public ContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ////////construtor que retorna a volar da injeção (sessão do usuário passado por parametro)
            ContaModel objconta = new ContaModel(HttpContextAccessor); // trafegar entre a controladora e a view
            ViewBag.ListaConta = objconta.ListaConta();
            return View();
        }

        [HttpPost]
        public IActionResult CriarConta(ContaModel formulario)
        {

            if (ModelState.IsValid)
            {
                formulario.HttpContextAccessor = HttpContextAccessor;
                formulario.Insert();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult CriarConta()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirConta(int id)
        {
            ContaModel objconta = new ContaModel(HttpContextAccessor);
            objconta.Excluir(id);
            return RedirectToAction("Index");
        }
    }
}