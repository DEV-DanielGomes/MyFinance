using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class PlanoContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;


        //injeção de dependência
        public PlanoContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ////////construtor que retorna a volar da injeção (sessão do usuário passado por parametro)
            PlanoContaModel objPlanoConta = new PlanoContaModel(HttpContextAccessor); // trafegar entre a controladora e a view
            ViewBag.ListaPlanoConta = objPlanoConta.ListaPlanoConta();
            return View();
        }

        [HttpPost]
        public IActionResult CriarPlanoConta(PlanoContaModel formulario) //Vem com o ID preenchido -> Tratamento no insert
        {

            if (ModelState.IsValid)
            {
                formulario.HttpContextAccessor = HttpContextAccessor; //Pegar a sessão do usuário
                formulario.Insert(); // chama o método Insert/Update 
                return RedirectToAction("Index"); //Redireciona para o Index
            }
            return View();
        }
        [HttpGet] // Para retornar o formulário vazio
        public IActionResult CriarPlanoConta(int? id) //Dizendo que o ID pode ser zero
        {
            if (id != null)
            {
                PlanoContaModel objplanoContaModel = new PlanoContaModel(HttpContextAccessor); //Estancionando o PlanoContaModel e passando a injeção por parametro
                ViewBag.Registro = objplanoContaModel.CarregarRegistro(id);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
            PlanoContaModel objconta = new PlanoContaModel(HttpContextAccessor);
            objconta.Excluir(id);
            return RedirectToAction("Index");
        }
    }
}   