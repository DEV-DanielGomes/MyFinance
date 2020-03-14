using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class TransacaoController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;


        //injeção de dependência
        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ////////construtor que retorna a volar da injeção (sessão do usuário passado por parametro)
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor); // trafegar entre a controladora e a view
            ViewBag.ListaTransacao = objTransacao.ListaTransacao();
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(TransacaoModel formulario) //Vem com o ID preenchido -> Tratamento no insert
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
        public IActionResult Registrar(int? id) //Dizendo que o ID pode ser zero "?" -> No primeiro momento eu informo o valor 0
        {
            if (id != null)
            {
                TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor); //Estancionando o TransacaoModel e passando a injeção por parametro
                ViewBag.Registro = objTransacao.CarregarRegistro(id);
            }

            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta().; //Retornar a listagem de contas por meio de uma viewbag (Passa o construtor e retorna uma lista de contas(Model Conta))
            ViewBag.ListaPlanoContas = new PlanoContaModel(HttpContextAccessor).ListaPlanoConta();
            return View();
        }

        [HttpGet]
        [HttpPost]

        public IActionResult Extrato (TransacaoModel formulario) // formulário é um objeto do tipo transação model
        {
            formulario.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListaTransacao = formulario.ListaTransacao();
            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta();

            return View();
        }

        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {

            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor); //Estancionando o PlanoContaModel e passando a injeção por parametro
            ViewBag.Registro = objTransacao.CarregarRegistro(id);

            return View();
        }
        [HttpGet]
        public IActionResult Excluir(int id)
        {
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
            objTransacao.Excluir(id);
            return RedirectToAction("Index");
        }

        public IActionResult Dashboard()
        {


            List<Dashboard> lista = new Dashboard(HttpContextAccessor).RetornarDadosGraficoPie(); //Aqui criando uma lista e chamando o método Retorna Graficos
            string valores = " ";
            string cores = " ";
            string labels = " ";
            var random = new Random();
            for (int i = 0; i < lista.Count; i++)
            {
                valores += lista[i].Total.ToString() + ","; //MyFinance.Models.Dashboard,MyFinance.Models.Dashboard,MyFinance.Models.Dashboard,],
                labels += "'" + lista[i].PlanoConta.ToString() + "',"; //Alimentacao is not defined -> é preciso aspas simples -> Como ficará no navegador: labels: [ 'MyFinance.Models.Dashboard','MyFinance.Models.Dashboard','MyFinance.Models.Dashboard',]

                cores += "'" + String.Format("#{0:X6}", random.Next(0x1000000)) + "',"; //					backgroundColor: [ '#520DD9','#AD2EFB','#5FB150',], 
            }
            ViewBag.Valores = valores;
            ViewBag.Cores = cores;
            ViewBag.Labels = labels;

            return View();
        }
    }
       
    
}