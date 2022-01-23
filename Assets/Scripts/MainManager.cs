using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText,highScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points, h_Points;
    
    private bool m_GameOver = false;
    private string highestScorer, currentUser;
    
    // Start is called before the first frame update
    void Start()
    {
        //recieving current user from previous scene
        currentUser = MenuManager.Instance.userName;

        //loading highest scorer information
        MenuManager.Instance.LoadNameAndScore();
        h_Points = MenuManager.Instance.userScore;
        highestScorer = MenuManager.Instance.userName;

        if(highestScorer == "")
        {
            highScoreText.text = "Best Score: 0";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
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
        if(highestScorer != "")
        {
            if(m_Points <= h_Points)
            {
                highScoreText.text = "Best Score: " + highestScorer + " : " + h_Points;
            }
            else
                highScoreText.text = "Best Score: " + currentUser + " : " + m_Points;
        }
        else
            highScoreText.text = "Best Score: " + currentUser + " : " + m_Points;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        if(h_Points < m_Points)
        {
            MenuManager.Instance.SaveNameAndScore();
        }
        GameOverText.SetActive(true);
    }
}
