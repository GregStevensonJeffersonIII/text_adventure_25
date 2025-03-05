using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> inventory=new List<string>();

    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.onRestart += RestartGame;
        Load();
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save")) { 
        BinaryFormatter bf = new BinaryFormatter();
            FileStream afile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            SaveState playerData=(SaveState)bf.Deserialize(afile); 
            NavigationManager.Instance.SetRoom(playerData.currentRoom);
        }else
            NavigationManager.Instance.RestartGame();
    }

    private void RestartGame() { 
        inventory.Clear();
    }

    public void Save()
    {
        SaveState playerState = new SaveState();
        playerState.currentRoom = NavigationManager.Instance.currRoom.name;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream afile = File.Create(Application.persistentDataPath + "/player.save");
        bf.Serialize(afile, playerState);
        afile.Close();
    }
}
