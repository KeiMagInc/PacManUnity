using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Variable estática para almacenar la única instancia de este objeto
    private static BackgroundMusic instancia;

    void Awake()
    {
        // Revisamos si ya existe una instancia de música
        if (instancia == null)
        {
            // Si no existe, esta es la instancia oficial
            instancia = this;
            
            // Esta función evita que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si YA existe una instancia (porque volviste al menú), 
            // destruimos este nuevo objeto para que no haya música doble.
            Destroy(gameObject);
        }
    }
}