using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk_1: MonoBehaviour
{
    public int damageAmount = 1; // �Ѿ��� ������ ������ ��
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �±װ� "Creature"�� ���
        if (collision.gameObject.CompareTag("Creature"))
        {
            // �浹�� ��ü�� HP�� ���ҽ�Ŵ
            CreatureHealth enemyHealth = collision.gameObject.GetComponent<CreatureHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            // �Ѿ��� �ı�
            Destroy(gameObject);
        }
    }

}
