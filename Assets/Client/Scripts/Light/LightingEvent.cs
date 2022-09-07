using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingEvent : MonoBehaviour
{
    public int ID;

    [SerializeField] PlayerController player;
    [SerializeField] GameObject[] ghost;
    [SerializeField] GameObject[] spotLight;
    [SerializeField] SceneData sceneData;
    private Animator animator;

    public LightingData lightingData;

    RectTransform[] switchOn = new RectTransform[15];
    RectTransform[] switchOff = new RectTransform[15];

    private bool isSwitchOn = true;
    private bool isSwitchOff = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        lightingData = new LightingData();
        ID = lightingData.ID;

        sceneData.lightingDataDic.Add(ID, lightingData);

        _LightingEvent();
        _InitSwitchText();

        isSwitchOn = !lightingData.isSwitch;
        isSwitchOff = lightingData.isSwitch;
    }

    void Update()
    {
        if (sceneData.lightingDataDic.TryGetValue(ID, out LightingData value))
        {
            sceneData.lightingDataDic.Remove(ID);
            lightingData = value;
            sceneData.lightingDataDic.Add(ID, lightingData);
        }
        else
        {
            LightingData lightingData = new LightingData();
            lightingData.ID = ID;
            lightingData.posX = transform.position.x;
            lightingData.posY = transform.position.y;
            lightingData.posZ = transform.position.z;
            lightingData.isSwitch = this.lightingData.isSwitch;
            lightingData.curCtuSwitchOffTime = this.lightingData.curCtuSwitchOffTime;
            lightingData.curCtuSwitchOnTime = this.lightingData.curCtuSwitchOnTime;
            sceneData.lightingDataDic.Add(ID, lightingData);
        }

        TimeManager.Instance().Update();
        _SwitchOnOrOff();
    }

    //遍历所有的开关灯提示文本，并将开灯提示设置为默认提示文本
    private void _InitSwitchText()
    {
        for (int i = 0; i < spotLight.Length; i++)
        {
            switchOn[i] = transform.GetChild(0) as RectTransform;
            switchOff[i] = transform.GetChild(1) as RectTransform;

            //常规状态下为开灯提示
            switchOn[i].gameObject.SetActive(true);
            switchOff[i].gameObject.SetActive(false);
        }
    }

    //开关灯
    private void _SwitchOnOrOff()
    {
        if (animator == null)
        {
            return;
        }

        for (int i = 0; i < spotLight.Length; i++)
        {
            //开灯
            if ((Vector3.Distance(player.transform.position, switchOn[i].transform.position) < 3f) && switchOn[i].gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("开灯");

                    switchOn[i].gameObject.SetActive(false);
                    switchOff[i].gameObject.SetActive(true);

                    if (isSwitchOn)
                    {
                        isSwitchOff = true;
                        isSwitchOn = false;

                        TimeManager.Instance().AddInterval(_Lighting, 0f);
                    }
                }
            }
            //关灯
            else if ((Vector3.Distance(player.transform.position, switchOff[i].transform.position) < 3f) && switchOff[i].gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Debug.Log("关灯");

                    switchOff[i].gameObject.SetActive(false);
                    switchOn[i].gameObject.SetActive(true);

                    if (isSwitchOff)
                    {
                        isSwitchOn = true;
                        isSwitchOff = false;

                        TimeManager.Instance().AddInterval(_Dark, 0f);
                    }
                }
            }
        }
    }

    private void _LightingEvent()
    {
        //每15秒亮一次灯
        TimeManager.Instance().AddInterval(_Lighting, lightingData.switchOffContinueTime);
    }

    private void _Lighting()
    {
        if(animator == null)
        {
            return;
        }

        animator.SetBool("Light", true);

        for (int i = 0; i < spotLight.Length; i++)
        {
            switchOn[i].gameObject.SetActive(false);
            switchOff[i].gameObject.SetActive(true);
        }

        //遍历所有怪物，使其照到灯光消失
        for (int i = 0; i < ghost.Length; i++)
        {
            if(ghost[i] == null)
            {
                return;
            }

            if (Vector3.Distance(ghost[i].transform.position, transform.position) < lightingData.lightingDistance)
            {
                ghost[i].gameObject.SetActive(false);
            }
        }

        if (isSwitchOn && !isSwitchOff)
        {
            isSwitchOn = false;
            isSwitchOff = true;

            //亮10秒自动熄灭
            TimeManager.Instance().AddInterval(_Dark, lightingData.switchOnContinueTime);

            //灯灭3秒后怪物重生
            TimeManager.Instance().AddInterval(_GhostAppear, lightingData.switchOnContinueTime + 3f);
        }
    }

    private void _GhostAppear()
    {
        for (int i = 0; i < ghost.Length; i++)
        {
            if(ghost[i] == null)
            {
                return;
            }

            ghost[i].gameObject.SetActive(true);
        }
    }

    private void _Dark()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool("Light", false);

        for (int i = 0; i < spotLight.Length; i++)
        {
            switchOff[i].gameObject.SetActive(false);
            switchOn[i].gameObject.SetActive(true);
        }

        if (isSwitchOff && !isSwitchOn)
        {
            isSwitchOff = false;
            isSwitchOn = true;

            TimeManager.Instance().AddInterval(_Lighting, lightingData.switchOffContinueTime);
        }
    }
}
