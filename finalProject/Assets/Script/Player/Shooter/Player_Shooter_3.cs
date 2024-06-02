using System.Collections;
using UnityEngine;

public class Player_Shooter_3 : MonoBehaviour
{
    public static Player_Shooter_3 instance;

    public GameObject swordPrefab; // Į ������
    public float summonInterval = 5f; // Į ��ȯ ����
    public float distanceFromPlayer = 10f; // �÷��̾�κ����� �Ÿ�
    public float damageAmount = 0f;
    public int swordNum = 0;
    public float orbitSpeed = 50f; // Į�� ȸ�� �ӵ�

    private bool isSlowed = false; // Slow ���� ����
    public float summonIntervalSlowMultiplier = 2f; // Slow ȿ�� �� �߻� ���� ���

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // ���� ���ݸ��� Į�� ��ȯ�ϴ� �ڷ�ƾ ����
        InvokeRepeating("SummonSword", 0f, summonInterval);
    }

    void Update()
    {
        CheckForSlowObjects();
    }

    void SummonSword()
    {
        // �÷��̾� ���ӿ�����Ʈ ��������
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (swordNum == 1)
            {
                // �÷��̾� ��ġ�� Į�� ��ȯ ��ġ ���
                Vector3 summonPosition = player.transform.position + new Vector3(distanceFromPlayer, 0f, 0f);
                Quaternion summonRotation = Quaternion.Euler(90f, 0f, 0f);

                // Į ��ȯ
                GameObject sword = Instantiate(swordPrefab, summonPosition, summonRotation);
                sword.AddComponent<SwordOrbit>().Initialize(player.transform, distanceFromPlayer, 1, 0, orbitSpeed);

                // �浹 ó�� ������Ʈ�� �������� �߰�
                BulletCollisionHandler collisionHandler = sword.AddComponent<BulletCollisionHandler>();
                collisionHandler.damageAmount = damageAmount;

                // 3�� �Ŀ� Į �ı�
                Destroy(sword, 3f);
            }
            else
            {
                // ���� ������ ���
                float radius = distanceFromPlayer;

                for (int i = 0; i < swordNum; i++)
                {
                    // �� Į�� ���� ���
                    float angle = i * (360f / swordNum);

                    // Į�� ��ȯ ��ġ ���
                    float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                    Vector3 summonPosition = player.transform.position + new Vector3(x, 0f, z);
                    Quaternion summonRotation = Quaternion.Euler(90f, 0f, angle);

                    // Į ��ȯ
                    GameObject sword = Instantiate(swordPrefab, summonPosition, summonRotation);
                    sword.AddComponent<SwordOrbit>().Initialize(player.transform, distanceFromPlayer, swordNum, i, orbitSpeed);

                    // �浹 ó�� ������Ʈ�� �������� �߰�
                    BulletCollisionHandler collisionHandler = sword.AddComponent<BulletCollisionHandler>();
                    collisionHandler.damageAmount = damageAmount;

                    // 3�� �Ŀ� Į �ı�
                    Destroy(sword, 3f);
                }
            }
        }
    }

    private void CheckForSlowObjects()
    {
        GameObject[] slowObjects = GameObject.FindGameObjectsWithTag("Slow");

        if (slowObjects.Length > 0 && !isSlowed)
        {
            summonInterval *= summonIntervalSlowMultiplier; // �߻� ������ �� ��� �ø�
            isSlowed = true;
        }
        else if (slowObjects.Length == 0 && isSlowed)
        {
            summonInterval /= summonIntervalSlowMultiplier; // �߻� ������ ������� ����
            isSlowed = false;
        }
    }

    public void IncreaseSwordNum()
    {
        swordNum++;
        Debug.Log("Į ���� : " + swordNum);
    }

    public void IncreaseDamage(float amount)
    {
        // Player_Shooter_4 Ŭ������ damageAmount ���� ����
        damageAmount += amount;
        Debug.Log("Į ������ : " + damageAmount);
    }

    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;

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

public class SwordOrbit : MonoBehaviour
{
    private Transform playerTransform;
    private float orbitRadius;
    private int totalSwords;
    private int swordIndex;
    private float orbitSpeed;

    public void Initialize(Transform player, float radius, int swordCount, int index, float speed)
    {
        playerTransform = player;
        orbitRadius = radius;
        totalSwords = swordCount;
        swordIndex = index;
        orbitSpeed = speed;
    }

    void Start()
    {
        // 3�� �Ŀ� Į �ı�
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // ������ ������Ű�鼭 ȸ��
        float angle = swordIndex * (360f / totalSwords) + (Time.time * orbitSpeed);
        float x = orbitRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = orbitRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        // ���ο� ��ġ ���
        Vector3 newPosition = playerTransform.position + new Vector3(x, 0f, z);
        transform.position = newPosition;

        // Į�� �÷��̾� �ݴ� ������ �ٶ󺸵��� ȸ�� ����
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-directionToPlayer);
        transform.rotation = lookRotation * Quaternion.Euler(90, 0, 0);
    }
}
