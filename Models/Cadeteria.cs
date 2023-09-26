namespace EspCadeteria;

public class Cadeteria
{
    private string nombre;
    private string telefono;
    private int cantPedidos;
    private List<Cadete> lcadetes;
    private List<Pedido> lpedidos;
    private static Cadeteria cadSingleton;

    public Cadeteria(string nombre, string telefono)
    {
        this.Nombre = nombre;
        this.telefono = telefono;
    }


    public string Nombre { set => nombre = value; }
    public string Telefono { set => telefono = value; }
    public List<Cadete> LCadetes { get => lcadetes; set => lcadetes = value; }
    public List<Pedido> LPedidos { get => lpedidos; set => lpedidos = value; }

    public static Cadeteria GetCadeteria()
    {
        if (cadSingleton == null)
        {
            cadSingleton = AccesoADatosCadeteria.Obtener();
            cadSingleton.LCadetes = AccesoADatosCadetes.Obtener();
            cadSingleton.LPedidos = AccesoADatosPedidos.Obtener();

            cadSingleton.cantPedidos = cadSingleton.LPedidos.Count();
        }

        return cadSingleton;
    }

    public List<Cadete> GetCadetes()
    {
        return LCadetes;
    }

    public string MostrarNombreCadeteria()
    {
        return nombre;
    }

    public int NroCantPedidos()
    {
        return cantPedidos;
    }

    public List<Cadete> ListaCadetes()
    {
        return LCadetes;
    }

    public List<Pedido> ListaPedidos()
    {
        return LPedidos;
    }


    public Pedido AltaPedido(string nombre, string direccion, string telefono, string referencia, string obs)
    {
        cantPedidos++;

        var Cliente = new Cliente(nombre, direccion, telefono, referencia);
        var Pedido = new Pedido(cantPedidos, obs, Cliente, Estados.Pendiente);
    
        LPedidos.Add(Pedido);

        AccesoADatosPedidos.Guardar(LPedidos);

        return Pedido;
        
    }

    public bool AsignarCadeteAPedido(int NroPed, int IdCad)
    {
        var Ped = BuscarPedido(NroPed);
        var Cad = BuscarCadete(IdCad);

        if(Cad != null && Ped != null)
        {
            Ped.Cadete = Cad;
            AccesoADatosPedidos.Guardar(LPedidos);
            return true;
        }
        
        return false;
    }

    public Pedido BuscarPedido(int NroPed)
    {
        return LPedidos.FirstOrDefault(ped => ped.Nro == NroPed);
    }

    public Cadete BuscarCadete(int idCad)
    {
        return LCadetes.FirstOrDefault(cad => cad.Id == idCad);
    }

    public bool CambiarEst(int num, int estado)
    {
        var ped = BuscarPedido(num);

        if (ped != null && ped.Estado==0 && (estado==1 || estado==2))
        {
            ped.Estado = (Estados)estado;
            AccesoADatosPedidos.Guardar(LPedidos);
            return true;
        }

        return false;
    }
   
    public float JornalACobrar(Cadete cad)
    {
        return PedidosEntregados(cad.Id)*500;
    }

    public int PedidosEntregados(int Id)
    {
       return LPedidos.Count(ped => ped.Cadete.Id == Id && ped.Estado == Estados.Entregado);
    }

    public Informe Informe()
    {
        int cantTotal = 0;
        float montoTotal = 0;
        var info = new Informe();

        foreach(var cad in ListaCadetes())
        {
            var infocadete = new InfCadete();
            infocadete.Cadete = cad.Nombre;
            infocadete.CantEnvios = PedidosEntregados(cad.Id);
            infocadete.MontoGanado = JornalACobrar(cad);
            
            info.Infocadete.Add(infocadete);

            cantTotal += PedidosEntregados(cad.Id);
            montoTotal += JornalACobrar(cad);
        }

        info.TotalEnvios = cantTotal;
        info.MontoTotal = montoTotal;
        info.Promedio = cantTotal/ListaCadetes().Count();
    
        return info;
    }
}

public class Cadete
{
    private int id;
    private string nombre;
    private string direccion;
    private string telefono;


    public Cadete (int Id, string Nombre, string Direccion, string Telefono)
    {
        id = Id;
        nombre = Nombre;
        direccion = Direccion;
        telefono = Telefono;
    }

    public int Id { get => id; }
    public string Nombre { get => nombre; }
    public string Direccion { get => direccion; }
    public string Telefono { get => telefono; }

    
}

public class Pedido
{
    private int nro;
    private string obs;
    private Cliente cliente;
    private Cadete cadete;
    private Estados estado;

    public Pedido(int nro, string obs, Cliente cliente, Estados estado)
    {
        this.nro = nro;
        this.obs = obs;
        this.cliente = cliente;
        this.estado = estado;
    }

    public int Nro { get => nro; set => nro = value; }
    public string Obs { get => obs; set => obs = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public Estados Estado { get => estado; set => estado = value; }
    public Cadete Cadete { get => cadete; set => cadete = value; }


}

public class Cliente
{
    private string nombre;
    private string direccion;
    private string telefono;
    private string datosreferenciadireccion;

    public Cliente(string nombre, string direccion, string telefono, string datosreferenciadireccion)
    {
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
        this.datosreferenciadireccion = datosreferenciadireccion;
    }

    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }
    public string Telefono { get => telefono; set => telefono = value; }
    public string DatosReferenciaDireccion { get => datosreferenciadireccion; set => datosreferenciadireccion = value; }
}
