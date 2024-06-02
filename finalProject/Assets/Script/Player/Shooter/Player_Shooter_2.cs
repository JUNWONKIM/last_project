using System.Collections;
using UnityEngine;

public class Player_Shooter_2 : MonoBehaviour
{
    public static Player_Shooter_2 instance;

    public GameObject spawnPrefab; // 총알 프리팹
    public float spawnInterval = 5f; // 총알 소환 간격
    public float spawnRadius = 20f; // 총알 소환 범위 반지름
    public float fixedYPosition = 2.5f; // 고정된 Y 축 위치
    public int projectilesPerFire = 0; // 한 번에 발사할 발사체 수
    public float bulletLifetime = 1.5f; // 총알의 수명
    public float damageAmount = 1f; // 총알의 데미지
    

    private float lastSpawnTime; // 마지막 소환 시간

    private bool isSlowed = false; // Slow 상태 여부
    public float spawnIntervalSlowMultiplier = 2f; // Slow 효과 시 발사 간격 배수

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InvokeRepeating("SpawnBulletWithExplosion", 0f, spawnInterval);
    }

    void Update()
    {
        CheckForSlowObjects();
    }
    void SpawnBulletWithExplosion()
    {
        for (int i = 0; i < projectilesPerFire; i++)
        {
            // 플레이어 주변 원형 범위 내에서 랜덤한 위치 계산
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = fixedYPosition;

            GameObject bullet = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);

            // 총알 파괴 타이머 설정
            Destroy(bullet, bulletLifetime);

            // 총알에 연결된 콜라이더 가져오기
            BulletCollisionHandler collisionHandler = bullet.AddComponent<BulletCollisionHandler>();
            collisionHandler.damageAmount = damageAmount;

            // 콜라이더를 비활성화
            Collider bulletCollider = bullet.GetComponent<Collider>();
            bulletCollider.enabled = false;

            // 일정 시간이 지난 후에 콜라이더를 활성화
            StartCoroutine(EnableColliderAfterDelay(bulletCollider, 1.0f));
        }
    }

    IEnumerator EnableColliderAfterDelay(Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    private void CheckForSlowObjects()
    {
        GameObject[] slowObjects = GameObject.FindGameObjectsWithTag("Slow");

        if (slowObjects.Length > 0 && !isSlowed)
        {
            spawnInterval *= spawnIntervalSlowMultiplier; // 발사 간격을 두 배로 늘림
            isSlowed = true;
        }
        else if (slowObjects.Length == 0 && isSlowed)
        {
            spawnInterval /= spawnIntervalSlowMultiplier; // 발사 간격을 원래대로 돌림
            isSlowed = false;
        }
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectilesPerFire += amount;
        Debug.Log("폭탄 개수 " + projectilesPerFire);
    }

    public void IncreaseDamage(float amount)
    {
       
        damageAmount += amount;
        Debug.Log("폭탄 데미지 : " + damageAmount);
    }

    public void IncreaseFireRate(float amount)
    {
        spawnInterval /= amount;
        if (spawnInterval < 0.1f) spawnInterval = 0.1f; 
        Debug.Log("폭탄 발사 속도 :" + spawnInterval);
    }



    public class BulletCollisionHandler : MonoBehaviour
    {
        public float damageAmount;
        private Player_Shooter_2 shooterInstance;

        // 생성자를 이용하여 Player_Shooter_2 인스턴스를 전달받음
        public BulletCollisionHandler(Player_Shooter_2 shooterInstance)
        {
            this.shooterInstance = shooterInstance;
        }

        void OnTriggerEnter(Collider other)
        {
            // 충돌한 객체의 태그가 "Creature"인 경우
            if (other.gameObject.CompareTag("Creature"))
            {
                // 충돌한 객체의 HP를 감소시킴
                CreatureHealth enemyHealth = other.gameObject.GetComponent<CreatureHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }

                Mummy enemyHealth2 = other.gameObject.GetComponent<Mummy>();
                if (enemyHealth2 != null)
                {
                    enemyHealth2.TakeDamage(damageAmount);
                }
            }

        }
    }
}
