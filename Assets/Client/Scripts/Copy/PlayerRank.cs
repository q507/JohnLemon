using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNode
{
    public int Rank { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }

    public PlayerNode(int rank, string name, int score)
    {
        Rank = rank;
        Name = name;
        Score = score;
    }
    
    public PlayerNode()
    {

    }
}

public class PlayerRank : MonoBehaviour
{
    [SerializeField] GameObject row;
    private List<PlayerNode> players = new List<PlayerNode>();

    private bool isFirstInsert = false;

    public void CreateNewLine(PlayerNode node)
    {
        GameObject playerRow = Instantiate(row);
        playerRow.transform.SetParent(transform);
        playerRow.transform.GetChild(0).GetComponent<Text>().text = node.Rank.ToString();
        playerRow.transform.GetChild(1).GetComponent<Text>().text = node.Name.ToString();
        playerRow.transform.GetChild(2).GetComponent<Text>().text = node.Score.ToString();
    }

    public void UpdateRank(List<PlayerNode> players)
    {
        /*if(transform.childCount == 0 || players == null)
        {
            return;
        }*/

        for (int i = 0; i < transform.childCount; i++)
        {
            //Destroy(transform.GetChild(i).gameObject);
            Transform row = transform.GetChild(i);
            row.transform.GetChild(0).GetComponent<Text>().text = players[i].Rank.ToString();
            row.transform.GetChild(1).GetComponent<Text>().text = players[i].Name.ToString();
            row.transform.GetChild(2).GetComponent<Text>().text = players[i].Score.ToString();
        }
        /*for (int i = 0; i < players.Count; i++)
        {
            CreateNewLine(players[i]);
        }*/
    }

    public void Load(string name, int score)
    {
        Debug.Log("开始生成");
        if(name != null)
        {
            //玩家登录昵称与游戏管理器计算出的积分
            PlayerNode playerNode = new PlayerNode(1, name, score);
            CreateNewLine(playerNode);
            if (isFirstInsert)
            {
                players.Add(playerNode);
                isFirstInsert = false;
            }
            else
            {
                int rankIndex = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    if(playerNode.Score > players[i].Score)
                    {
                        rankIndex = i;
                        //更新排名
                        playerNode.Rank = i + 1;
                        players.Insert(rankIndex, playerNode);
                        rankIndex = i + 1;
                        break;
                    }
                }
                if(rankIndex == 0)
                {
                    playerNode.Rank = players.Count + 1;
                    players.Insert(players.Count, playerNode);
                }
                else
                {
                    for (int i = rankIndex; i < players.Count; i++)
                    {
                        players[i].Rank = players[i - 1].Rank + 1;
                    }
                }
            }
            UpdateRank(players);
        }
        //gameObject.SetActive(false);
    }
}
