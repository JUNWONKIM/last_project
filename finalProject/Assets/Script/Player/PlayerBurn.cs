using System.Collections;
using UnityEngine;

public class PlayerBurn : MonoBehaviour
{
    public float burnDamage = 5.0f; // ȭ�� ���¿��� �ִ� ������
    public float burnDuration = 10.0f; // ȭ�� ���� ���� �ð�
    public GameObject burnEffectPrefab; // ȭ�� ����Ʈ ������

    private float burnEndTime = -1.0f; // ȭ�� ���� ���� �ð�
    private PlayerHP playerHP; // �÷��̾��� ü�� ��ũ��Ʈ
    private GameObject currentBurnEffect; // ���� ������ ȭ�� ����Ʈ

    void Start()
    {
        playerHP = GetComponent<PlayerHP>();
    }

    void Update()
    {
        if (burnEndTime > Time.time)
        {
            // ȭ�� ������ ����
            playerHP.TakeDamage(burnDamage * Time.deltaTime);

            // ȭ�� ����Ʈ�� ���� �÷��̾��� ��ġ�� ��ġ�ϵ��� ������Ʈ
            if (currentBurnEffect != null)
            {
                currentBurnEffect.transform.position = transform.position;
            }
        }
        else if (currentBurnEffect != null)
        {
            // ȭ�� ���°� ������ ����Ʈ�� ����
            Destroy(currentBurnEffect);
        }
    }

    public void ApplyBurn()
    {
        // ȭ�� ���� ������Ʈ
        burnEndTime = Mathf.Max(burnEndTime, Time.time + burnDuration);

        // ������ ȭ�� ����Ʈ�� �ִ� ��� ����
        if (currentBurnEffect != null)
        {
            Destroy(currentBurnEffect);
        }

        // ���ο� ȭ�� ����Ʈ ����
        currentBurnEffect = Instantiate(burnEffectPrefab, transform.position, Quaternion.identity);
    }
}
