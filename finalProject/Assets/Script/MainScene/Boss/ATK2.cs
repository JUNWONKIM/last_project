using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK2 : MonoBehaviour
{
    public float speed = 10.0f; // ATK2�� �̵� �ӵ�
    public float damage = 50.0f; // ATK2�� �÷��̾�� �� ������

    private Transform player; // �÷��̾��� Transform

    void Start()
    {
        // �÷��̾� ������Ʈ�� ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // �÷��̾ ���� �̵�
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾��� ��� �������� ��
        if (other.CompareTag("Player"))
        {
            PlayerHP playerHP = other.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            // �浹 �� ATK2 ������Ʈ�� �ı�
            Destroy(gameObject);
        }
    }
}
