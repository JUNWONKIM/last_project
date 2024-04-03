using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������Ʈ �������� �Ҵ��� ����

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
        // ���콺 Ŀ�� ��ġ�� �������� ���̸� ���� �浹�ϴ� ������ ã��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // �浹 ������ ���� ��ȯ
            Instantiate(enemyPrefab, hit.point, Quaternion.identity);
        }
    }
}
