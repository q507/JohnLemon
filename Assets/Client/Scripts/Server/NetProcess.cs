using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using TCCamp;
using UnityEngine;
using UnityEngine.Events;

public class NetProcess : MonoBehaviour
{
    public LoginAndRegister playerLogin      = null;  //登录注册
    public ShoppingMall shoppingMall    = null;  //商城列表
    public Backpack backPack            = null;  //背包系统
    private NetWork netWork             = null;  //网络io

    [SerializeField] PlayerController player;
    [SerializeField] Ghost ghost;
    [SerializeField] GameObject[] ghosts;
    [SerializeField] Gargoyle gargoyle;
    [SerializeField] GameObject[] gargoyles;
    [SerializeField] PlayerProjectileSpawner projectileSpawner;
    [SerializeField] PlayerProjectile projectile;
    [SerializeField] GroundSpike groundSpike;
    [SerializeField] GameObject[] groundSpikes;
    [SerializeField] DecelerationTrap decelerationTrap;
    [SerializeField] GameObject[] decelerationTraps;
    [SerializeField] LightingEvent lighting;
    [SerializeField] GameObject[] lights;
    [SerializeField] TreasureChest treasureChest;
    [SerializeField] GameObject[] treasureChests;

    [SerializeField] CopySelection copySelection;
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerRank playerRank;
    [SerializeField] SceneData sceneData;

    private List<string> sceneIntroList = new List<string>();

    public UnityAction reLoadSuccess;

    void Start()
    {
        netWork = gameObject.GetComponent<NetWork>();
        if (null == netWork)
        {
            Debug.Log("获取network fail");
            return;
        }

        projectile.projectileData = new ProjectileData();
    }

    //业务逻辑
    public void PlayerLogin(string ID, string PWD)
    {
        PlayerLoginReq req = new PlayerLoginReq();
        req.PlayerID = ID;
        req.Password = PWD;
        netWork._SendPBMessage(CLIENT_CMD.ClientPlayerloginReq, req);
        Debug.Log("send player login request");
        Debug.Log("login");
    }

    public void PlayerLoginResponse(PlayerLoginRsp rsp)
    {
        if (rsp.Result == 0)
        {
            Debug.Log("player login success");
            playerLogin.LoginSuccess();
            return;
        }
        else if (rsp.Result == -1)
        {
            Debug.Log("Player already online!");
        }
        else if (rsp.Result == -2 || rsp.Result == -3)
        {
            Debug.Log("Player not exist or Player password error!");
        }
        else
        {
            Debug.Log("player login failed:" + rsp.Reason);
        }
    }

    public void PlayerCreateReq(string ID, string PWD, string name)
    {
        PlayerCreateReq req = new PlayerCreateReq();
        req.PlayerID = ID;
        req.Password = PWD;
        req.Name = ByteString.CopyFromUtf8(name);
        netWork._SendPBMessage(CLIENT_CMD.ClientPlayercreateReq, req);
        Debug.Log("send player create request");
    }

    public void playerCreateRsp(PlayerCreateRsp rsp)
    {
        if (0 == rsp.Result)
        {
            Debug.Log("create player success, name:" + rsp.Name.ToStringUtf8());
            playerLogin.RegisterSuccess();
        }
        else
        {
            Debug.Log("create player failed:" + rsp.Reason);
        }
    }

    public void GetSceneIndexReq(string ID)
    {
        SceneGetALLReq req = new SceneGetALLReq();
        req.PlayerID = ID;
        netWork._SendPBMessage(CLIENT_CMD.ClientGetallsceneReq, req);
        Debug.Log("Send Load Scene request");
    }

    public void GetSceneIndexRsp(SceneGetALLRsp rsp)
    {
        //int count = 0;
        foreach (var scene in rsp.SceneMap)
        {
            sceneIntroList.Add(scene.Value.Intro.ToStringUtf8());
        }
        Debug.Log("SceneIntroduceCount :  " + sceneIntroList.Count);

        for (int i = 0; i < sceneIntroList.Count; i++)
        {
            copySelection.sceneIntroduces[i].text = sceneIntroList[i].ToString();
        }

        Debug.Log(rsp.Reason);
    }

    public void GetSceneReq(string ID, int index)
    {
        SceneGetOneReq req = new SceneGetOneReq();
        req.PlayerID = ID;
        req.SceneIndex = index;
        netWork._SendPBMessage(CLIENT_CMD.ClientGetsceneReq, req);
        Debug.Log("Send GetScene request");
    }

