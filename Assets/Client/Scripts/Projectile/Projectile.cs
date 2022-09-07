using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float projectileSpeed = 0;
    [SerializeField]
    protected Vector3 projectileDirection;
    [SerializeField]
    protected float projectileDamage = 0;

    protected void OnEnable()
    {
        StartCoroutine(ProjectileLaunch());
    }

    protected void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected virtual IEnumerator ProjectileLaunch()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(projectileSpeed * projectileDirection * Time.deltaTime);
            yield return null;
        }
    }
}
