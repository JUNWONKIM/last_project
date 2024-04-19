using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject playerPrefab; // �÷��̾� ������
    public GameObject enemyPrefab1; // 1�� �� ������Ʈ �������� �Ҵ��� ����
    public GameObject enemyPrefab2; // 2�� �� ������Ʈ �������� �Ҵ��� ����
    public float spawnRange = 5f; // �÷��̾���� �ּ� ��ȯ ����

    private int selectedEnemy = 1; // ���� ���õ� ���� ��ȣ

    void Update()
    {
        // Ű���� ���� 1�� ������ 1�� �� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedEnemy = 1;
            Debug.Log("Selected enemy: " + selectedEnemy);
        }
        // Ű���� ���� 2�� ������ 2�� �� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedEnemy = 2;
            Debug.Log("Selected enemy: " + selectedEnemy);
        }

        // ���콺 ��Ŭ���� �����ϴ� �κ�
        if (Input.GetMouseButtonDown(0)) // 0�� ���콺 ��Ŭ���� �ǹ��մϴ�.
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
                    // ���� ���õ� ���� ���� �ش� ���� ��ȯ�մϴ�.
                    if (selectedEnemy == 1)
                    {
                        Instantiate(enemyPrefab1, hit.point, Quaternion.identity);
                    }
                    else if (selectedEnemy == 2)
                    {
                        Instantiate(enemyPrefab2, hit.point, Quaternion.identity);
                    }
                }
            }
        }
    }
}
