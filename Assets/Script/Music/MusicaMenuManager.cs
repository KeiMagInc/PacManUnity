using UnityEngine;
using UnityEngine.SceneManagement; // IMPORTANTE: Necesario para detectar escenas

public class MusicaMenuManager : MonoBehaviour
{
    private static MusicaMenuManager instance;

    void Awake()
    {
        // Lógica Singleton estándar (la que ya conoces)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe uno sonando, destruimos el nuevo para no tener duplicados
            Destroy(gameObject);
        }
    }

    // Estas dos funciones nos permiten "escuchar" cuando Unity cambia de escena
    void OnEnable()
    {
        SceneManager.sceneLoaded += VerificarEscena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= VerificarEscena;
    }

    // Esta función se ejecuta automáticamente cada vez que carga una escena nueva
    void VerificarEscena(Scene scene, LoadSceneMode mode)
    {
        // LISTA BLANCA: ¿En qué escenas DEBE sonar esta música?
        // Escribe los nombres EXACTOS como aparecen en tu carpeta de Assets
        if (scene.name == "Menu" || scene.name == "Personajes" || scene.name == "Final")
        {
            // Si es una de estas, no hacemos nada. La música sigue.
            return;
        }
        else
        {
            // Si es cualquier otra escena (ej: Nivel1, Nivel2), destruimos la música del menú.
            
            // IMPORTANTE: Limpiamos la referencia estática para que si volvemos al menú,
            // se pueda crear uno nuevo.
            if (instance == this) 
            {
                instance = null; 
            }
            
            Destroy(gameObject);
        }
    }
}