using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter_1 : MonoBehaviour
{
    public static Player_Shooter_1 instance;

    public GameObject projectilePrefab; // �߻�ü �������� �Ҵ��� ����
    public float fireInterval = 1f; // �߻� ����
    public float fireIntervalSlowMultiplier = 2f; // Slow ȿ�� �� �߻� ���� ���
    public float detectionRange = 100f; // ���� Ž���� ����
    public float projectileSpeed = 100f;
    public int projectilesPerFire = 1; // �� ���� �߻��� �߻�ü ��
    public float burstInterval = 0.1f; // ���� �߻� ����
    private float lastFireTime; // ������ �߻� �ð�

    private bool isSlowed = false; // Slow ���� ����

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Time.time - lastFireTime > fireInterval)
        {
            StartCoroutine(FireProjectileBurst());
            lastFireTime = Time.time;
        }

        CheckForSlowObjects(); // Slow �±� üũ
    }

    IEnumerator FireProjectileBurst()
    {
        for (int i = 0; i < projectilesPerFire; i++)
        {
            FireProjectile();
            if (i < projectilesPerFire - 1)
            {
                yield return new WaitForSeconds(burstInterval);
            }
        }
    }

    void FireProjectile()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        List<GameObject> allCreatures = new List<GameObject>();
        allCreatures.AddRange(creatures);

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

        if (closestCreature != null)
        {
            Vector3 targetDirection = closestCreature.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, rotation);

            Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }
        }
    }

    private void CheckForSlowObjects()
    {
        GameObject[] slowObjects = GameObject.FindGameObjectsWithTag("Slow");

        if (slowObjects.Length > 0 && !isSlowed)
        {
            fireInterval *= fireIntervalSlowMultiplier; // �߻� ������ �� ��� �ø�
            isSlowed = true;
        }
        else if (slowObjects.Length == 0 && isSlowed)
        {
            fireInterval /= fireIntervalSlowMultiplier; // �߻� ������ ������� ����
            isSlowed = false;
        }
    }

    public void IncreaseFireRate(float amount)
    {
        fireInterval -= amount;
        if (fireInterval < 0.1f) fireInterval = 0.1f; // �ּ� �߻� ���� ����
        Debug.Log("Fire rate increased, interval is now: " + fireInterval);
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectilesPerFire += amount;
        Debug.Log("Projectile count increased, now firing: " + projectilesPerFire + " projectiles per shot.");
    }
}
