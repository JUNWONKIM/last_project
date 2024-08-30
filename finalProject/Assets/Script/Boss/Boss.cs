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
    public Slider uiSlider; // UISliderController���� ������ Slider ������Ʈ

    public float attackRange = 3.0f; // ��ź ������ �� �Ÿ�

    private Transform player; // �÷��̾��� ��ġ�� ������ ����
    private bool isAttacking = false; // ������ ���� ������ ����
    private bool isControlled = false; // ������ �÷��̾ ���� ����Ǵ��� ����
    private Animator animator; // �ִϸ����� ������Ʈ

    private float idleStartTime; // Idle ���� ���� �ð�
    private float idleTimeToReattack = 2.0f; // Idle ���°� ���ӵ� �� ����� �ð�
    private bool isIdle = false; // ���� Idle �������� ����

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking && !isControlled)
        {
            if (distanceToPlayer <= attackRange)
            {
                if (isIdle)
                {
                    // Idle ���°� 2�� �̻� ���ӵǾ��� �� �⺻ ������ �ٽ� ����
                    if (Time.time - idleStartTime > idleTimeToReattack)
                    {
                        StartCoroutine(BasicAttack());
                    }
                }
                else
                {
                    // �⺻ ������ ����
                    StartCoroutine(BasicAttack());
                }
            }
            else
            {
                // �÷��̾ ���� ��Ÿ� ������ �������Ƿ� �ٽ� �÷��̾ ���� �ɾ��
                isIdle = false;
                MoveTowardsPlayer();
            }
        }

        // Z Ű�� ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        // X Ű�� ������ �� ���� ����� Creature �������� ��ü
        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {
            StartCoroutine(ReplaceClosestCreatures());
        }

        // C Ű�� ������ �� 10�� ���� �÷��̾ ���� ����
        if (Input.GetKeyDown(KeyCode.C) && !isAttacking)
        {
            StartCoroutine(ControlBoss());
            if (uiSlider != null)
            {
                uiSlider.gameObject.SetActive(true); // �����̴��� Ȱ��ȭ
                StartCoroutine(StartSliderCountdown()); // �����̴��� ���� ���̱� ����
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        // �÷��̾ ���� �ٰ�����
        Vector3 direction = (player.position - transform.position).normalized;

        // �÷��̾ �ٶ󺸱�
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // �÷��̾ ���� �̵��ϱ�
        transform.position += transform.forward * speed * Time.deltaTime;

        // �̵� �߿��� Idle ���� ����
        if (isIdle)
        {
            // Idle ���� ����
            ResetAllTriggers();
            animator.SetBool("IsIdle", false);
            isIdle = false;
        }
    }

    IEnumerator BasicAttack()
    {
        // ���� ������ ǥ��
        isAttacking = true;

        // �⺻ ���� �ִϸ��̼� ����
        ResetAllTriggers();
        animator.SetBool("IsIdle", false);
        animator.SetBool("ATK0", true);

        // ������ ���ߵ��� �ӵ� ����
        float originalSpeed = speed;
        speed = 0;

        // �ִϸ��̼��� ���� ������ ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength); // �ִϸ��̼� ��ü ���̸�ŭ ���

        // ���� �÷��̾� ��ġ�� ��ź�� ����
        Vector3 attackPosition = player.position;
        Instantiate(atk0Prefab, attackPosition, Quaternion.identity);

        // �ִϸ��̼� ���� �� ������ ��� �ð�
        yield return new WaitForSeconds(animationLength / 2); // �ִϸ��̼� ���� �������� ���
        animator.SetBool("ATK0", false);

        // Idle ���·� ��ȯ�ϰ� 2�� ���
        isIdle = true;
        idleStartTime = Time.time;

        // �ִϸ��̼� ���¸� Idle�� ����
        ResetAllTriggers();
        animator.SetBool("IsIdle", true);

        // �⺻ ���� �Ϸ�
        isAttacking = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // ��� Ʈ���� ���� �� Attack �ִϸ��̼� ����
        ResetAllTriggers();
        animator.SetTrigger("ATK1");

        // ATK1 ������ Ȱ��ȭ
        GameObject atk1 = transform.Find("ATK1").gameObject;
        atk1.SetActive(true);

        // 3�� ���� ���
        yield return new WaitForSeconds(3.0f);

        // ATK1 ������ ��Ȱ��ȭ
        atk1.SetActive(false);

        // ���� �ִϸ��̼����� ���ư���
        animator.ResetTrigger("ATK1");

        isAttacking = false;
    }

    IEnumerator ReplaceClosestCreatures()
    {
        isAttacking = true;

        // ��� Ʈ���� ���� �� ReplaceClosestCreatures �ִϸ��̼� ����
        ResetAllTriggers();
        animator.SetTrigger("ATK2");

        // ������ ���ߵ��� �ӵ��� ȸ�� �ӵ� ����
        float originalSpeed = speed;
        float originalRotationSpeed = rotationSpeed;
        speed = 0;
        rotationSpeed = 0;

        // ���� ����� 10���� Creature ������Ʈ�� ã��
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        if (creatures.Length == 0) yield break;

        Vector3 currentPosition = transform.position;

        // ���� ����� 10���� Creature ������Ʈ�� �Ÿ������� �����Ͽ� ����
        var closestCreatures = creatures
            .OrderBy(creature => Vector3.Distance(creature.transform.position, currentPosition))
            .Take(10)
            .ToList();

        // ����� Creature ������Ʈ���� ���ο� ���������� ��ü
        foreach (GameObject closestCreature in closestCreatures)
        {
            Vector3 position = closestCreature.transform.position;
            Quaternion rotation = closestCreature.transform.rotation;
            Destroy(closestCreature);
            Instantiate(atk2Prefab, position, rotation);
        }

        // �ִϸ��̼��� ���� ������ ��ٸ�
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // ���� ���·� ���ư���
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
        float controlSpeed = 20.0f; // �÷��̾ ������ �� ������ �ӵ�
        float controlRotationSpeed = 720.0f; // �÷��̾ ������ �� ������ ȸ�� �ӵ�

        float lastFireTime = Time.time;
        float controlDuration = 10.0f; // ���� �ð� 10��
        float controlEndTime = Time.time + controlDuration;

        while (Time.time < controlEndTime)
        {
            // ����Ű�� ���� ����
            float moveHorizontal = Input.GetAxis("Boss_Horizontal");
            float moveVertical = Input.GetAxis("Boss_Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

            if (movement != Vector3.zero)
            {
                // ���� ��ġ ������Ʈ
                transform.position += movement * controlSpeed * Time.deltaTime;

                // �̵� �������� ���� ȸ��
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, controlRotationSpeed * Time.deltaTime);

                // �ִϸ��̼� Ʈ���� ����
                ResetAllTriggers();
                animator.SetTrigger("isRun");

                // 0.1�ʸ��� �� ������ ����
                if (Time.time - lastFireTime >= 0.1f)
                {
                    Vector3 firePosition = transform.position - transform.forward * 20f; // ���� ���ʿ� �� ����
                    Instantiate(atk3Prefab, firePosition, Quaternion.identity);
                    lastFireTime = Time.time;
                }
            }
            else
            {
                // �̵����� ������ Idle ���·� ����
                ResetAllTriggers();
                animator.SetTrigger("isIdle");
            }

            yield return null;
        }

        // �ִϸ��̼� Ʈ���Ÿ� ��� false�� ����
        ResetAllTriggers();

        // ���� ���·� ���ư���
        speed = originalSpeed;
        rotationSpeed = originalRotationSpeed;
        isControlled = false;
        isAttacking = false;
    }

    private IEnumerator StartSliderCountdown()
    {
        // �����̴��� Ȱ��ȭ�ϰ� �� ���̱� ����
        uiSlider.gameObject.SetActive(true);
        uiSlider.value = 1.0f; // �����̴��� �� �� ���·� ����

        float startTime = Time.time;

        while (Time.time < startTime + 10.0f) // 10�� ���� �����̴� ���� ����
        {
            uiSlider.value = Mathf.Lerp(1.0f, 0.0f, (Time.time - startTime) / 10.0f);
            yield return null;
        }

        // �����̴��� ������ �� ���·� ����
        uiSlider.value = 0.0f;

        // �����̴��� ���� ������Ʈ�� ��Ȱ��ȭ
        uiSlider.gameObject.SetActive(false);
    }

    private void ResetAllTriggers()
    {
        // ��� Ʈ���Ÿ� �����Ͽ� ���� �ִϸ��̼� ���°� ���� �ʵ��� ��
        animator.ResetTrigger("isIdle");
        animator.ResetTrigger("isRun");
        animator.ResetTrigger("ATK1");
        animator.ResetTrigger("ATK2");
        animator.ResetTrigger("ATK0"); // ATK0 Ʈ���� ���� �߰�
    }
}
