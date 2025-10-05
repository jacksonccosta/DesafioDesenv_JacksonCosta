using AspNetCore.Reporting;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Commands;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Queries;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries;
using WebApp_Desafio_Shared.ViewModels;
using WebApp_Desafio_Shared.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHostingEnvironment _hostEnvironment;

        public ChamadosController(IMediator mediator, IHostingEnvironment hostEnvironment)
        {
            _mediator = mediator;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index() => RedirectToAction(nameof(Listar));

        [HttpGet]
        public IActionResult Listar() => View("~/Views/Chamados/Listar.cshtml");

        [HttpGet]
        public async Task<IActionResult> Datatable()
        {
            var lstChamados = await _mediator.Send(new GetAllChamadosQuery());
            var vms = lstChamados.Select(c => new ChamadoViewModel
            {
                ID = c.ID,
                Assunto = c.Assunto,
                Solicitante = c.Solicitante?.Nome,
                IdDepartamento = c.IdDepartamento,
                Departamento = c.Departamento?.Descricao,
                DataAbertura = c.DataAbertura
            }).ToList();

            var dataTableVM = new DataTableAjaxViewModel() { data = vms };
            return Ok(dataTableVM);
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar()
        {
            var chamadoVM = new ChamadoViewModel { DataAbertura = DateTime.Now };
            ViewData["Title"] = "Cadastrar Novo Chamado";

            var departamentos = await _mediator.Send(new GetAllDepartamentosQuery());
            ViewData["ListaDepartamentos"] = departamentos.Select(d => new DepartamentoViewModel { ID = d.ID, Descricao = d.Descricao }).ToList();

            return View("~/Views/Chamados/Cadastrar.cshtml", chamadoVM);
        }

        [HttpGet]
        public async Task<IActionResult> Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Editar Chamado";
            var chamado = await _mediator.Send(new GetChamadoByIdQuery { Id = id });
            var chamadoVM = new ChamadoViewModel
            {
                ID = chamado.ID,
                Assunto = chamado.Assunto,
                IdSolicitante = chamado.IdSolicitante,
                Solicitante = chamado.Solicitante?.Nome,
                IdDepartamento = chamado.IdDepartamento,
                Departamento = chamado.Departamento?.Descricao,
                DataAbertura = chamado.DataAbertura
            };

            var departamentos = await _mediator.Send(new GetAllDepartamentosQuery());
            ViewData["ListaDepartamentos"] = departamentos.Select(d => new DepartamentoViewModel { ID = d.ID, Descricao = d.Descricao }).ToList();

            return View("~/Views/Chamados/Cadastrar.cshtml", chamadoVM);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(ChamadoViewModel chamadoVM)
        {
            var command = new GravarChamadoCommand
            {
                ID = chamadoVM.ID,
                Assunto = chamadoVM.Assunto,
                IdSolicitante = chamadoVM.IdSolicitante,
                IdDepartamento = chamadoVM.IdDepartamento,
                DataAbertura = chamadoVM.DataAbertura
            };

            var sucesso = await _mediator.Send(command);

            if (sucesso)
                return Ok(new ResponseViewModel("Sucesso!", "Chamado gravado com sucesso!", AlertTypes.success, "Chamados", nameof(Listar)));

            throw new ApplicationException("Falha ao gravar o Chamado.");
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir([FromRoute] int id)
        {
            var sucesso = await _mediator.Send(new ExcluirChamadoCommand { Id = id });
            if (sucesso)
                return Ok(new ResponseViewModel("Sucesso!", $"Chamado {id} excluído com sucesso!", AlertTypes.success));

            throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
        }

        [HttpGet]
        public async Task<IActionResult> SearchSolicitantes(string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }
            var query = new SearchSolicitantesQuery { TermoBusca = term };
            var solicitantes = await _mediator.Send(query);
            return Json(solicitantes.Select(s => new { id = s.Id, text = s.Text }));
        }

        [HttpGet]
        public async Task<IActionResult> Report()
        {
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "Reports", "rptChamados.rdlc");

            LocalReport localReport = new LocalReport(path);

            var lstChamados = await _mediator.Send(new GetAllChamadosQuery());

            var reportData = lstChamados.Select(c => new
            {
                c.ID,
                c.Assunto,
                Solicitante = c.Solicitante?.Nome,
                Departamento = c.Departamento?.Descricao,
                c.DataAbertura
            }).ToList();

            localReport.AddDataSource("dsChamados", reportData);

            var result = localReport.Execute(RenderType.Pdf);
            return File(result.MainStream, "application/pdf", "RelatorioChamados.pdf");
        }
    }
}