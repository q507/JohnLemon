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

    //TODO:��ɫ���������
    public void Heal(float healAmount)
    {

    }

    //����
    public void TakeDamage(float damage)
    {
        if(playerData.invincible)
        {
            Debug.Log(11);
            return;
        }

        Debug.Log("����");
        playerData.currentHealth -= damage;
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth, 0f, playerData.maxHealth);
        UIManager.Instance.UpdateHealth(playerData.currentHealth / playerData.maxHealth);

        _HandleDeath();
    }

    //���������¼�
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
