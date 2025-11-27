using UnityEngine;
using UnityEngine.InputSystem; // Importante

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    
    // Tiempo que hay que mantener presionado para bajar (para evitar caídas accidentales)
    public float startWaitTime = 0.1f; 
    private float waitedTime;

    // Aquí arrastraremos la Acción "Move" desde el Inspector
    public InputActionReference moveInput; 

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitedTime = startWaitTime; // Iniciamos el contador
    }

    void Update()
    {
        // Protección por si olvidaste asignar el input en el inspector
        if (moveInput == null) return;

        // 1. Leemos el valor del Input (Vector2)
        // Esto nos da (X, Y). Nos interesa la Y.
        Vector2 input = moveInput.action.ReadValue<Vector2>();

        // 2. Verificamos si se está presionando ABAJO
        // -0.5f asegura que se esté presionando con intención (útil para joysticks)
        if (input.y < -0.5f) 
        {
            if (waitedTime <= 0)
            {
                // ¡Aquí ocurre la magia! Invertimos la plataforma
                effector.rotationalOffset = 180f;
                
                // Reiniciamos el contador
                waitedTime = startWaitTime;
            }
            else
            {
                // Restamos tiempo
                waitedTime -= Time.deltaTime;
            }
        }
        else
        {
            // 3. Si NO se presiona abajo, restablecemos todo
            waitedTime = startWaitTime;
            effector.rotationalOffset = 0f; // La plataforma vuelve a ser sólida por arriba
        }
    }
}