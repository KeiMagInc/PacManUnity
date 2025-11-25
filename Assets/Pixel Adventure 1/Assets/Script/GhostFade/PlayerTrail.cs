using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public GameObject ghostPrefab; // Aquí arrastraremos el prefab que creamos antes
    public float tiempoEntreFantasmas = 0.1f; // Cada cuánto sale una copia
    
    private float tiempoRestante;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tiempoRestante = tiempoEntreFantasmas;
    }

    void Update()
    {
        // Solo generamos estela si el jugador se está moviendo
        // Mathf.Abs convierte el número a positivo (por si te mueves a la izquierda)
        // Unity 6 usa linearVelocity, versiones viejas usan velocity
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (tiempoRestante <= 0)
            {
                GenerarFantasma();
                tiempoRestante = tiempoEntreFantasmas;
            }
            else
            {
                tiempoRestante -= Time.deltaTime;
            }
        }
    }

    void GenerarFantasma()
    {
        // 1. Crear el fantasma en la posición del jugador
        GameObject fantasma = Instantiate(ghostPrefab, transform.position, transform.rotation);

        // 2. Copiar la imagen exacta que tiene el jugador AHORA (frame de la animación)
        SpriteRenderer srFantasma = fantasma.GetComponent<SpriteRenderer>();
        srFantasma.sprite = sr.sprite;
        
        // 3. Copiar la dirección (si usas FlipX o escala negativa)
        srFantasma.flipX = sr.flipX;
        fantasma.transform.localScale = transform.localScale;
        
        // 4. (Opcional) Ponerle un color plano o más suave al inicio
        srFantasma.color = new Color(1f, 1f, 1f, 0.5f); // 50% transparente al nacer
    }
}