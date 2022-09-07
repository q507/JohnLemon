using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Google.Protobuf;
using System.IO;
using TCCamp;
using System;
using System.Collections;

public class Packet
{
    public short  cmd;
    public short  len;
    public byte[] data;
}


public class NetWork : MonoBehaviour
{
    //private NetProcess netProcess = null;   //业务逻辑
    [SerializeField] NetProcess netProcess;
    [SerializeField] LoginAndRegister playerLogin;
    [SerializeField] LoadScene loadScene;
    [SerializeField] Animator reloadPanelAnim;
    [SerializeField] Animator reloadTextAnim;
    [SerializeField] Animator reloadIconAnim;

    public string ip              = "10.11.140.234";
    public int  port              = 8086;

    public int  numA              = 111;
    public int  numB              = 222;
    //public bool isPinging         = false;

    public TcpClient  tcpClient  = null;
    private float       intervalTime = 0.0f;
    private float sendPingIntervalTIme = 0.5f;
    private float       sendPingTime = 0f;
    private const float DEFAULTTIME  = 0f;

    private byte[] recvBuf           = new byte[64 * 1024];     //接收缓冲区
    private int recvBufLen           = 0;                       //接收缓冲区当前的长度

    const int offestSize = 2048;

    enum ConnectStatus
    {
        CONNECT_STATE_NONE         = 0,                           //未连接
        CONNECT_STATE_CONNECTING   = 1,                           //连接中
        CONNECT_STATE_CONNECTED    = 2,                           //已连接
        CONNECT_STATE_RECONNECTING = 3                            //断线重连中
    }

    private ConnectStatus connectStatus = ConnectStatus.CONNECT_STATE_NONE;

    //接收发送
    private void _TcpClientInit()
    {
        if (null == tcpClient)
        {
            tcpClient = new TcpClient();

        }
    }

    private void _ClientConnect()
    {
        if (null == tcpClient|| connectStatus != ConnectStatus.CONNECT_STATE_NONE)
            return;

        IPAddress addr = IPAddress.Parse(ip);
        IPEndPoint ipPoint = new IPEndPoint(addr, port);
        tcpClient.Connect(ipPoint);                                 //连接服务端
        if (tcpClient.Connected)
        {
            connectStatus = ConnectStatus.CONNECT_STATE_CONNECTED;
            Debug.Log("连接服务端成功");
        }
        else
        {
            connectStatus = ConnectStatus.CONNECT_STATE_CONNECTING; //尝试连接服务端
            Debug.Log("尝试连接服务端...");
        }
    }


    private void _ReClientConnect()
    {
        if (null == tcpClient || connectStatus != ConnectStatus.CONNECT_STATE_RECONNECTING)
            return;
        IPAddress addr = IPAddress.Parse(ip);
        IPEndPoint ipPoint = new IPEndPoint(addr, port);
        tcpClient.Connect(ipPoint);                                 //连接服务端
        if (tcpClient.Connected)
        {
            connectStatus = ConnectStatus.CONNECT_STATE_CONNECTED;
            Debug.Log("连接服务端成功");
        }
    }

    private void _ReLoadSuccess()
    {
        //重连成功UI
        UIManager.Instance.tryToReloadPanel.SetActive(false);
        UIManager.Instance.reloadSuccessPanel.SetActive(true);

        StartCoroutine(CloseReloadSuccessPanel());
    }

    IEnumerator CloseReloadSuccessPanel()
    {
        yield return new WaitForSeconds(1.5f);
        reloadPanelAnim.SetBool("GameReloadSuccess", true);
        yield return new WaitForSeconds(1.5f);
        reloadTextAnim.SetBool("ReloadSuccessText", true);
        yield return new WaitForSeconds(1.5f);
        reloadIconAnim.SetBool("ReloadSuccessIcon", true);
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.gameUI.SetActive(true);
        UIManager.Instance.reloadSuccessPanel.SetActive(false);
    }

    private void _RecvMsg()
    {
        if(tcpClient.Connected == false)
        {
            _OnClose();
            _TcpClientInit();
            return;
        }

        NetworkStream readStream = tcpClient.GetStream();
        int    checkLen;
        Packet packet;

        while (readStream.DataAvailable)
        {
            int readCount = readStream.Read(recvBuf, recvBufLen, recvBuf.Length - recvBufLen);
            recvBufLen   += readCount;
        }

        while (true)
        {
            if (recvBufLen <= 0)
            {
                //Debug.Log("未接收到服务端数据包");
                break;
            }
            
            checkLen = _Check(recvBuf, recvBufLen);
            
            if(checkLen <= 0 )
            {
                Debug.Log("快速校验不合法或者数据包不完整（缓冲区大小不够）");
                //清空数据
                _BytesArrClear();
                recvBufLen = 0;
                break;
            }

            packet = new Packet();
            _Decode(packet, recvBuf);

            Debug.Log("当前接收缓冲区的大小：" + recvBufLen + "  cmd:" + packet.cmd);

            recvBufLen -= offestSize;

            if(recvBufLen > 0)
            {
                _Menmove(recvBuf, offestSize , recvBufLen);
            }


            //包处理完毕逻辑
            _OnPackBackProcess(packet);
        }

        if (!tcpClient.Connected)
        {
            connectStatus = ConnectStatus.CONNECT_STATE_NONE;
        }
    }

