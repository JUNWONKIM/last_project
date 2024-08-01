using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // �÷��̾� ������
    public GameObject[] creaturePrefabs; // �� ������Ʈ ������ �迭
    public float spawnRange = 30f; // �÷��̾���� �ּ� ��ȯ ����

    private const string GroundTag = "ground";
    private const int LeftMouseButton = 0;
    public int selectedCreature = 1;

    private LineRenderer lineRenderer;

    public UI_selectCreature[] uiButtons; // UI ��ư ��ũ��Ʈ �迭

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 360;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = false; // ���� ��ǥ�� ���
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    public bool IsWithinSpawnRange(Vector3 position)
    {
        Vector3 playerPosition = playerPrefab.transform.position;
        return Vector3.Distance(playerPosition, position) <= spawnRange;
    }

    void Update()
    {
        HandleCreatureSelection();

        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            if (selectedCreature == 4)
            {
                SpawnRandomCreature();
            }
            else
            {
                SpawnSelectedCreature(selectedCreature - 1);
            }
        }

        DrawSpawnRange();
    }

    void HandleCreatureSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedCreature = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedCreature = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedCreature = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedCreature = 4;
        }
    }

    void DrawSpawnRange()
    {
        Vector3 playerPosition = playerPrefab.transform.position;
        Vector3 center = new Vector3(playerPosition.x, 1f, playerPosition.z);

        for (int i = 0; i < 360; i++)
        {
            float rad = Mathf.Deg2Rad * i;
            float x = center.x + Mathf.Cos(rad) * spawnRange;
            float z = center.z + Mathf.Sin(rad) * spawnRange;
            lineRenderer.SetPosition(i, new Vector3(x, center.y, z));
        }
    }

    void SpawnSelectedCreature(int index)
    {
        if (index < creaturePrefabs.Length)
        {
            if (!uiButtons[index].IsOnCooldown())
            {
                GameObject creatureToSpawn = creaturePrefabs[index];
                SpawnCreature(creatureToSpawn);

                // ��ư�� ��Ÿ�� ����
                TriggerButtonCooldown(index);
            }
        }
    }

    void SpawnRandomCreature()
    {
        int randomIndex = Random.Range(3, creaturePrefabs.Length);
        if (!uiButtons[randomIndex].IsOnCooldown())
        {
            GameObject creatureToSpawn = creaturePrefabs[randomIndex];
            SpawnCreature(creatureToSpawn);

            // ��ư�� ��Ÿ�� ����
            TriggerButtonCooldown(randomIndex);
        }
    }

    void SpawnCreature(GameObject creaturePrefab)
    {
        Vector3 playerPosition = playerPrefab.transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag(GroundTag))
        {
            Vector3 spawnPosition = hit.point;

            if (Vector3.Distance(new Vector3(playerPosition.x, 0f, playerPosition.z), new Vector3(spawnPosition.x, 0f, spawnPosition.z)) > spawnRange)
            {
                if (creaturePrefab != null)
                {
                    Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    void TriggerButtonCooldown(int index)
    {
        if (index < uiButtons.Length)
        {
            uiButtons[index].StartCooldown();
        }
    }
}
