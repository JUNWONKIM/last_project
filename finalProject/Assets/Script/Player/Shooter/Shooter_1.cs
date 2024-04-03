using UnityEngine;

public class Shooter_1 : MonoBehaviour
{
    public GameObject projectilePrefab; // �߻�ü �������� �Ҵ��� ����
    public float fireInterval = 1f; // �߻� ����
    public float detectionRange = 100f; // ���� Ž���� ����
    public float projectileSpeed = 100f;
    private float lastFireTime; // ������ �߻� �ð�
    private LineRenderer detectionRangeVisual; // �� Ž�� ������ �ð������� ǥ���� ���� ������

    void Start()
    {
        // LineRenderer ������Ʈ �߰�
        detectionRangeVisual = gameObject.AddComponent<LineRenderer>();

        // ���� ������ ����
        detectionRangeVisual.material = new Material(Shader.Find("Sprites/Default"));
        detectionRangeVisual.startColor = Color.red;
        detectionRangeVisual.endColor = Color.red;
        detectionRangeVisual.startWidth = 0.1f;
        detectionRangeVisual.endWidth = 0.1f;
        detectionRangeVisual.positionCount = 37; // ���� ������ ����

        // �� ����� �� ����
        Vector3[] points = new Vector3[37];
        for (int i = 0; i < 37; i++)
        {
            float angle = i * Mathf.PI * 2f / 36f;
            points[i] = new Vector3(Mathf.Sin(angle) * detectionRange, 0f, Mathf.Cos(angle) * detectionRange);
        }

        // ���� �������� �� ����
        detectionRangeVisual.SetPositions(points);
    }

    void Update()
    {
        // ���� �������� ���� ����� ���� Ž���ϰ� �߻�ü�� �߻�
        if (Time.time - lastFireTime > fireInterval)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }
    }

    void FireProjectile()
    {
        // ���� ����� ���� Ž��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= detectionRange)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        // �߻�ü�� �߻��� ���� �ִ� ��� �߻�
        if (closestEnemy != null)
        {
            Vector3 targetDirection = closestEnemy.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, rotation);

            // �߻�ü�� �̵� �ӵ� �ο�
            Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                // �߻��� ���� �������� �߻�ü�� �̵���Ŵ
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component.");
            }
        }
    }
}
