using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_API.Extensions;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands;
using WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries;

using WebApp_Desafio_Shared.ViewModels;
using WebApp_Desafio_Shared.ViewModels.Enums;

namespace WebApp_Desafio_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartamentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Listar")]
        [ProducesResponseType(typeof(IEnumerable<DepartamentoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var departamentos = await _mediator.Send(new GetAllDepartamentosQuery());

                var response = departamentos.Select(d => new DepartamentoResponse
                {
                    id = d.ID,
                    descricao = d.Descricao
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        [HttpGet]
        [Route("Obter")]
        [ProducesResponseType(typeof(DepartamentoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Obter([FromQuery] int id)
        {
            try
            {
                var departamento = await _mediator.Send(new GetDepartamentoByIdQuery { Id = id });
                var response = new DepartamentoResponse
                {
                    id = departamento.ID,
                    descricao = departamento.Descricao
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        [HttpPost]
        [Route("Gravar")]
        [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Gravar([FromBody] DepartamentoResponse departamento)
        {
            try
            {
                var command = new GravarDepartamentoCommand
                {
                    ID = departamento.id,
                    Descricao = departamento.descricao
                };

                var sucesso = await _mediator.Send(command);

                if (sucesso)
                    return Ok(new ResponseViewModel("Sucesso!", "Departamento gravado com sucesso!", AlertTypes.success));
                else
                    throw new ApplicationException("Falha ao gravar o Departamento.");
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        [HttpDelete]
        [Route("Excluir")]
        [ProducesResponseType(typeof(ResponseViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Excluir([FromQuery] int id)
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
                return this.ExceptionProcess(ex);
            }
        }
    }
}