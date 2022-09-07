using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    private bool isDeath = false;

    public UnityAction<float, GameObject> onDamage;

    public UnityAction onDie;

    PlayerData playerData;

    private void Start()
    {
        playerData = new PlayerData();

        playerData.currentHealth = playerData.maxHealth;
    }

    //TODO:角色捡道具治疗
    public void Heal(float healAmount)
    {

    }

    //受伤
    public void TakeDamage(float damage)
    {
        if(playerData.invincible)
        {
            Debug.Log(11);
            return;
        }

        Debug.Log("受伤");
        playerData.currentHealth -= damage;
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth, 0f, playerData.maxHealth);
        UIManager.Instance.UpdateHealth(playerData.currentHealth / playerData.maxHealth);

        _HandleDeath();
    }

    //处理死亡事件
    private void _HandleDeath()
    {
        if (isDeath)
        {
            return;
        }

        if(playerData.currentHealth <= 0f)
        {
            if(onDie != null)
            {
                isDeath = true;
                onDie.Invoke();
            }
        }
    }
}