    void Start()
    {
        //netProcess = gameObject.GetComponent<NetProcess>();
        if (netProcess == null)
        {
            Debug.Log("netProcess get fail");
        }
        _TcpClientInit();

        netProcess.reLoadSuccess += _ReLoadSuccess;
    }

    void Update()
    {
        intervalTime += Time.deltaTime;
        sendPingTime += Time.deltaTime;
        if (intervalTime >= DEFAULTTIME)
        {
            intervalTime -= DEFAULTTIME;
            if (connectStatus == ConnectStatus.CONNECT_STATE_NONE)
            {
                _ClientConnect();
            }
            else if (connectStatus == ConnectStatus.CONNECT_STATE_CONNECTED)
            {
                _RecvMsg();
            }
            else if (connectStatus == ConnectStatus.CONNECT_STATE_RECONNECTING)
            {
                UIManager.Instance.gameUI.SetActive(false);
                UIManager.Instance.tryToReloadPanel.SetActive(true);

                //暂停游戏尝试重连
                Time.timeScale = 0;

                _OnClose();
                _TcpClientInit();

                connectStatus = ConnectStatus.CONNECT_STATE_RECONNECTING;
                _ReClientConnect();//重新连接服务端

                Debug.Log("Try to reConnectSetver");


                if (connectStatus == ConnectStatus.CONNECT_STATE_CONNECTED)
                {
                    Time.timeScale = 1;

                    netProcess.PlayerLogin(playerLogin.inputNumber, playerLogin.inputPassword);

                    Debug.Log("发送请求");

                    netProcess.GetSceneReq(playerLogin.inputNumber, loadScene.sceneIndex);
                }
            }
        }

        if (connectStatus == ConnectStatus.CONNECT_STATE_CONNECTED)
        {
            if (sendPingTime - sendPingIntervalTIme > 0)
            {
                _SendPBMessage(CLIENT_CMD.ClientPing, null);
                sendPingTime = 0;
            }
        }
    }


    private void _OnPackBackProcess(Packet packet)
    {
        //AddRsp              rsp;
        //PlayerLoginRsp      loginRsp;
        //PlayerCreateRsp     createRsp;
        //ShopListDataSysRsp  getShopListRsp;
        switch (packet.cmd)
        {
            //case (short)SERVER_CMD.ServerAddRsp:
            //    {
            //        rsp = new AddRsp(AddRsp.Parser.ParseFrom(packet.data));
            //        Debug.Log("收到服务端消息:" + rsp.Result.ToString());
            //        break;
            //    }
            case (short)SERVER_CMD.ServerPong:
                {
                    Debug.Log("server pong...");
                    break;
                }
            case (short)SERVER_CMD.ServerPlayerloginRsp:
                {
                    PlayerLoginRsp loginRsp = new PlayerLoginRsp(PlayerLoginRsp.Parser.ParseFrom(packet.data));
                    netProcess.PlayerLoginResponse(loginRsp);
                    break;
                }
            case (short)SERVER_CMD.ServerPlayercreateRsp:
                {
                    PlayerCreateRsp createRsp = new PlayerCreateRsp(PlayerCreateRsp.Parser.ParseFrom(packet.data));
                    netProcess.playerCreateRsp(createRsp);
                    break;
                }
            //case (short)SERVER_CMD.ServerShoplistsynRsp:
            //    {
            //        getShopListRsp = new ShopListDataSysRsp(ShopListDataSysRsp.Parser.ParseFrom(packet.data));
            //        netProcess.GetShopListRsp(getShopListRsp);
            //        break;
            //    }
            //case (short)SERVER_CMD.ServerShopitembuyRsp:
            //    {
            //        BuyShopItemRsp buyShopItemRsp = new BuyShopItemRsp(TCCamp.BuyShopItemRsp.Parser.ParseFrom(packet.data));
            //        netProcess.BuyShopItemRsp(buyShopItemRsp);
            //        break;
            //    }
            case (short)SERVER_CMD.ServerGetallsceneRsp:
                {
                    SceneGetALLRsp getAllSceneRsp = new SceneGetALLRsp(SceneGetALLRsp.Parser.ParseFrom(packet.data));
                    netProcess.GetSceneIndexRsp(getAllSceneRsp);
                    break;
                }
            case (short)SERVER_CMD.ServerGetsceneRsp:
                {
                    SceneGetOneRsp getSceneRsp = new SceneGetOneRsp(SceneGetOneRsp.Parser.ParseFrom(packet.data));
                    netProcess.GetSceneRsp(getSceneRsp);
                    break;
                }
            case (short)SERVER_CMD.ServerGameoverRsp:
                {
                    GameOverRsp gameOverRsp = new GameOverRsp(GameOverRsp.Parser.ParseFrom(packet.data));
                    netProcess.GetGameOverRsp(gameOverRsp);
                    break;
                }
            case (short)SERVER_CMD.ServerGetscorelistRsp:
                {
                    ScoreListRsp scoreListRsp = new ScoreListRsp(ScoreListRsp.Parser.ParseFrom(packet.data));
                    netProcess.GetScoreListRsp(scoreListRsp);
                    break;
                }
            case (short)SERVER_CMD.ServerScenesyncRsp:
                {
                    SceneSyncRsp sceneSyncRsp = new SceneSyncRsp(SceneSyncRsp.Parser.ParseFrom(packet.data));
                    netProcess.GetSceneSyncRsp(sceneSyncRsp);
                    break;
                }
            default:
                break;
        }
    }

