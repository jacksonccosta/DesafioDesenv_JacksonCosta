using AspNetCore.Reporting;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries;
using WebApp_Desafio_Shared.ViewModels;
using WebApp_Desafio_Shared.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHostingEnvironment _hostEnvironment;

        public DepartamentosController(IMediator mediator, IHostingEnvironment hostEnvironment)
        {
            _mediator = mediator;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index() => RedirectToAction(nameof(Listar));

        [HttpGet]
        public IActionResult Listar() => View("~/Views/Departamentos/Listar.cshtml");

        [HttpGet]
        public async Task<IActionResult> Datatable()
        {
            try
            {
                var lstDepartamentos = await _mediator.Send(new GetAllDepartamentosQuery());
                var dataTableVM = new DataTableAjaxViewModel() { data = lstDepartamentos };
                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var departamentoVM = new DepartamentoViewModel();
            ViewData["Title"] = "Cadastrar Novo Departamento";
            return View("~/Views/Departamentos/Cadastrar.cshtml", departamentoVM);
        }

        [HttpGet]
        public async Task<IActionResult> Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Editar Departamento";
            var departamento = await _mediator.Send(new GetDepartamentoByIdQuery { Id = id });
            var departamentoVM = new DepartamentoViewModel
            {
                ID = departamento.ID,
                Descricao = departamento.Descricao
            };
            return View("~/Views/Departamentos/Cadastrar.cshtml", departamentoVM);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(DepartamentoViewModel departamentoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    throw new ApplicationException(string.Join(" ", errors));
                }

                var command = new GravarDepartamentoCommand
                {
                    ID = departamentoVM.ID,
                    Descricao = departamentoVM.Descricao
                };

                var sucesso = await _mediator.Send(command);

                if (sucesso)
                    return Ok(new ResponseViewModel("Sucesso!", "Departamento gravado com sucesso!", AlertTypes.success, "Departamentos", nameof(Listar)));
                else
                    throw new ApplicationException("Falha ao gravar o Departamento.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Excluir([FromRoute] int id)
        {
            try
            {
                var sucesso = await _mediator.Send(new ExcluirDepartamentoCommand { Id = id });
                if (sucesso)
                    return Ok(new ResponseViewModel("Sucesso!", $"Departamento {id} excluído com sucesso!", AlertTypes.success));
                else
                    throw new ApplicationException($"Falha ao excluir o Departamento {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Relatorio()
        {
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "Reports", "rptDepartamentos.rdlc");

            LocalReport localReport = new LocalReport(path);
            var lstDepartamentos = await _mediator.Send(new GetAllDepartamentosQuery());
            localReport.AddDataSource("dsDepartamentos", lstDepartamentos);

            var result = localReport.Execute(RenderType.Pdf);
            return File(result.MainStream, "application/pdf", "RelatorioDepartamentos.pdf");
        }
    }
}