using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter_2 : MonoBehaviour
{
    public static Player_Shooter_2 instance;

    public GameObject objectToSpawn; // ��ȯ�� ������Ʈ ������
    public float spawnInterval = 3f; // ��ȯ ����
    public float spawnRadius = 20f; // ��ȯ ���� ������
    public float fixedYPosition = 2.5f; // ������ Y �� ��ġ
    public int projectilesPerFire = 1; // �� ���� �߻��� �߻�ü ��

    private float lastSpawnTime; // ������ ��ȯ �ð�

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // ���� �������� ������Ʈ ��ȯ
        if (Time.time - lastSpawnTime > spawnInterval)
        {
            SpawnObject();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnObject()
    {
        for (int i = 0; i < projectilesPerFire; i++)
        {
            // �÷��̾� �ֺ� ���� ���� ������ ������ ��ġ ���
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = fixedYPosition; // ���̸� ������ Y ������ ����

            // ������Ʈ ��ȯ
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectilesPerFire += amount;
        Debug.Log("Projectile count increased, now firing: " + projectilesPerFire + " projectiles per shot.");
    }
}
