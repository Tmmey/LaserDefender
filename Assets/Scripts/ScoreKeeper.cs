using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int currentScore = 0;
    //for singleton
    static ScoreKeeper instance;

    //singleton Get Instance
    public ScoreKeeper GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        ManageSingleton();
    }

    private void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ModifyScore(int amount)
    {
        currentScore += amount;
        Mathf.Clamp(currentScore, 0, int.MaxValue);
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public int GetLowestHighsscore()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        var highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(x => x.score).ToList();

        return highscores.highscoreEntryList[Mathf.Min(4, highscores.highscoreEntryList.Count - 1)].score;
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
