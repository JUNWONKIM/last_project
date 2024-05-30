using System.Collections;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public static PlayerAI instance;

    public float moveSpeed = 100f; // �̵� �ӵ�
    public float slowSpeed = 2f;
    public bool isFreezed = false;

    public float avoidanceDistance = 3f; // �Ѿ��� ���ϴ� �Ÿ�
    public float bulletDetectionRange = 20f;
    private Transform target; // ���� ����� ���� ��ġ
    private Transform nearestBullet; // ���� ����� �Ѿ��� ��ġ
    private Rigidbody rb; // �÷��̾��� Rigidbody ������Ʈ

    private enum PlayerState
    {
        MoveTowardsCreature,
        AvoidBullet,
        MoveAwayFromCreature
    }

    private PlayerState currentState = PlayerState.MoveTowardsCreature;
    private float stateChangeTime = 0f;
    private float stateChangeDuration = 0.5f; // ���� ���� ���� �ð�


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(FindClosestCreatureCoroutine());
        StartCoroutine(FindClosestBulletCoroutine());
    }

    void Update()
    {
        // �÷��̾��� ���¿� ���� �ٸ� �ൿ�� ����
        switch (currentState)
        {
            case PlayerState.MoveTowardsCreature:
                MoveTowardsCreature();
                break;
            case PlayerState.AvoidBullet:
                if (nearestBullet != null)
                {
                    AvoidBullet(nearestBullet.position);
                }
                break;
            case PlayerState.MoveAwayFromCreature:
                MoveAwayFromCreature();
                break;
        }

        // ���� ���� ����� �Ѿ��� �÷��̾� ���� ���� ������ ������ ��
        if (nearestBullet != null && currentState != PlayerState.AvoidBullet && Vector3.Distance(transform.position, nearestBullet.position) < bulletDetectionRange)
        {
            ChangeState(PlayerState.AvoidBullet);
        }

        // ���� ���°� ����� �� ������ �ð��� ����ϸ� ���¸� �������� �ǵ���
        if (Time.time - stateChangeTime >= stateChangeDuration)
        {
            if (currentState == PlayerState.AvoidBullet && nearestBullet == null)
            {
                ChangeState(PlayerState.MoveTowardsCreature);
            }
            else if (currentState == PlayerState.MoveAwayFromCreature && target == null)
            {
                ChangeState(PlayerState.MoveTowardsCreature);
            }
        }

        CheckForSlowObjects();
    }

    void MoveTowardsCreature()
    {
        if (target != null)
        {
            // creature�� �ݴ� �������� �̵�
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

            // creature �ٶ󺸱�
            Vector3 lookAtDirection = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookAtDirection);
        }
    }

    void AvoidBullet(Vector3 bulletPosition)
    {
        // �Ѿ��� ��ġ���� �÷��̾��� ��ġ�� �� ���� ����
        Vector3 directionToPlayer = transform.position - bulletPosition;

        // �Ѿ��� �̵� ���� ����
        Vector3 bulletDirection = bulletPosition - nearestBullet.GetComponent<Rigidbody>().velocity.normalized;

        // �Ѿ��� ����� ������ ���� ��� (y ���� ����)
        Vector3 perpendicular = new Vector3(bulletDirection.z, 0f, -bulletDirection.x).normalized;

        // �÷��̾ �ش� �������� �̵�
        rb.MovePosition(transform.position + perpendicular * moveSpeed * Time.deltaTime);
    }

    void MoveAwayFromCreature()
    {
        if (target != null)
        {
            // creature�� �ݴ� �������� �̵�
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator FindClosestCreatureCoroutine()
    {
        while (true)
        {
            FindClosestCreature();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FindClosestBulletCoroutine()
    {
        while (true)
        {
            FindClosestBullet();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void FindClosestCreature()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        float closestDistance = Mathf.Infinity;
        GameObject closestCreature = null;

        foreach (GameObject creature in creatures)
        {
            float distance = Vector3.Distance(transform.position, creature.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCreature = creature;
            }
        }

        if (closestCreature != null)
        {
            if (closestCreature.transform != target) // ���� Ÿ�ٰ� ���� ����� creature�� �ٸ� ���� Ÿ���� ����
            {
                target = closestCreature.transform;

                // ����� creature�� �ٶ󺸱�
                Vector3 lookAtDirection = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookAtDirection);
            }
        }
    }

    void FindClosestBullet()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("C_Bullet");
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

    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        stateChangeTime = Time.time;
    }

    private void CheckForSlowObjects()
    {
        GameObject[] freezeObjects = GameObject.FindGameObjectsWithTag("Freeze");

        if (freezeObjects.Length > 0 && !isFreezed)
        {
            moveSpeed /= slowSpeed; // �߻� ������ �� ��� �ø�
            isFreezed = true;
        }
        else if (freezeObjects.Length == 0 && isFreezed)
        {
            moveSpeed *= slowSpeed; // �߻� ������ �� ��� �ø�
            isFreezed = false;
        }
    }


    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed *= amount;
        Debug.Log("Move speed increased to: " + moveSpeed);
    }
}
