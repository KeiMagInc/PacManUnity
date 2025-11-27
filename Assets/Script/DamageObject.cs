using System.Collections;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    // Tiempo que dura tu animación de golpe (ajústalo según tu clip)
    public float tiempoAnimacion = 0.5f; 
    
    // Variable para evitar que el jugador muera dos veces seguidas
    private bool estaRespawning = false; 

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Verificamos si es el jugador y si no está muriendo ya
        if (other.gameObject.CompareTag("Player") && !estaRespawning)
        {
            StartCoroutine(SecuenciaMuerte(other.gameObject));
        }
    }

    IEnumerator SecuenciaMuerte(GameObject player)
    {
        estaRespawning = true;

        // 1. Frenar al jugador y quitarle el control
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();
        
        // Buscamos tu script de movimiento para desactivarlo temporalmente
        // (Asegúrate que se llame 'PlayerController' o el nombre que le hayas puesto)
        MonoBehaviour scriptMovimiento = player.GetComponent<PlayerController>(); 

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Frenado total
            rb.bodyType = RigidbodyType2D.Kinematic; // Lo congelamos en el aire (opcional)
        }

        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = false; // El jugador ya no puede moverse con las teclas
        }

        // 2. Activar la animación de HIT
        if (anim != null)
        {
            anim.SetTrigger("hit"); // Asegúrate de haber creado este Trigger en el Animator
        }

        // 3. Esperar a que termine la animación
        yield return new WaitForSeconds(tiempoAnimacion);

        // 4. Buscar el punto de Respawn
        GameObject respawnPoint = GameObject.FindWithTag("Respawn");
        if (respawnPoint != null)
        {
            // Movemos al jugador
            player.transform.position = respawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("¡Falta el objeto con tag Respawn!");
        }

        // 5. Devolver el control al jugador
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Vuelve a tener física
            rb.linearVelocity = Vector2.zero;
        }

        if (scriptMovimiento != null)
        {
            scriptMovimiento.enabled = true; // Vuelve a poder moverse
        }
        
        // 6. Volver a animación normal (Idle/Run)
        // Normalmente el Animator lo hace solo si la transición Hit -> Exit tiene "Has Exit Time"
        // O puedes forzarlo:
        // anim.Play("Idle"); 

        estaRespawning = false;
    }
}