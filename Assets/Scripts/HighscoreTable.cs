using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    //private List<HighscoreEntry> highscoreEntryList;
    private Highscores highscores;
    private List<Transform> highscoreEntryTransformList;
    private ScoreKeeper scoreKeeper;

    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] Button PlayAgainButton;
    //private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        LoadHighscores();
    }

    private void Start()
    {
        if (scoreKeeper.GetCurrentScore() == 0)
        {
            HideControls();
            PlayAgainButton.gameObject.SetActive(false);
        }
    }

    void HideControls()
    {
        var hideableObjects = GameObject.FindGameObjectsWithTag("Hidable");

        foreach (var hideableObject in hideableObjects)
        {
            hideableObject.gameObject.SetActive(false);
        }
    }

    private void LoadHighscores()
    {
        //scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(100000, "AAASS");

        // var highscoreEntryList = new List<HighscoreEntry>()
        // {
        //     new HighscoreEntry{ name = "AAAAA", score = 10001},
        // }.OrderByDescending(x => x.score).ToList();

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(x => x.score).ToList();
        //highscoreEntryList = highscoreEntryList.OrderByDescending(x => x.score).ToList();

        highscoreEntryTransformList = new List<Transform>();
        // foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        // {
        //     CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        // }

        for (int i = 0; i < Mathf.Min(5, highscores.highscoreEntryList.Count); i++)
        {
            var highscoreEntry = highscores.highscoreEntryList[i];
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        // highscores = new Highscores { highscoreEntryList = highscoreEntryList };
        // string json = JsonUtility.ToJson(highscores);
        // PlayerPrefs.SetString("highscoreTable", json);
        // PlayerPrefs.Save();
        // Debug.Log(PlayerPrefs.GetString("highscoreTable"));
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry hightscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 45f;

        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "th";
                break;

            case 1:
                rankString = rank + "st";
                break;
            case 2:
                rankString = rank + "nd";
                break;
            case 3:
                rankString = rank + "rd";
                break;
        }

        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = hightscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = hightscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { name = name, score = score };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscores.highscoreEntryList.Add(highscoreEntry);
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(x => x.score).ToList();

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public void OkButtonPressed()
    {
        AddHighscoreEntry(scoreKeeper.GetCurrentScore(), Name.text);

        //Transform okButton = transform.Find("OkButton");
        // highscores = null;
        // highscoreEntryTransformList.Clear();
        // entryContainer = null;
        HideControls();

        var hets = GameObject.FindGameObjectsWithTag("HighscoreEntryTemplate");

        foreach (var het in hets)
        {
            het.SetActive(false);
        }

        LoadHighscores();
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
