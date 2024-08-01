using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ�� ������ ����
    public float speed = 5.0f; // ������ �̵��� �ӵ�
    public float rotationSpeed = 5.0f; // ������ ȸ���� �ӵ�

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ���� �ٰ�����
        Vector3 direction = (player.position - transform.position).normalized;

        // �÷��̾ �ٶ󺸱�
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // �÷��̾ ���� �̵��ϱ�
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
