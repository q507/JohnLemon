using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public ProjectileData projectileData;

    private void Start()
    {
        projectileData = new ProjectileData();
    }
    
    public void InitProjectileData(float speed, float damage)
    {
        projectileData.projectileSpeed = speed;

        projectileData.projectileDamage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            other.gameObject.GetComponent<Ghost>().Hurt(projectileDamage);
            Destroy(gameObject);
        }
    }
}
