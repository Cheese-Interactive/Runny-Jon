using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerData : MonoBehaviour {

    [Header("Statistics")]
    [SerializeField] private string levelDataFileName;
    [SerializeField] private string gameDataFileName;
    private LevelDataRootObject dataRootObject;
    private GameData gameData;
    private string levelDataFilePath;
    private string gameDataFilePath;

    private void Awake() {

        DontDestroyOnLoad(this);
        gameDataFilePath = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + gameDataFileName;
        levelDataFilePath = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + levelDataFileName;

        if (File.Exists(gameDataFilePath)) {

            using (StreamReader sr = new StreamReader(gameDataFilePath)) {

                if (Application.isEditor)
                    Debug.LogWarning("Importing Game Data...");

                if (sr.EndOfStream)
                    gameData = new GameData();
                else
                    gameData = JsonConvert.DeserializeObject<GameData>(sr.ReadToEnd());

            }
        } else {

            gameData = new GameData();

        }

        if (File.Exists(levelDataFilePath)) {

            using (StreamReader sr = new StreamReader(levelDataFilePath)) {

                if (Application.isEditor)
                    Debug.LogWarning("Importing Level Data...");

                if (sr.EndOfStream)
                    dataRootObject = new LevelDataRootObject();
                else
                    dataRootObject = JsonConvert.DeserializeObject<LevelDataRootObject>(sr.ReadToEnd());

            }
        } else {

            dataRootObject = new LevelDataRootObject();

        }
    }

    private void OnApplicationQuit() {

        SerializeData();

    }

    private void SerializeData() {

        using (StreamWriter sw = new StreamWriter(gameDataFilePath)) {

            if (Application.isEditor)
                Debug.LogWarning("Serializing Game Data...");

            sw.Write(JsonConvert.SerializeObject(gameData, Formatting.Indented, new JsonSerializerSettings {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            }));
        }

        using (StreamWriter sw = new StreamWriter(levelDataFilePath)) {

            if (Application.isEditor)
                Debug.LogWarning("Serializing Level Data...");

            sw.Write(JsonConvert.SerializeObject(dataRootObject, Formatting.Indented, new JsonSerializerSettings {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            }));
        }
    }

    public bool OnLevelComplete(Level level, int deaths, float time, int quesos) {

        gameData.AddQuesos(quesos);
        gameData.AddTotalDeaths(deaths);
        gameData.IncrementLevelsCompleted();

        Dictionary<int, LevelData> levelData = dataRootObject.GetLevelData();
        int ID = level.ID;
        bool newRecord = false;

        if (levelData.ContainsKey(ID)) {

            levelData[ID].IncrementPlays();
            levelData[ID].AddDeaths(deaths);

            if (time < levelData[ID].GetRecord() || levelData[ID].GetRecord() == null) {

                newRecord = true;
                levelData[ID].SetRecord(time);

            }
        } else {

            levelData.Add(ID, new LevelData(1, deaths, time));
            newRecord = true;

        }

        SerializeData();
        return newRecord;

    }

    public int GetQuesos() {

        return gameData.GetQuesos();

    }

    public int GetLevelPlays(Level level) {

        Dictionary<int, LevelData> levelData = dataRootObject.GetLevelData();
        int ID = level.ID;

        if (levelData.ContainsKey(ID))
            return levelData[ID].GetPlays();

        levelData.Add(ID, new LevelData());
        return 0;

    }

    public float? GetLevelRecord(Level level) {

        Dictionary<int, LevelData> levelData = dataRootObject.GetLevelData();
        int ID = level.ID;

        if (levelData.ContainsKey(ID))
            return levelData[ID].GetRecord();

        levelData.Add(ID, new LevelData());
        return null;

    }
}

public class LevelDataRootObject {

    public Dictionary<int, LevelData> levelData {

        get; set;

    }

    public LevelDataRootObject() {

        levelData = new Dictionary<int, LevelData>();

    }

    public Dictionary<int, LevelData> GetLevelData() {

        return levelData;

    }
}

public class LevelData {

    public int plays {

        get; set;

    }

    public int deaths {

        get; set;

    }

    public float? record {

        get; set;

    }

    public LevelData() {

        plays = 0;
        deaths = 0;
        record = null;

    }

    public LevelData(int plays, int deaths, float record) {

        this.plays = plays;
        this.deaths = deaths;
        this.record = record;

    }

    public int GetPlays() {

        return plays;

    }

    public void IncrementPlays() {

        plays++;

    }

    public int GetDeaths() {

        return deaths;

    }

    public void AddDeaths(int deaths) {

        this.deaths += deaths;

    }

    public float? GetRecord() {

        return record;

    }

    public void SetRecord(float record) {

        this.record = record;

    }
}

public class GameData {

    public int quesos {

        get; set;

    }

    public int levelsCompleted {

        get; set;

    }

    public int totalDeaths {

        get; set;

    }

    public GameData() {

        quesos = 0;
        levelsCompleted = 0;
        totalDeaths = 0;

    }

    public GameData(int quesos, int levelsCompleted, int totalDeaths) {

        this.quesos = quesos;
        this.levelsCompleted = levelsCompleted;
        this.totalDeaths = totalDeaths;

    }

    public int GetQuesos() {

        return quesos;

    }

    public void AddQuesos(int amount) {

        quesos += amount;

    }

    public void RemoveQuesos(int amount) {

        quesos -= amount;

    }

    public int GetLevelsCompleted() {

        return levelsCompleted;

    }

    public void IncrementLevelsCompleted() {

        levelsCompleted++;

    }

    public int GetTotalDeaths() {

        return totalDeaths;

    }

    public void AddTotalDeaths(int deaths) {

        totalDeaths += deaths;

    }
}