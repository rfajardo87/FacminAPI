using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Hlp;
using App;

namespace Controllers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FacturaController> _logger;
        private FacturaApp _factura;

        public FacturaController(ILogger<FacturaController> logger)
        {
            _logger = logger;
            this._factura = new FacturaApp();
        }

        [HttpGet]
        [Route("{year}/{mes}")]
        public IActionResult Get(int year, int mes)
        {
            Rsp<List<Factura>> rsp = new Rsp<List<Factura>>();

            try
            {
                rsp = this._factura.getAll(mes, year);
            }
            catch (Exception ex)
            {
                rsp.data = null;
                rsp.mensajes.Add(ex.Message);
                rsp.tipo = Tipo.Fail;
            }

            return Ok(rsp);
        }

        [HttpGet]
        [Route("pageInfo/{year}/{mes}/{perPage}")]
        public IActionResult pageInfo(int year, int mes, int perPage)
        {
            Rsp<Dictionary<string, int>> rsp = new Rsp<Dictionary<string, int>>();
            try
            {
                rsp = this._factura.pageInfo(mes, year, perPage);
            }
            catch (System.Exception ex)
            {
                rsp.mensajes.Add(ex.Message);
                rsp.tipo = Tipo.Fail;
            }
            return Ok(rsp);
        }

        [HttpPost]
        public async Task<IActionResult> Load()
        {
            Rsp<List<Factura>> rsp = new Rsp<List<Factura>>();
            try
            {
                await this._factura.nuevasFacturas();
                rsp.tipo = Tipo.Success;
                rsp.mensajes.Add($"Facturas cargadas correctamente");
            }
            catch (System.Exception ex)
            {
                rsp.data = null;
                rsp.tipo = Tipo.Fail;
                rsp.mensajes.Add(ex.Message);
            }
            return Ok(rsp);
        }
    }
}
