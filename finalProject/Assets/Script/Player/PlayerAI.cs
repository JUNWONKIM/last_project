using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    private Transform target; // ���� ����� ���� ��ġ
    private Rigidbody rb; // �÷��̾��� Rigidbody ������Ʈ
    private float timeSinceLastFind = 0f; // ���������� ���� ã�� �ð�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 0.5�ʸ��� ���� ����� ���� ã��
        timeSinceLastFind += Time.deltaTime;
        if (timeSinceLastFind >= 0.2f)
        {
            FindClosestEnemy();
            timeSinceLastFind = 0f;
        }

        MoveTowardsEnemy(); // �� �����Ӹ��� ���� �ݴ� �������� �̵�
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
}
