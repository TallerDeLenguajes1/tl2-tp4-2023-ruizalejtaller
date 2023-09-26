using Microsoft.AspNetCore.Mvc;

namespace EspCadeteria.Controllers;

[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private Cadeteria cad;

    private readonly ILogger<CadeteriaController> _logger;

    public CadeteriaController(ILogger<CadeteriaController> logger)
    {
        _logger = logger;
        cad = Cadeteria.GetCadeteria();
    }

    [HttpGet]
    [Route("GetNombreCad")]
    public ActionResult<string> GetNombreCadeteria()
    {
        return Ok(cad.MostrarNombreCadeteria());
    }

    [HttpGet("GetPedidos")]
    public ActionResult<IEnumerable<Pedido>> GetPedidos()
    {
        return Ok(cad.ListaPedidos());
    }

    [HttpGet("GetCadetes")]
    public ActionResult<IEnumerable<Cadete>> GetCadetes()
    {
        return Ok(cad.ListaCadetes());
    }

    [HttpGet("GetInforme")]
    public ActionResult<IEnumerable<Informe>> GetInforme()
    {
        return Ok(cad.Informe());
    }

    [HttpPost("AgregarPedido")]
    public ActionResult<IEnumerable<Pedido>> AgregarPedido(string obs, string nombreCliente, string dirCliente, string telCliente, string refDireccion)
    {
        var ped = cad.AltaPedido(nombreCliente, dirCliente, telCliente, refDireccion, obs);

        return Ok(ped);
    }

    [HttpPut("AsignarPedido")]
    public ActionResult<string> AsignarPedido(int NroPedido, int idCadete)
    {
        
        if(cad.AsignarCadeteAPedido(NroPedido, idCadete))
        {
            return Ok("Pedido " + NroPedido + " asignado a: " + cad.BuscarCadete(idCadete).Nombre);
        } else
        {
            return NotFound("Error en número de Pedido o Cadete");
        }
    }

    [HttpPut("CambiarEstadoPedido")]
    public ActionResult<string> CambiarEstadoPedido(int NroPedido, int NuevoEstado)
    {
        if(cad.CambiarEst(NroPedido, NuevoEstado))
        {
            return Ok("Pedido " + NroPedido + " cambio su estado a: " + (Estados)NuevoEstado);
        } else
        {
            return NotFound("Error en número de Pedido o Estado (1) Cancelado (2) Entregado");
        }
    }

    [HttpPut("CambiarCadetePedido")]
    public ActionResult<string> CambiarCadetePedido(int NroPedido, int idNuevoCadete)
    {
        
        if(cad.AsignarCadeteAPedido(NroPedido, idNuevoCadete))
        {
            return Ok("Pedido " + NroPedido + " reasignado a: " + cad.BuscarCadete(idNuevoCadete).Nombre);
        } else
        {
            return NotFound("Error en número de Pedido o Cadete");
        }
    }
}
