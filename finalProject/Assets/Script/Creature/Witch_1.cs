using UnityEngine;

public class Witch_1 : MonoBehaviour
{
    public float moveSpeed = 3.0f;  // ������ �̵� �ӵ�
    public float stopDistance = 2.0f;  // ���డ ���ߴ� �Ÿ�
    public GameObject attackParticlePrefab;  // ���� ��ƼŬ ������
    public float attackCooldown = 5.0f;  // ���� ��Ÿ��
    private Transform player;  // �÷��̾��� Transform
    private Rigidbody rb;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime;
    private bool initialAttack = true;  // ù ���� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();  // Animator ������Ʈ�� �����ɴϴ�.
        player = GameObject.FindGameObjectWithTag("Player").transform;  // "Player" �±׸� ���� ������Ʈ�� ã���ϴ�.
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stopDistance && !isAttacking)
            {
                animator.SetBool("isIdle", false);  // �̵� ���̹Ƿ� idle �ִϸ��̼� ��Ȱ��ȭ

                Vector3 direction = (player.position - transform.position).normalized;
                Vector3 move = direction * moveSpeed * Time.fixedDeltaTime;

                rb.MovePosition(transform.position + move);
                LookAtPlayer();  // �÷��̾ �ٶ󺸰� �մϴ�.
            }
            else if (distance <= stopDistance)
            {
                rb.velocity = Vector3.zero;  // �÷��̾�� ���� �Ÿ� �̳��� �ٰ����� ����ϴ�.
                LookAtPlayer();  // ���� ���¿����� �÷��̾ �ٶ󺸰� �մϴ�.

                if (!isAttacking && (initialAttack || Time.time >= lastAttackTime + attackCooldown))
                {
                    Attack();  // ������ �����մϴ�.
                }
                else if (!isAttacking)
                {
                    animator.SetBool("isIdle", true);  // ��Ÿ�� ���� �� idle �ִϸ��̼� Ȱ��ȭ
                }
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0;  // y �� ȸ���� �����Ͽ� ���డ �������θ� ȸ���ϵ��� �մϴ�.
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10.0f);
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetBool("isIdle", false);  // ���� ���̹Ƿ� idle �ִϸ��̼� ��Ȱ��ȭ
        animator.SetBool("isAttack", true);  // ���� �ִϸ��̼� ����
        Instantiate(attackParticlePrefab, player.position, Quaternion.identity);  // ���� ��ƼŬ ����
        lastAttackTime = Time.time;  // ������ ���� �ð��� ����
        initialAttack = false;  // ù ������ �������Ƿ� false�� ����
        Invoke("ResetAttack", 1.0f);  // �ִϸ��̼��� ���� �� isAttacking ���¸� ����
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttack", false);  // ���� �ִϸ��̼� ����

        if (Vector3.Distance(transform.position, player.position) <= stopDistance)
        {
            animator.SetBool("isIdle", true);  // ��Ÿ�� ���� �� idle �ִϸ��̼� Ȱ��ȭ
        }
    }
}
