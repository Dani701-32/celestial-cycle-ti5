using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;

    public TimeControllerData timeControllerData;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }
   
    public void SaveGame()
    {
        TimeControllerManager.InstanceTime.SaveVariables();

        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/game_save/timecontroller_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/timecontroller_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/game_save/timecontroller_data/timecontroller_save.bin");
        var json = JsonUtility.ToJson(timeControllerData);
        bf.Serialize(file, json);
        file.Close();
        Debug.Log("Salvo");
    }

    public void LoadGame()
    {
        TimeControllerManager.InstanceTime.InitializeVariables();
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/timecontroller_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/timecontroller_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/game_save/timecontroller_data/timecontroller_save.bin"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/timecontroller_data/timecontroller_save.bin", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), timeControllerData);
            file.Close();
        }
        Debug.Log("Carregado");

    }
}
