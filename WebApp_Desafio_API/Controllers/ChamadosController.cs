using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_API.Extensions;
using WebApp_Desafio_Shared.ViewModels;
using WebApp_Desafio_Shared.ViewModels.Enums;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Commands;
using WebApp_Desafio_BackEnd.CQRS.Chamados.Queries;

namespace WebApp_Desafio_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChamadosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Listar")]
        [ProducesResponseType(typeof(IEnumerable<ChamadoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var chamados = await _mediator.Send(new GetAllChamadosQuery());
                var response = chamados.Select(c => new ChamadoResponse
                {
                    id = c.ID,
                    assunto = c.Assunto,
                    solicitante = c.Solicitante,
                    idDepartamento = c.IdDepartamento,
                    departamento = c.Departamento,
                    dataAbertura = c.DataAbertura
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
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Obter([FromQuery] int id)
        {
            try
            {
                var chamado = await _mediator.Send(new GetChamadoByIdQuery { Id = id });
                var response = new ChamadoResponse
                {
                    id = chamado.ID,
                    assunto = chamado.Assunto,
                    solicitante = chamado.Solicitante,
                    idDepartamento = chamado.IdDepartamento,
                    departamento = chamado.Departamento,
                    dataAbertura = chamado.DataAbertura
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
        public async Task<IActionResult> Gravar([FromBody] ChamadoRequest chamadoRequest)
        {
            try
            {
                var command = new GravarChamadoCommand
                {
                    ID = chamadoRequest.id,
                    Assunto = chamadoRequest.assunto,
                    IdSolicitante = chamadoRequest.idSolicitante,
                    IdDepartamento = chamadoRequest.idDepartamento,
                    DataAbertura = chamadoRequest.dataAbertura
                };

                var sucesso = await _mediator.Send(command);

                if (sucesso)
                    return Ok(new ResponseViewModel("Sucesso!", "Chamado gravado com sucesso!", AlertTypes.success));
                else
                    throw new ApplicationException("Falha ao gravar o Chamado.");
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
                var sucesso = await _mediator.Send(new ExcluirChamadoCommand { Id = id });

                if (sucesso)
                    return Ok(new ResponseViewModel("Sucesso!", $"Chamado {id} excluído com sucesso!", AlertTypes.success));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }

        [HttpGet]
        [Route("SearchSolicitantes")]
        [ProducesResponseType(typeof(IEnumerable<SolicitanteDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSolicitantes([FromQuery] string term)
        {
            try
            {
                var query = new SearchSolicitantesQuery { TermoBusca = term ?? "" };
                var solicitantes = await _mediator.Send(query);
                return Ok(solicitantes);
            }
            catch (Exception ex)
            {
                return this.ExceptionProcess(ex);
            }
        }
    }
}