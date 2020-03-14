using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpGet]
        public IActionResult Login(int? id)

        {
            if (id != null)
            {
                if (id == 0)
                {
                    //Terminando a sessão do usuário após receber ID=0 pela link sair com valor 0. 
                    HttpContext.Session.SetString("NomeUsuarioLogado", string.Empty);
                    HttpContext.Session.SetString("IDUsuarioLogado", string.Empty);
                }
            }

            return View();
        }
        //Método de retorno POST. Método Action Result retorna um resultado e uma ação. No caso abaixo é uma conexão com a Model. 
        [HttpPost]
        public IActionResult ValidaLogin(UsuarioModel Usuario)
        {
            bool login = Usuario.ValidaLogin();

            if (login)
            {
                //Sessão do usuário

                HttpContext.Session.SetString("NomeUsuarioLogado", Usuario.Nome);
                HttpContext.Session.SetString("IdUsuarioLogado", Usuario.Id.ToString()); //set da injeção de dependência (Aqui passo o ID da minha modelo Usuario)
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                TempData["MensagemLoginInvalido"] = "Dados de login inválido";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioModel usuario) //Aqui recebe os dados inseridos no formulário via POST
        {
            if (ModelState.IsValid) //Se os dados requerido estão ok, passa a usar o método insert, senão, ele volta para tela formulário
            {
                //Registra Usuario
                usuario.RegistraUsuario();
                return RedirectToAction("Sucesso");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult Sucesso()
        {

            return View();
        }
    }
}