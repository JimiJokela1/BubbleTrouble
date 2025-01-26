using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !FindAnyObjectByType<Enemy>())
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
