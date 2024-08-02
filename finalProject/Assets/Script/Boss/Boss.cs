using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ�� ������ ����
    public float speed = 5.0f; // ������ �̵��� �ӵ�
    public float rotationSpeed = 5.0f; // ������ ȸ���� �ӵ�
    public GameObject atk1Prefab; // ATK1 ������
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

        // Q Ű�� ������ �� ���� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Attack());
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
}
