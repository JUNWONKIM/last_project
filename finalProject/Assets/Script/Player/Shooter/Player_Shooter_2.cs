using System.Collections;
using UnityEngine;

public class Player_Shooter_2 : MonoBehaviour
{
    public static Player_Shooter_2 instance;

    public GameObject spawnPrefab; // �Ѿ� ������
    public float spawnInterval = 5f; // �Ѿ� ��ȯ ����
    public float spawnRadius = 20f; // �Ѿ� ��ȯ ���� ������
    public float fixedYPosition = 2.5f; // ������ Y �� ��ġ
    public int projectilesPerFire = 0; // �� ���� �߻��� �߻�ü ��
    public float bulletLifetime = 1.5f; // �Ѿ��� ����
    public float damageAmount = 1f; // �Ѿ��� ������
    

    private float lastSpawnTime; // ������ ��ȯ �ð�

    private bool isSlowed = false; // Slow ���� ����
    public float spawnIntervalSlowMultiplier = 2f; // Slow ȿ�� �� �߻� ���� ���

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating("SpawnBulletWithExplosion", 0f, spawnInterval);
    }

    void Update()
    {
        CheckForSlowObjects();
    }
    void SpawnBulletWithExplosion()
    {
        for (int i = 0; i < projectilesPerFire; i++)
        {
            // �÷��̾� �ֺ� ���� ���� ������ ������ ��ġ ���
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = fixedYPosition;

            GameObject bullet = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);

            // �Ѿ� �ı� Ÿ�̸� ����
            Destroy(bullet, bulletLifetime);

            // �Ѿ˿� ����� �ݶ��̴� ��������
            BulletCollisionHandler collisionHandler = bullet.AddComponent<BulletCollisionHandler>();
            collisionHandler.damageAmount = damageAmount;

            // �ݶ��̴��� ��Ȱ��ȭ
            Collider bulletCollider = bullet.GetComponent<Collider>();
            bulletCollider.enabled = false;

            // ���� �ð��� ���� �Ŀ� �ݶ��̴��� Ȱ��ȭ
            StartCoroutine(EnableColliderAfterDelay(bulletCollider, 1.0f));
        }
    }

    IEnumerator EnableColliderAfterDelay(Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    private void CheckForSlowObjects()
    {
        GameObject[] slowObjects = GameObject.FindGameObjectsWithTag("Slow");

        if (slowObjects.Length > 0 && !isSlowed)
        {
            spawnInterval *= spawnIntervalSlowMultiplier; // �߻� ������ �� ��� �ø�
            isSlowed = true;
        }
        else if (slowObjects.Length == 0 && isSlowed)
        {
            spawnInterval /= spawnIntervalSlowMultiplier; // �߻� ������ ������� ����
            isSlowed = false;
        }
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectilesPerFire += amount;
        Debug.Log("��ź ���� " + projectilesPerFire);
    }

    public void IncreaseDamage(float amount)
    {
       
        damageAmount += amount;
        Debug.Log("��ź ������ : " + damageAmount);
    }

    public void IncreaseFireRate(float amount)
    {
        spawnInterval /= amount;
        if (spawnInterval < 0.1f) spawnInterval = 0.1f; 
        Debug.Log("��ź �߻� �ӵ� :" + spawnInterval);
    }



    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;
        private Player_Shooter_2 shooterInstance;

        // �����ڸ� �̿��Ͽ� Player_Shooter_2 �ν��Ͻ��� ���޹���
        public BulletCollisionHandler(Player_Shooter_2 shooterInstance)
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

                Mummy enemyHealth2 = other.gameObject.GetComponent<Mummy>();
                if (enemyHealth2 != null)
                {
                    enemyHealth2.TakeDamage(damageAmount);
                }
            }

        }
    }
}
