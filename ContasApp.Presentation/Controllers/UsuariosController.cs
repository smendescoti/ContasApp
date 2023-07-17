using ContasApp.Data.Entities;
using ContasApp.Data.Repositories;
using ContasApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContasApp.Presentation.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        //Método para abrir a página /Usuarios/AlterarSenha
        public IActionResult AlterarSenha()
        {
            return View();
        }

        //Método para receber o SUBMIT POST da página
        [HttpPost]
        public IActionResult AlterarSenha(UsuariosAlterarSenhaViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //capturando o usuário autenticado no sistema
                    var usuario = JsonConvert.DeserializeObject<Usuario>(User.Identity.Name);
                    var usuarioRepository = new UsuarioRepository();

                    //verificar se a senha atual está correta
                    if(usuarioRepository.GetByEmailAndSenha(usuario.Email, model.SenhaAtual) != null)
                    {
                        //atualizando a senha do usuário no banco de dados
                        usuarioRepository.UpdatePassword(usuario.Id, model.NovaSenha);
                        TempData["MensagemSucesso"] = "Senha de acesso atualizada com sucesso.";
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Senha atual inválida, por favor verifique.";
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de preenchimento do formulário de alteração de senha, por favor verifique.";
            }

            return View();
        }
    }
}
