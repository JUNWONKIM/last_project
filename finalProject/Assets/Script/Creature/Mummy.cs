using UnityEngine;

public class Mummy : MonoBehaviour
{
    public enum State
    {
        MovingTowardsPlayer,
        ChargingTowardsPlayer,
        Retreating,
        Stopped // ���� �߰��� ����: ����
    }

    public float moveSpeed = 5f; // �̵� �ӵ�
    public float chargingSpeed = 10f; // ���� �ӵ�
    public float chargingDistance = 5f; // ���� ���� �Ÿ�
    public float retreatDistance = 2f; // �浹 �� ���� �Ÿ�
    public float retreatDuration = 2f; // ���� ���� �ð�
    private Transform player; // �÷��̾��� ��ġ
    private Rigidbody rb; // ������ Rigidbody ������Ʈ
    private State currentState = State.MovingTowardsPlayer; // ���� ����
    private Vector3 retreatStartPosition; // ���� ���� ��ġ
    private float retreatStartTime; // ���� ���� �ð�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� ��ġ ã��
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
    }

    void Update()
    {
        if (player != null)
        {
            // �÷��̾�� ���� ���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            switch (currentState)
            {
                case State.MovingTowardsPlayer:
                    if (distanceToPlayer <= chargingDistance)
                    {
                        currentState = State.ChargingTowardsPlayer;
                    }
                    else
                    {
                        MoveTowardsPlayer();
                    }
                    break;
                case State.ChargingTowardsPlayer:
                    if (distanceToPlayer >= retreatDistance)
                    {
                        currentState = State.Stopped; // �浹���� �ʰ� �÷��̾�� �����ϸ� ����
                    }
                    else
                    {
                        ChargeTowardsPlayer();
                    }
                    break;
                case State.Retreating:
                    if (Time.time - retreatStartTime >= retreatDuration)
                    {
                        currentState = State.MovingTowardsPlayer;
                    }
                    else
                    {
                        Retreat();
                    }
                    break;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // �÷��̾ ���� �ٶ󺸵��� ȸ��
        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        rb.MoveRotation(rotation);

        // �÷��̾� �������� �̵�
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }

    void ChargeTowardsPlayer()
    {
        // �÷��̾ ���� �ٶ󺸵��� ȸ��
        Vector3 lookDirection = (player.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        rb.MoveRotation(rotation);

        // �÷��̾� �������� ����
        rb.MovePosition(transform.position + transform.forward * chargingSpeed * Time.deltaTime);
    }

    void Retreat()
    {
        // ���� ���� ��ġ�� �̵�
        rb.MovePosition(Vector3.Lerp(retreatStartPosition, transform.position, (Time.time - retreatStartTime) / retreatDuration));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �浹 �� ���� ���� ���� �� ���� ���·� ����
            currentState = State.Retreating;
            retreatStartPosition = transform.position;
            retreatStartTime = Time.time;
        }
    }
}
