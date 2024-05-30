using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk_2_1 : MonoBehaviour
{
    public static Player_Atk_2_1 Instance;
    public float damageAmount = 1f; // �Ѿ��� ������ ������ ��

    private bool isIncrease = false;

    public float lifetime = 0.5f; // ������Ʈ�� ������������ �ð�

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ������Ʈ�� lifetime �ð� �Ŀ� ������� ��
        StartCoroutine(DestroyAfterLifetime());
    }

    private IEnumerator DestroyAfterLifetime()
    {
        // lifetime �ð� ���� ���
        yield return new WaitForSeconds(lifetime);

        // ���� ������Ʈ�� �ı�
        Destroy(gameObject);
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

        }
    }

    public void IncreaseDamage(float amount)
    {
        damageAmount += amount;
        Debug.Log("Damage increased to: " + damageAmount);
    }
}
