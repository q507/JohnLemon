using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProjectilePool", menuName = "Pool/PlayerProjectilePool")]
public class PlayerProjectilePoolSO : PoolSO<PlayerProjectile>
{
    [SerializeField] PlayerProjectile[] playerProjectPrefabs;

    public override PlayerProjectile Create()
    {
        int index = Random.Range(0, playerProjectPrefabs.Length);
        PlayerProjectile ins = Instantiate(playerProjectPrefabs[index]);
        return ins;
    }
}
