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
    public float damageAmount = 1; // ������ ��

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
            Shoot();
            if (i < projectilesPerFire - 1)
            {
                yield return new WaitForSeconds(burstInterval);
            }
        }
    }

    void Shoot()
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
            GameObject bullet = Instantiate(projectilePrefab, transform.position, rotation);

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }

            // �浹 ó�� ������Ʈ�� �������� �߰�
            BulletCollisionHandler collisionHandler = bullet.AddComponent<BulletCollisionHandler>();
            collisionHandler.damageAmount = damageAmount;
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
        Debug.Log("����ü �߻� �ӵ� :" + fireInterval);
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectilesPerFire += amount;
        Debug.Log("����ü ���� : " + projectilesPerFire);
    }

    public void IncreaseDamage(float amount)
    {
        // Player_Shooter_4 Ŭ������ damageAmount ���� ����
        damageAmount += amount;
        Debug.Log("����ü  ������ : " + damageAmount);
    }

    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;
        private Player_Shooter_1 shooterInstance;

        public BulletCollisionHandler(Player_Shooter_1 shooterInstance)
        {
            this.shooterInstance = shooterInstance;
        }
        void OnTriggerEnter(Collider other)
        {
            // �浹�� ��ü�� �±װ� "Creature"�� ���
            if (other.gameObject.CompareTag("Creature"))
            {
                // �浹�� ��ü�� HP�� ���ҽ�Ŵ
                CreatureHealth enemyHealth = other.gameObject.GetComponent<CreatureHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }

                // �Ѿ��� �ı�
                Destroy(gameObject);
            }
        }
    }
}
