using System.Collections;
using UnityEngine;

public class Player_Shooter_2 : MonoBehaviour
{
    public static Player_Shooter_2 instance;

    public GameObject objectToSpawn; // �Ѿ� ������
    public float spawnInterval = 3f; // �Ѿ� ��ȯ ����
    public float spawnRadius = 20f; // �Ѿ� ��ȯ ���� ������
    public float fixedYPosition = 2.5f; // ������ Y �� ��ġ
    public int projectilesPerFire = 0; // �� ���� �߻��� �߻�ü ��
    public float bulletLifetime = 1.5f; // �Ѿ��� ����
    public float damageAmount = 1f; // �Ѿ��� ������
    

    private float lastSpawnTime; // ������ ��ȯ �ð�

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating("SpawnBulletWithExplosion", 0f, spawnInterval);
    }

    void SpawnBulletWithExplosion()
    {
        for (int i = 0; i < projectilesPerFire; i++)
        {
            // �÷��̾� �ֺ� ���� ���� ������ ������ ��ġ ���
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = fixedYPosition;

            GameObject bullet = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

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
            }
        }
    }
}
