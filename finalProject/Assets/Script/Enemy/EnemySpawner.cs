using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject playerPrefab; // �÷��̾� ������
    public GameObject enemyPrefab; // �� ������Ʈ �������� �Ҵ��� ����
    public float spawnRange = 5f; // �÷��̾���� �ּ� ��ȯ ����

    void Update()
    {
        // ���콺 ��Ŭ���� �����ϴ� �κ�
        if (Input.GetMouseButtonDown(0)) // 1�� ���콺 ��Ŭ���� �ǹ��մϴ�.
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // �÷��̾� �������� ��ġ�� �����ɴϴ�.
        Vector3 playerPosition = playerPrefab.transform.position;

        // ���콺 Ŀ�� ��ġ�� �������� ���̸� ���� �浹�ϴ� ������ ã���ϴ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // �浹 ������ �±װ� "ground"�� ���� ���� ��ȯ�մϴ�.
            if (hit.collider.CompareTag("ground"))
            {
                Vector3 spawnPosition = hit.point; // ��ȯ ��ġ ��������

                // �÷��̾���� �Ÿ��� ����Ͽ� Ư�� ���� ���̸� ���� ��ȯ�մϴ�.
                if (Vector3.Distance(new Vector3(playerPosition.x, 0f, playerPosition.z), new Vector3(spawnPosition.x, 0f, spawnPosition.z)) > spawnRange)
                {
                    Instantiate(enemyPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
