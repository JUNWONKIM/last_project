using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_ATK3_cooldown : MonoBehaviour
{
    private Slider uiSlider;
    private bool isActive = false;
    private GameObject bossObject;

    void Start()
    {
        // Slidebar_ATK3 �±׸� ���� �����̴��� ã��
        GameObject sliderObject = GameObject.FindGameObjectWithTag("Slidebar_ATK3");

        if (sliderObject != null)
        {
            uiSlider = sliderObject.GetComponent<Slider>();
            uiSlider.gameObject.SetActive(false); // ������ �� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogWarning("Slidebar_ATK3 �±׸� ���� �����̴��� ã�� �� �����ϴ�.");
        }
    }

    void Update()
    {
        // Boss ������Ʈ�� ã�� (�� �����Ӹ��� Ȯ��)
        bossObject = GameObject.FindGameObjectWithTag("Boss");

        // Boss ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ� ��쿡�� �����̴��� �۵�
        if (bossObject != null && bossObject.activeInHierarchy)
        {
            // C Ű�� ������ �� �����̴��� Ȱ��ȭ�ϰ� ī��Ʈ�ٿ� ����
            if (Input.GetKeyDown(KeyCode.C) && !isActive && uiSlider != null)
            {
                StartCoroutine(StartSliderCountdown());
            }
        }
    }

    private IEnumerator StartSliderCountdown()
    {
        isActive = true;

        uiSlider.gameObject.SetActive(true); // �����̴� Ȱ��ȭ
        uiSlider.value = 1.0f; // �����̴��� �� �� ���·� ����

        float startTime = Time.time;

        while (Time.time < startTime + 10.0f) // 10�� ���� �����̴� ���� ����
        {
            uiSlider.value = Mathf.Lerp(1.0f, 0.0f, (Time.time - startTime) / 10.0f);
            yield return null;
        }

        // �����̴��� ������ �� ���·� ����
        uiSlider.value = 0.0f;

        // �����̴��� ��Ȱ��ȭ
        uiSlider.gameObject.SetActive(false);

        isActive = false;
    }
}