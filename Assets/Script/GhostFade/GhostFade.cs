using UnityEngine;

public class GhostFade : MonoBehaviour
{
    public float velocidadDesvanecer = 4f; // Velocidad a la que desaparece
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. Obtenemos el color actual
        Color colorActual = sr.color;
        
        // 2. Le restamos Alpha (transparencia) usando el tiempo
        colorActual.a -= velocidadDesvanecer * Time.deltaTime;
        
        // 3. Aplicamos el nuevo color
        sr.color = colorActual;

        // 4. Si la transparencia llega a 0, destruimos el objeto
        if (sr.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}