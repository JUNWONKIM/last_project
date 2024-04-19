using UnityEngine;

public class Enemy_2 : MonoBehaviour
{
    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float stoppingDistance = 5f; // �÷��̾���� ���ߴ� �Ÿ�
    public float retreatDistance = 5f; // �÷��̾�κ��� �����ϴ� �Ÿ�

    public GameObject projectile; // �߻��� ����ü
    public Transform firePoint; // �߻� ����
    public float fireRate = 1f; // �߻� �ӵ� (1�ʴ� �� ��)

    private Transform player; // �÷��̾��� ��ġ
    private float nextFireTime = 0f; // ���� �߻� �ð�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� ��ġ ã��
    }

    void Update()
    {

        if (player != null)
        {
            // �÷��̾ ���� �̵�
            if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.LookAt(player);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            // �÷��̾�� ���� �Ÿ��� �Ǹ� ���߰� ����ü �߻�
            else if (Vector3.Distance(transform.position, player.position) <= stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
            {
                if (Time.time >= nextFireTime)
                {
                    // �÷��̾ ���� ����ü �߻�
                    transform.LookAt(player);
                    Vector3 direction = player.position - firePoint.position;
                    direction.Normalize();
                    GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody>().velocity = direction * 100f; // ����ü �ӵ�
                    nextFireTime = Time.time + 1f / fireRate; // ���� �߻� �ð� ����
                }
            }
            // �÷��̾�κ��� ���� �Ÿ� �̻� �������� ����
            else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
            }
        }
    }
}
