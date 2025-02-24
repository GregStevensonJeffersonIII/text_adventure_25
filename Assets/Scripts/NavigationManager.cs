using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{

    public static NavigationManager Instance;
    public Room startingRoom;
    public Room currRoom;

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
        Debug.Log(startingRoom.description);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
