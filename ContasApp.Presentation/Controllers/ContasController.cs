using ContasApp.Data.Entities;
using ContasApp.Data.Repositories;
using ContasApp.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing;

namespace ContasApp.Presentation.Controllers
{
    [Authorize]
    public class ContasController : Controller
    {
        /// <summary>
        /// Método para abrir a página /Contas/Cadastro
        /// </summary>
        public IActionResult Cadastro()
        {
            ViewBag.Categorias = ObterCategorias();
            return View();
        }

        /// <summary>
        /// Método para capturar o SUBMIT POST do formulário
        /// </summary>
        [HttpPost]
        public IActionResult Cadastro(ContasCadastroViewModel model)
        {
            //verificar se os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //capturar o usuário autenticado no arquivo de cookie do Asp.Net
                    var usuario = JsonConvert.DeserializeObject<Usuario>(User.Identity.Name);

                    //capturando os dados da conta
                    var conta = new Conta
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = model.Data.Value,
                        Valor = model.Valor.Value,
                        Tipo = model.Tipo.Value,
                        Observacoes = model.Observacoes,
                        CategoriaId = model.CategoriaId.Value,
                        UsuarioId = usuario.Id
                    };

                    //gravar a conta no banco de dados
                    var contaRepository = new ContaRepository();
                    contaRepository.Add(conta);

                    TempData["MensagemSucesso"] = "Conta cadastrada com sucesso!";
                    ModelState.Clear(); //limpar os campos do formulário
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros no preenchimento do formulário de cadastro, por favor verifique.";
            }

            ViewBag.Categorias = ObterCategorias();
            return View();
        }

        /// <summary>
        /// Método para abrir a página /Contas/Consulta
        /// </summary>
        public IActionResult Consulta()
        {
            var model = new ContasConsultaViewModel();

            try
            {
                var qtdDiasMesAtual = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

                //primeiro dia do mês atual
                model.DataInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                model.DataFim = new DateTime(DateTime.Today.Year, DateTime.Today.Month, qtdDiasMesAtual);

                //capturando o usuário autenticado no sistema
                var usuario = JsonConvert.DeserializeObject<Usuario>(User.Identity.Name);

                //consultando as contas do usuário no banco de dados
                var contaRepository = new ContaRepository();
                ViewBag.Contas = contaRepository.GetByDatasAndUsuario(model.DataInicio.Value, model.DataFim.Value, usuario.Id);
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }

        /// <summary>
        /// Método para capturar o SUBMIT POST da página de consulta
        /// </summary>
        [HttpPost]
        public IActionResult Consulta(ContasConsultaViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //capturando o usuário autenticado no sistema
                    var usuario = JsonConvert.DeserializeObject<Usuario>(User.Identity.Name);

                    //consultar as contas do usuário
                    var contaRepository = new ContaRepository();
                    var contas = contaRepository.GetByDatasAndUsuario(model.DataInicio.Value, model.DataFim.Value, usuario.Id);

                    if(contas.Count > 0) //a consulta obteve resultados
                    {
                        ViewBag.Contas = contas; //enviando a lista de contas para a página
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhuma conta foi encontrada para o período de datas selecionado.";
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros no preenchimento do formulário de consulta, por favor verifique.";
            }

            return View();
        }

        /// <summary>
        /// Método para processar a ação de exclusão /Contas/Exclusao
        /// </summary>
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var contaRepository = new ContaRepository();

                //buscando a conta no banco de dados através do ID
                var conta = contaRepository.GetById(id);

                //excluindo a conta
                contaRepository.Delete(conta);

                TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', excluído com sucesso.";
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionando para a página de consulta
            return RedirectToAction("Consulta");
        }

        /// <summary>
        /// Método para abrir a página /Contas/Edicao
        /// </summary>
        public IActionResult Edicao(Guid id)
        {
            var model = new ContasEdicaoViewModel();

            try
            {
                //buscar a conta no repositório através do ID
                var contaRepository = new ContaRepository();
                var conta = contaRepository.GetById(id);

                //preencher o objeto 'model' com os dados da conta
                model.Id = conta.Id;
                model.Nome = conta.Nome;
                model.Data = conta.Data;
                model.Valor = conta.Valor;
                model.Tipo = conta.Tipo;
                model.Observacoes = conta.Observacoes;
                model.CategoriaId = conta.CategoriaId;

                //gerando os dados das categorias
                ViewBag.Categorias = ObterCategorias();
            }
            catch(Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }

        /// <summary>
        /// Método para capturar o SUBMIT POST da página de edição
        /// </summary>
        [HttpPost]
        public IActionResult Edicao(ContasEdicaoViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var conta = new Conta
                    {
                        Id = model.Id,
                        Nome = model.Nome,
                        Data = model.Data.Value,
                        Valor = model.Valor.Value,
                        Tipo = model.Tipo.Value,
                        Observacoes = model.Observacoes,
                        CategoriaId = model.CategoriaId.Value
                    };

                    //atualizando a conta no banco de dados
                    var contaRepository = new ContaRepository();
                    contaRepository.Update(conta);

                    TempData["MensagemSucesso"] = $"Conta '{conta.Nome}', atualizada com sucesso.";
                    return RedirectToAction("Consulta"); //redirecionando para a página de consulta
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário de edição.";
            }

            ViewBag.Categorias = ObterCategorias();
            return View(model);
        }

        /// <summary>
        /// Método para gerar uma lista de categorias
        /// para preencher um campo DropDownList na página
        /// </summary>
        private List<SelectListItem> ObterCategorias()
        {
            var lista = new List<SelectListItem>();

            //consultar todas as categorias cadastradas no banco de dados
            var categoriaRepository = new CategoriaRepository();
            var categorias = categoriaRepository.GetAll();

            //preencher a lista com as categorias
            foreach (var item in categorias)
            {
                lista.Add(new SelectListItem 
                { 
                    Value = item.Id.ToString(), //valor do campo
                    Text = item.Descricao //texto exibido no campo
                });
            }

            return lista;
        }
    }
}
