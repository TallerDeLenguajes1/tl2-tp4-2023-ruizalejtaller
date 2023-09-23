namespace EspCadeteria;
using System.Text.Json;

public static class AccesoADatosCadeteria
{
    public static Cadeteria Obtener()
    {
        return DataAccess.CargarCSVCadeteria();
    }
}

public class AccesoADatosCadetes
{
    public static List<Cadete> Obtener()
    {
        return DataAccess.CargarCSVCadetes();
    }
}

public class AccesoADatosPedidos
{
    public static List<Pedido> Obtener()
    {
        return DataAccess.CargarPedidos();
    }

    public static void Guardar (List<Pedido> LPed)
    {
        DataAccess.GuardarPedidos(LPed);
    }
}

public abstract class DataAccess
{
    public abstract Cadeteria CargarCadeteria(string file);
    public abstract List<Cadete> CargarCadetes(string file);

    public static Cadeteria CargarCSVCadeteria()
    {
        DataAccess DataCadeteria = new DataCSV();
        return CargarDatosCadeteria(DataCadeteria, "Cadeteria.csv");
    }

    public static List<Cadete> CargarCSVCadetes()
    {
        DataAccess DataCadeteria = new DataCSV();
        var listacad = CargarDatosCadetes(DataCadeteria, "Cadetes.csv");

        return listacad;
    }

    public static List<Pedido> CargarPedidos()
    {
        var LPed = new List<Pedido>();

        string file = "Pedidos.json";

            if(File.Exists(file))
            {
                using StreamReader archivo = new(file);

                string objson = archivo.ReadToEnd();
                LPed = JsonSerializer.Deserialize<List<Pedido>>(objson);

                archivo.Close();
            }

        return LPed;
    }

    public static void GuardarPedidos(List<Pedido> Ped)
    {
        string file = "Pedidos.json";
        string jsonString = JsonSerializer.Serialize(Ped);

        try
        {
            using StreamWriter archivo = new(file);
            archivo.WriteLine(jsonString);

            archivo.Close();

        }   catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
   
    }

    public static Cadeteria CargarDatosCadeteria(DataAccess DataCadeteria, string FileCadeteria)
    {
        Cadeteria Cadeteria = null;
        if(File.Exists(FileCadeteria))
        {
            Cadeteria = DataCadeteria.CargarCadeteria(FileCadeteria);
        } 

        return Cadeteria;
    }

    public static List<Cadete> CargarDatosCadetes(DataAccess DataCadeteria, string FileCadetes)
    {

        if(File.Exists(FileCadetes))
        {
            return DataCadeteria.CargarCadetes(FileCadetes);
        } 

        return null;
    }
}

public class DataCSV : DataAccess
{
    public override Cadeteria CargarCadeteria(string file)
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

    public override List<Cadete> CargarCadetes(string file)
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
    public override Cadeteria CargarCadeteria(string file)
    {
        Cadeteria cadeteria = null;

            using StreamReader archivo = new(file);

            string objson = archivo.ReadToEnd();
            cadeteria = JsonSerializer.Deserialize<Cadeteria>(objson);
            
            archivo.Close();

        return cadeteria;
    }

    public override List<Cadete> CargarCadetes(string file)
    {
        var LCadetes = new List<Cadete>();

            using StreamReader archivo = new(file);

            string objson = archivo.ReadToEnd();
            LCadetes = JsonSerializer.Deserialize<List<Cadete>>(objson);

            archivo.Close();

        return LCadetes;
    }


}