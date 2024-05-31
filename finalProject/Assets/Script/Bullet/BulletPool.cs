using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance; // 싱글톤 인스턴스

    public GameObject[] bulletPrefabs; // 총알 프리팹 배열
    public int poolSizePerBulletType = 10; // 각 총알 타입별 풀의 초기 크기

    private Dictionary<GameObject, List<GameObject>> bulletPools; // 총알 풀 딕셔너리

    void Awake()
    {
        instance = this;
        InitializePools();
    }

    // 풀 초기화
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

    // 사용 가능한 총알 오브젝트 반환
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

        // 사용 가능한 총알이 없으면 풀에 추가 생성하여 반환
        GameObject newBullet = Instantiate(bulletPrefab);
        pool.Add(newBullet);
        return newBullet;
    }

    // 총알 오브젝트를 풀에 반환
    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
