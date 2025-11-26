using Unity.VisualScripting;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private Animator fruitAnimator;
    public AudioClip collectedSoud;
    private AudioSource fruitAudioSource;

    private void Awake()
    {
        //Recuperaci�n de componente ya creado en el GameObject Padre
        fruitAnimator = GetComponent<Animator>();
        //Creaci�n de componente por codigo
        fruitAudioSource = this.transform.AddComponent<AudioSource>();
        //Llamado al ScriptableObject 
        playerInfo = Resources.Load<PlayerInfo>("PlayerInfo");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Cambiar animaci�n a collected
            playerInfo.score += 1;
            fruitAnimator.SetBool("collected", true);
            fruitAudioSource.PlayOneShot(collectedSoud, 1f);
            Destroy(this.gameObject, 0.5f);
        }
    }
    private void OnDestroy()
    {
        fruitAudioSource.PlayOneShot(collectedSoud, 1f);
    }
}