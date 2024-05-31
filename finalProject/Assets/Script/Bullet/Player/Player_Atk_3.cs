using UnityEngine;

public class Player_Atk_3 : MonoBehaviour
{
    public float rotationSpeed = 180f; // Į�� ȸ�� �ӵ� (Y�� ����)
    public float distanceFromPlayer = 2f; // �÷��̾�κ����� �Ÿ�
    public float lifetime = 3f;
    public float damageAmount = 1f;

    private GameObject player; // �÷��̾� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        // Player �±׸� ���� ������Ʈ�� ã�Ƽ� �ش� ������Ʈ�� ����
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Į�� �÷��̾ �ٶ󺸵��� ����
            transform.LookAt(player.transform);

            // Į�� �÷��̾� ������ �߽����� ȸ����Ű�� �ڵ�
            transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

            // �÷��̾� ��ġ�� �������� �� ���� ���� ���
            Vector3 directionFromPlayer = transform.position - player.transform.position;
            directionFromPlayer.y = 0f; // y �� ������ ������� ����

            // �÷��̾�κ��� ���� �Ÿ��� �����ϱ� ���� ��ġ ���
            Vector3 targetPosition = player.transform.position + directionFromPlayer.normalized * distanceFromPlayer;

            // Į�� �÷��̾�κ��� ���� �Ÿ��� �����ϸ鼭 �̵�
            transform.position = targetPosition;

            transform.Rotate(Vector3.right, -90f);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �±װ� "Creature"�� ���
        if (other.gameObject.CompareTag("Creature"))
        {
            // �浹�� ��ü�� HP�� ���ҽ�Ŵ
            CreatureHealth enemyHealth = other.gameObject.GetComponent<CreatureHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                
            }
        }
    }

    public void IncreaseDamage(float amount)
    {
        damageAmount += amount;
        Debug.Log("Į ������ :  " + damageAmount);
    }
}
