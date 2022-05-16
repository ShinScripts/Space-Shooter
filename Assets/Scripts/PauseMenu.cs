using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool over = false;
    public GameObject pauseMenuUI;

    void Start()
    {
        over = false;
    }

    void Update()
    {
        //* Ser till att man bara kan pausa om over är falskt
        if (Input.GetKeyDown(KeyCode.Escape) && !over)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
