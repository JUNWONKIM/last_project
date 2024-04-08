using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1; // �ִ� ü��
    private int currentHealth; // ���� ü��

    void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ ����
    }

    // �������� �Ծ��� �� ȣ��Ǵ� �Լ�
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // ��������ŭ ü�� ����

        if (currentHealth <= 0)
        {
            Die(); // ü���� 0 �����̸� ��� ó��
        }
    }

    void Die()
    {
        // �� ������Ʈ�� �ı�
        Destroy(gameObject);
    }
}
