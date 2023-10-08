namespace EspCadeteria;
using System.Text.Json;

public class AccesoADatosCadeteria
{
    public Cadeteria Obtener()
    {
        string file = "Cadeteria.csv";
        string linea, nombre="", telefono="";

        if(File.Exists(file))
        {
            using StreamReader archivo = new(file);

            archivo.ReadLine();
            linea = archivo.ReadLine();

            string[] fila = linea.Split(';');
            nombre = fila[0];
            telefono = fila[1];

            archivo.Close();
        }

        var cadeteria = new Cadeteria(nombre, telefono);
        return cadeteria;
    }


}

public class AccesoADatosCadetes
{
    public List<Cadete> Obtener()
    {
        var cadetes = new List<Cadete>();
        string file = "Cadetes.json";
        if(File.Exists(file))
        {
            using StreamReader archivo = new(file);

            string objson = archivo.ReadToEnd();
            cadetes = JsonSerializer.Deserialize<List<Cadete>>(objson);
                
            archivo.Close();
        }

        return cadetes;
    }

    public void Guardar (List<Cadete> LCad)
    {
        string file = "Cadetes.json";
        string jsonString = JsonSerializer.Serialize(LCad);

        using StreamWriter archivo = new(file);
        archivo.WriteLine(jsonString);
        archivo.Close();
    }
}

public class AccesoADatosPedidos
{
    public List<Pedido> Obtener()
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

    public void Guardar (List<Pedido> LPed)
    {
        string file = "Pedidos.json";
        string jsonString = JsonSerializer.Serialize(LPed);

        using StreamWriter archivo = new(file);
        archivo.WriteLine(jsonString);
        archivo.Close();

    }
}

