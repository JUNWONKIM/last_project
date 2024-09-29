using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float damageAmount = 1f; //������


    private Transform player;
    private Rigidbody rb;
    private Animator animator; 
    private bool canDealDamage = true; // �������� �� �� �ִ� ���� ����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
       
        if (!animator.GetBool("isDie")) // ��� ���� ���
        {
            // ��縦 ���� �̵�
            Vector3 moveDirection = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            // ��縦 ���� ȸ��
            Vector3 lookDirection = (player.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(rotation);
        }
    }

  
    private IEnumerator DamageCooldown() //���ظ� ������ 0.5�� ���ظ� ���� ���ϰ� ����
    {
        canDealDamage = false;
        yield return new WaitForSeconds(0.5f);
        canDealDamage = true;
    }


    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player") && canDealDamage)
        {
            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.hp -= damageAmount;  //��翡�� ���ظ� ��
                StartCoroutine(DamageCooldown()); //���� ��Ÿ�� 
            }
        }
    }

}
