using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField] Brick _brickPrefab;
    [SerializeField] int _lineCount = 6;
    [SerializeField] Rigidbody Ball;

    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] GameObject _gameOverText;
    [SerializeField] TextMeshProUGUI _highScoreText;

    private bool m_Started = false;
    private int m_Points;
    private int _highScore;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < _lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(_brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        _gameOverText.SetActive(false);

        _highScore = PlayerPrefs.GetInt("High Score");
        SaveAndGetData.Instance.ShowData();
        _highScoreText.SetText("Current High Score : " + PlayerPrefs.GetString("Player Name") + " : " + PlayerPrefs.GetInt("High Score"));
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (_highScore < m_Points)
        {
            _highScore = m_Points;
            SetHighScore();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        _scoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        _gameOverText.SetActive(true);
    }

    void SetHighScore()
    {
        PlayerPrefs.SetInt("High Score", _highScore);
        PlayerPrefs.SetString("Player Name", SaveAndGetData.Instance.PreviewName);
    }
}
