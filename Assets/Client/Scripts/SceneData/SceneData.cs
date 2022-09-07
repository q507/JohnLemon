using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCCamp;
using Google.Protobuf;

public class SceneData : MonoBehaviour
{
    public PlayerData playData = new PlayerData();
    public Dictionary<int, GhostData> ghostDataDic = new Dictionary<int, GhostData>();
    public Dictionary<int, GroundSpikeData> groundSpikeDataDic = new Dictionary<int, GroundSpikeData>();
    public Dictionary<int, LightingData> lightingDataDic = new Dictionary<int, LightingData>();
    public Dictionary<int, DecelerationTrapData> decelerationDataDic = new Dictionary<int, DecelerationTrapData>();

    [SerializeField] LoginAndRegister playerLogin;
    [SerializeField] PlayerController playerC;
    [SerializeField] NetWork netWork;

    private string playerAnim;

    private float sceneSyncIntervalTime = 3f;
    private float sceneSyncTime = 0;

    public bool isGameStart = false;

    void Start()
    {
        playData = new PlayerData();
        Dictionary<int, GhostData> ghostDataDic = new Dictionary<int, GhostData>();
        Dictionary<int, GroundSpikeData> groundSpikeDataDic = new Dictionary<int, GroundSpikeData>();
        Dictionary<int, LightingData> lightingDataDic = new Dictionary<int, LightingData>();
        Dictionary<int, DecelerationTrapData> decelerationDataDic = new Dictionary<int, DecelerationTrapData>();
    }

    void Update()
    {
        playerAnim = playData.playerAnim.ToString();

        //每三秒发一次同步请求
        sceneSyncTime += Time.deltaTime;
        if(sceneSyncTime - sceneSyncIntervalTime > 0)
        {
            if (isGameStart)
            {
                GetSceneSyncReq(playerLogin.inputNumber, playData, ghostDataDic, groundSpikeDataDic, lightingDataDic, decelerationDataDic);
            }
            sceneSyncTime = 0;
        }
    }

