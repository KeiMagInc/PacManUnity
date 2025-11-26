using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Riguid Body y fisica
    public Rigidbody2D rbPlayer;

    //Gestion de las inputs
    public InputActionAsset input;
    private InputActionMap inputMap;
    private InputAction move;
    private InputAction jump;

    //Parametros de movimiento
    public float speed = 1.0f;
    public float jumpSpeed = 2.0f;

    //Logica de los pies
    public FootLogic footLogic;

    //Componentes de animacion
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputMap = input.FindActionMap("Player");
        move = inputMap.FindAction("Move");
        jump = inputMap.FindAction("Jump");
    }
    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        inputMap.Enable();
        jump.started += jumpFunction;
        Debug.Log(jump);
    }
    private void OnDisable()
    {
        inputMap.Disable();
        jump.started -= jumpFunction;
    }
    // Update is called once per frame
    void Update()
    {
        walk();


    }
    void walk()
    {

        //Vector del vector del input Ssytem
        float linearXvelocity = move.ReadValue<Vector2>().x;
        //gestion animaciones de caminar
        if (linearXvelocity != 0) playerAnimator.SetBool("walk", true);
        else playerAnimator.SetBool("walk", false);
        if (linearXvelocity < 0) playerSprite.flipX = true;
        if (linearXvelocity > 0) playerSprite.flipX = false;

        //Este es el movimiento en eje X del rigidbody del Player
        rbPlayer.linearVelocity = new Vector2(linearXvelocity * speed, rbPlayer.linearVelocity.y);
    }
    void jumpFunction(InputAction.CallbackContext context)
    {

        if (footLogic.isGrounded)
        {

            rbPlayer.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);

            playerAnimator.SetBool("jump", true);
        }


    }
    public void exitJump()
    {
        playerAnimator.SetBool("jump", false);

    }
}