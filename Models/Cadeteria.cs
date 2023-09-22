namespace EspCadeteria;
using System.IO;
using System.Text.Json;

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
        this.lpedidos = new List<Pedido>();
    }


    public string Nombre { set => nombre = value; }
    public string Telefono { set => telefono = value; }
    public List<Cadete> LCadetes { get => lcadetes; set => lcadetes = value; }


    public static Cadeteria GetCadeteria()
    {
        if (cadSingleton == null)
        {
            cadSingleton = DataAccess.CargarCSV();
        }

        return cadSingleton;
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
        return lpedidos;
    }


    public Pedido AltaPedido(string nombre, string direccion, string telefono, string referencia, string obs)
    {
        cantPedidos++;

        var Cliente = new Cliente(nombre, direccion, telefono, referencia);
        var Pedido = new Pedido(cantPedidos, obs, Cliente, Estados.Pendiente);
    
        lpedidos.Add(Pedido);

        return Pedido;
        
    }

    public bool AsignarCadeteAPedido(int NroPed, int IdCad)
    {
        int Ix = BuscarPedido(NroPed);

        if(Ix != -1)
        {
            lpedidos[Ix].Cadete = BuscarCadete(IdCad);
            return true;
        } else
        {
            return false;
        }

    }

    public int BuscarPedido(int NroPed)
    {
        int index = -1;

        for(int i=0; i<lpedidos.Count(); i++)
        {
            if(lpedidos[i].Nro == NroPed)
            {
                index = i;
            }
        }

        return index;
    }

    public Cadete BuscarCadete(int idCad)
    {
        return LCadetes.Find(cad => cad.Id == idCad);
    }

    public bool CambiarEst(int num, int estado)
    {

        foreach(var ped in lpedidos)
        {
            if(ped.Nro == num)
            {
                ped.Estado = (Estados)estado;
                return true;
            }
        }

        return false;
    }
   
   public bool ReasignarPedidoACadete(int NroPed, int IdCad)
   {
        int IndexPed = BuscarPedido(NroPed);

        if(IndexPed != -1)
        {
            if(lpedidos[IndexPed].Estado == Estados.Pendiente)
            {
                AsignarCadeteAPedido(NroPed, IdCad);
                return true;
            } else return false;
        } else return false;
   }

    public float JornalACobrar(Cadete cad)
    {
        return PedidosEntregados(cad)*500;
    }

    public int PedidosEntregados(Cadete cad)
    {
        int cant = 0;
        foreach (var ped in lpedidos)
        {
            if (ped.Cadete == cad && ped.Estado == Estados.Entregado)
            {
                cant++;
            }
        }
        return cant;
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
            infocadete.CantEnvios = PedidosEntregados(cad);
            infocadete.MontoGanado = JornalACobrar(cad);
            
            info.Infocadete.Add(infocadete);

            cantTotal += PedidosEntregados(cad);
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

public abstract class DataAccess
{
    public abstract Cadeteria GetCadeteria(string file);
    public abstract List<Cadete> GetCadetes(string file);

    public static Cadeteria CargarCSV()
    {
        DataAccess DataCadeteria = new DataCSV();
        return CargarDatos(DataCadeteria, "Cadeteria.csv", "Cadetes.csv");
    }
    public static Cadeteria CargarJson()
    {
        DataAccess DataCadeteria = new DataJson();
        return CargarDatos(DataCadeteria, "Cadeteria.json", "Cadetes.json");
    }

    public static Cadeteria CargarDatos(DataAccess DataCadeteria, string FileCadeteria, string FileCadetes)
    {
        Cadeteria Cadeteria = null;
        if(File.Exists(FileCadeteria) && File.Exists(FileCadetes))
        {
            Cadeteria = DataCadeteria.GetCadeteria(FileCadeteria);
            Cadeteria.LCadetes = DataCadeteria.GetCadetes(FileCadetes);
        } 

        return Cadeteria;
    }
}

public class DataCSV : DataAccess
{
    public override Cadeteria GetCadeteria(string file)
    {
        string linea, nombre="", telefono="";

            using StreamReader archivo = new(file);

            archivo.ReadLine();
            linea = archivo.ReadLine();

            string[] fila = linea.Split(';');
            nombre = fila[0];
            telefono = fila[1];

            archivo.Close();


        var cadeteria = new Cadeteria(nombre, telefono);

        return cadeteria;
    }

    public override List<Cadete> GetCadetes(string file)
    {
        string linea;
        var ListaCadetes = new List<Cadete>();

            using StreamReader archivo = new(file);

            archivo.ReadLine();

            while ((linea = archivo.ReadLine()) != null)
            {
                string[] fila = linea.Split(';');
                int id = Convert.ToInt32(fila[0]);
                string nombre = fila[1];
                string direccion = fila[2];
                string telefono = fila[3];

                var cadete = new Cadete(id, nombre, direccion, telefono);

                ListaCadetes.Add(cadete);

            }
            archivo.Close();

        return ListaCadetes;
    }
}

public class DataJson : DataAccess
{
    public override Cadeteria GetCadeteria(string file)
    {
        Cadeteria cadeteria = null;

            using StreamReader archivo = new(file);

            string objson = archivo.ReadToEnd();
            cadeteria = JsonSerializer.Deserialize<Cadeteria>(objson);
            
            archivo.Close();

        return cadeteria;
    }

    public override List<Cadete> GetCadetes(string file)
    {
        var LCadetes = new List<Cadete>();

            using StreamReader archivo = new(file);

            string objson = archivo.ReadToEnd();
            LCadetes = JsonSerializer.Deserialize<List<Cadete>>(objson);

            archivo.Close();

        return LCadetes;
    }


}