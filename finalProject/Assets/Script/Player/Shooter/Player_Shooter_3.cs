using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player_Shooter_3 : MonoBehaviour
{
    public static Player_Shooter_3 instance;

    public GameObject swordPrefab; // Į ������
    public float summonInterval = 5f; // Į ��ȯ ����
    public float distanceFromPlayer = 10f; // �÷��̾�κ����� �Ÿ�
    public float damageAmount = 0f;
    public int swordNum = 0;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // ���� ���ݸ��� Į�� ��ȯ�ϴ� �ڷ�ƾ ����
        InvokeRepeating("SummonSword", 0f, summonInterval);
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
                Quaternion summonRotation = Quaternion.Euler(90f, 90f, 0f);

                // Į ��ȯ
                GameObject sword = Instantiate(swordPrefab, summonPosition, summonRotation);

                // �浹 ó�� ������Ʈ�� �������� �߰�
                BulletCollisionHandler collisionHandler = sword.AddComponent<BulletCollisionHandler>();
                collisionHandler.damageAmount = damageAmount;
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
                    Quaternion summonRotation = Quaternion.Euler(90f, 90f, angle);

                    // Į ��ȯ
                    GameObject sword = Instantiate(swordPrefab, summonPosition, summonRotation);

                    // �浹 ó�� ������Ʈ�� �������� �߰�
                    BulletCollisionHandler collisionHandler = sword.AddComponent<BulletCollisionHandler>();
                    collisionHandler.damageAmount = damageAmount;
                }
            }
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
        private Player_Shooter_3 shooterInstance;

        // �����ڸ� �̿��Ͽ� Player_Shooter_4 �ν��Ͻ��� ���޹���
        public BulletCollisionHandler(Player_Shooter_3 shooterInstance)
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
