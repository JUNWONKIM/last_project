using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ�� ������ ����
    public float speed = 5.0f; // ������ �̵��� �ӵ�
    public float rotationSpeed = 5.0f; // ������ ȸ���� �ӵ�
    public GameObject atk1Prefab; // ATK1 ������
    public GameObject replacementPrefab; // ��ü�� ������
    public GameObject firePrefab; // ������ �̵��� �ڸ��� ���� �� ������
    public float keepDistance = 5.0f; // �÷��̾�� ������ �ּ� �Ÿ�
    private bool isAttacking = false; // ������ ���� ������ ����
    private bool isControlled = false; // ������ �÷��̾ ���� ����Ǵ��� ����
    private Animator animator; // �ִϸ����� ������Ʈ

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� ���̰ų� �÷��̾ ���� ����Ǵ� ���� ���� �̵����� ����
        if (isAttacking || isControlled)
            return;

        // �÷��̾���� �Ÿ� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > keepDistance)
        {
            // �÷��̾ ���� �ٰ�����
            Vector3 direction = (player.position - transform.position).normalized;

            // �÷��̾ �ٶ󺸱�
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // �÷��̾ ���� �̵��ϱ�
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        // Z Ű�� ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Attack());
        }

        // X Ű�� ������ �� ���� ����� Creature �������� ��ü
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(ReplaceClosestCreatures());
        }

        // C Ű�� ������ �� 10�� ���� �÷��̾ ���� ����
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(ControlBoss());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // �ִϸ��̼� Ʈ���� ����
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

        // �ִϸ��̼� Ʈ���� ����
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
            Instantiate(replacementPrefab, position, rotation);
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
        float originalSpeed = speed;
        float originalRotationSpeed = rotationSpeed;
        float controlSpeed = 20.0f; // �÷��̾ ������ �� ������ �ӵ�
        float controlRotationSpeed = 720.0f; // �÷��̾ ������ �� ������ ȸ�� �ӵ�

        // �ִϸ������� �ӵ��� 2��� ����
        float originalAnimatorSpeed = animator.speed;
        animator.speed = 2.0f;

        float startTime = Time.time;
        while (Time.time - startTime < 10.0f)
        {
            // ����Ű�� ���� ����
            float moveHorizontal = Input.GetAxis("Boss_Horizontal");
            float moveVertical = Input.GetAxis("Boss_Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

            // ���� ��ġ�� �̵� ���� ���
            Vector3 newPosition = transform.position + movement * controlSpeed * Time.deltaTime;

            // �÷��̾���� �Ÿ� Ȯ��
            float distanceToPlayer = Vector3.Distance(newPosition, player.position);
            if (distanceToPlayer <= keepDistance)
            {
                // �÷��̾� �ݴ������� �����̵�
                Vector3 directionToPlayer = (newPosition - player.position).normalized;
                newPosition = player.position + directionToPlayer * keepDistance;
            }

            // ���� ��ġ ������Ʈ
            transform.position = newPosition;

            // �̵� �������� ���� ȸ��
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, controlRotationSpeed * Time.deltaTime);
            }

            // ������ �̵��� �ڸ��� �� ������ ����
            Instantiate(firePrefab, transform.position, Quaternion.identity);

            yield return null;
        }

        // �ִϸ����� �ӵ��� ������� ����
        animator.speed = originalAnimatorSpeed;

        // ���� ���·� ���ư���
        speed = originalSpeed;
        rotationSpeed = originalRotationSpeed;
        isControlled = false;
    }
}
