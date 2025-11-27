using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Tiempos")]
    public float tiempoEspera = 0.5f;
    public float tiempoCaida = 2f;
    public float tiempoReset = 2f;

    [Header("Configuración")]
    public float intensidadVibracion = 0.02f; 
    public float velocidadReaparicion = 2f; // Qué tan rápido hace el Fade In (mayor número = más rápido)

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer rend;
    private Vector3 posicionInicial;
    private bool estaCayendo = false;
    private Color colorOriginal; // Para guardar el color normal de la plataforma

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rend = GetComponent<SpriteRenderer>();
        posicionInicial = transform.position;
        
        // Guardamos el color original (por si la plataforma tiene algún tinte, no perderlo)
        colorOriginal = rend.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !estaCayendo)
        {
            // Comprobamos si el jugador está arriba
            if (collision.transform.position.y > transform.position.y + 0.1f)
            {
                StartCoroutine(CaidaYReset());
            }
        }
    }

    IEnumerator CaidaYReset()
    {
        estaCayendo = true;

        // --- FASE 1: VIBRACIÓN ---
        float timer = 0;
        while (timer < tiempoEspera)
        {
            transform.position = new Vector3(
                posicionInicial.x + Random.Range(-intensidadVibracion, intensidadVibracion), 
                posicionInicial.y, 
                posicionInicial.z
            );
            timer += Time.deltaTime;
            yield return null;
        }

        // --- FASE 2: CAÍDA ---
        transform.position = posicionInicial; 
        rb.bodyType = RigidbodyType2D.Dynamic; 
        rb.gravityScale = 2f; 

        yield return new WaitForSeconds(tiempoCaida);

        // --- FASE 3: "DESAPARECER" ---
        rb.linearVelocity = Vector2.zero; 
        rb.bodyType = RigidbodyType2D.Kinematic; 
        
        rend.enabled = false; 
        col.enabled = false;  

        // --- FASE 4: ESPERAR PARA RESETEAR ---
        yield return new WaitForSeconds(tiempoReset);

        // --- FASE 5: REAPARICIÓN SUAVE (FADE IN) ---
        transform.position = posicionInicial;
        
        // 1. Ponemos la imagen totalmente transparente
        Color colorTransparente = colorOriginal;
        colorTransparente.a = 0f; // Alpha 0 = Invisible
        rend.color = colorTransparente;
        
        // 2. Activamos la imagen (pero no se ve porque es transparente)
        rend.enabled = true;

        // 3. Ciclo para subir la opacidad poco a poco
        float fadeTimer = 0f;
        while (fadeTimer < 1f)
        {
            fadeTimer += Time.deltaTime * velocidadReaparicion;
            
            // Lerp nos ayuda a ir de 0 a 1 suavemente
            colorTransparente.a = Mathf.Lerp(0f, 1f, fadeTimer);
            rend.color = colorTransparente;
            
            yield return null; // Esperar al siguiente frame
        }

        // 4. Nos aseguramos de que quede totalmente sólida al final
        rend.color = colorOriginal;
        
        // 5. SOLO AHORA activamos la colisión (para que no te pares en un fantasma)
        col.enabled = true;   
        estaCayendo = false;
    }
}