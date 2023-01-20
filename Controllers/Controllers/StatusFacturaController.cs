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
    public class StatusFacturaController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<StatusFacturaController> _logger;
        private StatusFacturaApp _statusFactura;

        public StatusFacturaController(ILogger<StatusFacturaController> logger)
        {
            _logger = logger;
            this._statusFactura = new StatusFacturaApp();
        }

        [HttpGet]
        public IActionResult Get()
        {
            Rsp<List<StatusFactura>> rsp = new Rsp<List<StatusFactura>>();

            try
            {
                rsp = this._statusFactura.getAll();
            }
            catch (Exception ex)
            {
                rsp.data = null;
                rsp.mensajes.Add(ex.Message);
                rsp.tipo = Tipo.Fail;
            }

            return Ok(rsp);
        }
    }
}
