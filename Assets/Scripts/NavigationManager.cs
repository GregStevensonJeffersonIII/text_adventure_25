using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{

    public static NavigationManager Instance;
    public Room startingRoom;
    public Room currRoom;

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
        currRoom = startingRoom;
        Unpack();
    }

    void Unpack() { 
        string description= currRoom.description;
        foreach (Exit e in currRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }
        InputManager.instance.UpdateStory(description);
        
    }

    public bool SwitchRooms(string direction) {
        if (exitRooms.ContainsKey(direction)) {
            currRoom=exitRooms[direction];
            InputManager.instance.UpdateStory("You go "+direction);
            exitRooms.Clear();
            Unpack();
            return true;
        }
        return false;
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
}
