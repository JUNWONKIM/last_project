using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter : MonoBehaviour
{
    public GameObject projectilePrefab; // �߻�ü �������� �Ҵ��� ����
    public float fireInterval = 1f; // �߻� ����
    public float detectionRange = 100f; // ���� Ž���� ����
    public float projectileSpeed = 100f;
    private float lastFireTime; // ������ �߻� �ð�

    private LineRenderer detectionRangeVisual; // �� Ž�� ������ �ð������� ǥ���� ���� ������

    void Start()
    {
        // LineRenderer ������Ʈ �߰�
        detectionRangeVisual = gameObject.AddComponent<LineRenderer>();

        // ���� ������ ����
        detectionRangeVisual.material = new Material(Shader.Find("Sprites/Default"));
        detectionRangeVisual.startColor = Color.red;
        detectionRangeVisual.endColor = Color.red;
        detectionRangeVisual.startWidth = 0.1f;
        detectionRangeVisual.endWidth = 0.1f;
        detectionRangeVisual.positionCount = 37; // ���� ������ ����
    }

    void Update()
    {
        // ���� �������� ���� ����� ���� Ž���ϰ� �߻�ü�� �߻�
        if (Time.time - lastFireTime > fireInterval)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }

        // Ž�� ������ �÷��̾��� �߽��� ����ٴϵ��� ������Ʈ
        UpdateDetectionRange();
    }

    void FireProjectile()
    {
        // ���� ����� ���� Ž��
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        GameObject[] creatures2 = GameObject.FindGameObjectsWithTag("Creature_2");
        List<GameObject> allCreatures = new List<GameObject>();
        allCreatures.AddRange(creatures);
        allCreatures.AddRange(creatures2);

        GameObject closestCreature = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject creature in allCreatures)
        {
            float distance = Vector3.Distance(transform.position, creature.transform.position);
            if (distance < closestDistance && distance <= detectionRange)
            {
                closestCreature = creature;
                closestDistance = distance;
            }
        }

        // �߻�ü�� �߻��� ���� �ִ� ��� �߻�
        if (closestCreature != null)
        {
            Vector3 targetDirection = closestCreature.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, rotation);

            // �߻�ü�� �̵� �ӵ� �ο�
            Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                // �߻��� ���� �������� �߻�ü�� �̵���Ŵ
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }
        }
    }


    // Ž�� ������ �÷��̾��� �߽��� ����ٴϵ��� ������Ʈ�ϴ� �Լ�
    private void UpdateDetectionRange()
    {
        // �� ����� �� ����
        Vector3[] points = new Vector3[37];
        for (int i = 0; i < 37; i++)
        {
            float angle = i * Mathf.PI * 2f / 36f;
            points[i] = transform.position + new Vector3(Mathf.Sin(angle) * detectionRange, 0f, Mathf.Cos(angle) * detectionRange);
        }

        // ���� �������� �� ����
        detectionRangeVisual.SetPositions(points);
    }
}
