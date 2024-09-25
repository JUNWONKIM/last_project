using UnityEngine;

public class Mummy : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    public float normalSpeed = 2f; // �̶��� �⺻ �̵� �ӵ�
    public float chaseSpeed = 5f; // �÷��̾ �߰��� ���� �ӵ�
    public float chaseRange = 10f; // �÷��̾ �߰��ϱ� �����ϴ� ����
    public float explodeRange = 1.5f; // �����ϴ� ����
    public GameObject explosionPrefab; // ���� ����Ʈ ������
    public float chaseDuration = 3f; // �߰� ���� �ð� (��)
    public float damageAmount = 1f;
    public float maxHealth = 1; // �ִ� ü��
    public float currentHealth; // ���� ü��

    private Rigidbody rb;
    private bool isChasing = false;
    private bool hasDirectionSet = false; // ������ �����Ǿ����� ����
    private Vector3 chaseDirection; // ������ ���� ����
    private float chaseTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ ����

        rb = GetComponent<Rigidbody>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= explodeRange)
        {
            // �÷��̾���� �Ÿ��� ���� ���� �ȿ� ������
            Explode();
        }
        else if (!isChasing && distanceToPlayer <= chaseRange)
        {
            // �÷��̾���� �Ÿ��� �߰� ���� �ȿ� ������
            isChasing = true;
            chaseTimer = chaseDuration;
            hasDirectionSet = false; // ���� �ʱ�ȭ
        }

        if (isChasing)
        {
            chaseTimer -= Time.deltaTime;

            if (!hasDirectionSet)
            {
                SetChaseDirection();
            }

            if (chaseTimer <= 0f)
            {
                // �߰� ���� �� 3�ʰ� ������ ����
                Explode();
            }
        }
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            MoveTowardsChaseDirection(chaseSpeed);
            RotateTowardsDirection(chaseDirection); // �����ϴ� ������ �ٶ󺸵��� ȸ��
        }
        else
        {
            Vector3 direction = (player.position - transform.position).normalized;
            MoveTowardsDirection(direction, normalSpeed);
            RotateTowardsDirection(direction); // �÷��̾ ���ϴ� ������ �ٶ󺸵��� ȸ��
        }
    }

    void MoveTowardsDirection(Vector3 direction, float speed)
    {
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    void MoveTowardsChaseDirection(float speed)
    {
        Vector3 newPosition = transform.position + chaseDirection * speed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    void RotateTowardsDirection(Vector3 direction)
    {
        // �ش� ������ �ٶ󺸵��� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(targetRotation);
    }

    void SetChaseDirection()
    {
        // �÷��̾��� ��ġ�� �������� ������ ������ ���� ����
        Vector3 randomDirection = Random.insideUnitSphere * 10f; // 2f�� ���� �ݰ��Դϴ�.
        randomDirection += player.position;
        randomDirection.y = transform.position.y; // ���� �̵� ����
        chaseDirection = (randomDirection - transform.position).normalized;
        hasDirectionSet = true; // ������ �����Ǿ����� ǥ��
    }

    void Explode()
    {
        // ���� ȿ�� ����
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        PlayerLV.IncrementCreatureDeathCount();
        // �̶� ����
        Destroy(gameObject);
    }

    // ����Ʈ���� �浹 ó��


    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // ��������ŭ ü�� ����

        if (currentHealth <= 0)
        {
            Explode(); // ü���� 0 �����̸� ��� ó��
        }
    }

    
}
