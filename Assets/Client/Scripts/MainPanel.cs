using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
