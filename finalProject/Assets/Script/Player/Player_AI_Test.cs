using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI_Test : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�

    private Vector3 targetPosition; // ��ǥ ����

    void Start()
    {
        // ��ȯ�Ǿ��� �� ���� ���� ������ �̵��ϵ��� ����
        SetTargetPosition();
    }

    void Update()
    {
        // ��ǥ �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ��ǥ ������ �����ϸ� ���ο� ��ǥ ���� ����
        if (transform.position == targetPosition)
        {
            SetTargetPosition();
        }

        // �÷��̾ �� ���͸� ���ϵ��� ����
        LookAtEnemy();
    }

    void SetTargetPosition()
    {
        // ��� �� ���͵� ���� ���� �� �Ÿ��� ã�Ƽ� �ش� �������� �̵�
        Vector3 fleeDirection = FindFleeDirection();
        targetPosition = transform.position + fleeDirection * 10f; // �̵� �Ÿ��� �����Ͽ� ��ǥ ��ġ ����
    }

    Vector3 FindFleeDirection()
    {
        Vector3 fleeDirection = Vector3.zero;
        float farthestDistance = 0f;
        Vector3 currentPosition = transform.position;

        // ��� ���� ã�Ƽ� ���� �� �Ÿ��� ���� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != gameObject) // ���� �ڱ� �ڽ��� ����
            {
                float distanceToEnemy = Vector3.Distance(currentPosition, enemy.transform.position);
                if (distanceToEnemy > farthestDistance)
                {
                    farthestDistance = distanceToEnemy;
                    fleeDirection = currentPosition - enemy.transform.position;
                }
            }
        }

        return fleeDirection.normalized;
    }

    void LookAtEnemy()
    {
        // ���� ����� ���� ã�� �÷��̾ �ش� ���� ���ϵ��� ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            transform.LookAt(closestEnemy.transform);
        }
    }
}
