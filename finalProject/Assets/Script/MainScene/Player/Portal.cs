using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float minDistance = 200.0f; // ��Ż�κ����� �ּ� �Ÿ�

    void OnTriggerEnter(Collider other)
    {
        // ��� ������Ʈ�� ��Ż�� �浹�� ��
        MoveObjectToRandomLocation(other.transform);
    }

    void MoveObjectToRandomLocation(Transform objectTransform)
    {
        // ��Ż ��ġ���� ��� ���� ��ġ�� ���
        Vector3 randomDirection = Random.insideUnitSphere * minDistance;
        randomDirection.y = 0; // y ���� 0���� �����Ͽ� ���鿡 ��ġ�ϵ��� ��

        // ���ο� ��ġ�� ��Ż���� minDistance �̻� ������ ������ ���
        Vector3 newPosition = transform.position + randomDirection;

        // ������Ʈ�� ��ġ�� ���ο� ��ġ�� ����
        objectTransform.position = newPosition;
    }
}
