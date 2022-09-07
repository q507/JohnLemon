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
        Debug.Log("�û���¼�ɹ����˺�Ϊ��" + inputNumber + "��" + "����Ϊ��" + inputPassword);
    }

    public void RegisterEvent()
    {
        inputNumber = registerNumber.text;
        inputPassword = registerPassword.text;
        inputName = registerName.text;
        netProcess.PlayerCreateReq(inputNumber, inputPassword, inputName);
        Debug.Log("ע�����û��ɹ����˺�Ϊ��" + inputNumber + "��" + "����Ϊ��" + inputPassword + "��" + "�ǳ�Ϊ��" + inputName);
    }

    /// <summary>
    /// ��ʼע��
    /// </summary>
    public void BeginToRegister()
    {
        loginPanel.gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��¼�ɹ�
    /// </summary>
    public void LoginSuccess()
    {
        loginPanel.gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        copySelectionPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ע��ɹ�
    /// </summary>
    public void RegisterSuccess()
    {
        //TODO: ������ͨ���󣬴˴���Ϊ������ʾ����ȷ���Ƿ񷵻ص�¼������Ƿ���ע������޸���Ϣ
        registerPanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ص�¼����
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
