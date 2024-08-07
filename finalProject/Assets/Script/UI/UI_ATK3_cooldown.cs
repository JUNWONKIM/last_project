using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_ATK3_cooldown : MonoBehaviour
{
    public Slider slider; // �����̴� UI ������Ʈ
    public float duration = 10.0f; // �����̴��� �پ�� ��ü �ð�

    void Start()
    {
        // �����̴��� ó���� ��Ȱ��ȭ ���·� ����
        slider.gameObject.SetActive(false);
    }

    public void StartSlider()
    {
        StartCoroutine(StartSliderCountdown());
    }

    private IEnumerator StartSliderCountdown()
    {
        slider.gameObject.SetActive(true); // �����̴��� Ȱ��ȭ
        slider.value = 1.0f; // �����̴��� �� �� ���·� ����

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // �����̴��� ���� �پ��� ����
            slider.value = Mathf.Lerp(1.0f, 0.0f, (Time.time - startTime) / duration);
            yield return null;
        }

        // �����̴��� ������ �� ���·� ����
        slider.value = 0.0f;

        // �����̴��� ���� ������Ʈ�� ��Ȱ��ȭ
        slider.gameObject.SetActive(false);
    }
}