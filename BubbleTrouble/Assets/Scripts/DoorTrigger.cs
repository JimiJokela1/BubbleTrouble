using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string nextScene;
    bool doorOpen = false;

    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 1f)
        {
            return;
        }

        if (!doorOpen && FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length == 0)
        {
            doorOpen = true;
            StartCoroutine(DoorOpen());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !FindAnyObjectByType<Enemy>())
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator DoorOpen()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, -1f);
            yield return null;

            if (transform.rotation.eulerAngles.y <= 90)
            {
                break;
            }
        }
        
    }
}
