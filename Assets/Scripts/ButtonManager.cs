using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnPlayButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void onQuitPressed()
    {
        Application.Quit();
    }

    public void onMenuPress()
    {
        SceneManager.LoadScene("Menu");
    }
}
