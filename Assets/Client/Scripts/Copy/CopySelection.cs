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

    //ѡ�񳡾�������Ϸ
    public void EnterCopy()
    {
        //����ѡ���������
        copySelectionUI.SetActive(false);
        /*//�����������������
        UIManager.Instance.pointPanel.SetActive(false);

        //������Ϸʱ�䡢�������Ƿ������Ϸ����ɫ�Ƿ�����
        gameManager.gameTime = 0;
        gameManager.score = 0;
        gameManager.isGameOver = false;
        gameManager.isStartGameDissolve = true;*/
    }

    //ѡ�񸱱�
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
