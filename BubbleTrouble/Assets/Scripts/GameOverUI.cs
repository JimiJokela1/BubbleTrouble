using System.Linq;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject GameOverUi;
    public GameObject VictoryUi;

    private void Awake()
    {
        FindObjectsByType<GameOverUI>(sortMode: FindObjectsSortMode.None, findObjectsInactive: FindObjectsInactive.Include).ToList().ForEach((ui) =>
        {
            if (ui != this)
            {
                Destroy(ui.gameObject);
            }
        });
        DontDestroyOnLoad(gameObject);
        GameOverUi.SetActive(false);
        VictoryUi.SetActive(false);
    }

    private void Update()
    {
        if (GameOverUi.activeSelf || VictoryUi.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
    }

    public void ShowGameOver()
    {
        GameOverUi.SetActive(true);
    }

    public void ShowVictory()
    {
        VictoryUi.SetActive(true);
    }

    public void Restart()
    {
        GameOverUi.SetActive(false);
        VictoryUi.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
