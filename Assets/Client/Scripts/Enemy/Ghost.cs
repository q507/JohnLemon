using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// ������Ϊ
/// </summary>
public enum GhostState
{
    Walk,
    Pursuit,
    Shower
}

public class Ghost : MonoBehaviour
{
    public int ID;

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] GameObject[] ghostPatrolPoints;
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] PlayerController player;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] SceneData sceneData;

    public GhostData ghostData;

    private GhostState ghostState;
    public GhostState currentAnim;

    public List<Vector3> pathNodes = new List<Vector3>();

    private int patrolPointRandomIndex = 0;
    private int patrolPointRandomTempIndex;
    private int patrolPointRandomNewIndex;

    private Vector3 auxiliaryEmissionRayPosition = new Vector3(0, 1f, 0);

    public GhostState GhostState { get => ghostState;
        set {
            ghostState = value;
            switch (ghostState)
            {
                case GhostState.Walk:
                    animator.SetBool((currentAnim = GhostState.Walk).ToString(), true);
                    animator.SetBool((currentAnim = GhostState.Shower).ToString(), false);
                    break;
                case GhostState.Pursuit:
                    animator.SetBool((currentAnim = GhostState.Walk).ToString(), true);
                    animator.SetBool((currentAnim = GhostState.Shower).ToString(), false);
                    break;
                case GhostState.Shower:
                    animator.SetBool((currentAnim = GhostState.Shower).ToString(), true);
                    animator.SetBool((currentAnim = GhostState.Walk).ToString(), false);
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        //SceneData.ghostDataList = new List<GhostData>();

        ghostData = new GhostData();
        ID = ghostData.ID;

        sceneData.ghostDataDic.Add(ID, ghostData);
    }

    private void Update()
    {
        if (sceneData.ghostDataDic.TryGetValue(ID, out GhostData value))
        {
            sceneData.ghostDataDic.Remove(ID);
            ghostData = value;
            sceneData.ghostDataDic.Add(ID, ghostData);
        }
        else
        {
            GhostData ghostData = new GhostData();
            ghostData.ID = ID;
            ghostData.posX = transform.position.x;
            ghostData.posY = transform.position.y;
            ghostData.posZ = transform.position.z;
            ghostData.isDeath = this.ghostData.isDeath;
            ghostData.currentHealth = this.ghostData.currentHealth;
            ghostData.currentAnim = this.ghostData.currentAnim;
            ghostData.curCtuRebirthTime = this.ghostData.curCtuRebirthTime;
            sceneData.ghostDataDic.Add(ID, ghostData);
        }

        switch (ghostState)
        {
            case GhostState.Walk:
                {
                    if (ghostData.isPursuit)
                    {
                        return;
                    }

                    //���ﳯ���ж����ɫ�ľ��룬���������߼�
                    _GhostPatrolRoad();
                    break;
                }
            case GhostState.Pursuit:
                {
                    _PursuitTarget();
                    break;
                }
            case GhostState.Shower:
                {
                    if (ghostData.isPursuit)
                    {
                        return;
                    }

                    StartCoroutine(GhostShowerEvent());
                    break;
                }
            default:
                break;
        }
    }

    private void _CreateNodes()
    {
        for (int i = 0; i < pathNodes.Count; i++)
        {

        }
    }

    //Ѳ��
    private void _GhostPatrolRoad()
    {
        if (Vector3.Distance(transform.position, ghostPatrolPoints[patrolPointRandomIndex].transform.position) > 0.5f)
        {
            //ִ��Ѳ�ߵ�ͬʱ�ᴥ��ԭ��ϴ���¼�
            navMeshAgent.SetDestination(ghostPatrolPoints[patrolPointRandomIndex].transform.position);
            StartCoroutine(GhostWalkEvent());
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < ghostData.patrolDistance)
        {
            ghostData.isPursuit = true;
            ghostState = GhostState.Pursuit;
            navMeshAgent.isStopped = false;
        }
        else
        {
            //����·���㣬������µ�·�������ϴ��ظ����������¸���
            patrolPointRandomTempIndex = patrolPointRandomIndex;
            patrolPointRandomNewIndex = Random.Range(0, ghostPatrolPoints.Length);
            patrolPointRandomIndex = patrolPointRandomNewIndex;
        }
    }

    //׷��
    private void _PursuitTarget()
    {
        /*//�ӹ�������ķ���һ���������ڼ����ң�ģ�������۾�
        if (Physics.Raycast(transform.position + auxiliaryEmissionRayPosition, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, ghostPatrolDistance, playerLayer))
        {
            takeDamege = false;
            if (Vector3.Distance(hitInfo.point, player.transform.position) < ghostPatrolDistance)
            {
                //�����￴�����ʱ����������1���˺�
                if (!takeDamege)
                {
                    takeDamege = true;
                    player.Hurt(1f);
                }
            }
        }*/
        navMeshAgent.SetDestination(player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Hurt(ghostData.hurtNum);
        }
    }

    public void Hurt(float damage)
    {
        Debug.Log("����");
        ghostData.currentHealth -= damage;

        if(ghostData.currentHealth <= 0)
        {
            UIManager.Instance.BeatCount(1);
            Destroy(gameObject);
            //transform.gameObject.SetActive(false);
            StartCoroutine(GhostRebirthEvent());
        }
    }

    IEnumerator GhostWalkEvent()
    {
        if (ghostData.isPursuit)
        {
            yield break;
        }
        
        //Ѳ�߹�����Ѳ��ʮ���ԭ��ϴ��
        yield return new WaitForSeconds(8f);
        if (ghostState == GhostState.Shower)
        {
            yield break;
        }
        navMeshAgent.isStopped = true;
        ghostState = GhostState.Shower;
    }

    IEnumerator GhostShowerEvent()
    {
        if (ghostData.isPursuit)
        {
            yield break;
        }

        //ϴ�������ָ�Ѳ��״̬
        yield return new WaitForSeconds(2f);
        if (ghostState == GhostState.Walk)
        {
            yield break;
        }
        navMeshAgent.isStopped = false;
        ghostState = GhostState.Walk;
    }

    IEnumerator GhostRebirthEvent()
    {
        Debug.Log("׼����������");
        yield return new WaitForSeconds(ghostData.aliveIntervalTime);
        Instantiate(ghostPrefab, player.transform);
    }
}
