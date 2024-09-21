using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float hp = 1000f;
    public float max_hp = 1000f;
    public GameObject bossPrefab; // Boss ������
    public float bossSpawnRadius = 200f; // Boss ���� �ݰ�
    private GameObject boss; // ���� ��Ȱ��ȭ �Ǿ� �ִ� Boss
    public UI_BossHp uiBossHp; // UI_BossHp ��ũ��Ʈ ����

    void Start()
    {
        hp = max_hp;

        if (bossPrefab != null)
        {
            boss = Instantiate(bossPrefab);
            boss.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
        }
    }

    void Update()
    {
        if (hp <= max_hp * 0.3f && boss != null && !boss.activeInHierarchy)
        {
            SpawnBossNearPlayer();
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log("Player HP: " + hp);
    }

    public void SpawnBossNearPlayer()
    {
        // �÷��̾��� ��ġ�� ������
        Vector3 playerPosition = transform.position;

        // Boss�� �÷��̾� �ֺ� 200 ���� ���� ���� ��ġ�� ����
        Vector3 randomPosition = playerPosition + new Vector3(
            Random.Range(-bossSpawnRadius, bossSpawnRadius),
            0,
            Random.Range(-bossSpawnRadius, bossSpawnRadius)
        );

        // y�� 0���� ����
        randomPosition.y = 0;

        // Boss�� Ȱ��ȭ�ϰ� ��ġ�� ����
        boss.transform.position = randomPosition;
        boss.SetActive(true);

        // �����̴��� ���� ü�� ����
        UI_BossHp uiBossHp = FindObjectOfType<UI_BossHp>();
        if (uiBossHp != null)
        {
            CreatureHealth bossHealth = boss.GetComponent<CreatureHealth>();
            uiBossHp.SetBossHealth(bossHealth);
        }

        // Boss�� �����Ǹ� �� �̻� �������� �ʵ���
        enabled = false;
    }


}
