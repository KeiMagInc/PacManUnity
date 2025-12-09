using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Configuración")]
    public float fuerzaSalto = 15f; // Qué tan alto te lanza

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Verificamos si es el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. Verificamos que el jugador caiga desde ARRIBA.
            // (collision.relativeVelocity.y <= 0 significa que el jugador está cayendo)
            if (collision.relativeVelocity.y <= 0f) 
            {
                // 3. Activar animación
                animator.SetTrigger("Jump");

                // 4. Impulsar al jugador
                Rigidbody2D rbJugador = collision.gameObject.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                {
                    // En Unity 6 usamos 'linearVelocity', en versiones viejas 'velocity'
                    // Mantenemos la velocidad X actual, pero cambiamos la Y bruscamente
                    rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaSalto);
                }
            }
        }
    }
}