    private void _OnClose()
    {
        if (connectStatus == ConnectStatus.CONNECT_STATE_CONNECTED)
        {
            tcpClient.Close();
            tcpClient = null;
            connectStatus = ConnectStatus.CONNECT_STATE_RECONNECTING;
        }
    }


    //_Menmove(recvBuf, offestSize, recvBufLen);
    private void _Menmove(byte[] arr, int offest, int size)
    {
        for (int i = 0; i < size; i++)
        {
            arr[i] = arr[i + offest];
        }
    }

    private void _BytesArrClear()
    {
        for (int i = 0; i < recvBuf.Length; i++)
        {
            recvBuf[i] = 0;
        }
    }

    

    public void _SendPBMessage(CLIENT_CMD cmd, IMessage msg)
    {
        byte[] data = null;
        byte[] sendBytes;
        if (cmd == CLIENT_CMD.ClientPing)
        {
            data = new byte[0];
        }
        else
        {
            MemoryStream memoryStream = new MemoryStream();

            msg.WriteTo(memoryStream);
            data = memoryStream.ToArray();
        }
        sendBytes = new byte[data.Length + 6];
        _Encode(sendBytes, cmd, data);
        NetworkStream writeStream = tcpClient.GetStream();
        writeStream.Write(sendBytes, 0, sendBytes.Length);
        writeStream.Flush();
    }

    //编码
    private int _Encode(byte[] sendBytes, CLIENT_CMD cmd, byte[] data)
    {
        sendBytes[0]    = BitConverter.GetBytes('T')[0];
        sendBytes[1]    = BitConverter.GetBytes('C')[0];

        byte[] lenBytes = BitConverter.GetBytes(data.Length + 2);
        sendBytes[2]    = lenBytes[0];
        sendBytes[3]    = lenBytes[1];

        byte[] proBytes = BitConverter.GetBytes((short)cmd);
        sendBytes[4]    = proBytes[0];
        sendBytes[5]    = proBytes[1];
        for (int i = 0; i < data.Length; i++)
        {
            sendBytes[i + 6] = data[i];
        }
        return data.Length + 6;
    }

    //解码
    private int _Decode(Packet packet, byte[] readbyte)
    {
        byte[] lenBytes = new byte[2];
        lenBytes[0]     = readbyte[2];
        lenBytes[1]     = readbyte[3];
        packet.len      = BitConverter.ToInt16(lenBytes, 0);        //(short)((((short)readbyte[3]) << 8) + (short)readbyte[2]);
        //Debug.Log(packet.len);

        byte[] cmdBytes = new byte[2];
        cmdBytes[0]     = readbyte[4];
        cmdBytes[1]     = readbyte[5];

        packet.cmd  = BitConverter.ToInt16(cmdBytes, 0);            //(short)((((short)readbyte[5]) << 8) + (short)readbyte[4]);
        //Debug.Log(packet.cmd);
        packet.data     = new byte[packet.len - 2];
        for (int i = 0; i < packet.len - 2; i++)
        {
            packet.data[i] = readbyte[i + 6];
        }
        return 0;
    }

    //校验
    private int _Check(byte[] buffer, int len)
    {
        int res = -1;
        if (buffer[0] != 'T' || buffer[1] != 'C')
        {
            //Debug.Log("快速校验失败");
            return res;
        }
        res = ((short)buffer[3]) * 256 + buffer[2];

        if (res + 4 > len)
        {
            Debug.Log("包不完整");
            return 0;
        }
        return res + 4;
    }
}
