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
    private bool isAttacking = false; // ������ ���� ������ ����
    private Animator animator; // �ִϸ����� ������Ʈ

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� ���� ���� �̵����� ����
        if (isAttacking)
            return;

        // �÷��̾ ���� �ٰ�����
        Vector3 direction = (player.position - transform.position).normalized;

        // �÷��̾ �ٶ󺸱�
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // �÷��̾ ���� �̵��ϱ�
        transform.position += transform.forward * speed * Time.deltaTime;

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
    }
}
