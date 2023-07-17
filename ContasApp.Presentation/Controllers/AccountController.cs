using ContasApp.Data.Entities;
using ContasApp.Data.Repositories;
using ContasApp.Presentation.Helpers;
using ContasApp.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
        /// Método para capturar o SUBMIT POST da página /Account/Login
        /// </summary>
        [HttpPost]
        public IActionResult Login(AccountLoginViewModel model)
        {
            //verificando se todos os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //consultando o usuário no banco de dados através do email e da senha
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, model.Senha);

                    //verificando se o usuário foi encontrado
                    if(usuario != null)
                    {
                        //gravar o cookie no navegador com os dados do usuário autenticado
                        //este cookie irá gerar a autorização do usuário para acessar as 
                        //páginas restritas do sistema [Authorize]

                        //serializando o objeto usuário para JSON
                        var json = JsonConvert.SerializeObject(usuario);

                        //criando a identificação do usuário no Asp.NET
                        var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, json) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        //gravando os dados no Cookie de autenticação do Asp.Net
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        //redirecionamento para outra página
                        //HOME -> Controller, Index -> View (Home/Index)
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Acesso negado. Usuário inválido.";
                    }
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

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
                    var usuarioRepository = new UsuarioRepository();
                    if(usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["Mensagem"] = "O email informado já está cadastrado, por favor tente outro.";
                    }
                    else
                    {
                        //capturando os dados do usuário
                        var usuario = new Usuario();

                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = model.Senha;
                        usuario.DataHoraCriacao = DateTime.Now;

                        //gravando o usuário no banco de dados                   
                        usuarioRepository.Add(usuario);

                        TempData["Mensagem"] = "Parabéns, sua conta de usuário foi cadastrada com sucesso!";
                    }                    
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

        /// <summary>
        /// Método para capturar o SUBMIT POST da página /Account/ForgotPassword
        /// </summary>
        [HttpPost] //Receber o SUBMIT POST do formulário
        public IActionResult ForgotPassword(AccountForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //buscar o usuário no banco de dados através do email
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmail(model.Email);

                    //verificando se o usuário foi encontrado
                    if(usuario != null)
                    {
                        //gerando uma nova senha para o usuário
                        var novaSenha = PasswordHelper.GeneratePassword(true, true, true, true, 10);

                        //escrevendo o email para o usuário
                        var subject = "Recuperação de senha de usuário - COTI Informática";
                        var body = $@"
                            <div style='padding: 40px; margin: 40px; border: 1px solid #ccc; text-align: center;'>
                                <img src='https://www.cotiinformatica.com.br/imagens/logo-coti-informatica.png'/>
                                <hr/>
                                <h5>Olá {usuario.Nome}</h5>
                                <p>Uma nova senha de acesso foi gerada para você.</p>
                                <p>Acesse o sistema com a senha: {novaSenha}</p>
                                <br/>
                                <p>Att, equipe COTI Informática</p>
                            </div>
                        ";

                        //enviando o email para o usuário
                        EmailMessageHelper.SendMessage(usuario.Email, subject, body);
                        //atualizando a senha no banco de dados
                        usuarioRepository.UpdatePassword(usuario.Id, novaSenha);

                        TempData["Mensagem"] = "Recuperação de senha realizada com sucesso.";
                        ModelState.Clear();
                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário não encontrado.";
                    }
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

            return View();
        }

        /// <summary>
        /// Método para fazer o logout do usuário no sistema
        /// /Account/Logout
        /// </summary>
        public IActionResult Logout()
        {
            //apagar o cookir de autenticação gravado no navegador
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar o usuário de volta para a página de login
            return RedirectToAction("Login");
        }
    }
}