    public void GetSceneRsp(SceneGetOneRsp rsp)
    {
        /*********** role ***********/
        if(rsp.Result == 0)
        {
            RolePB rolePB = rsp.Scene.Role;
            //basic
            player.sceneData.playData.playerPosX = rolePB.Base.X;
            player.sceneData.playData.playerPosY = rolePB.Base.Y;
            player.sceneData.playData.playerPosZ = rolePB.Base.Z;
            player.transform.position = new Vector3(player.sceneData.playData.playerPosX, player.sceneData.playData.playerPosY, player.sceneData.playData.playerPosZ);

            //move
            player.sceneData.playData.touchHorizontal = rolePB.Move.Dir.HorizontalDir;
            player.sceneData.playData.touchVertical = rolePB.Move.Dir.VerticalDir;

            player.sceneData.playData.isMoving = rolePB.Move.Speed.IsMoving;
            player.sceneData.playData.moveSpeed = rolePB.Move.Speed.MoveSpeed;

            //health
            player.sceneData.playData.currentHealth = rolePB.Health.Blood.CurBlood;
            player.sceneData.playData.maxHealth = rolePB.Health.Blood.InitialBlood;

            player.sceneData.playData.isDeath = rolePB.Health.Blood.IsDeath;
            player.sceneData.playData.invincible = rolePB.Health.NoBeat.IsNoBeat;
            player.sceneData.playData.invincibleIntervalTime = rolePB.Health.NoBeat.NoBeatIntervalTime;

            //shoot
            player.sceneData.playData.isWeapon = rolePB.Shoot.IsWeapon;
            player.sceneData.playData.shootIntervalTime = rolePB.Shoot.ShootIntervalTime;

            /*********** ghost ***********/
            int ghostSize = rsp.Scene.GhostLists.GhostList.Count;
            for (int i = 0; i < ghostSize; i++)
            {
                //basic
                ghost.ghostData.posX = rsp.Scene.GhostLists.GhostList[i].Base.X;
                ghost.ghostData.posY = rsp.Scene.GhostLists.GhostList[i].Base.Y;
                ghost.ghostData.posZ = rsp.Scene.GhostLists.GhostList[i].Base.Z;
                ghosts[i].transform.position = new Vector3(ghost.ghostData.posX, ghost.ghostData.posY, ghost.ghostData.posZ);

                //state
                ghost.ghostData.currentState = rsp.Scene.GhostLists.GhostList[i].CurStatus.ToString();

                //blood
                ghost.ghostData.isDeath = rsp.Scene.GhostLists.GhostList[i].Blood.IsDeath;
                ghost.ghostData.currentHealth = rsp.Scene.GhostLists.GhostList[i].Blood.CurBlood;
                ghost.ghostData.maxHealth = rsp.Scene.GhostLists.GhostList[i].Blood.InitialBlood;

                //ai
                ghost.ghostData.patrolDistance = rsp.Scene.GhostLists.GhostList[i].PatrolDis;

                //attack
                ghost.ghostData.aliveIntervalTime = rsp.Scene.GhostLists.GhostList[i].AliveIntervalTime;
                ghost.ghostData.hurtNum = rsp.Scene.GhostLists.GhostList[i].HurtNum;
            }

            /*********** gargoyle ***********/
            int gargoyleSize = rsp.Scene.GargoyleLists.GargoyleList.Count;
            for (int i = 0; i < gargoyleSize; i++)
            {
                //basic
                gargoyle.gargoyleData.posX = rsp.Scene.GargoyleLists.GargoyleList[i].Base.X;
                gargoyle.gargoyleData.posY = rsp.Scene.GargoyleLists.GargoyleList[i].Base.Y;
                gargoyle.gargoyleData.posZ = rsp.Scene.GargoyleLists.GargoyleList[i].Base.Z;
                gargoyles[i].transform.position = new Vector3(gargoyle.gargoyleData.posX, gargoyle.gargoyleData.posY, gargoyle.gargoyleData.posZ);

                //ai
                gargoyle.gargoyleData.patrolDistance = rsp.Scene.GargoyleLists.GargoyleList[i].GargoylePatrolDis;

                //attack
                gargoyle.gargoyleData.hurtNum = rsp.Scene.GargoyleLists.GargoyleList[i].HurtNum;
            }

            /*********** bullet ***********/
            //bullet 无需list
            BulletPB bulletPB = rsp.Scene.Bullet;
            projectile.InitProjectileData(bulletPB.BulletSpeed, bulletPB.BulletAttack);

            /*********** groundSpike ***********/
            int groundSpikeSize = rsp.Scene.GroudSpikeLists.GroudSpikeList.Count;
            for (int i = 0; i < groundSpikeSize; i++)
            {
                GroudSpikePB groudSpikePB = rsp.Scene.GroudSpikeLists.GroudSpikeList[i];
                //basic
                groundSpike.spikeData.posX = groudSpikePB.Base.X;
                groundSpike.spikeData.posY = groudSpikePB.Base.Y;
                groundSpike.spikeData.posZ = groudSpikePB.Base.Z;
                groundSpikes[i].transform.position = new Vector3(groundSpike.spikeData.posX, groundSpike.spikeData.posY, groundSpike.spikeData.posZ);

                //damage
                groundSpike.spikeData.damage = groudSpikePB.Hurt.HurtNum;
                groundSpike.spikeData.hurtIntervalTime = groudSpikePB.Hurt.HurtIntervalTime;
            }

            /*********** slowTrap ***********/
            int slowTrapSize = rsp.Scene.SlowTrapLists.SlowTrapList.Count;
            for (int i = 0; i < slowTrapSize; i++)
            {
                SlowTrapPB slowTrapPB = rsp.Scene.SlowTrapLists.SlowTrapList[i];
                //basic
                decelerationTrap.decelerationData.posX = slowTrapPB.Base.X;
                decelerationTrap.decelerationData.posY = slowTrapPB.Base.Y;
                decelerationTrap.decelerationData.posZ = slowTrapPB.Base.Z;
                decelerationTraps[i].transform.position = new Vector3(decelerationTrap.decelerationData.posX, decelerationTrap.decelerationData.posY, decelerationTrap.decelerationData.posZ);

                //slowDown
                decelerationTrap.decelerationData.isSlowDown = slowTrapPB.Status.IsSlowDown;
                decelerationTrap.decelerationData.slowMultiple = slowTrapPB.SlowMultiple;
                decelerationTrap.decelerationData.slowContinueTime = slowTrapPB.SlowContinueTime;
            }

            /*********** light ***********/
            int lightSize = rsp.Scene.LightLists.LightList.Count;
            for (int i = 0; i < lightSize; i++)
            {
                LightPB lightPB = rsp.Scene.LightLists.LightList[i];
                //basic
                lighting.lightingData.posX = lightPB.Base.X;
                lighting.lightingData.posY = lightPB.Base.Y;
                lighting.lightingData.posZ = lightPB.Base.Z;
                lights[i].transform.position = new Vector3(lighting.lightingData.posX, lighting.lightingData.posY, lighting.lightingData.posZ);

                //lighting
                lighting.lightingData.isSwitch = lightPB.Status.IsSwitch;
                lighting.lightingData.switchOffContinueTime = lightPB.SwitchOffContinueTime;
                lighting.lightingData.switchOnContinueTime = lightPB.SwitchOnContinueTime;
                lighting.lightingData.lightingDistance = lightPB.LightingDis;
            }

            /*********** treasure ***********/
            int treasure = rsp.Scene.TreasureLists.TreasureList.Count;
            for (int i = 0; i < treasure; i++)
            {
                TreasurePB treasurePB = rsp.Scene.TreasureLists.TreasureList[i];
                //basic
                treasureChest.chestData.posX = treasurePB.Base.X;
                treasureChest.chestData.posY = treasurePB.Base.Y;
                treasureChest.chestData.posZ = treasurePB.Base.Z;
                treasureChests[i].transform.position = new Vector3(treasureChest.chestData.posX, treasureChest.chestData.posY, treasureChest.chestData.posZ);
            }
            Debug.Log("------------Load New Scene---------");

            //播放倒计时UI
        }
        else if(rsp.Result == 1)
        {
            Debug.Log("断线重连");
            reLoadSuccess?.Invoke();

            RolePB rolePB = rsp.Scene.Role;
            //basic
            player.sceneData.playData.playerPosX = rolePB.Base.X;
            player.sceneData.playData.playerPosY = rolePB.Base.Y;
            player.sceneData.playData.playerPosZ = rolePB.Base.Z;
            player.transform.position = new Vector3(player.sceneData.playData.playerPosX, player.sceneData.playData.playerPosY, player.sceneData.playData.playerPosZ);

            player.sceneData.playData.isMoving = rolePB.Move.Speed.IsMoving;
            player.sceneData.playData.moveSpeed = rolePB.Move.Speed.MoveSpeed;

            //health
            player.sceneData.playData.currentHealth = rolePB.Health.Blood.CurBlood;

            player.sceneData.playData.isDeath = rolePB.Health.Blood.IsDeath;
            player.sceneData.playData.invincible = rolePB.Health.NoBeat.IsNoBeat;

            //shoot
            //player.sceneData.playData.isWeapon = rolePB.Shoot.IsWeapon;

            //Anim
            //((string)player.sceneData.playData.playerAnim) = rolePB.Status.Animation.ToStringUtf8();

            /*********** ghost ***********/
            int ghostSize = rsp.Scene.GhostLists.GhostList.Count;
            for (int i = 0; i < ghostSize; i++)
            {
                //basic
                ghost.ghostData.posX = rsp.Scene.GhostLists.GhostList[i].Base.X;
                ghost.ghostData.posY = rsp.Scene.GhostLists.GhostList[i].Base.Y;
                ghost.ghostData.posZ = rsp.Scene.GhostLists.GhostList[i].Base.Z;
                ghosts[i].transform.position = new Vector3(ghost.ghostData.posX, ghost.ghostData.posY, ghost.ghostData.posZ);

                //state
                ghost.ghostData.currentState = rsp.Scene.GhostLists.GhostList[i].Status.Animation.ToStringUtf8();

                //blood
                ghost.ghostData.isDeath = rsp.Scene.GhostLists.GhostList[i].Blood.IsDeath;
                ghost.ghostData.currentHealth = rsp.Scene.GhostLists.GhostList[i].Blood.CurBlood;

                //ai
                ghost.ghostData.patrolDistance = rsp.Scene.GhostLists.GhostList[i].PatrolDis;
            }

            /*********** groundSpike ***********/
            int groundSpikeSize = rsp.Scene.GroudSpikeLists.GroudSpikeList.Count;
            int randomLength = Random.Range(-3, 3);
            for (int i = 0; i < groundSpikeSize; i++)
            {
                GroudSpikePB groudSpikePB = rsp.Scene.GroudSpikeLists.GroudSpikeList[i];
                //basic
                groundSpike.spikeData.posX = groudSpikePB.Base.X;
                groundSpike.spikeData.posY = groudSpikePB.Base.Y;
                groundSpike.spikeData.posZ = groudSpikePB.Base.Z;
                groundSpikes[i].transform.position = new Vector3(groundSpike.spikeData.posX + randomLength, groundSpike.spikeData.posY, groundSpike.spikeData.posZ + randomLength);

                //damage
                groundSpike.spikeData.isUp = groudSpikePB.Status.IsUp;
                groundSpike.spikeData.curCtuIntervalTime = groudSpikePB.Status.CurCtuInitIntervalTime;
                groundSpike.spikeData.curCtuContinueTime = groudSpikePB.Status.CurCtuInitContinueTime;
            }

            /*********** slowTrap ***********/
            int slowTrapSize = rsp.Scene.SlowTrapLists.SlowTrapList.Count;
            for (int i = 0; i < slowTrapSize; i++)
            {
                SlowTrapPB slowTrapPB = rsp.Scene.SlowTrapLists.SlowTrapList[i];
                //basic
                decelerationTrap.decelerationData.posX = slowTrapPB.Base.X;
                decelerationTrap.decelerationData.posY = slowTrapPB.Base.Y;
                decelerationTrap.decelerationData.posZ = slowTrapPB.Base.Z;
                decelerationTraps[i].transform.position = new Vector3(decelerationTrap.decelerationData.posX, decelerationTrap.decelerationData.posY, decelerationTrap.decelerationData.posZ);

                //slowDown
                decelerationTrap.decelerationData.isSlowDown = slowTrapPB.Status.IsSlowDown;
                decelerationTrap.decelerationData.curCtuSlowTime = slowTrapPB.Status.CurCtuSlowTime;
            }

            /*********** light ***********/
            int lightSize = rsp.Scene.LightLists.LightList.Count;
            for (int i = 0; i < lightSize; i++)
            {
                LightPB lightPB = rsp.Scene.LightLists.LightList[i];
                //basic
                lighting.lightingData.posX = lightPB.Base.X;
                lighting.lightingData.posY = lightPB.Base.Y;
                lighting.lightingData.posZ = lightPB.Base.Z;
                lights[i].transform.position = new Vector3(lighting.lightingData.posX, lighting.lightingData.posY, lighting.lightingData.posZ);

                //lighting
                lighting.lightingData.isSwitch = lightPB.Status.IsSwitch;
                lighting.lightingData.curCtuSwitchOffTime = lightPB.Status.CurCtuSwitchOffTime;
                lighting.lightingData.curCtuSwitchOnTime = lightPB.Status.CurCtuSwitchOnTime;
            }

            /*********** treasure ***********/
            int treasure = rsp.Scene.TreasureLists.TreasureList.Count;
            for (int i = 0; i < treasure; i++)
            {
                TreasurePB treasurePB = rsp.Scene.TreasureLists.TreasureList[i];
                //basic
                treasureChest.chestData.posX = treasurePB.Base.X;
                treasureChest.chestData.posY = treasurePB.Base.Y;
                treasureChest.chestData.posZ = treasurePB.Base.Z;
                treasureChests[i].transform.position = new Vector3(treasureChest.chestData.posX, treasureChest.chestData.posY, treasureChest.chestData.posZ);
            }
            Debug.Log("-----------Load Old Scene---------");

            //todo 关闭尝试连接UI
        }
        Debug.Log(rsp.Reason);
    }

