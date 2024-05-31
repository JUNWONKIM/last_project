using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Atk_1 : MonoBehaviour
{
    public static Player_Atk_1 Instance;

    public float damageAmount = 1f; // �Ѿ��� ������ ������ ��

    

    void Awake()
    {
        Instance = this;
    }



   void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �±װ� "Creature"�� ���
        if (other.gameObject.CompareTag("Creature"))
        {
            // �浹�� ��ü�� HP�� ���ҽ�Ŵ
            CreatureHealth enemyHealth = other.gameObject.GetComponent<CreatureHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
               
             }

            // �Ѿ��� �ı�
            Destroy(gameObject);
        }
    }


    public void IncreaseDamage(float amount)
    {
        
        damageAmount += amount;
        Debug.Log("����ü ������ : " + damageAmount);
        
    }

}
