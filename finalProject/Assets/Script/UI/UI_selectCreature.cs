using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_selectCreature : MonoBehaviour
{
    public Button myButton;
    public int buttonIndex; // �� ��ư�� �ε��� (1���� 4����)
    private ColorBlock originalColors;
    private CreatureSpawner spawner;

    void Start()
    {
        if (myButton != null)
        {
            // ��ư�� ���� ���� ����
            originalColors = myButton.colors;
        }

        // CreatureSpawner ������Ʈ�� ã���ϴ�.
        spawner = FindObjectOfType<CreatureSpawner>();

        // ������ �� ��ư ���¸� �����մϴ�.
        UpdateButtonState();
    }

    void Update()
    {
        if (spawner != null && myButton != null)
        {
            // selectedCreature ���� ���� ��ư ���¸� ������Ʈ�մϴ�.
            UpdateButtonState();
        }
    }

    void UpdateButtonState()
    {
        if (spawner.selectedCreature == buttonIndex)
        {
            PressButton();
        }
        else
        {
            ReleaseButton();
        }
    }

    void PressButton()
    {
        var colors = myButton.colors;
        colors.normalColor = colors.pressedColor;
        myButton.colors = colors;

        // ��ư�� ������ ������ �� ����Ǵ� �ݹ� �Լ� ȣ�� (���û���)
        // myButton.onClick.Invoke();
    }

    void ReleaseButton()
    {
        myButton.colors = originalColors;
    }
}
