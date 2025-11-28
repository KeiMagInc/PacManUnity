using UnityEngine;
using System.Collections;

public class SpikeHead : MonoBehaviour
{
    [Header("Configuración de Ataque (Mario Style)")]
    public float rangoDeteccion = 10f;
    public float anchoDeteccion = 1.5f; // NUEVO: Qué tan "gordo" es el rayo
    public float velocidadSubida = 2f;
    public float gravedadCaida = 5f;
    public float tiempoTemblor = 0.5f;
    public float intensidadTemblor = 0.1f;
    public float tiempoEsperaPiso = 1f;
    public LayerMask capaJugador;

    // Variables internas
    private Vector3 posicionInicial;
    private Rigidbody2D rb;
    private bool atacando = false;

    // --- TUS VARIABLES ORIGINALES ---
    public float tiempoAnimacion = 0.5f; 
    private bool estaRespawning = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (atacando || estaRespawning) return;

        // --- CAMBIO AQUI: Usamos BoxCast en lugar de Raycast ---
        // Parámetros: Origen, Tamaño de la caja (Ancho, Alto), Ángulo, Dirección, Distancia, Capa
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position, 
            new Vector2(anchoDeteccion, 0.5f), // Tamaño de la caja sensora
            0f, 
            Vector2.down, 
            rangoDeteccion, 
            capaJugador
        );

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            StartCoroutine(RutinaAtaque());
        }
    }

    // --- RUTINA DE MOVIMIENTO ---
    IEnumerator RutinaAtaque()
    {
        atacando = true;

        // 1. TEMBLAR
        float timer = 0f;
        while (timer < tiempoTemblor)
        {
            transform.position = posicionInicial + new Vector3(Random.Range(-intensidadTemblor, intensidadTemblor), 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = posicionInicial; 

        // 2. CAER
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravedadCaida;
        rb.linearVelocity = Vector2.zero; 

        yield return new WaitForFixedUpdate(); 
        
        while (rb.linearVelocity.y < -0.1f) 
        {
            yield return null;
        }

        // 3. ESPERAR EN EL SUELO
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        
        yield return new WaitForSeconds(tiempoEsperaPiso);

        // 4. REGRESAR ARRIBA
        while (Vector3.Distance(transform.position, posicionInicial) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionInicial, velocidadSubida * Time.deltaTime);
            yield return null;
        }

        transform.position = posicionInicial;
        atacando = false;
    }

    // --- LOGICA DE MUERTE ---
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player") && !estaRespawning)
        {
            StartCoroutine(SecuenciaMuerte(other.gameObject));
        }
    }

    IEnumerator SecuenciaMuerte(GameObject player)
    {
        estaRespawning = true;

        Rigidbody2D rbPlayer = player.GetComponent<Rigidbody2D>();
        Animator animPlayer = player.GetComponent<Animator>();
        MonoBehaviour scriptMovimiento = player.GetComponent<PlayerController>(); 

        if (rbPlayer != null)
        {
            rbPlayer.linearVelocity = Vector2.zero; 
            rbPlayer.bodyType = RigidbodyType2D.Kinematic; 
        }

        if (scriptMovimiento != null) scriptMovimiento.enabled = false; 

        if (animPlayer != null) animPlayer.SetTrigger("hit"); 

        yield return new WaitForSeconds(tiempoAnimacion);

        GameObject respawnPoint = GameObject.FindWithTag("Respawn");
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.transform.position;
        }

        if (rbPlayer != null)
        {
            rbPlayer.bodyType = RigidbodyType2D.Dynamic; 
            rbPlayer.linearVelocity = Vector2.zero;
        }

        if (scriptMovimiento != null) scriptMovimiento.enabled = true; 
        
        estaRespawning = false;
    }

    // --- GIZMOS ACTUALIZADOS PARA VER LA CAJA ---
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Dibujamos una caja alámbrica al final del rango para visualizar el área
        Vector3 centroAbajo = transform.position + Vector3.down * rangoDeteccion;
        Gizmos.DrawWireCube(centroAbajo, new Vector3(anchoDeteccion, rangoDeteccion * 2, 0)); 
        // Nota: Dibujar la caja exacta del BoxCast es complejo en Gizmos, 
        // pero esto te da una idea del ancho y largo.
    }
}