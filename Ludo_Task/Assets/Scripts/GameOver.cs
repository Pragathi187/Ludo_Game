using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text first;
    public Text second;
    public Text third;


    // Start is called before the first frame update
    void Start()
    {
        first.text = "Winner 1" + SaveSettings.winners[0];
        second.text = "Winner 2" + SaveSettings.winners[1];
        third.text = "Winner 3" + SaveSettings.winners[2];
    }

    public void BackButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
