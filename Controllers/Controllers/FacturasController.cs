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
        public IActionResult Get()
        {
            Rsp<List<Factura>> rsp = new Rsp<List<Factura>>();

            try
            {
                rsp = this._factura.getAll();
            }
            catch (Exception ex)
            {
                rsp.data = null;
                rsp.mensajes.Add(ex.Message);
                rsp.tipo = Tipo.Fail;
            }

            return Ok(rsp);
        }

        [HttpPost]
        public IActionResult Load()
        {
            Rsp<List<Factura>> rsp = new Rsp<List<Factura>>();
            try
            {
                this._factura.nuevasFacturas();
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
