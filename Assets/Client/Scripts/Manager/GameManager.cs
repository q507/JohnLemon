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
        //角色死亡触发游戏失败的委托事件
        healthManager.onDie += LoseGameEvent;
        //todo 游戏失败事件
        //超时、120秒内到达终点，时间UI展示

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
                //请求服务端结算
                netProcess.GetGameOverReq(playerLogin.inputNumber, UIManager.Instance.beatCount, gameTime);
            }
        }

        //对游戏胜利判定的模糊处理
        if (Vector3.Distance(player.transform.position, gameWinPoint.transform.position) < 1f)
        {
            isGameOver = true;
        }/*
        else if(gameTime > 180)
        {
            isGameOver = true;
        }*/
    }

    //角色开始游戏渐变效果
    private void _StartLevel()
    {
        if (!isGameOver && isStartGame)
        {
            sceneData.isGameStart = true;

            //当游戏为结束且玩家点击开始游戏开始计时
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
        //计算玩家积分
        score = UIManager.Instance.beatCount * 100 + (180 - gameTime) * 300;
    }

    //游戏胜利
    public void WinGameEvent()
    {
        //停止发送同步请求
        sceneData.isGameStart = false;

        //计时
        UIManager.Instance.CountTime(gameTime);
        //积分
        if (score <= 0)
        {
            score = 0;
        }
        UIManager.Instance.Score(score);

        //胜利画面
        UIManager.Instance.winGameImage.gameObject.SetActive(true);

        StartCoroutine(WinGameCoroutine());
    }

    //游戏失败
    public void LoseGameEvent()
    {
        //停止发送同步请求
        sceneData.isGameStart = false;

        UIManager.Instance.CountTime(gameTime);
        score = 0;
        UIManager.Instance.Score(score);

        PlayerDeathEvent?.Invoke();
        StartCoroutine(LoseGameCoroutine());
    }

    public void ReturnChoiceCopy()
    {
        //返回副本选择界面
        UIManager.Instance.winGameImage.gameObject.SetActive(false);
        UIManager.Instance.loseGameImage.gameObject.SetActive(false);
        UIManager.Instance.pointPanel.SetActive(false);
        UIManager.Instance.gameUI.SetActive(false);
        UIManager.Instance.copySelectionUI.SetActive(true);
    }

    IEnumerator WinGameCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        //积分面板
        UIManager.Instance.pointPanel.SetActive(true);
        /*Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;*/
    }

    IEnumerator LoseGameCoroutine()
    {
        //角色死亡淡出效果
        if (dissolve < 1)
        {
            dissolve += 0.01f;
            playerHead.SetFloat("_Dissolve", dissolve);
            playerBody.SetFloat("_Dissolve", dissolve);
        }
        yield return new WaitForSeconds(3.5f);
        UIManager.Instance.loseGameImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        //积分面板
        UIManager.Instance.pointPanel.gameObject.SetActive(true);
        /*Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;*/
    }
}
