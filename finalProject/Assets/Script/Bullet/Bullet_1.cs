using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_1 : MonoBehaviour
{
    public int damageAmount = 1; // �Ѿ��� ������ ������ ��
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �±װ� "Enemy"�� ���
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // �浹�� ��ü�� HP�� ���ҽ�Ŵ
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }

            // �Ѿ��� �ı�
            Destroy(gameObject);
        }
    }

}
