using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingLoading : MonoBehaviour
{
    public static SavingLoading instance;
    public bool loadByMenu;
    private string SavePath => $"{Application.persistentDataPath}/saveGame.txt";

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(this);

        if (StatusFile())
        {
            Load();
            Debug.Log("Carregou o jogo a partir do Save");
        }
        else
        {
            Debug.Log("Iniciou um Novo Jogo");
        }
    }

    public bool StatusFile()
    {
        if (!File.Exists(SavePath)) return false;
        else return true;
    }

    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
        GameController.gameController.inventorySystem.SaveInventory();
        Debug.Log("O Jogo foi Salvo");
    }

    public void Load()
    {
        loadByMenu = false;
        var state = LoadFile();
        RestoreState(state);
        GameController.gameController.inventorySystem.LoadInventory();
        
    }

    private Dictionary<string, object> LoadFile()
    {
        if(!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
