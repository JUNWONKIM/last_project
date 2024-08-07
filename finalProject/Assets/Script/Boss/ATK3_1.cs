using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK3_1 : MonoBehaviour
{
    public float damagePerSecond = 10.0f; // 1�ʸ��� ���� ������
    private Transform playerTransform;
    private PlayerHP playerHP;
    private bool isDamaging = false;

    void Start()
    {
        // �÷��̾� ������Ʈ�� ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHP = player.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                StartCoroutine(DamagePlayer());
            }
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // ����Ʈ�� �÷��̾��� �߹ؿ� ��ġ��Ŵ
            transform.position = playerTransform.position;
        }
    }

    IEnumerator DamagePlayer()
    {
        while (true)
        {
            if (playerHP != null)
            {
                playerHP.TakeDamage(damagePerSecond);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