    //结算请求
    public void GetGameOverReq(string ID, int beatNum, int time)
    {
        GameOverReq req = new GameOverReq();
        req.PlayerID = ID;
        req.CurKillNum = beatNum;
        req.TimeConsume = time;
        netWork._SendPBMessage(CLIENT_CMD.ClientGameoverReq, req);
        Debug.Log("Send GameOver request");
    }

    public void GetGameOverRsp(GameOverRsp rsp)
    {
        if (rsp.Result == 0)
        {
            UIManager.Instance.totalScoreText.text = rsp.GetAllScore.ToString();
            Debug.Log(rsp.GetAllScore);
            Debug.Log("gameover rsp:" + rsp.PlayerID + " " + rsp.Reason + " " + rsp.PlayerName + " " + "totalScore :" + rsp.GetAllScore);
            Debug.Log("Player Game Over");
        }
        else if (rsp.Result == -1)
        {
            Debug.Log("Player is not login");
        }
        else if(rsp.Result == -2)
        {
            Debug.Log("Game over: " + rsp.Reason);
        }
    }
    
    //排行榜请求
    public void GetScoreListReq(string ID)
    {
        ScoreListReq req = new ScoreListReq();
        req.PlayerID = ID;
        netWork._SendPBMessage(CLIENT_CMD.ClientGetscorelistReq, req);
        Debug.Log("Send get score list req");
    }

    public void GetScoreListRsp(ScoreListRsp rsp)
    {
        if(rsp.Result == 0)
        {
            Debug.Log(rsp.ScoreList.Count);
            for (int i = 0; i < rsp.ScoreList.Count; i++)
            {
                if (playerLogin.isPlayerLogin)
                {
                    playerRank.Load(rsp.ScoreList[i].PlayerName.ToStringUtf8(), rsp.ScoreList[i].Score);
                }
                Debug.Log("Get score list success");
            }
        }
        else if(rsp.Result == -1 || rsp.Result == -2)
        {
            Debug.Log("Get score list lose");
            Debug.Log(rsp.Reason);
        }
    }

    public void GetSceneSyncRsp(SceneSyncRsp rsp)
    {
        if(rsp.Result == 0)
        {
            playerLogin.inputNumber = rsp.PlayerID;
            Debug.Log(rsp.Reason);
            Debug.Log("Get Scene Sync Success");
        }
        else if(rsp.Result == -1)
        {
            Debug.Log(rsp.Reason);
        }
        else if(rsp.Result == -2)
        {
            Debug.Log(rsp.Reason);
        }
        else if(rsp.Result == -3)
        {
            Debug.Log(rsp.Reason);
        }
    }
}
