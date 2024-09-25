using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_BossHp : MonoBehaviour
{
    public Slider healthSlider; // ü���� ǥ���� �����̴�

    // ������ ü���� �����ϰ� UI ������Ʈ
    public void SetBossHealth(BossHP bossHealth)
    {
        if (bossHealth != null && healthSlider != null)
        {
            healthSlider.maxValue = bossHealth.maxHealth;
            healthSlider.value = bossHealth.currentHealth;

            // ���������� ������ ü�� UI�� ������Ʈ�ϱ� ���� �ڷ�ƾ�� ����
            StartCoroutine(UpdateHealthBar(bossHealth));
        }
    }

    private IEnumerator UpdateHealthBar(BossHP bossHealth)
    {
        // ������ ����ִ� ���� ü�¹ٸ� ������Ʈ
        while (bossHealth != null && bossHealth.currentHealth > 0)
        {
            healthSlider.value = bossHealth.currentHealth;
            yield return null; // �� �����Ӹ��� ������Ʈ
        }

        // ������ ������ ü�¹ٸ� ����
        healthSlider.gameObject.SetActive(false);
    }
}
