using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level_0")
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            FindFirstObjectByType<PlayerMovement>().SetHealth(100);

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            FindFirstObjectByType<PlayerMovement>().SetHealth(30);
        }
    }
}
