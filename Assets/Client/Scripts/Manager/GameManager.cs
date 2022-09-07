using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Ghost ghost;
    [SerializeField] HealthManager healthManager;
    [SerializeField] Gargoyle gargoyle;
    [SerializeField] GameObject birthPoint;
    [SerializeField] GameObject gameWinPoint;
    [SerializeField] NetProcess netProcess;
    [SerializeField] LoginAndRegister playerLogin;
    [SerializeField] PlayerRank playerRank;
    [SerializeField] SceneData sceneData;
    public Material playerHead;
    public Material playerBody;

    private float gameRealTime = 0;
    public int gameTime = 0;
    public int score = 0;

    public float dissolve = 1.0f;
    public bool isStartGame = false;
    public bool isStartGameDissolve = true;
    public bool isGameOver = false;
    private bool isSendGameOverReq = true;

    private bool isSendWinScoreList = true;
    private bool isSendLoseScoreList = true;

    public UnityAction PlayerRebirth;
    public UnityAction PlayerDeathEvent;

    private void Start()
    {
        //��ɫ����������Ϸʧ�ܵ�ί���¼�
        healthManager.onDie += LoseGameEvent;
        //todo ��Ϸʧ���¼�
        //��ʱ��120���ڵ����յ㣬ʱ��UIչʾ

        playerHead.SetFloat("_Dissolve", 1);
        playerBody.SetFloat("_Dissolve", 1);
    }

    private void Update()
    {
        _StartLevel();
        GameScoreCount();
        BoolGameOver();

        if (isGameOver)
        {
            if (isSendGameOverReq)
            {
                isSendGameOverReq = false;
                //�������˽���
                netProcess.GetGameOverReq(playerLogin.inputNumber, UIManager.Instance.beatCount, gameTime);
            }
        }

        //����Ϸʤ���ж���ģ������
        if (Vector3.Distance(player.transform.position, gameWinPoint.transform.position) < 1f)
        {
            isGameOver = true;
        }/*
        else if(gameTime > 180)
        {
            isGameOver = true;
        }*/
    }

    //��ɫ��ʼ��Ϸ����Ч��
    private void _StartLevel()
    {
        if (!isGameOver && isStartGame)
        {
            sceneData.isGameStart = true;

            //����ϷΪ��������ҵ����ʼ��Ϸ��ʼ��ʱ
            gameRealTime += Time.deltaTime;
            if (gameRealTime >= 1)
            {
                gameRealTime = 0;
                gameTime++;
                Debug.Log(gameTime);
            }

            if(gameTime > 180)
            {
                score = 0;
            }
        }
        UIManager.Instance.countTIme.text = gameTime.ToString();

        if (dissolve > 0)
        {
            if (isStartGameDissolve)
            {
                dissolve -= 0.01f;
                playerHead.SetFloat("_Dissolve", dissolve);
                playerBody.SetFloat("_Dissolve", dissolve);
            }
        }
        else
        {
            isStartGameDissolve = false;
        }
    }

    public void BoolGameOver()
    {
        if(isGameOver && gameTime <= 180)
        {
            WinGameEvent();

            if (isSendWinScoreList)
            {
                netProcess.GetScoreListReq(playerLogin.inputNumber);
                isSendWinScoreList = false;
            }
        }
        else if (isGameOver && gameTime > 180)
        {
            LoseGameEvent();

            if (isSendLoseScoreList)
            {
                netProcess.GetScoreListReq(playerLogin.inputNumber);
                isSendLoseScoreList = false;
            }
        }
    }

    public void GameScoreCount()
    {
        //������һ���
        score = UIManager.Instance.beatCount * 100 + (180 - gameTime) * 300;
    }

    //��Ϸʤ��
    public void WinGameEvent()
    {
        //ֹͣ����ͬ������
        sceneData.isGameStart = false;

        //��ʱ
        UIManager.Instance.CountTime(gameTime);
        //����
        if (score <= 0)
        {
            score = 0;
        }
        UIManager.Instance.Score(score);

        //ʤ������
        UIManager.Instance.winGameImage.gameObject.SetActive(true);

        StartCoroutine(WinGameCoroutine());
    }

    //��Ϸʧ��
    public void LoseGameEvent()
    {
        //ֹͣ����ͬ������
        sceneData.isGameStart = false;

        UIManager.Instance.CountTime(gameTime);
        score = 0;
        UIManager.Instance.Score(score);

        PlayerDeathEvent?.Invoke();
        StartCoroutine(LoseGameCoroutine());
    }

    public void ReturnChoiceCopy()
    {
        //���ظ���ѡ�����
        UIManager.Instance.winGameImage.gameObject.SetActive(false);
        UIManager.Instance.loseGameImage.gameObject.SetActive(false);
        UIManager.Instance.pointPanel.SetActive(false);
        UIManager.Instance.gameUI.SetActive(false);
        UIManager.Instance.copySelectionUI.SetActive(true);
    }

    IEnumerator WinGameCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        //�������
        UIManager.Instance.pointPanel.SetActive(true);
        /*Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;*/
    }

    IEnumerator LoseGameCoroutine()
    {
        //��ɫ��������Ч��
        if (dissolve < 1)
        {
            dissolve += 0.01f;
            playerHead.SetFloat("_Dissolve", dissolve);
            playerBody.SetFloat("_Dissolve", dissolve);
        }
        yield return new WaitForSeconds(3.5f);
        UIManager.Instance.loseGameImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        //�������
        UIManager.Instance.pointPanel.gameObject.SetActive(true);
        /*Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;*/
    }
}
