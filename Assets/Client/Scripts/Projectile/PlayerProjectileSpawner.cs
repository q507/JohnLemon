using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileSpawner : MonoBehaviour
{
    [SerializeField] PlayerProjectilePoolSO playerProjectilePool;
    [SerializeField] GameObject shootPoint;

    public void SpawnProjectile()
    {
        PlayerProjectile projectile = playerProjectilePool.Create();
        projectile.transform.position = shootPoint.transform.position;
        projectile.transform.rotation = shootPoint.transform.rotation;
    }
}
