using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenu : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscenaJuego = "Nivel1"; // Escribe aquí el nombre exacto de tu escena de juego

    // Esta función se asignará a los botones
    public void SeleccionarPersonaje(int indicePersonaje)
    {
        // Guardamos el índice (0, 1, 2, 3...) en la memoria
        PlayerPrefs.SetInt("PersonajeSeleccionado", indicePersonaje);
        PlayerPrefs.Save(); 

        // Cambiamos de escena
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}