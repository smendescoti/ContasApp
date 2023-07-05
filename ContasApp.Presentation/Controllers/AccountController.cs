using ContasApp.Data.Entities;
using ContasApp.Data.Repositories;
using ContasApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContasApp.Presentation.Controllers
{
    /// <summary>
    /// Classe de controle do Asp.Net MVC
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Método para abrir a página /Account/Login
        /// </summary>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Método para abrir a página /Account/Register
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Método para capturar o SUBMIT POST da página /Account/Register
        /// </summary>
        [HttpPost] //Receber o SUBMIT POST do formulário
        public IActionResult Register(AccountRegisterViewModel model)
        {
            //verificar se todos os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //capturando os dados do usuário
                    var usuario = new Usuario();

                    usuario.Id = Guid.NewGuid();
                    usuario.Nome = model.Nome;
                    usuario.Email = model.Email;
                    usuario.Senha = model.Senha;
                    usuario.DataHoraCriacao = DateTime.Now;

                    //gravando o usuário no banco de dados
                    var usuarioRepository = new UsuarioRepository();
                    usuarioRepository.Add(usuario);

                    TempData["Mensagem"] = "Parabéns, sua conta de usuário foi cadastrada com sucesso!";
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

            return View();
        }

        /// <summary>
        /// Método para abrir a página /Account/ForgotPassword
        /// </summary>
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
