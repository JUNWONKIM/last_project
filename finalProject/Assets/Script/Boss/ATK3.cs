using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK3 : MonoBehaviour
{
    void Start()
    {
        // 30�� �ڿ� ������Ʈ�� �ı�
        Destroy(gameObject, 30.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹�ϸ� ȭ�� ȿ�� ����
        if (other.CompareTag("Player"))
        {
            // PlayerBurn ��ũ��Ʈ�� ã�Ƽ� ȭ�� ȿ�� ����
            PlayerBurn playerBurn = other.GetComponent<PlayerBurn>();
            if (playerBurn != null)
            {
                playerBurn.ApplyBurn();
            }
        }
    }
}
