using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeLeaderboard : MonoBehaviour
{
    [Header("Referencias UI Tabla")]
    public Transform containerFila;
    public GameObject plantillaFila;

    [Header("Referencias UI Nuevo Récord")]
    public GameObject panelNuevoRecord; // El panel que creaste en el Paso 1
    public TMP_InputField inputNombre;  // El campo de texto para el nombre
    public Button botonGuardar;         // El botón para confirmar

    private string prefsKey = "Top5BestScores_V2"; // Cambié el nombre para no mezclar con la versión anterior

    // Clase para guardar Nombre Y Tiempo
    [System.Serializable]
    public class ScoreEntry
    {
        public string nombre;
        public float tiempo;
    }

    [System.Serializable]
    public class HighScoreList
    {
        public List<ScoreEntry> list = new List<ScoreEntry>();
    }

    public HighScoreList highScores = new HighScoreList();
    private float tiempoPendiente; // Para guardar el tiempo mientras escriben el nombre

    void Start()
    {
        CargarTiempos();

        // Configurar el botón de guardar
        botonGuardar.onClick.AddListener(GuardarNuevoRecord);

        // Verificar si venimos de jugar
        float tiempoReciente = PlayerPrefs.GetFloat("LastRunTime", 0f);

        if (tiempoReciente > 0.1f)
        {
            // Verificamos si entra en el Top 5
            if (EsNuevoRecord(tiempoReciente))
            {
                // SI es récord: Guardamos el tiempo en variable temporal y mostramos el input
                tiempoPendiente = tiempoReciente;
                panelNuevoRecord.SetActive(true); // Aparece la ventana para escribir nombre

                // Limpiamos el PlayerPrefs para que no salte de nuevo al recargar
                PlayerPrefs.SetFloat("LastRunTime", 0f);
                PlayerPrefs.Save();
            }
            else
            {
                // NO es récord: Mostramos la tabla normal
                panelNuevoRecord.SetActive(false);
                PlayerPrefs.SetFloat("LastRunTime", 0f);
                PlayerPrefs.Save();
                MostrarTabla();
            }
        }
        else
        {
            // Solo abrimos la escena sin jugar
            panelNuevoRecord.SetActive(false);
            MostrarTabla();
        }
    }

    bool EsNuevoRecord(float tiempo)
    {
        // Si hay menos de 5 puntajes, seguro entra
        if (highScores.list.Count < 5) return true;

        // Si ya hay 5, comparamos con el PEOR (el último de la lista)
        // Recordamos: Menor tiempo es mejor. 
        // Si mi tiempo es MENOR que el último de la lista (el más lento), entro.
        if (tiempo < highScores.list[highScores.list.Count - 1].tiempo)
        {
            return true;
        }

        return false;
    }

    // Esta función se llama al dar click en el botón "Guardar"
    void GuardarNuevoRecord()
    {
        string nombreJugador = inputNombre.text;

        if (string.IsNullOrEmpty(nombreJugador)) nombreJugador = "Anonimo";

        // Crear nueva entrada
        ScoreEntry nuevaEntrada = new ScoreEntry { nombre = nombreJugador, tiempo = tiempoPendiente };

        // Añadir y ordenar
        highScores.list.Add(nuevaEntrada);

        // Ordenar por tiempo (ascendente)
        highScores.list.Sort((x, y) => x.tiempo.CompareTo(y.tiempo));

        // Cortar si sobran
        if (highScores.list.Count > 5)
        {
            highScores.list.RemoveRange(5, highScores.list.Count - 5);
        }

        // Guardar en disco
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(prefsKey, json);
        PlayerPrefs.Save();

        // Ocultar panel y actualizar tabla
        panelNuevoRecord.SetActive(false);
        MostrarTabla();
    }

    void MostrarTabla()
    {
        // Limpiar tabla visual
        foreach (Transform child in containerFila)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < highScores.list.Count; i++)
        {
            GameObject fila = Instantiate(plantillaFila, containerFila);

            // Obtenemos los datos
            ScoreEntry entry = highScores.list[i];
            int minutos = Mathf.FloorToInt(entry.tiempo / 60);
            int segundos = Mathf.FloorToInt(entry.tiempo % 60);
            string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

            // --- NUEVO: Usamos el script de la fila para asignar datos a cada columna ---
            LeaderboardRow scriptFila = fila.GetComponent<LeaderboardRow>();
            if (scriptFila != null)
            {
                scriptFila.ConfigurarFila(i + 1, entry.nombre, tiempoFormateado);
            }
        }
    }

    void CargarTiempos()
    {
        if (PlayerPrefs.HasKey(prefsKey))
        {
            string json = PlayerPrefs.GetString(prefsKey);
            highScores = JsonUtility.FromJson<HighScoreList>(json);
        }
    }

    [ContextMenu("Borrar Datos")]
    public void BorrarDatos()
    {
        PlayerPrefs.DeleteKey(prefsKey);
    }
}