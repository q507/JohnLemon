using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpike : MonoBehaviour
{
    public int ID;

    private Animator animator;

    [SerializeField] PlayerController player;
    [SerializeField] SceneData sceneData;

    public GroundSpikeData spikeData;

    private float hurtTime = 0;
    private float groundSpikeUpTime = 0;
    private float groundSpikeContinueTime = 0;
    private float tempTime = 0;

    public bool isUp = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spikeData = new GroundSpikeData();
        ID = spikeData.ID;

        sceneData.groundSpikeDataDic.Add(ID, spikeData);
    }

    private void Update()
    {
        if (sceneData.groundSpikeDataDic.TryGetValue(ID, out GroundSpikeData value))
        {
            sceneData.groundSpikeDataDic.Remove(ID);
            spikeData = value;
            sceneData.groundSpikeDataDic.Add(ID, spikeData);
        }
        else
        {
            GroundSpikeData spikeData = new GroundSpikeData();
            spikeData.ID = ID;
            spikeData.posX = transform.position.x;
            spikeData.posY = transform.position.y;
            spikeData.posZ = transform.position.z;
            spikeData.isUp = this.spikeData.isUp;
            spikeData.curCtuIntervalTime = this.spikeData.curCtuIntervalTime;
            spikeData.curCtuContinueTime = this.spikeData.curCtuContinueTime;
            sceneData.groundSpikeDataDic.Add(ID, spikeData);
        }

        hurtTime += Time.deltaTime;

        //抬起或落下间隔时间
        tempTime = Time.deltaTime;
        groundSpikeContinueTime = tempTime;
        groundSpikeUpTime += groundSpikeContinueTime;

        _GroundSpike();
    }

    private void _GroundSpike()
    {
        isUp = false;

        if (groundSpikeUpTime - spikeData.intervalTime > 0)
        {
            animator.SetBool("GroundSpikeUp", false);
            animator.SetBool("GroundSpikeDown", true);

            if (groundSpikeUpTime - spikeData.intervalTime > spikeData.continueTime && groundSpikeUpTime - spikeData.intervalTime < 2 * spikeData.continueTime)
            {
                isUp = true;

                animator.SetBool("GroundSpikeUp", true);
                animator.SetBool("GroundSpikeDown", false);
                groundSpikeUpTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (hurtTime - spikeData.hurtIntervalTime > 0)
            {
                player.Hurt(spikeData.damage);
                hurtTime = 0;
            }
        }
    }
}
