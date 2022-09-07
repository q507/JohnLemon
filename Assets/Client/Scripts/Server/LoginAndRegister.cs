using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginAndRegister : MonoBehaviour
{
    [SerializeField] InputField loginNumber;
    [SerializeField] InputField loginPassword;

    [SerializeField] InputField registerNumber;
    [SerializeField] InputField registerPassword;
    [SerializeField] InputField registerName;

    [SerializeField] GameObject copySelectionPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject panel;

    [SerializeField] NetProcess netProcess;

    private Text moneyTxt;

    public string inputNumber;
    public string inputPassword;
    public string inputName;
    public int money;

    public bool isPlayerLogin = false;

    private void Start()
    {
        loginPanel.gameObject.SetActive(true);
        registerPanel.gameObject.SetActive(false);
        copySelectionPanel.gameObject.SetActive(false);
    }

    public void LoginEvent()
    {
        isPlayerLogin = true;

        inputNumber = loginNumber.text;
        inputPassword = loginPassword.text;
        netProcess.PlayerLogin(inputNumber, inputPassword);
        netProcess.GetSceneIndexReq(inputNumber);
        netProcess.GetScoreListReq(inputNumber);
        Debug.Log("用户登录成功！账号为：" + inputNumber + "，" + "密码为：" + inputPassword);
    }

    public void RegisterEvent()
    {
        inputNumber = registerNumber.text;
        inputPassword = registerPassword.text;
        inputName = registerName.text;
        netProcess.PlayerCreateReq(inputNumber, inputPassword, inputName);
        Debug.Log("注册新用户成功！账号为：" + inputNumber + "，" + "密码为：" + inputPassword + "，" + "昵称为：" + inputName);
    }

    /// <summary>
    /// 开始注册
    /// </summary>
    public void BeginToRegister()
    {
        loginPanel.gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 登录成功
    /// </summary>
    public void LoginSuccess()
    {
        loginPanel.gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        copySelectionPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 注册成功
    /// </summary>
    public void RegisterSuccess()
    {
        //TODO: 待测试通过后，此处改为弹出提示窗并确认是否返回登录界面或是返回注册界面修改信息
        registerPanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回登录界面
    /// </summary>
    public void ReturnToLogin()
    {
        registerPanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    public void SendMoney(int money)
    {
        this.money = money;
        moneyTxt.text = this.money.ToString();
    }

    public void UpdateMoney(int shopSprice, bool isDis, double disNum)
    {
        int cur;
        if (isDis)
        {
            cur = (int)Math.Round(Math.Ceiling(shopSprice * disNum), 0);
        }
        else
        {
            cur = shopSprice;
        }
        this.money -= cur;
        moneyTxt.text = this.money.ToString();
    }
}
