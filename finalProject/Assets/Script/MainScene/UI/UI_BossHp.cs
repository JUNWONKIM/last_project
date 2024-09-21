using UnityEngine;
using UnityEngine.UI;

public class UI_BossHp : MonoBehaviour
{
    private Image healthBar; // ü���� ǥ���� UI �̹���

    void Start()
    {
        healthBar = GetComponent<Image>(); // ���� GameObject�� ������ �̹��� ������Ʈ�� �����ɴϴ�.
    }

    public void SetBossHealth(CreatureHealth bossHealth)
    {
        if (bossHealth != null)
        {
            // ���� ü���� �ִ� ü������ ������ ������ ���
            float healthPercentage = bossHealth.currentHealth / bossHealth.maxHealth;
            healthBar.fillAmount = healthPercentage; // UI �̹����� fillAmount�� ������Ʈ
        }
    }
}
