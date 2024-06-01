using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter_4 : MonoBehaviour
{
    public static Player_Shooter_4 instance;

    public GameObject bulletPrefab; // źȯ ������
    public Transform firePoint; // źȯ �߻� ��ġ
    public float bulletSpeed = 20f; // źȯ �ӵ�
    public int bulletCount = 0; // �Ѿ� ��
    public float fireRate = 1f; // �߻� ���� (�� ����)
    public float damageAmount = 10; // ������ ��
    public float lifetime = 2f; // źȯ�� ���� (��)

    private Transform playerTransform; // �÷��̾� Ʈ������


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Player �±��� ������Ʈ�� ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ���� �ð����� Shoot �޼��� ȣ��
        InvokeRepeating("Shoot", 0f, fireRate);
    }

    void Shoot()
    {
        if (playerTransform == null)
        {
            return; // Player ������Ʈ�� ã�� ���� ��� �߻����� ����
        }

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // źȯ Ȯ�� ����
            float xSpread = Random.Range(-30f, 30f);
            float ySpread = Random.Range(2f, 5f); // y�� Ȯ�� ���� 2���� 5 ����

            // ȸ���� ���� ���� ���
            Quaternion spreadRotation = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(-30f, 30f), 0);
            Vector3 spreadDirection = spreadRotation * playerTransform.forward;

            // ���� ���Ϳ��� y �� ����
            spreadDirection.y = Mathf.Clamp(spreadDirection.y, 0.1f, 1f); // y ���� ������ �������� �ʵ��� Ŭ����

            Vector3 direction = spreadDirection.normalized;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = direction * bulletSpeed;

            // źȯ �ı��� ��ũ��Ʈ���� ���� ����
            Destroy(bullet, lifetime);

            // �浹 ó�� ������Ʈ�� �������� �߰�
            BulletCollisionHandler collisionHandler = bullet.AddComponent<BulletCollisionHandler>();
            collisionHandler.damageAmount = damageAmount;
        }
    }

    public void IncreaseBulletCount(int amount)
    {

        bulletCount += amount;
        Debug.Log("���� ���� : " + bulletCount);

    }

    public void IncreaseDamage(float amount)
    {
        // Player_Shooter_4 Ŭ������ damageAmount ���� ����
        damageAmount += amount;
        Debug.Log("���� ������ : " + damageAmount);
    }

    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;
        private Player_Shooter_4 shooterInstance;

        // �����ڸ� �̿��Ͽ� Player_Shooter_4 �ν��Ͻ��� ���޹���
        public BulletCollisionHandler(Player_Shooter_4 shooterInstance)
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
