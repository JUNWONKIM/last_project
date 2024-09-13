using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject groundTilePrefab; // �� Ÿ�� ������
    public float tileSize = 50f; // �� Ÿ���� ũ��
    public float tileSpacing = 125f; // Ÿ�� ���� ����
    public int viewDistance = 5; // �÷��̾� �ֺ��� ������ Ÿ���� ����
    public Transform tilesParent; // Ÿ���� �ڽ����� ���� �θ� ������Ʈ

    private Dictionary<Vector2, GameObject> terrainTiles = new Dictionary<Vector2, GameObject>();
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateInitialTerrain();
    }

    void Update()
    {
        if (playerTransform == null) return;

        GenerateTerrainAroundPlayer();
        RemoveDistantTiles();
    }

    void GenerateInitialTerrain()
    {
        Vector2 playerPosition = GetCurrentPlayerTileCoord();

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2 tileCoord = new Vector2(playerPosition.x + x, playerPosition.y + z);
                CreateTileAt(tileCoord);
            }
        }
    }

    void GenerateTerrainAroundPlayer()
    {
        Vector2 playerPosition = GetCurrentPlayerTileCoord();

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2 tileCoord = new Vector2(playerPosition.x + x, playerPosition.y + z);

                if (!terrainTiles.ContainsKey(tileCoord))
                {
                    CreateTileAt(tileCoord);
                }
            }
        }
    }

    void RemoveDistantTiles()
    {
        List<Vector2> tilesToRemove = new List<Vector2>();
        Vector2 playerPosition = GetCurrentPlayerTileCoord();

        foreach (var tile in terrainTiles)
        {
            if (Vector2.Distance(tile.Key, playerPosition) > viewDistance)
            {
                tilesToRemove.Add(tile.Key);
            }
        }

        foreach (var tileCoord in tilesToRemove)
        {
            Destroy(terrainTiles[tileCoord]);
            terrainTiles.Remove(tileCoord);
        }
    }

    Vector2 GetCurrentPlayerTileCoord()
    {
        // �÷��̾��� ��ġ�� �������� ���� Ÿ�� ��ǥ ���
        float xCoord = Mathf.Floor(playerTransform.position.x / tileSpacing);
        float zCoord = Mathf.Floor(playerTransform.position.z / tileSpacing);

        return new Vector2(xCoord, zCoord);
    }

    void CreateTileAt(Vector2 tileCoord)
    {
        // Ÿ���� ��ġ�� ������ ��ġ�� ���
        Vector3 tilePosition = new Vector3(tileCoord.x * tileSpacing, 0, tileCoord.y * tileSpacing);

        // Ÿ���� ��ġ�� ȸ���� ������ ������ ����
        GameObject newTile = Instantiate(groundTilePrefab, tilePosition, Quaternion.identity);

        // Ÿ���� �θ� ����
        newTile.transform.parent = tilesParent;

        newTile.transform.localScale = new Vector3(tileSize, 1, tileSize);

        // Ÿ�� ��ųʸ��� �߰�
        terrainTiles.Add(tileCoord, newTile);
    }
}
