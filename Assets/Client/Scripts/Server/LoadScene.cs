using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] NetProcess netProcess;
    [SerializeField] LoginAndRegister playerLogin;
    [SerializeField] GameObject[] sceneBtns;

    RectTransform[] scenes = new RectTransform[5];
    private Dictionary<RectTransform, Button> scenesDic = new Dictionary<RectTransform, Button>();

    private string[] sceneName = { "场景一", "场景二", "场景三", "场景四", "场景五"};
    public int sceneIndex = 0;

    private void Start()
    {
        for (int i = 0; i < sceneBtns.Length; i++)
        {
            scenes[i] = sceneBtns[i].transform as RectTransform;
        }

        foreach (var scene in scenes)
        {
            scenesDic.Add(scene, scene.GetComponent<Button>());

            scene.GetComponent<Button>().onClick.AddListener(() =>
            {
                if(scene.GetComponentInChildren<Text>().text == sceneName[0])
                {
                    sceneIndex = 0;
                }
                else if (scene.GetComponentInChildren<Text>().text == sceneName[1])
                {
                    sceneIndex = 1;
                }
                else if (scene.GetComponentInChildren<Text>().text == sceneName[2])
                {
                    sceneIndex = 2;
                }
                else if (scene.GetComponentInChildren<Text>().text == sceneName[3])
                {
                    sceneIndex = 3;
                }
                else if (scene.GetComponentInChildren<Text>().text == sceneName[4])
                {
                    sceneIndex = 4;
                }
            });
        }
    }

    public void LoadSceneIndex()
    {
        netProcess.GetSceneReq(playerLogin.inputNumber, sceneIndex);
        //SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        Debug.Log("Load Scene Success");
    }
}
