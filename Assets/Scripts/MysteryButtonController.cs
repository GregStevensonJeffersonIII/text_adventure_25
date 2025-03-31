using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MysteryButtonController : MonoBehaviour
{
    public GameObject image;//game object holding the image

    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        int pref = PlayerPrefs.GetInt("image", 0);//default 0/off
        if (pref == 1)
            toggle.isOn = true;
        else
            toggle.isOn = false;
        isOn = toggle.isOn;

        ChangeImage();

        toggle.onValueChanged.AddListener(ProcessChange);//a listener
    }

    void ProcessChange(bool value)
    {
        isOn = value;
        PlayerPrefs.SetInt("image", isOn ? 1 : 0);
        ChangeImage();
    }
    void ChangeImage()
    {
        if (isOn)
        {
            image.SetActive(true);
        }
        else
        {
           image.SetActive(false);
        }
    }
}
