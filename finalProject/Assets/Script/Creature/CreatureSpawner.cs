using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // �÷��̾� ������
    public GameObject creaturePrefab1; // 1�� �� ������Ʈ �������� �Ҵ��� ����
    public GameObject creaturePrefab2; // 2�� �� ������Ʈ �������� �Ҵ��� ����
    public float spawnRange = 5f; // �÷��̾���� �ּ� ��ȯ ����

    private int selectedCreature = 1; // ���� ���õ� ���� ��ȣ

    void Update()
    {
        // Ű���� ���� 1�� ������ 1�� �� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedCreature = 1;
            Debug.Log("Selected creature: " + selectedCreature);
        }
        // Ű���� ���� 2�� ������ 2�� �� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedCreature = 2;
            Debug.Log("Selected creature: " + selectedCreature);
        }

        // ���콺 ��Ŭ���� �����ϴ� �κ�
        if (Input.GetMouseButtonDown(0)) // 0�� ���콺 ��Ŭ���� �ǹ��մϴ�.
        {
            SpawnCreature();
        }
    }

    void SpawnCreature()
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
                    if (selectedCreature == 1)
                    {
                        Instantiate(creaturePrefab1, hit.point, Quaternion.identity);
                    }
                    else if (selectedCreature == 2)
                    {
                        Instantiate(creaturePrefab2, hit.point, Quaternion.identity);
                    }
                }
            }
        }
    }
}
