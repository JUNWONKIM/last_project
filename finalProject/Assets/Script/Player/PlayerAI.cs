using System.Collections;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public static PlayerAI instance;

    public float moveSpeed = 100f; // �̵� �ӵ�
    public float slowSpeed = 2f; // Slow ȿ�� ���
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
        StartCoroutine(FindClosestCreatureOrBossCoroutine());
        StartCoroutine(FindClosestBulletCoroutine());
    }

    void FixedUpdate()
    {
        // ���¿� ���� �ٸ� �ൿ�� ����
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

        // �Ѿ��� ������ ���� ��
        if (nearestBullet != null && currentState != PlayerState.AvoidBullet && Vector3.Distance(transform.position, nearestBullet.position) < bulletDetectionRange)
        {
            ChangeState(PlayerState.AvoidBullet);
        }

        // ���� ���� �ð� üũ
        if (Time.time - stateChangeTime >= stateChangeDuration)
        {
            if (currentState == PlayerState.AvoidBullet && nearestBullet == null)
            {
                ChangeState(PlayerState.MoveAwayFromCreature);
            }
            else if (currentState == PlayerState.MoveAwayFromCreature && target == null)
            {
                ChangeState(PlayerState.MoveTowardsCreature);
            }
        }

        // ���� �׻� �ٶ󺸰� ����
        LookAtTarget();

        CheckForSlowObjects();
    }

    void LookAtTarget()
    {
        if (target != null)
        {
            // ���� �ٶ󺸴� ����
            Vector3 lookAtDirection = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookAtDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 10f)); // �ε巴�� ȸ��
        }
    }

    void MoveTowardsCreature()
    {
        if (target != null)
        {
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            Vector3 lookAtDirection = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(lookAtDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 10f)); // �ӵ� ������ ���� ��
        }
    }

    void AvoidBullet(Vector3 bulletPosition)
    {
        if (nearestBullet != null)
        {
            Vector3 directionToPlayer = transform.position - bulletPosition;
            Vector3 bulletDirection = nearestBullet.GetComponent<Rigidbody>().velocity.normalized;
            Vector3 perpendicular = Vector3.Cross(bulletDirection, Vector3.up).normalized;

            // ������ �������� �̵�
            rb.MovePosition(rb.position + perpendicular * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void MoveAwayFromCreature()
    {
        if (target != null)
        {
            Vector3 moveDirection = (transform.position - target.position).normalized;
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator FindClosestCreatureOrBossCoroutine()
    {
        while (true)
        {
            FindClosestCreatureOrBoss();
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

    void FindClosestCreatureOrBoss()
    {
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject creature in creatures)
        {
            float distance = Vector3.Distance(transform.position, creature.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = creature;
            }
        }

        foreach (GameObject boss in bosses)
        {
            float distance = Vector3.Distance(transform.position, boss.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = boss;
            }
        }

        if (closestTarget != null)
        {
            if (closestTarget.transform != target)
            {
                target = closestTarget.transform;

                Vector3 lookAtDirection = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookAtDirection);
                rb.MoveRotation(lookRotation);
            }
        }
        else
        {
            target = null;
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
        else
        {
            nearestBullet = null;
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
            moveSpeed /= slowSpeed;
            isFreezed = true;
        }
        else if (freezeObjects.Length == 0 && isFreezed)
        {
            moveSpeed *= slowSpeed;
            isFreezed = false;
        }
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed *= amount;
        Debug.Log("Move speed increased to: " + moveSpeed);
    }
}
