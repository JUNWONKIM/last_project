using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float speed = 5.0f; // ������ �̵��� �ӵ�
    public float rotationSpeed = 5.0f; // ������ ȸ���� �ӵ�

    public GameObject atk0Prefab; // ��ź ������
    public GameObject atk1Prefab; // ATK1 ������
    public GameObject atk2Prefab; // ��ü�� ������
    public GameObject atk3Prefab; // ������ �̵��� �ڸ��� ���� �� ������

    public float attackRange = 3.0f; // ��ź ������ �� �Ÿ�

    private Transform player; // �÷��̾��� ��ġ�� ������ ����
    private bool isAttacking = false; // ������ ���� ������ ����
    private bool isControlled = false; // ������ �÷��̾ ���� ����Ǵ��� ����
    private Animator animator; // �ִϸ����� ������Ʈ
    private Rigidbody rb; // Rigidbody ������Ʈ

    private float idleStartTime; // Idle ���� ���� �ð�
    private float idleTimeToReattack = 2.0f; // Idle ���°� ���ӵ� �� ����� �ð�
    private bool IsIdle = false; // ���� Idle �������� ����

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ �����Ÿ� ���� ���� ��
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                // �����Ÿ� ������ �ٷ� ����
                StartCoroutine(BasicAttack());
            }
        }
        else
        {
            IsIdle = false;
            // �÷��̾ �����Ÿ� ������ ���� ��� ������ �ٽ� �÷��̾ �����ϵ��� ����
            if (!isAttacking)
            {
                MoveTowardsPlayer();
            }
        }

        // �÷��̾��� Ű���� �Է¿� ���� ���� ó��
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {
            StartCoroutine(ReplaceClosestCreatures());
        }

        if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
        {
            StartCoroutine(ControlBoss());
        }
    }

    private void MoveTowardsPlayer()
    {
        if (IsIdle) return; // idle ���¿����� �̵����� ����

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // ȸ��
        rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // �̵�
        Vector3 moveDirection = direction * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);

        ResetAllTriggers();
        animator.SetBool("IsWalk", true); // Walk �ִϸ��̼� Ʈ����
    }



    private void HandleIdleState(float distanceToPlayer)
    {
        if (Time.time - idleStartTime > idleTimeToReattack)
        {
            // Idle ���¿��� �÷��̾ ������ �����Ÿ� �ȿ� ������ �⺻ ������ �ٽ� ����
            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(BasicAttack());
            }
        }
    }

    IEnumerator BasicAttack()
    {
        isAttacking = true;

        while (true)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(rb.rotation, lookRotation) < 5.0f)
            {
                break;
            }

            yield return null;
        }

        ResetAllTriggers();
        animator.SetBool("IsIdle", false);
        animator.SetBool("ATK0", true);

        // ������ ���� ���� �� �̵��� ���߰� �ϰ� ���� �ӵ� ����
        float originalSpeed = speed;
        speed = 0; // ���� �� �̵��� ����

        // ���� ��� ���� �ִϸ��̼��� ���̸� ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        // ���� ��ġ�� ��ź ����
        Vector3 attackPosition = player.position;
        Instantiate(atk0Prefab, attackPosition, Quaternion.identity);

        // �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(animationLength / 2);
        animator.SetBool("ATK0", false);

        // ������ ���� �� idle ���·� ��ȯ
        IsIdle = true;
        idleStartTime = Time.time;

        ResetAllTriggers();
        animator.SetBool("IsIdle", true); // Idle ���·� ��Ȯ�� ��ȯ

        // Speed�� ���� ������ ����
        speed = originalSpeed; // �̵� �ӵ��� ���� ������ ����

        isAttacking = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        ResetAllTriggers();
        animator.SetTrigger("ATK1");

        GameObject atk1 = transform.Find("ATK1").gameObject;
        atk1.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        atk1.SetActive(false);

        animator.ResetTrigger("ATK1");

        isAttacking = false;
    }

    IEnumerator ReplaceClosestCreatures()
    {
        isAttacking = true;

        ResetAllTriggers();
        animator.SetTrigger("ATK2");

        float originalSpeed = speed;
        float originalRotationSpeed = rotationSpeed;
        speed = 0;
        rotationSpeed = 0;

        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        if (creatures.Length == 0) yield break;

        Vector3 currentPosition = transform.position;
        var closestCreatures = creatures
            .OrderBy(creature => Vector3.Distance(creature.transform.position, currentPosition))
            .Take(10)
            .ToList();

        foreach (GameObject closestCreature in closestCreatures)
        {
            Vector3 position = closestCreature.transform.position;
            Quaternion rotation = closestCreature.transform.rotation;
            Destroy(closestCreature);
            Instantiate(atk2Prefab, position, rotation);
        }

        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // ������ speed�� rotationSpeed ����
        speed = originalSpeed;
        rotationSpeed = originalRotationSpeed;
        animator.ResetTrigger("ATK2");

        isAttacking = false;
    }

    IEnumerator ControlBoss()
    {
        isControlled = true;
        isAttacking = true;

        float originalSpeed = speed;
        float originalRotationSpeed = rotationSpeed;
        float controlSpeed = speed * 2.0f;
        float controlRotationSpeed = 720.0f;

        float lastFireTime = Time.time;
        float controlDuration = 10.0f;
        float controlEndTime = Time.time + controlDuration;

        while (Time.time < controlEndTime)
        {
            float moveHorizontal = Input.GetAxis("Boss_Horizontal");
            float moveVertical = Input.GetAxis("Boss_Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

            if (movement != Vector3.zero)
            {
                rb.MovePosition(rb.position + movement * controlSpeed * Time.deltaTime);

                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, controlRotationSpeed * Time.deltaTime);

                ResetAllTriggers();
                animator.SetTrigger("IsRun");

                if (Time.time - lastFireTime >= 0.1f)
                {
                    Vector3 firePosition = transform.position - transform.forward * 20f;
                    Instantiate(atk3Prefab, firePosition, Quaternion.identity);
                    lastFireTime = Time.time;
                }
            }
            else
            {
                ResetAllTriggers();
                animator.SetTrigger("IsIdle");
            }

            yield return null;
        }

        ResetAllTriggers();

        // ������ speed�� rotationSpeed ����
        speed = originalSpeed;
        rotationSpeed = originalRotationSpeed;
        isControlled = false;
        isAttacking = false;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void ResetAllTriggers()
    {
        animator.ResetTrigger("IsIdle");
        animator.ResetTrigger("IsRun");
        animator.ResetTrigger("ATK1");
        animator.ResetTrigger("ATK2");
        animator.ResetTrigger("ATK0");
    }
}
