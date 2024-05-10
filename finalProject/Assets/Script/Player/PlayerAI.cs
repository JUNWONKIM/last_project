using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float avoidanceDistance = 3f; // �Ѿ��� ���ϴ� �Ÿ�
    public float bulletDetectionRange = 20f;
    private Transform target; // ���� ����� ���� ��ġ
    private Transform nearestBullet; // ���� ����� �Ѿ��� ��ġ
    private Rigidbody rb; // �÷��̾��� Rigidbody ������Ʈ
    private float timeSinceLastFind = 0f; // ���������� ���� ã�� �ð�
    private float timeSinceLastBulletFind = 0f; // ���������� �Ѿ��� ã�� �ð�

    private LineRenderer detectionRangeVisual; // �� Ž�� ������ �ð������� ǥ���� ���� ������

    private enum PlayerState
    {
        MoveTowardsEnemy,
        AvoidBullet,
        MoveAwayFromEnemy
    }

    private PlayerState currentState = PlayerState.MoveTowardsEnemy;
    private float stateChangeTime = 0f;
    private float stateChangeDuration = 0.5f; // ���� ���� ���� �ð�

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        detectionRangeVisual = gameObject.AddComponent<LineRenderer>();

        // ���� ������ ����
        detectionRangeVisual.material = new Material(Shader.Find("Sprites/Default"));
        detectionRangeVisual.startColor = Color.green;
        detectionRangeVisual.endColor = Color.green;
        detectionRangeVisual.startWidth = 0.1f;
        detectionRangeVisual.endWidth = 0.1f;
        detectionRangeVisual.positionCount = 37; // ���� ������ ����
    }

    void Update()
    {
       
        // 0.2�ʸ��� ���� ����� ���� ã��
        timeSinceLastFind += Time.deltaTime;
        if (timeSinceLastFind >= 0.2f)
        {
            FindClosestEnemy();
            timeSinceLastFind = 0f;
        }

        // 0.2�ʸ��� ���� ����� �Ѿ��� ã��
        timeSinceLastBulletFind += Time.deltaTime;
        if (timeSinceLastBulletFind >= 0.2f)
        {
            FindClosestBullet();
            timeSinceLastBulletFind = 0f;
        }

        // �÷��̾��� ���¿� ���� �ٸ� �ൿ�� ����
        switch (currentState)
        {
            case PlayerState.MoveTowardsEnemy:
                MoveTowardsEnemy();
                break;
            case PlayerState.AvoidBullet:
                // ���� ����� ���� �Ѿ� �߿��� �� ����� ���� ���ϵ��� ����
                if (target != null && nearestBullet != null)
                {
                    float enemyDistance = Vector3.Distance(transform.position, target.position);
                    float bulletDistance = Vector3.Distance(transform.position, nearestBullet.position);
                    if (bulletDistance < enemyDistance)
                    {
                        AvoidBullet(nearestBullet.position);
                    }
                    else
                    {
                        MoveAwayFromEnemy();
                    }
                }
                break;
            case PlayerState.MoveAwayFromEnemy:
                MoveAwayFromEnemy();
                break;
        }

        // ���� ���� ����� �Ѿ��� �÷��̾� ���� ���� ������ ������ ��
        if (nearestBullet != null && currentState != PlayerState.AvoidBullet)
        {
            // �÷��̾ �ش� �Ѿ��� ���ϴ� ������ �����ϵ��� ���� ����
            ChangeState(PlayerState.AvoidBullet);
        }

        // ���� ���°� ����� �� ������ �ð��� ����ϸ� ���¸� �������� �ǵ���
        if (Time.time - stateChangeTime >= stateChangeDuration)
        {
            if (currentState == PlayerState.AvoidBullet)
            {
                // �Ѿ��� ���ϴ� ������ ���� ���� ����� ���� �����̿� ������ �ٽ� ���� ���ϴ� ���·� ����
                if (target == null)
                {
                    ChangeState(PlayerState.MoveTowardsEnemy);
                }
            }
            else if (currentState == PlayerState.MoveAwayFromEnemy)
            {
                // ���� ���ϴ� ������ ���� ���� ����� ���� �����̿� ������ �ٽ� ���� �Ǵ� ���·� ����
                if (target == null)
                {
                    ChangeState(PlayerState.MoveTowardsEnemy);
                }
            }
        }

        UpdateDetectionRange();
        Debug.Log("Current State: " + currentState);

    }

    void MoveTowardsEnemy()
    {
        if (target != null)
        {
            // ���� �ݴ� �������� �̵�
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            // �� �ٶ󺸱�
            Vector3 lookAtDirection = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookAtDirection);
        }
    }

    void AvoidBullet(Vector3 bulletPosition)
    {
        // �Ѿ��� ��ġ���� �÷��̾��� ��ġ�� �� ���� ����
        Vector3 directionToPlayer = transform.position - bulletPosition;

        // �Ѿ��� �̵� ���� ����
        Vector3 bulletDirection = nearestBullet.position - nearestBullet.position - nearestBullet.GetComponent<Rigidbody>().velocity.normalized;

        // �Ѿ��� ����� ������ ���� ��� (y ���� ����)
        Vector3 perpendicular = new Vector3(bulletDirection.z, 0f, -bulletDirection.x).normalized;

        // �÷��̾ �ش� �������� �̵�
        rb.MovePosition(transform.position + perpendicular * moveSpeed * Time.deltaTime);


    }

    void MoveAwayFromEnemy()
    {
        if (target != null)
        {
            // ���� �ݴ� �������� �̵�
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            // ���� ���ϱ� ���� �����ϹǷ� �þ߸� ������ ���ϵ��� ȸ������ ����
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            if (closestEnemy.transform != target) // ���� Ÿ�ٰ� ���� ����� ���� �ٸ� ���� Ÿ���� ����
            {
                target = closestEnemy.transform;

                // ����� ���� �ٶ󺸱�
                Vector3 lookAtDirection = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookAtDirection);
            }
        }
    }

    void FindClosestBullet()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("E_Bullet");
        float closestDistance = Mathf.Infinity;
        GameObject closestBullet = null;

        foreach (GameObject bullet in bullets)
        {
            float distance = Vector3.Distance(transform.position, bullet.transform.position);
            if (distance < closestDistance && distance <= bulletDetectionRange)
            {
                closestDistance = distance;
                closestBullet = bullet;
            }
        }

        if (closestBullet != null)
        {
            nearestBullet = closestBullet.transform;
        }
    }

    private void UpdateDetectionRange()
    {
        // �� ����� �� ����
        Vector3[] points = new Vector3[37];
        for (int i = 0; i < 37; i++)
        {
            float angle = i * Mathf.PI * 2f / 36f;
            points[i] = transform.position + new Vector3(Mathf.Sin(angle) * bulletDetectionRange, 0f, Mathf.Cos(angle) * bulletDetectionRange);
        }

        // ���� �������� �� ����
        detectionRangeVisual.SetPositions(points);
    }

    // �÷��̾� ���¸� �����ϴ� �޼���
    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        stateChangeTime = Time.time;
    }
}
