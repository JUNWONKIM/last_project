using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance; // �̱��� �ν��Ͻ�

    public GameObject[] bulletPrefabs; // �Ѿ� ������ �迭
    public int poolSizePerBulletType = 10; // �� �Ѿ� Ÿ�Ժ� Ǯ�� �ʱ� ũ��

    private Dictionary<GameObject, List<GameObject>> bulletPools; // �Ѿ� Ǯ ��ųʸ�

    void Awake()
    {
        instance = this;
        InitializePools();
    }

    // Ǯ �ʱ�ȭ
    void InitializePools()
    {
        bulletPools = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject bulletPrefab in bulletPrefabs)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < poolSizePerBulletType; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                pool.Add(bullet);
            }

            bulletPools.Add(bulletPrefab, pool);
        }
    }

    // ��� ������ �Ѿ� ������Ʈ ��ȯ
    public GameObject GetBulletFromPool(GameObject bulletPrefab)
    {
        if (!bulletPools.ContainsKey(bulletPrefab))
        {
            Debug.LogError("Bullet prefab is not in the pool.");
            return null;
        }

        List<GameObject> pool = bulletPools[bulletPrefab];
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // ��� ������ �Ѿ��� ������ Ǯ�� �߰� �����Ͽ� ��ȯ
        GameObject newBullet = Instantiate(bulletPrefab);
        pool.Add(newBullet);
        return newBullet;
    }

    // �Ѿ� ������Ʈ�� Ǯ�� ��ȯ
    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
