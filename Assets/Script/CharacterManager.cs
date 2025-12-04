using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // Aquí arrastraremos los archivos .controller desde la carpeta
    public RuntimeAnimatorController[] controladores;
    
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        // 1. Leemos qué personaje se eligió (si no hay ninguno, usa el 0 por defecto)
        int indiceElegido = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);

        // 2. Verificamos que el número sea válido para evitar errores
        if (indiceElegido < controladores.Length)
        {
            // 3. Cambiamos el controlador del Animator
            anim.runtimeAnimatorController = controladores[indiceElegido];
        }
        else
        {
            Debug.LogError("Error: El índice seleccionado no existe en la lista de controladores.");
        }
    }
}