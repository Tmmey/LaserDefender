using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadGame()
    {
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver()
    {
        var currentScore = scoreKeeper.GetCurrentScore();
        var lowestHighScore = scoreKeeper.GetLowestHighsscore();

        if (lowestHighScore < currentScore)
        {
            StartCoroutine(WaitOnLoad("HighScore", sceneLoadDelay));
        }
        else
        {
            StartCoroutine(WaitOnLoad("GameOver", sceneLoadDelay));
        }
    }

    public void LoadHighscores()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    IEnumerator WaitOnLoad(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
