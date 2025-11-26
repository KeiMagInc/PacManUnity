using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 1. Buscamos el objeto que tenga el tag "Respawn"
            GameObject respawnPoint = GameObject.FindWithTag("Respawn");

            if (respawnPoint != null)
            {
                // 2. Movemos al jugador a la posición de ese objeto
                other.transform.position = respawnPoint.transform.position;

                // 3. (IMPORTANTE) Reseteamos la física
                // Si el jugador iba corriendo rápido y choca, al reaparecer seguiría con esa velocidad.
                // Hay que frenarlo a cero.
                Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // En Unity 6 se recomienda usar 'linearVelocity' en lugar de 'velocity'
                    rb.linearVelocity = Vector2.zero; 
                }
            }
            else
            {
                Debug.LogWarning("¡No encontré el objeto con el tag Respawn!");
            }
        }
    }
}