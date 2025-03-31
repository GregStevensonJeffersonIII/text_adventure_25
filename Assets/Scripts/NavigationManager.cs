using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{

    public static NavigationManager Instance;
    public Room startingRoom;
    public Room currRoom;
    public Room orbRoom;//for restart
    public Room keyRoom;//
    public List<Room> rooms;

    //game over delegate
    public delegate void GameOver();
    public event GameOver onGameOver;


    public Exit toKeyNorth;
    
    private Dictionary<string, Room> exitRooms=new Dictionary<string, Room>();

  

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.onRestart += RestartGame;
        /*
        toKeyNorth.isHidden = true;
        currRoom = startingRoom;
        Unpack();
        */

    }

    public void RestartGame() {
        toKeyNorth.isHidden = true;
        currRoom = startingRoom;
        orbRoom.hasOrb=true;
        keyRoom.hasKey=true;
        Unpack();
    }

    void Unpack() { 
        string description= currRoom.description;
        exitRooms.Clear();
        foreach (Exit e in currRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }
        InputManager.instance.UpdateStory(description);
        if (exitRooms.Count == 0) 
            if(onGameOver!=null)
                onGameOver();
        
    }

    public bool SwitchRooms(string direction) {
        if (exitRooms.ContainsKey(direction))
        {
            if (!GetExit(direction).isLocked || GameManager.instance.inventory.Contains("key"))
            {
                currRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                exitRooms.Clear();
                Unpack();
                return true;
            }
        }
        return false;
    }
    public Exit GetExit(string direction) {
        foreach (Exit e in currRoom.exits) {
            if (e.direction.ToString()==direction) {
                return e;
            }
        }
        return null;

    }
    public bool TakeItem(string item) {
        if (currRoom.hasKey && item=="key") {
            return true;
        } else if (currRoom.hasOrb && item=="orb") {
            toKeyNorth.isHidden = false;
            currRoom.hasOrb = false;
            return true;
        }
        return false;
    }

    public void SetRoom(string room)
    {
        foreach (Room e in rooms) {
            if (e.name == room) { 
                currRoom=e;
                Unpack();
                break;
            }
        }
    }
}
