using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SubsystemsImplementation;

public class PlayerData : MonoBehaviour {

    [Header("Statistics")]
    [SerializeField] private string dataFileName;
    private DataRootObject dataRootObject;
    private string dataFilePath;

    private void Awake() {

        DontDestroyOnLoad(this);
        dataFilePath = Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + dataFileName;

        if (File.Exists(dataFilePath)) {

            using (StreamReader sr = new StreamReader(dataFilePath)) {

                if (Application.isEditor)
                    Debug.LogWarning("Importing Level Data...");

                if (sr.EndOfStream)
                    dataRootObject = new DataRootObject();
                else
                    dataRootObject = JsonConvert.DeserializeObject<DataRootObject>(sr.ReadToEnd());

            }
        } else {

            dataRootObject = new DataRootObject();

        }
    }

    private void OnApplicationQuit() {

        SerializeData();

    }

    private void SerializeData() {

        using (StreamWriter sw = new StreamWriter(dataFilePath)) {

            if (Application.isEditor)
                Debug.LogWarning("Serializing Level Data...");

            sw.Write(JsonConvert.SerializeObject(dataRootObject, Formatting.Indented, new JsonSerializerSettings {

                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            }));
        }
    }

    public void OnLevelComplete(Level level, float time) {

        Dictionary<int, LevelData> levelData = dataRootObject.GetLevelData();
        int ID = level.ID;

        if (levelData.ContainsKey(ID)) {

            levelData[ID].IncrementPlays();

            if (time < levelData[ID].GetRecord() || levelData[ID].GetRecord() == null) {
                Debug.Log("new record");
                levelData[ID].SetRecord(time);
            }

            Debug.Log(levelData[ID].GetRecord());
            SerializeData();

        } else {

            levelData.Add(ID, new LevelData(1, time));

        }
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

public class DataRootObject {

    public Dictionary<int, LevelData> levelData {

        get; set;

    }

    public DataRootObject() {

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

    public float? record {

        get; set;

    }

    public LevelData() {

        plays = 0;
        record = null;

    }

    public LevelData(int plays, float record) {

        this.plays = plays;
        this.record = record;

    }

    public int GetPlays() {

        return plays;

    }

    public void IncrementPlays() {

        plays++;

    }

    public float? GetRecord() {

        return record;

    }

    public void SetRecord(float record) {

        this.record = record;

    }
}