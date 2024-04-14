using UnityEngine;

public class Player_Test : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float detectionRange = 10f; // Ž�� ����
    public Color detectionColor = Color.red; // Ž�� ������ ����
    public float updateInterval = 1f; // ������Ʈ ����
    public float lookAtInterval = 0.2f; // �ٶ󺸱� ����

    private LineRenderer detectionRangeVisual; // �� Ž�� ������ �ð������� ǥ���� ���� ������

    private void Start()
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

        // �ڷ�ƾ ����
        InvokeRepeating("UpdateDetection", 0f, updateInterval);
        InvokeRepeating("LookAtEnemy", 0f, lookAtInterval);
    }

    private void Update()
    {
        // �÷��̾� �̵�
        MovePlayer();
    }

    private void MovePlayer()
    {
        // ���� ���� �����߽� �������� �̵�
        Vector3 centerOfMass = FindCenterOfMass();
        Vector3 moveDirection = centerOfMass - transform.position;
        moveDirection.y = 0; // Y �� �̵� ����
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    private void UpdateDetection()
    {
        // Ž�� ������ �ð������� ��Ÿ����
        DrawDetectionRange(transform.position, Vector3.up, detectionRange, detectionColor);
    }

    // �ֺ��� �ִ� ������ ��ġ�� �����߽��� ã�� �Լ�
    private Vector3 FindCenterOfMass()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // ������ ���� ������Ʈ�� ������

        Vector3 center = Vector3.zero;
        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            // ������ �Ÿ��� ����Ͽ� Ž�� ���� ���� �ִ��� Ȯ��
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange)
            {
                center += enemy.transform.position; // ���� ��ġ�� ����
                count++;
            }
        }

        if (count == 0)
        {
            // Ž�� ���� ���� ���� ������ ���� ��ġ�� ��ȯ
            return transform.position;
        }

        center /= count; // ����� ���ؼ� �����߽��� ã��

        return center;
    }

    // Ž�� ������ �ð������� ��Ÿ���� �Լ�
    private void DrawDetectionRange(Vector3 position, Vector3 up, float radius, Color color, int segments = 36)
    {
        // �� ����� �� ����
        Vector3[] points = new Vector3[37];
        for (int i = 0; i < 37; i++)
        {
            float angle = i * Mathf.PI * 2f / 36f;
            points[i] = position + new Vector3(Mathf.Sin(angle) * radius, 0f, Mathf.Cos(angle) * radius);
        }

        // ���� �������� �� ����
        detectionRangeVisual.SetPositions(points);
    }

    private void LookAtEnemy()
    {
        // ���� ����� ���� ã�� �÷��̾ �ش� ���� ���ϵ��� ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            transform.LookAt(closestEnemy.transform);
        }
    }
}
