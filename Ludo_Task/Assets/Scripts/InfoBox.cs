using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public Text infoText;

    public static InfoBox instance;

    void Awake()
    {
        instance = this;
        infoText.text = "";
    }

    public void ShowMessage(string message)
    {
        infoText.text = message;
    }
}
