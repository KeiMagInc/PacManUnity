using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rbPlayer;
    public InputActionAsset input;
    private InputActionMap inputMap;
    private InputAction move;
    public float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputMap = input.FindActionMap("Player");
        move = inputMap.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        walk();
    }
    void walk()
    {
        //Este es el movimiento en eje X del rigidbody del Player
        float linearXvelocity = move.ReadValue<Vector2>().x;
        rbPlayer.linearVelocity = new Vector2(linearXvelocity * speed, rbPlayer.linearVelocity.y);
    }
}