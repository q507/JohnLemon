using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ToJsonGameObject : MonoBehaviour
{
	[Serializable]
	public struct Dir
	{
		public float h;
		public float v;
	}

	[Serializable]
	public struct Speed
	{
		public bool isMoving;
		public float moveSpeed;
	}


	[Serializable]
	public struct Move
	{
		public Dir dir;
		public Speed speed;
	}

	[Serializable]
	public struct Blood
	{
		public bool isDeath;
		public float curBlood;
		public float initialBlood;
	}

	[Serializable]
	public struct NoBeat
	{
		public bool isNoBeat;
		public float noBeatIntervalTime;
	}


	[Serializable]
	public struct Health
	{
		public NoBeat noBeat;
		public Blood blood;
	}

	[SerializeField]
	public enum PlayerAnim
	{
		Hit,
		Death,
		Idle,
		Fight,
		BoxOpen,
		SwitchOn,
		SwitchOff,
		Walk,
		Fight_Die,
		Fight_Hit,
		Fight_Idle,
		Fight_Attack,
		Fight_BoxOpen,
		Fight_SwitchOn,
		Fight_SwitchOff,
		Fight_Walk
	}

	[Serializable]
	public struct Shoot
	{
		public bool isWeapon;
		public float shootIntervalTime;
	}

	[Serializable]
	public struct RoleStatus
	{
		public PlayerAnim playerAnim;
		public float curCtuNoBeatTime;
	}

	[Serializable]
	public enum GhostState
	{
		Walk,
		Pursuit,
		Shower
	}

	[Serializable]
	public struct GhostStatus
	{
		public GhostState animation;
		public float curCtuRebirthTime;
	}

	[Serializable]
	public struct GroundSpikeStatus
	{
		public bool isUp;
		public float curCtuInitIntervalTime;
		public float curCtuInitContinueTime;
	}


	[Serializable]
	public struct Hurt
	{
		public float hurtIntervalTime;
		public float hurtNum;
	}

	[Serializable]
	public struct SlowTrapStatus
	{
		public bool isSlowDown;
		public float curCtuSlowTime;
	}

	[Serializable]
	public struct LightStatus
	{
		public bool isSwitch;
		public float curCtuSwitchOffTime;
		public float curCtuSwitchOnTime;
	}


	[Serializable]
	public struct Role
	{
		public Move move;
		public Health health;
		public Shoot shoot;
		public RoleStatus roleStatus;
	}


	[Serializable]
	public struct Ghost
	{
		public float aliveInrTime;
		public float hurtNum;
		public float patrolDis;
		public Blood blood;
		public GhostStatus status;
	}


	[Serializable]
	public struct GhostList
	{
		public int count;
		public List<Ghost> ghostlist;
	}


	[Serializable]
	public struct Gargoyle
	{
		public float hurtNum;
		public float patrolDis;
	}

	[Serializable]
	public struct GargoyleList
	{
		public int count;
		public List<Gargoyle> gargoylelist;
	}


	[Serializable]
	public struct Bullet
	{
		public float bulletSpeed;
		public float bulletAttack;
	}


	[Serializable]
	public struct GroundSpike
	{
		public float initIntervalTime;
		public float initContinueTime;
		public Hurt hurt;
		public GroundSpikeStatus status;
	}


	[Serializable]
	public struct GroundSpikeList
	{
		public int count;
		public List<GroundSpike> groundSpikelist;
	}

	[Serializable]
	public struct SlowTrap
	{
		public float slowMultiple;
		public float slowContinueTime;
		public SlowTrapStatus status;
	}


	[Serializable]
	public struct SlowTrapList
	{
		public int count;
		public List<SlowTrap> slowTraplist;
	}


	[Serializable]
	public struct Light
	{
		public float switchOffContinueTime;
		public float switchOnContinueTime;
		public float lightingDistance;
		public LightStatus status;
	}


	[Serializable]
	public struct LightList
	{
		public int count;
		public List<Light> lightlist;
	}

	[Serializable]
	public struct Scene
	{
		public int sceneID;
		public int limitedTime;
		public string sceneIntro;


		public Role role;
		public GhostList ghostList;
		public GargoyleList gargoyleList;
		public Bullet bullet;
		public GroundSpikeList groundSpikeList;
		public SlowTrapList slowTrapList;
		public LightList lightList;
	}

	[SerializeField]
	List<Scene> sceneList = new List<Scene>();


	public void ExportSceneListJson()
    {
		string targetpath = EditorUtility.SaveFilePanel("Save json file to", "", "SceneList", "json");
		if (targetpath.Length != 0)
		{
			FileStream filestream = File.Open(targetpath, FileMode.Create);
			StreamWriter jsonfile = new StreamWriter(filestream);

			GameObject obj = GameObject.Find("JsonTool");
			//Scene scene        = SceneManager.GetActiveScene ();
			JSONObject rootobj = JSONObject.obj;

            //int root_count = scene.rootCount;
            //GameObject[] goArray = scene.GetRootGameObjects ();
            //for (int i = 0; i < root_count; ++i) 
            //{
            //	GameObject go = goArray [i];
            //	ToJsonGameObject.ExportGameObject (rootobj, go);
            //}

            ToJsonGameObject.ToJsonSceneList(rootobj, sceneList);


            jsonfile.Write(rootobj.Print(true));
			jsonfile.Close();
			filestream.Close();
		}
	}
	
	
	
	//场景列表
	public static void ToJsonSceneList(JSONObject rootJsonObj, List<Scene> sceneList)
    {
		JSONObject sceneJsonObj;
		JSONObject sceneListJsonObj = JSONObject.arr;

		for (int i = 0;i < sceneList.Count;i++)
        {
			sceneJsonObj = JSONObject.obj;
			sceneJsonObj.AddField("SceneID",	 sceneList[i].sceneID);
			sceneJsonObj.AddField("LimitedTime", sceneList[i].limitedTime);
			sceneJsonObj.AddField("SceneIntro",  sceneList[i].sceneIntro);
			ToJsonScene(sceneJsonObj, sceneList[i]);
			sceneListJsonObj.Add(sceneJsonObj);
		}
		rootJsonObj.AddField("SceneList", sceneListJsonObj);
	}



	//场景
	static void ToJsonScene(JSONObject parentJsonObj, Scene scene)
    {

		GameObject   roleObj		    = GameObject.FindGameObjectWithTag("Player");
		GameObject[] ghostListObj       = GameObject.FindGameObjectsWithTag("Ghost");
		GameObject[] gargoyleListObj    = GameObject.FindGameObjectsWithTag("Gargoyle");
		GameObject[] groundSpikeListObj = GameObject.FindGameObjectsWithTag("GroundSpike");
		GameObject[] slowTrapListObj    = GameObject.FindGameObjectsWithTag("SlowTrap");
		GameObject[] lightListObj       = GameObject.FindGameObjectsWithTag("Light");
		GameObject[] treasureListObj    = GameObject.FindGameObjectsWithTag("Treasure");
		
		JSONObject sceneJsonObj;

		JSONObject ghostJsonObj;
		JSONObject ghostListJsonObj;

		JSONObject gargoyleJsonObj;
		JSONObject gargoyleListJsonObj;

		JSONObject groundSpikeJsonObj;
		JSONObject groundSpikeListJsonObj;

		JSONObject slowTrapJsonObj;
		JSONObject slowTrapListJsonObj;

		JSONObject lightJsonObj;
		JSONObject lightListJsonObj;

		JSONObject treasureJsonObj;
		JSONObject treasureListJsonObj;

		if (null == roleObj)
		{
			Debug.LogError("roleObj empty");
			return;
		}


		if (null == ghostListObj)
		{
			Debug.LogError("ghostListObj empty");
			return;
		}


		if (null == gargoyleListObj)
		{
			Debug.LogError("gargoyleListObj empty");
			return;
		}


		if (null == groundSpikeListObj)
		{
			Debug.LogError("groundSpikeListObj empty");
			return;
		}


		if (null == slowTrapListObj)
		{
			Debug.LogError("slowTrapListObj empty");
			return;
		}


		if (null == lightListObj)
		{
			Debug.LogError("lightListObj empty");
			return;
		}


		if (null == treasureListObj)
		{
			Debug.LogError("treasureListObj empty");
			return;
		}

		sceneJsonObj = JSONObject.obj;
		//角色
		ToJsonPlayerController(sceneJsonObj, roleObj, scene.role);

		//幽灵
		ghostJsonObj     = JSONObject.obj;
		ghostListJsonObj = JSONObject.arr;
		ghostJsonObj.AddField("ghostNum",  scene.ghostList.count);
		ToJsonGhostList(ghostListJsonObj, ghostListObj, scene.ghostList.ghostlist);
		ghostJsonObj.AddField("ghostList", ghostListJsonObj);
		sceneJsonObj.AddField("Ghost",     ghostJsonObj);

		//石像怪
		gargoyleJsonObj     = JSONObject.obj;
		gargoyleListJsonObj = JSONObject.arr;
		gargoyleJsonObj.AddField("gargoyleNum",  scene.gargoyleList.count);
		ToJsonGargoyle(gargoyleListJsonObj, gargoyleListObj, scene.gargoyleList.gargoylelist);
		gargoyleJsonObj.AddField("gargoyleList", gargoyleListJsonObj);
		sceneJsonObj.AddField("Gargoyle",		 gargoyleJsonObj);

		//子弹
		ToJsonBullet(sceneJsonObj, scene.bullet);

		//地刺
		groundSpikeJsonObj     = JSONObject.obj;
		groundSpikeListJsonObj = JSONObject.arr;
		groundSpikeJsonObj.AddField("groundSpike",  scene.groundSpikeList.count);
		ToJsonGroundSpike(groundSpikeListJsonObj,   groundSpikeListObj, scene.groundSpikeList.groundSpikelist);
		groundSpikeJsonObj.AddField("gargoyleList", groundSpikeListJsonObj);
		sceneJsonObj.AddField("GroundSpike",        groundSpikeJsonObj);

		//减速陷阱
		slowTrapJsonObj     = JSONObject.obj;
		slowTrapListJsonObj = JSONObject.arr;
		slowTrapJsonObj.AddField("slowTrapNum", scene.slowTrapList.count);
		ToJsonSlowTrap(slowTrapListJsonObj, slowTrapListObj, scene.slowTrapList.slowTraplist);
		slowTrapJsonObj.AddField("slowTrapList", slowTrapListJsonObj);
		sceneJsonObj.AddField("SlowTrap",		 slowTrapJsonObj);


		//灯
		lightJsonObj	 = JSONObject.obj;
		lightListJsonObj = JSONObject.arr;
		lightJsonObj.AddField("lightNum", scene.lightList.count);
		ToJsonLight(lightListJsonObj, lightListObj, scene.lightList.lightlist);
		lightJsonObj.AddField("lightList", lightListJsonObj);
		sceneJsonObj.AddField("Light",	   lightJsonObj);


		//宝箱
		treasureJsonObj		= JSONObject.obj;
		treasureListJsonObj = JSONObject.arr;
		treasureJsonObj.AddField("treasureNum", scene.slowTrapList.count);
		ToJsonTreasure(treasureListJsonObj, treasureListObj);
		treasureJsonObj.AddField("treasureList", treasureListJsonObj);
		sceneJsonObj.AddField("Treasure", treasureJsonObj);

		parentJsonObj.AddField("Scene", sceneJsonObj);
	}

	//角色
	static void ToJsonPlayerController(JSONObject parentJsonObj, GameObject obj, Role role)
    {
		JSONObject jsonRole   = JSONObject.obj;
							  
		JSONObject jsonMove   = JSONObject.obj;
		JSONObject jsonDir    = JSONObject.obj;
		JSONObject jsonSpeed  = JSONObject.obj;
							  
		JSONObject jsonHealth = JSONObject.obj;
		JSONObject jsonNoBeat = JSONObject.obj;
		JSONObject jsonBlood  = JSONObject.obj;
							  
		JSONObject jsonShoot  = JSONObject.obj;

		JSONObject jsonStatus = JSONObject.obj;

		JSONObject translateobj;

		Move		move       = role.move;
		Health		health     = role.health;
		Shoot		shoot      = role.shoot;
		RoleStatus	status	   = role.roleStatus;

		jsonRole.AddField("name", "role");

		translateobj = ToJsonCommon.ToJsonObjectVector3(obj.transform.localPosition);

		jsonMove.AddField("Pos", translateobj);

		jsonDir.AddField("h", move.dir.h);
		jsonDir.AddField("v", move.dir.v);
		jsonMove.AddField("Dir", jsonDir);

		jsonSpeed.AddField("isMoving",  move.speed.isMoving);
		jsonSpeed.AddField("moveSpeed", move.speed.moveSpeed);
		jsonMove.AddField("Speed", jsonSpeed);

		jsonRole.AddField("Move",  jsonMove);


		jsonNoBeat.AddField("isNoBeat",           health.noBeat.isNoBeat);
		jsonNoBeat.AddField("noBeatIntervalTime", health.noBeat.noBeatIntervalTime);
		jsonHealth.AddField("NoBeat", jsonNoBeat);

		jsonBlood.AddField("isDeath",      health.blood.isDeath);
		jsonBlood.AddField("curBlood",     health.blood.curBlood);
		jsonBlood.AddField("initialBlood", health.blood.initialBlood);
		jsonHealth.AddField("Blood", jsonBlood);

		jsonRole.AddField("Health", jsonHealth);


		jsonShoot.AddField("isWeapon",          shoot.isWeapon);
		jsonShoot.AddField("shootIntervalTime", shoot.shootIntervalTime);

		jsonRole.AddField("Shoot", jsonShoot);


		jsonStatus.AddField("animation",        (int)status.playerAnim);
		jsonStatus.AddField("curCtuNoBeatTime", status.curCtuNoBeatTime);

		jsonRole.AddField("Status", jsonStatus);

		parentJsonObj.AddField("RoleData", jsonRole);
	}


	//幽灵
	static void ToJsonGhostList(JSONObject parentJsonArr, GameObject[] objArr, List<Ghost> objDataArr)
    {
		JSONObject jsonGhost;
		JSONObject jsonBlood;
		JSONObject jsonStatus;

		float       aliveInrTime;
		float       hurtNum;
		float       patrolDis;

		Blood		blood;
		GhostStatus status;

		for (int i = 0;i < objArr.Length;i++)
        {
			jsonGhost  = JSONObject.obj;
			jsonBlood  = JSONObject.obj;
			jsonStatus = JSONObject.obj;
			JSONObject translateobj;
			jsonGhost.AddField("name", "ghost");
			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);

			aliveInrTime = objDataArr[i].aliveInrTime;
			hurtNum		 = objDataArr[i].hurtNum;
			patrolDis	 = objDataArr[i].patrolDis;
			blood		 = objDataArr[i].blood;
			status		 = objDataArr[i].status;

			jsonGhost.AddField("Pos",      translateobj);
		    jsonGhost.AddField("hurtNum",  hurtNum);
		    jsonGhost.AddField("aliveIntervalTime", aliveInrTime);
		    jsonGhost.AddField("patrolDistance",    patrolDis);
		    
		    jsonBlood.AddField("isDeath",      blood.isDeath);
		    jsonBlood.AddField("curBlood",     blood.curBlood);
		    jsonBlood.AddField("initialBlood", blood.initialBlood);
		    jsonGhost.AddField("Blood",        jsonBlood);
		    
		    jsonStatus.AddField("animation",         (int)status.animation);
		    jsonStatus.AddField("curCtuRebirthTime", status.curCtuRebirthTime);
		    jsonGhost.AddField("Status", jsonStatus);

			parentJsonArr.Add(jsonGhost);
		}
	}



	//石像怪
	static void ToJsonGargoyle(JSONObject parentJsonArr, GameObject[] objArr, List<Gargoyle> gargoyleDataArr)
    {
		JSONObject jsonGargoyle;
		JSONObject translateobj;

		for (int i = 0;i < objArr.Length;i++)
        {
			jsonGargoyle = JSONObject.obj;
			jsonGargoyle.AddField("name", "gargoyle");

			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);

			jsonGargoyle.AddField("Pos",			translateobj);
			jsonGargoyle.AddField("patrolDistance", gargoyleDataArr[i].patrolDis);
			jsonGargoyle.AddField("hurtNum",		gargoyleDataArr[i].hurtNum);

			parentJsonArr.Add(jsonGargoyle);
		}
	}


	//子弹
	static void ToJsonBullet(JSONObject parentJsonObj, Bullet bulletData)
    {
		JSONObject jsonBullet = JSONObject.obj;

		jsonBullet.AddField("name", "bullet");

		jsonBullet.AddField("bulletSpeed",  bulletData.bulletSpeed);
		jsonBullet.AddField("bulletAttack", bulletData.bulletAttack);

		parentJsonObj.AddField("Bullet", jsonBullet);
	}


	//地刺
	static void ToJsonGroundSpike(JSONObject parentJsonArr, GameObject[] objArr,List<GroundSpike> objDataArr)
    {
		JSONObject jsonGroundSpike;
		JSONObject jsonHurt;
		JSONObject jsonStatus;
		JSONObject translateobj;

		float initIntervalTime;
		float initContinueTime;

		Hurt			  hurt;
		GroundSpikeStatus status;

		for (int i = 0;i < objArr.Length; i++)
        {
			jsonGroundSpike = JSONObject.obj;
			jsonHurt        = JSONObject.obj;
			jsonStatus      = JSONObject.obj;

			jsonGroundSpike.AddField("name", "groundSpike");
			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);
			jsonGroundSpike.AddField("Pos", translateobj);

			initIntervalTime = objDataArr[i].initIntervalTime;
			initContinueTime = objDataArr[i].initContinueTime;
			hurt   = objDataArr[i].hurt;
			status = objDataArr[i].status;

			jsonGroundSpike.AddField("initIntervalTime", initIntervalTime);
			jsonGroundSpike.AddField("initContinueTime", initContinueTime);

			jsonHurt.AddField("hurtIntervalTime", hurt.hurtIntervalTime);
			jsonHurt.AddField("hurtNum",     hurt.hurtNum);
			jsonGroundSpike.AddField("Hurt", jsonHurt);

			jsonStatus.AddField("isUp", status.isUp);
			jsonStatus.AddField("curCtuInitIntervalTime", status.curCtuInitIntervalTime);
			jsonStatus.AddField("curCtuInitContinueTime", status.curCtuInitContinueTime);
			jsonGroundSpike.AddField("Status", jsonStatus);

			parentJsonArr.Add(jsonGroundSpike);
		}
	}

	//减速陷阱
	static void ToJsonSlowTrap(JSONObject parentJsonArr, GameObject[] objArr, List<SlowTrap> objDataArr)
	{
		JSONObject jsonSlowTrap;
		JSONObject jsonStatus;
		JSONObject translateobj;

		float slowMultiple;
		float slowContinueTime;

		SlowTrapStatus status;

		for (int i = 0; i < objArr.Length; i++)
		{
			jsonSlowTrap = JSONObject.obj;
			jsonStatus = JSONObject.obj;

			jsonSlowTrap.AddField("name", "slowTrap");

			slowMultiple = objDataArr[i].slowMultiple;
			slowContinueTime = objDataArr[i].slowContinueTime;
			status = objDataArr[i].status;

			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);
			jsonSlowTrap.AddField("Pos", translateobj);
			jsonSlowTrap.AddField("slowMultiple", slowMultiple);
			jsonSlowTrap.AddField("slowContinueTime", slowContinueTime);

			jsonStatus.AddField("isSlowDown", status.isSlowDown);
			jsonStatus.AddField("curCtuSlowTime", status.curCtuSlowTime);
			jsonSlowTrap.AddField("Status", jsonStatus);

			parentJsonArr.Add(jsonSlowTrap);
		}
	}


	//灯
	static void ToJsonLight(JSONObject parentJsonArr, GameObject[] objArr, List<Light> objDataArr)
	{
		JSONObject jsonLigh;
		JSONObject jsonStatus;
		JSONObject translateobj;

		float       switchOffContinueTime;
		float       switchOnContinueTime;
		float       lightingDistance;

		LightStatus status;

		for (int i = 0; i < objArr.Length; i++)
        {
			jsonLigh   = JSONObject.obj;
			jsonStatus = JSONObject.obj;

			jsonLigh.AddField("name", "light");
			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);

			switchOffContinueTime = objDataArr[i].switchOffContinueTime;
			switchOnContinueTime  = objDataArr[i].switchOnContinueTime;
			lightingDistance      = objDataArr[i].lightingDistance;
			status				  = objDataArr[i].status;

			jsonLigh.AddField("Pos", translateobj);
			jsonLigh.AddField("switchOffContinueTime", switchOffContinueTime);
			jsonLigh.AddField("switchOnContinueTime",  switchOnContinueTime);
			jsonLigh.AddField("lightingDistance",	   lightingDistance);

			jsonStatus.AddField("isSwitch",			   status.isSwitch);
			jsonStatus.AddField("curCtuSwitchOffTime", status.curCtuSwitchOffTime);
			jsonStatus.AddField("curCtuSwitchOnTime",  status.curCtuSwitchOnTime);
			jsonLigh.AddField("Status", jsonStatus);

			parentJsonArr.Add(jsonLigh);
		}
	}


	//宝箱
	static void ToJsonTreasure(JSONObject parentJsonArr, GameObject[] objArr)
    {
		JSONObject jsonTreasure;
		JSONObject translateobj;

		for (int i = 0;i < objArr.Length; i++)
        {
			jsonTreasure = JSONObject.obj;

			jsonTreasure.AddField("name", "treasure");

			translateobj = ToJsonCommon.ToJsonObjectVector3(objArr[i].transform.localPosition);

			jsonTreasure.AddField("Pos",  translateobj);

			parentJsonArr.Add(jsonTreasure);
		}
	}

}
