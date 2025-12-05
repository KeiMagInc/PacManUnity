using UnityEngine;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public TextMeshProUGUI textoTiempoFinal;
    public TextMeshProUGUI textoPuntajeFinal; // Opcional si quieres mostrar el score también

    void Start()
    {
        // Recuperamos el tiempo total guardado en el ScriptableObject
        float tiempo = playerInfo.tiempoTotal;

        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);

        // Mostramos el texto final
        textoTiempoFinal.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        
        // Opcional: Mostrar puntaje
        if(textoPuntajeFinal != null)
        {
            textoPuntajeFinal.text = playerInfo.score.ToString();
        }
    }
}