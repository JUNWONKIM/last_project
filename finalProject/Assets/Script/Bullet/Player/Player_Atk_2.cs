using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk_2 : MonoBehaviour
{
    public static Player_Atk_2 Instance;

    public float lifetime = 1.5f; // ����Ʈ�� ������������ �ð�
    public float damageAmount = 1f; // ���߷� ���� ������
    public SphereCollider explosionCollider; // ���� ���� �ݶ��̴�

    private bool hasExploded = false; // ������ �̹� �߻��ߴ��� ����

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // ���� ���� �ݶ��̴��� ��Ȱ��ȭ
        explosionCollider.enabled = false;

        // �ݶ��̴��� Ȱ��ȭ�ϰ� �������� �� �� ����Ʈ�� �ı�
        StartCoroutine(ActivateColliderAndDealDamage());
    }

    private IEnumerator ActivateColliderAndDealDamage()
    {
        // lifetime - 0.2f �ð� ���� ���
        yield return new WaitForSeconds(lifetime - 0.2f);

        // ���� ���� �ݶ��̴��� Ȱ��ȭ
        explosionCollider.enabled = true;
        hasExploded = true;

        // ���� ���� ���� ���鿡�� �������� ��
        DealDamage();

        // ��� ��� �� �ݶ��̴��� ��Ȱ��ȭ
        yield return new WaitForSeconds(0.2f);
        explosionCollider.enabled = false;

        // ������Ʈ �ı�
        Destroy(gameObject);
    }

    void DealDamage()
    {
        // ���� ȿ���� Ʈ���� ���� ���� ��� �ݶ��̴��� ������
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionCollider.radius);
        foreach (var hitCollider in hitColliders)
        {
            // �浹�� ��ü�� �±װ� "Creature"�� ���
            if (hitCollider.CompareTag("Creature"))
            {
                // �浹�� ��ü�� HP�� ���ҽ�Ŵ
                CreatureHealth enemyHealth = hitCollider.GetComponent<CreatureHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �±װ� "Creature"�� ���
        if (other.CompareTag("Creature") && hasExploded)
        {
            // �浹�� ��ü�� HP�� ���ҽ�Ŵ
            CreatureHealth enemyHealth = other.GetComponent<CreatureHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }

    public void IncreaseDamage(float amount)
    {
        damageAmount += amount;
        Debug.Log("��ź ������ : " + damageAmount);
    }
}
