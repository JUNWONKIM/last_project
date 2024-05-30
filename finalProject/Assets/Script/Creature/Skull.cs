using UnityEngine;

public class Skull : MonoBehaviour
{
    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float retreatDistance = 2f; // �÷��̾���� �Ÿ��� �� �� ������ �� �����ϴ� �Ÿ�
    public float retreatSpeed = 3f; // ���� �ӵ�
    private Transform player; // �÷��̾��� ��ġ
    private Rigidbody rb; // ���� Rigidbody ������Ʈ
    private Animator animator; // ���� Animator ������Ʈ

    void Start()
    {
        // �÷��̾� ���� ������Ʈ�� ã�� Ʈ�������� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    void FixedUpdate()
    {
        // isDie�� false�� ��쿡�� �̵� �� ȸ�� ����
        if (!animator.GetBool("isDie"))
        {
            // �÷��̾���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > retreatDistance)
            {
                // �÷��̾ ���� �̵�
                Vector3 moveDirection = (player.position - transform.position).normalized;
                rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                // �÷��̾�� �ʹ� ������ ����
                Vector3 retreatDirection = (transform.position - player.position).normalized;
                rb.MovePosition(transform.position + retreatDirection * retreatSpeed * Time.fixedDeltaTime);
            }

            // ���� �÷��̾ �ٶ󺸵��� ȸ��
            Vector3 lookDirection = (player.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(rotation);
        }
    }

    // Animator���� isDie�� true�� �� �� ȣ��Ǵ� �Լ�
   
}
