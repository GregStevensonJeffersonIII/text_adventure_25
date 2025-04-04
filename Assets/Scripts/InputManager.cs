using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Text storyText; // the story 
    public InputField userInput; // the input field object
    public Text inputText; // part of the input field where user enters response
    public Text placeHolderText; // part of the input field for initial placeholder text
                                 // public Button button;
    //delegate things
    public delegate void Restart();
    public event Restart onRestart;
    
    private string story; // holds the story to display
    private List<string> commands = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");
        commands.Add ("restart");
        commands.Add("save");
        commands.Add("commands");
        commands.Add("inventory");

        userInput.onEndEdit.AddListener(GetInput);
       // button.onClick.AddListener(DoSomething);
        story = storyText.text;
        NavigationManager.Instance.onGameOver += EndGame;
    }

    void EndGame() {
        UpdateStory("\n Please enter 'restart' to play again");
    }
/*
    void DoSomething() {
        Debug.Log("stuff");
    }
*/
    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;

    }
    private void GetInput(string msg) {
        if (msg != "")
        {
            char[] splitInfo = { ' ' };
            string[] parts = msg.ToLower().Split(splitInfo);//['go','north']
            if (commands.Contains(parts[0]))
            {
                if (parts[0] == "go")
                {
                    if (NavigationManager.Instance.SwitchRooms(parts[1]))
                    {
                        //fill later
                    }
                    else
                    {
                        UpdateStory("You walked into a wall or a locked door you moron");
                    }
                }

                else if (parts[0] == "get")
                {
                    if (NavigationManager.Instance.TakeItem(parts[1]))
                    {
                        GameManager.instance.inventory.Add(parts[1]);
                        UpdateStory("You added " + parts[1] + " to your inventory");
                    }
                    else
                    {
                        UpdateStory("Gwt What!? There ain't nothin like that in here");
                    }
                }
                else if (parts[0] == "restart")
                {
                    if (onRestart != null)
                        onRestart();
                }
                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                }
                else if (parts[0] == "commands")
                {
                    string com = "Commands:  ";//temp variable
                    foreach(string e in commands)
                        com+= e+"  ";
                        UpdateStory(com);
                }
                else if (parts[0] == "inventory")
                {
                    UpdateStory(GameManager.instance.ListInventory());
                }
            }
        }
        //reset
        userInput.text = "";
        userInput.ActivateInputField();
    }
}
