using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreTable : MonoBehaviour
{
    private Transform _entryContainer;
    private Transform _entryTemplate;
    private List<HighScoreEntry> _highScoreEntryList;
    private List<Transform> _highScoreEntryTransformList;

    void Awake()
    {

        _entryContainer = transform.Find("highScoreEntryContainer");
        _entryTemplate = _entryContainer.Find("highScoreEntryTemplate");

        _entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // Sort entry list by score
        for (int i = 0; i < highScores.HighScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.HighScoreEntryList.Count; j++)
            {
                if (highScores.HighScoreEntryList[j].Score > highScores.HighScoreEntryList[i].Score)
                {
                    // Swap
                    HighScoreEntry tmp = highScores.HighScoreEntryList[i];
                    highScores.HighScoreEntryList[i] = highScores.HighScoreEntryList[j];
                    highScores.HighScoreEntryList[j] = tmp;
                }
            }
        }

        _highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScores.HighScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, _entryContainer, _highScoreEntryTransformList);
        }

        // _highScoreEntryList = new List<HighScoreEntry>();

        // HighScores highScores1 = new HighScores { HighScoreEntryList = _highScoreEntryList };
        // string json = JsonUtility.ToJson(highScores1);
        // PlayerPrefs.SetString("HighScoreTable", json);
        // PlayerPrefs.Save();
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 65f;

        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;

            default: rankString = rank + "th"; break;
        }

        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highScoreEntry.Score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highScoreEntry.Name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        transformList.Add(entryTransform);
    }

    public static void AddHighScoreEntry(int score, string name)
    {
        // Create a highscore entry
        HighScoreEntry highScoreEntry = new HighScoreEntry { Score = score, Name = name };

        // Load saved HighScores
        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // Add new entry to HighScores
        highScores.HighScoreEntryList.Add(highScoreEntry);

        // Save updated HighScores
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighScoreTable", json);
        PlayerPrefs.Save();
    }

    private class HighScores
    {
        public List<HighScoreEntry> HighScoreEntryList;
    }

    /*
     * Represents a single high score entry
     * */

    [System.Serializable]
    private class HighScoreEntry
    {
        public int Score;
        public string Name;
    }
}
