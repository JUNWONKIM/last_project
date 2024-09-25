using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    public GameObject portalPrefab; // ������ ��Ż ������
    public float distanceToSpawnPortal = 40.0f; // ��Ż ���� �Ÿ��� �����ϴ� ����
    public float portalOffset = 20.0f; // ��Ż ���� ��ġ�� �������� �����ϴ� ����
    public float portalHeight = 15.0f; // ��Ż�� y ��ġ�� �����ϴ� ����

    private GameObject[] portals; // ������ ��Ż�� ������ �迭
    private bool portalSpawned = false; // ��Ż�� �̹� �����Ǿ����� ���θ� �����ϴ� �÷���
    private GameObject boss; // ������ GameObject�� ������ ����

    void Start()
    {
        // ��Ż�� ������ �迭�� �ʱ�ȭ (8���� ��Ż ����)
        portals = new GameObject[8];

        // �±׸� �̿��� ���� ã��
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    void Update()
    {
        // ���� ������Ʈ�� ���������� ã��, ������ ������ ������Ʈ�� �������� ����
        if (boss == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            return;
        }

        // �������� �Ÿ� Ȯ��
        float distanceToBoss = Vector3.Distance(transform.position, boss.transform.position);

        // �������� �Ÿ��� distanceToSpawnPortal ������ ��, ��Ż�� �������� ���� ��쿡�� ��Ż ����
        if (distanceToBoss <= distanceToSpawnPortal && !portalSpawned)
        {
            SpawnPortals();
        }
        // ������ �Ÿ� ������ ������ ��Ż�� ����
        else if (distanceToBoss > distanceToSpawnPortal && portalSpawned)
        {
            DestroyPortals();
        }
        else if (portalSpawned)
        {
            // ��Ż�� ��ġ�� �������� �Ÿ��� �����Ͽ� ������Ʈ
            UpdatePortalPositions();
        }
    }

    void SpawnPortals()
    {
        // 8 ���� ���͸� ���� (�������� + �밢��)
        Vector3[] directions = new Vector3[] {
            transform.forward,                // ����
            -transform.forward,               // ����
            transform.right,                  // ����
            -transform.right,                 // ����
            (transform.forward + transform.right).normalized,    // �ϵ���
            (transform.forward - transform.right).normalized,    // �ϼ���
            (-transform.forward + transform.right).normalized,   // ������
            (-transform.forward - transform.right).normalized    // ������
        };

        // �� ���⿡ �´� ȸ������ ����
        Quaternion[] rotations = new Quaternion[] {
            Quaternion.LookRotation(transform.forward),                // ������ �ٶ󺸴� ȸ��
            Quaternion.LookRotation(-transform.forward),               // ������ �ٶ󺸴� ȸ��
            Quaternion.LookRotation(transform.right),                  // ������ �ٶ󺸴� ȸ��
            Quaternion.LookRotation(-transform.right),                 // ������ �ٶ󺸴� ȸ��
            Quaternion.LookRotation((transform.forward + transform.right).normalized),    // �ϵ����� �ٶ󺸴� ȸ��
            Quaternion.LookRotation((transform.forward - transform.right).normalized),    // �ϼ����� �ٶ󺸴� ȸ��
            Quaternion.LookRotation((-transform.forward + transform.right).normalized),   // �������� �ٶ󺸴� ȸ��
            Quaternion.LookRotation((-transform.forward - transform.right).normalized)    // �������� �ٶ󺸴� ȸ��
        };

        for (int i = 0; i < directions.Length; i++)
        {
            // ��Ż�� ��ġ�� ���
            Vector3 portalPosition = transform.position + directions[i] * portalOffset;
            portalPosition.y = portalHeight;

            // ��Ż�� �����ϰ�, ������ ȸ�������� ȸ��
            portals[i] = Instantiate(portalPrefab, portalPosition, rotations[i]);
        }

        // ��Ż�� �����Ǿ����� ǥ��
        portalSpawned = true;
    }

    void DestroyPortals()
    {
        // ������ ��Ż�� ����
        for (int i = 0; i < portals.Length; i++)
        {
            if (portals[i] != null)
            {
                Destroy(portals[i]);
            }
        }

        // ��Ż�� �����Ǿ����� ǥ��
        portalSpawned = false;
    }

    void UpdatePortalPositions()
    {
        // ��Ż�� ��ġ�� �������� �Ÿ��� �����Ͽ� ������Ʈ
        Vector3[] directions = new Vector3[] {
            transform.forward,                // ����
            -transform.forward,               // ����
            transform.right,                  // ����
            -transform.right,                 // ����
            (transform.forward + transform.right).normalized,    // �ϵ���
            (transform.forward - transform.right).normalized,    // �ϼ���
            (-transform.forward + transform.right).normalized,   // ������
            (-transform.forward - transform.right).normalized    // ������
        };

        for (int i = 0; i < portals.Length; i++)
        {
            if (portals[i] != null)
            {
                // ��Ż�� ���ο� ��ġ�� ���
                Vector3 portalPosition = transform.position + directions[i] * portalOffset;
                portalPosition.y = portalHeight;

                // ��Ż�� ��ġ�� ������Ʈ
                portals[i].transform.position = portalPosition;
                portals[i].transform.rotation = Quaternion.LookRotation(directions[i]);
            }
        }
    }
}
