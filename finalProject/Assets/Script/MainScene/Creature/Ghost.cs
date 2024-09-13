using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ghost : MonoBehaviour
{
    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float stoppingDistance = 5f; // �÷��̾���� ���ߴ� �Ÿ�
    public float retreatDistance = 5f; // �÷��̾�κ��� �����ϴ� �Ÿ�

    public float bulletSpeed = 50f;
    public GameObject projectilePrefab; // �߻��� ����ü ������
    public Transform firePoint; // �߻� ����
    public float fireRate = 1f; // �߻� �ӵ� (1�ʴ� �� ��)
    public float nextFireTime = 0f;

    public float damageAmount = 1f;

    private Transform player; // �÷��̾��� ��ġ
    private Rigidbody rb; // ��Ʈ�� Rigidbody ������Ʈ
    private Animator animator; // ��Ʈ�� �ִϸ����� ������Ʈ

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� ��ġ ã��
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
    }

    void Update()
    {
        if (player != null && !animator.GetBool("isDie"))
        {
            // �÷��̾���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stoppingDistance)
            {
                // �÷��̾ ���� �̵�
                Move();
            }
            else if (distanceToPlayer <= stoppingDistance && distanceToPlayer > retreatDistance)
            {
                // ���� ���·� ��ȯ
                Attack();
            }
        }
        else
        {
            Transform childObject_1 = transform.Find("Ghost");
            Transform childObject_2 = transform.Find("GhostArmature");

            Transform effect = transform.Find("Ghost_die");

            childObject_1.gameObject.SetActive(false);
            childObject_2.gameObject.SetActive(false);

            effect.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        // �̵� �ִϸ��̼�
        animator.SetBool("isAttack", false);

        // �÷��̾ �ٶ󺸵��� ȸ��
        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        rb.MoveRotation(rotation);

        // �÷��̾� �������� �̵�
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        // ���� �ִϸ��̼�
        animator.SetBool("isAttack", true);

        // ����ü �߻�
        if (Time.time >= nextFireTime)
        {
            transform.LookAt(player);
            Vector3 direction = player.position - firePoint.position;
            direction.Normalize();
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed; // ����ü �ӵ�
            Destroy(bullet, 3f); // 3�� �Ŀ� �ı�
            nextFireTime = Time.time + 1f / fireRate; // ���� �߻� �ð� ����

            BulletCollisionHandler collisionHandler = bullet.AddComponent<BulletCollisionHandler>();
            collisionHandler.damageAmount = damageAmount;
        }
    }

    public void DieAnimationComplete()
    {
        // ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� �̵� �� ȸ�� ����
        enabled = false;
    }

    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;
        void OnTriggerEnter(Collider other)
        {
            PlayerHP playerHP = other.gameObject.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.hp -= damageAmount; // �÷��̾��� ü���� 1 ����
                
            
            Destroy(gameObject);
            }
        }
    }
}
