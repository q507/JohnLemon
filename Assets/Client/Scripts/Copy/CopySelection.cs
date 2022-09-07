using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopySelection : MonoBehaviour
{
    [SerializeField] GameObject copySelectionUI;
    [SerializeField] GameObject copySelectPanel;
    [SerializeField] GameObject playerRankUI;
    [SerializeField] Text sceneIndexText;
    public Text[] sceneIntroduces;
    [SerializeField] RectTransform[] sceneBtns;
    [SerializeField] GameObject shootPoint;

    [SerializeField] PlayerController player;
    [SerializeField] GameManager gameManager;
    [SerializeField] Ghost ghost;

    private Dictionary<RectTransform, Button> scenesBtnDic = new Dictionary<RectTransform, Button>();

    private void Start()
    {
        foreach (var btns in sceneBtns)
        {
            scenesBtnDic.Add(btns, btns.GetComponent<Button>());

            btns.GetComponent<Button>().onClick.AddListener(() =>
            {
                sceneIndexText.text = btns.GetComponentInChildren<Text>().text;
            });
        }
    }

    //选择场景进入游戏
    public void EnterCopy()
    {
        //副本选择界面隐藏
        copySelectionUI.SetActive(false);
        /*//分数面板结算界面隐藏
        UIManager.Instance.pointPanel.SetActive(false);

        //重置游戏时间、分数与是否结束游戏、角色是否显形
        gameManager.gameTime = 0;
        gameManager.score = 0;
        gameManager.isGameOver = false;
        gameManager.isStartGameDissolve = true;*/
    }

    //选择副本
    public void ChoiceCopy()
    {
        copySelectPanel.SetActive(true);
    }

    public void CheckPanking()
    {
        playerRankUI.SetActive(true);
    }

    public void ConfirmCopy()
    {
        copySelectPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
