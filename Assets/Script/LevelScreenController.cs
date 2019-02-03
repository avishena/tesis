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

        if (PlayerPrefs.HasKey(subject))
        {
            if (subject == "highestScoreOperasiHitung")
            {
                playerProgress.highestScoreOperasiHitung = PlayerPrefs.GetInt(subject);
            }

            if (subject == "highestScoreBilanganPangkatDanAkar")
            {
                playerProgress.highestScoreBilanganPangkatDanAkar = PlayerPrefs.GetInt(subject);
            }

            if (subject == "highestScoreFPBDanKPK")
            {
                playerProgress.highestScoreFPBDanKPK = PlayerPrefs.GetInt(subject);
            }

            if (subject == "highestScoreBangunDatarDanRuang")
            {
                playerProgress.highestScoreBangunDatarDanRuang = PlayerPrefs.GetInt(subject);
            }

            if (subject == "highestScoreDiagram")
            {
                playerProgress.highestScoreDiagram = PlayerPrefs.GetInt(subject);
            }
        }
        print("loadHS:" + subject);
    }

    private void LoadPlayerTimeProgress(string subject)
    {
        if (PlayerPrefs.HasKey(subject))
        {
            if (subject == "bestTimeOperasiHitung")
            {
                playerProgress.bestTimeOperasiHitung = PlayerPrefs.GetInt(subject);
            }

            if (subject == "bestTimeBilanganPangkatDanAkar")
            {
                playerProgress.bestTimeBilanganPangkatDanAkar = PlayerPrefs.GetInt(subject);
            }

            if (subject == "bestTimeFPBDanKPK")
            {
                playerProgress.bestTimeFPBDanKPK = PlayerPrefs.GetInt(subject);
            }

            if (subject == "bestTimeBangunDatarDanRuang")
            {
                playerProgress.bestTimeBangunDatarDanRuang = PlayerPrefs.GetInt(subject);
            }

            if (subject == "bestTimeDiagram")
            {
                playerProgress.bestTimeDiagram = PlayerPrefs.GetInt(subject);
            }
        } else
        {
            PlayerPrefs.SetInt(subject, 180);
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

        if (subject == "highestScoreBilanganPangkatDanAkar")
        {
            if (newScore > playerProgress.highestScoreBilanganPangkatDanAkar)
            {
                playerProgress.highestScoreBilanganPangkatDanAkar = newScore;
                SavePlayerProgress(subject);
            }
        }

        if (subject == "highestScoreFPBDanKPK")
        {
            if (newScore > playerProgress.highestScoreFPBDanKPK)
            {
                playerProgress.highestScoreFPBDanKPK = newScore;
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

        if (subject == "highestScoreDiagram")
        {
            if (newScore > playerProgress.highestScoreDiagram)
            {
                playerProgress.highestScoreDiagram = newScore;
                SavePlayerProgress(subject);
            }
        }

    }

    public int getHighestPlayerScore(string subject)
    {
        //print("getHS:" + subject);
        if (subject == "highestScoreOperasiHitung")
        {
            return playerProgress.highestScoreOperasiHitung;
        }
        else if (subject == "highestScoreBilanganPangkatDanAkar")
        {
            return playerProgress.highestScoreBilanganPangkatDanAkar;
        }
        else if (subject == "highestScoreFPBDanKPK")
        {
            return playerProgress.highestScoreFPBDanKPK;
        }
        else if (subject == "highestScoreBangunDatarDanRuang")
        {
            return playerProgress.highestScoreBangunDatarDanRuang;
        }
        else if (subject == "highestScoreDiagram")
        {
            return playerProgress.highestScoreDiagram;
        }

        return playerProgress.highestScore;
    }

    public void SubmitNewPlayerTime(int newTime, string subject)
    {
        if (subject == "bestTimeOperasiHitung")
        {
            if (newTime < playerProgress.bestTimeOperasiHitung)
            {
                playerProgress.bestTimeOperasiHitung = newTime;
                SavePlayerTimeProgress(subject);
            }

        }

        if (subject == "bestTimeBilanganPangkatDanAkar")
        {
            if (newTime < playerProgress.bestTimeBilanganPangkatDanAkar)
            {
                playerProgress.bestTimeBilanganPangkatDanAkar = newTime;
                SavePlayerTimeProgress(subject);
            }
        }

        if (subject == "bestTimeFPBDanKPK")
        {
            if (newTime < playerProgress.bestTimeFPBDanKPK)
            {
                playerProgress.bestTimeFPBDanKPK = newTime;
                SavePlayerTimeProgress(subject);
            }
        }

        if (subject == "bestTimeBangunDatarDanRuang")
        {
            if (newTime < playerProgress.bestTimeBangunDatarDanRuang)
            {
                playerProgress.bestTimeBangunDatarDanRuang = newTime;
                SavePlayerTimeProgress(subject);
            }
        }

        if (subject == "bestTimeDiagram")
        {
            if (newTime < playerProgress.bestTimeDiagram)
            {
                playerProgress.bestTimeDiagram = newTime;
                SavePlayerTimeProgress(subject);
            }
        }

    }

    public int getBestPlayerTime(string subject)
    {
        //print("getHS:" + subject);
        if (subject == "bestTimeOperasiHitung")
        {
            return playerProgress.bestTimeOperasiHitung;
        }
        else if (subject == "bestTimeBilanganPangkatDanAkar")
        {
            return playerProgress.bestTimeBilanganPangkatDanAkar;
        }
        else if (subject == "bestTimeFPBDanKPK")
        {
            return playerProgress.bestTimeFPBDanKPK;
        }
        else if (subject == "bestTimeBangunDatarDanRuang")
        {
            return playerProgress.bestTimeBangunDatarDanRuang;
        }
        else if (subject == "bestTimeDiagram")
        {
            return playerProgress.bestTimeDiagram;
        }

        return playerProgress.bestTime;
    }

    private void SavePlayerProgress(string subject)
    {
        print("saveHS:" + playerProgress.highestScore);
        if (subject == "highestScoreOperasiHitung")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreOperasiHitung);
        }

        if (subject == "highestScoreBilanganPangkatDanAkar")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreBilanganPangkatDanAkar);
        }

        if (subject == "highestScoreFPBDanKPK")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreFPBDanKPK);
        }

        if (subject == "highestScoreBangunDatarDanRuang")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreBangunDatarDanRuang);
        }

        if (subject == "highestScoreDiagram")
        {
            PlayerPrefs.SetInt(subject, playerProgress.highestScoreDiagram);
        }

    }

    private void SavePlayerTimeProgress(string subject)
    {
        print("saveHS:" + playerProgress.bestTime);
        if (subject == "bestTimeOperasiHitung")
        {
            PlayerPrefs.SetInt("bestTimeOperasiHitung", playerProgress.bestTimeOperasiHitung);
        }

        if (subject == "bestTimeBilanganPangkatDanAkar")
        {
            PlayerPrefs.SetInt("bestTimeBilanganPangkatDanAkar", playerProgress.bestTimeBilanganPangkatDanAkar);
        }

        if (subject == "bestTimeFPBDanKPK")
        {
            PlayerPrefs.SetInt("bestTimeFPBDanKPK", playerProgress.bestTimeFPBDanKPK);
        }

        if (subject == "bestTimeBangunDatarDanRuang")
        {
            PlayerPrefs.SetInt("bestTimeBangunDatarDanRuang", playerProgress.bestTimeBangunDatarDanRuang);
        }

        if (subject == "bestTimeDiagram")
        {
            PlayerPrefs.SetInt("bestTimeDiagram", playerProgress.bestTimeDiagram);
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

        playerProgress = new PlayerProgress();
        LoadPlayerProgress("highestScoreOperasiHitung");
        LoadPlayerTimeProgress("bestTimeOperasiHitung");
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

        playerProgress = new PlayerProgress();
        LoadPlayerProgress("highestScoreBilanganPangkatDanAkar");
        LoadPlayerTimeProgress("bestTimeBilanganPangkatDanAkar");
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

        playerProgress = new PlayerProgress();
        LoadPlayerProgress("highestScoreFPBDanKPK");
        LoadPlayerTimeProgress("bestTimeFPBDanKPK");
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

        playerProgress = new PlayerProgress();
        LoadPlayerProgress("highestScoreBangunDatarDanRuang");
        LoadPlayerTimeProgress("bestTimeBangunDatarDanRuang");
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
        
        playerProgress = new PlayerProgress();
        LoadPlayerProgress("highestScoreDiagram");
        LoadPlayerTimeProgress("bestTimeDiagram");
        StartGame();
    }
}
