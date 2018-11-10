using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelScreenController : MonoBehaviour {

    private RoundData[] allRoundData;

    private PlayerProgress playerProgress;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void LoadPlayerProgress(string subject)
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey(subject))
        {
            if (subject == "highestScoreOperasiHitung")
            {
                playerProgress.highestScoreOperasiHitung = PlayerPrefs.GetInt(subject);
            }

            if (subject == "highestScoreBangunDatarDanRuang")
            {
                playerProgress.highestScoreBangunDatarDanRuang = PlayerPrefs.GetInt(subject);
            }
        }
        print("loadHS:" + subject);
    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

    public void SubmitNewPlayerScore(int newScore, string subject)
    {
        if (subject == "highestScoreOperasiHitung")
        {
            if (newScore > playerProgress.highestScoreOperasiHitung)
            {
                playerProgress.highestScoreOperasiHitung = newScore;
                SavePlayerProgress(subject);
            }
        }

        if (subject == "highestScoreBangunDatarDanRuang")
        {
            if (newScore > playerProgress.highestScoreBangunDatarDanRuang)
            {
                playerProgress.highestScoreBangunDatarDanRuang = newScore;
                SavePlayerProgress(subject);
            }
        }

    }

    public int getHighestPlayerScore(string subject)
    {
        print("getHS:" + subject);
        if (subject == "highestScoreOperasiHitung")
        {
            return playerProgress.highestScoreOperasiHitung;
        }
        else if (subject == "highestScoreBangunDatarDanRuang")
        {
            return playerProgress.highestScoreBangunDatarDanRuang;
        }

        return playerProgress.highestScore;
    }

    private void SavePlayerProgress(string subject)
    {
        print("saveHS:" + playerProgress.highestScore);
        if (subject == "highestScoreOperasiHitung")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreOperasiHitung);
        }

        if (subject == "highestScoreBangunDatarDanRuang")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreBangunDatarDanRuang);
        }

    }

    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("opening");
    }

    public void LoadOperasiHitung()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "operasiHitung.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data!");
        }

        LoadPlayerProgress("highestScoreOperasiHitung");
        StartGame();
    }

    public void LoadPangkatDanAkarBilangan()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "pangkatDanAkarBilangan.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data!");
        }

        StartGame();
    }

    public void LoadFPBdanKPK()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "FPBDanKPK.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data!");
        }

        StartGame();
    }

    public void LoadBangunDatarDanRuang()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "bangunDatarDanRuang.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data!");
        }

        LoadPlayerProgress("highestScoreBangunDatarDanRuang");
        StartGame();
    }

    public void LoadDiagram()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Diagram.json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot Load Game Data!");
        }

        StartGame();
    }
}
