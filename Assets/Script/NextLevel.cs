using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevel : MonoBehaviour
{

    public int sceneIndex;
    public GameObject fruits;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && fruits.transform.childCount == 0)
        {

            SceneManager.LoadScene(sceneIndex);
        }
    }
    public void nextScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}