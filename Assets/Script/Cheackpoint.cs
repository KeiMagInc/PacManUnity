using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool activado = false; // Para que no suene ni se active 2 veces

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca el jugador y la bandera NO está activada aún
        if (collision.CompareTag("Player") && !activado)
        {
            activarCheckpoint();
        }
    }

    void activarCheckpoint()
    {
        activado = true;

        // 1. Activar animación (Palo -> Sale Bandera)
        if (anim != null)
        {
            anim.SetTrigger("activar");
        }

        // 2. ACTUALIZAR EL RESPAWN
        // Buscamos el objeto vacío que creaste al inicio del juego
        GameObject respawnPoint = GameObject.FindWithTag("Respawn");

        if (respawnPoint != null)
        {
            // MOVER ese objeto a la posición de ESTA bandera.
            // Así, el script DamageObject te traerá aquí cuando mueras.
            respawnPoint.transform.position = transform.position;
            
            // Opcional: Guardar partida aquí si usaras PlayerPrefs
            Debug.Log("¡Checkpoint Activado!");
        }
        else
        {
            Debug.LogWarning("No encontré el objeto con tag 'Respawn'");
        }
    }
}