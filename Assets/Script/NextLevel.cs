using System.Collections; // <--- NECESARIO para usar Corrutinas (IEnumerator)
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevel : MonoBehaviour
{
    public int sceneIndex;
    public GameObject fruits;
    
    [Header("Configuración de Transición")]
    public Animator transitionAnim; // Arrastra aquí el objeto "Crossfade" (la imagen negra)
    public float transitionTime = 1f; // Tiempo de espera (debe durar lo mismo que la animación)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca el jugador Y no quedan frutas
        if (collision.CompareTag("Player") && fruits.transform.childCount == 0)
        {
            // En vez de cargar de golpe, iniciamos la rutina
            StartCoroutine(LoadLevelWithTransition());
        }
    }

    // Si usas este método para botones, también lo actualizamos
    public void nextScene()
    {
        StartCoroutine(LoadLevelWithTransition());
    }

    // Esta es la rutina que hace la magia
    IEnumerator LoadLevelWithTransition()
    {
        // 1. Activar la animación de fundido a negro
        // Asegúrate de que tu parámetro en el Animator se llame "Start" (o cambia el nombre aquí)
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("Start");
        }

        // 2. Esperar el tiempo que dura la animación
        yield return new WaitForSeconds(transitionTime);

        // 3. Cargar la escena
        SceneManager.LoadScene(sceneIndex);
    }
}