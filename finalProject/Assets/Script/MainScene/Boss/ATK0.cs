using System.Collections;
using UnityEngine;

public class ATK0 : MonoBehaviour
{
    public float damage = 100f; // ���߹��� �÷��̾�� �� ���ط�
    public float explosionDelay = 2f; // ���� �� ������������ �ð�

    private void Start()
    {
        Destroy(gameObject, explosionDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ��
        PlayerHP playerHP = other.GetComponent<PlayerHP>();
        if (playerHP != null)
        {
            // �÷��̾�� ���ظ� ��
            playerHP.TakeDamage(damage);

          
        }
    }

 
}
