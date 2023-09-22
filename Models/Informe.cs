namespace EspCadeteria;

public class Informe
{
    private List<InfCadete> infocadete;
    private int totalEnvios;
    private float montoTotal;
    private int promedio;

    public Informe()
    {
        this.infocadete = new List<InfCadete>();
    }

    public List<InfCadete> Infocadete { get => infocadete; set => infocadete = value; }
    public int TotalEnvios { get => totalEnvios; set => totalEnvios = value; }
    public float MontoTotal { get => montoTotal; set => montoTotal = value; }
    public int Promedio { get => promedio; set => promedio = value; }
}

public class InfCadete
{
    private string cadete;
    private int cantEnvios;
    private float montoGanado;

    public string Cadete { get => cadete; set => cadete = value; }
    public int CantEnvios { get => cantEnvios; set => cantEnvios = value; }
    public float MontoGanado { get => montoGanado; set => montoGanado = value; }
}