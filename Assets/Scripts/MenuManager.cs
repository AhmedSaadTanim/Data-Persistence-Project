using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject inputField;
    public string userName;
    public int userScore;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void LoadNameAndScore()
    {
        string dataPath = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            SaveData highScorer = JsonUtility.FromJson<SaveData>(json);
            userName = highScorer.name;
            userScore = highScorer.score;
        }
        else
        {
            userName = "";
            userScore = 0;
        }
    }

    public void SaveNameAndScore()
    {
        SaveData highScorer = new SaveData();
        highScorer.name  = userName;
        highScorer.score = userScore;

        string json = JsonUtility.ToJson(highScorer);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void playGame()
    {
        userName = inputField.GetComponent<TMP_Text>().text;
        SceneManager.LoadScene(1);
    }
}