    //场景同步请求
    public void GetSceneSyncReq(string ID, PlayerData player, Dictionary<int, GhostData> ghostDataDic, Dictionary<int, GroundSpikeData> groundSpikeDic, Dictionary<int, LightingData> lightingDic, Dictionary<int, DecelerationTrapData> decelerationData)
    {
        SceneSyncReq req = new SceneSyncReq();
        req.PlayerID = ID;

        /*********** Role ***********/
        //Pos
        req.SceneSync = new SceneSyncData();

        req.SceneSync.Role = new RolePB();
        RolePB role = req.SceneSync.Role;
        role.Base = new BasePB();

        BasePB roleBase = role.Base;
        roleBase.X = playerC.transform.position.x;
        roleBase.Y = playerC.transform.position.y;
        roleBase.Z = playerC.transform.position.z;

        role.Move = new MovePB();
        MovePB roleMove = role.Move;
        roleMove.Speed = new SpeedPB();
        SpeedPB roleSpeed = roleMove.Speed;

        //Move
        roleSpeed.IsMoving = player.isMoving;
        roleSpeed.MoveSpeed = player.moveSpeed;

        role.Health = new HealthPB();
        HealthPB roleHealth = role.Health;
        roleHealth.Blood = new BloodPB();
        BloodPB roleBlood = roleHealth.Blood;
        roleHealth.NoBeat = new NoBeatPB();
        NoBeatPB noBeatPB = roleHealth.NoBeat;

        //Health
        roleBlood.IsDeath = player.isDeath;
        roleBlood.CurBlood = player.currentHealth;
        noBeatPB.IsNoBeat = player.invincible;

        role.Shoot = new ShootPB();
        ShootPB roleShoot = role.Shoot;
        //Weapon
        roleShoot.IsWeapon = player.isWeapon;

        role.Status = new RoleStatusPB();
        RoleStatusPB roleStatus = role.Status;
        roleStatus.Animation = ByteString.CopyFromUtf8(playerAnim);

        /*********** Ghost **********/
        req.SceneSync.GhostLists = new GhostListPB();
        GhostListPB ghostListPB = req.SceneSync.GhostLists;
        ghostListPB.Count = req.SceneSync.GhostLists.GhostList.Count;

        foreach (var ghost in ghostDataDic)
        {
            GhostPB ghostPB = new GhostPB();

            ghostPB.Base = new BasePB();
            BasePB ghostBase = ghostPB.Base;
            ghostPB.Status = new GhostStatusPB();
            GhostStatusPB ghostStatusPB = ghostPB.Status;
            ghostPB.Blood = new BloodPB();
            BloodPB bloodPB = ghostPB.Blood;

            //Pos
            ghostBase.X = ghost.Value.posX;
            ghostBase.Y = ghost.Value.posY;
            ghostBase.Z = ghost.Value.posZ;

            //Health
            bloodPB.IsDeath = ghost.Value.isDeath;
            bloodPB.CurBlood = ghost.Value.currentHealth;

            //State
            ghostPB.PatrolDis = ghost.Value.patrolDistance;
            ghostStatusPB.Animation = ByteString.CopyFromUtf8(ghost.Value.currentAnim.ToString());
            ghostStatusPB.CurCtuRebirthTime = ghost.Value.curCtuRebirthTime;

            //Attack
            ghostPB.HurtNum = ghost.Value.hurtNum;

            ghostListPB.GhostList.Add(ghostPB);
        }

        /*********** GroundSpike **********/
        req.SceneSync.GroudSpikeLists = new GroudSpikeListPB();
        GroudSpikeListPB groundSpikeListPB = req.SceneSync.GroudSpikeLists;
        groundSpikeListPB.Count = req.SceneSync.GroudSpikeLists.GroudSpikeList.Count;

        foreach (var groundSpike in groundSpikeDataDic)
        {
            GroudSpikePB groundSpikePB = new GroudSpikePB();

            groundSpikePB.Base = new BasePB();
            BasePB groundSpikeBase = groundSpikePB.Base;
            groundSpikePB.Status = new GroudSpikeStatusPB();
            GroudSpikeStatusPB groundSpikeStatusPB = groundSpikePB.Status;

            //Pos
            groundSpikeBase.X = groundSpike.Value.posX;
            groundSpikeBase.Y = groundSpike.Value.posY;
            groundSpikeBase.Z = groundSpike.Value.posZ;

            //Hurt
            groundSpikeStatusPB.IsUp = groundSpike.Value.isUp;
            groundSpikeStatusPB.CurCtuInitIntervalTime = groundSpike.Value.curCtuIntervalTime;
            groundSpikeStatusPB.CurCtuInitContinueTime = groundSpike.Value.curCtuContinueTime;

            groundSpikeListPB.GroudSpikeList.Add(groundSpikePB);
        }

        /*********** Light **********/
        req.SceneSync.LightLists = new LightListPB();
        LightListPB lightListPB = req.SceneSync.LightLists;
        lightListPB.Count = req.SceneSync.LightLists.LightList.Count;

        foreach (var light in lightingDataDic)
        {
            LightPB lightPB = new LightPB();
            lightPB.Base = new BasePB();
            BasePB lightBase = lightPB.Base;
            lightPB.Status = new LightStatusPB();
            LightStatusPB lightStatusPB = lightPB.Status;

            //Pos
            lightBase.X = light.Value.posX;
            lightBase.Y = light.Value.posY;
            lightBase.Z = light.Value.posZ;

            //Status
            lightStatusPB.IsSwitch = light.Value.isSwitch;
            lightStatusPB.CurCtuSwitchOffTime = light.Value.curCtuSwitchOffTime;
            lightStatusPB.CurCtuSwitchOnTime = light.Value.curCtuSwitchOnTime;

            lightListPB.LightList.Add(lightPB);
        }

        /*********** DecelerationTrap **********/
        req.SceneSync.SlowTrapLists = new SlowTrapListPB();
        SlowTrapListPB slowTrapListPB = req.SceneSync.SlowTrapLists;
        slowTrapListPB.Count = req.SceneSync.SlowTrapLists.SlowTrapList.Count;

        foreach (var deceleration in decelerationDataDic)
        {
            SlowTrapPB slowTrapPB = new SlowTrapPB();
            slowTrapPB.Base = new BasePB();
            BasePB slowTrapBase = slowTrapPB.Base;
            slowTrapPB.Status = new SlowTrapStatusPB();
            SlowTrapStatusPB slowTrapStatusPB = slowTrapPB.Status;

            //Pos
            slowTrapBase.X = deceleration.Value.posX;
            slowTrapBase.Y = deceleration.Value.posY;
            slowTrapBase.Z = deceleration.Value.posZ;

            //Status
            slowTrapStatusPB.IsSlowDown = deceleration.Value.isSlowDown;
            slowTrapStatusPB.CurCtuSlowTime = deceleration.Value.curCtuSlowTime;

            slowTrapListPB.SlowTrapList.Add(slowTrapPB);
        }
        netWork._SendPBMessage(CLIENT_CMD.ClientScenesyncReq, req);
        Debug.Log("Send Get Scene Sync Request");
    }
}
