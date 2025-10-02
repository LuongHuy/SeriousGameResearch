using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScreenScript : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }
    public void LevelMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    public void LevelSelectMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

    public void Level1()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(2);
    }

    public void Level2()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(3);
    }
    public void Level3()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(4);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
