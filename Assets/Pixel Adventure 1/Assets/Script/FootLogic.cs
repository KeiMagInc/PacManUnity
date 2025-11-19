using UnityEngine;

public class FootLogic : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("terrain"))
        {
            GetComponentInParent<PlayerController>().exitJump();
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("terrain"))
        {
            isGrounded = false;
        }
    }
}