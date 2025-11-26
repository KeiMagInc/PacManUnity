using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public Animator transitionAnim; // Aquí arrastras el objeto Crossfade
    public float transitionTime = 1f; // El tiempo que dura la animación (1 segundo)

    // Esta función la llamaremos desde el script de la Meta
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCoroutine());
    }

    IEnumerator LoadLevelCoroutine()
    {
        // 1. Ejecutar la animación de "Fade Out" (Oscurecer)
        transitionAnim.SetTrigger("Start");

        // 2. Esperar a que termine la animación
        yield return new WaitForSeconds(transitionTime);

        // 3. Cargar la siguiente escena
        // Opción A: Cargar por índice (Build Settings)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        // Opción B: Cargar por nombre (si prefieres)
        // SceneManager.LoadScene("NombreDeTuNivel");
    }
}