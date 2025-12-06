using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("Configuración")]
    public PlayerInfo playerInfo;
    public TextMeshProUGUI textoTiempo;
    
    [Header("Opciones de Nivel")]
    public bool esNivel1 = false;

    private float tiempoActual;
    private bool timerCorriendo = true; // NUEVO: Interruptor para el reloj

    void Start()
    {
        timerCorriendo = true; // Nos aseguramos de que arranque encendido

        if (esNivel1)
        {
            tiempoActual = 0f;
            playerInfo.tiempoTotal = 0f;
            playerInfo.score = 0; 
        }
        else
        {
            tiempoActual = playerInfo.tiempoTotal;
        }
    }

    void Update()
    {
        // Solo sumamos tiempo si el interruptor está encendido
        if (timerCorriendo) 
        {
            tiempoActual += Time.deltaTime;
            playerInfo.tiempoTotal = tiempoActual;
            ActualizarUI();
        }
    }

    void ActualizarUI()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);
        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // Función pública para detener el reloj desde otro script
    public void DetenerReloj()
    {
        timerCorriendo = false;

        // --- NUEVO: Guardamos el tiempo final para la tabla de puntuación ---
        // "LastRunTime" es la clave que buscará la pantalla final
        PlayerPrefs.SetFloat("LastRunTime", tiempoActual);
        PlayerPrefs.Save();
        // ------------------------------------------------------------------
    }
}