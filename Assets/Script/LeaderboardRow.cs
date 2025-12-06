using UnityEngine;
using TMPro;

public class LeaderboardRow : MonoBehaviour
{
    public TextMeshProUGUI textoRango;  // Para el "1."
    public TextMeshProUGUI textoNombre; // Para el nombre
    public TextMeshProUGUI textoTiempo; // Para "01:30"

    public void ConfigurarFila(int rango, string nombre, string tiempo)
    {
        textoRango.text = rango + ".";
        textoNombre.text = nombre;
        textoTiempo.text = tiempo;
    }
}