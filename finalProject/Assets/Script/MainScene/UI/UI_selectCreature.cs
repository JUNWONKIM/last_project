using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_selectCreature : MonoBehaviour
{
    public Button myButton;
    public int buttonIndex; // ��ư �ε���
    public float cooldownTime = 0f; // ��ư�� ��Ÿ�� �ð�
    public Color activeColor = Color.white; // ��ư Ȱ��ȭ ����
    public Color cooldownColor = Color.red; // ��ư ��Ÿ�� ����

    private ColorBlock originalColors; // ��ư�� ���� ���� ����
    private bool isOnCooldown = false; // ��ٿ� Ȯ��
    private float cooldownEndTime; // ��Ÿ���� ������ �ð��� ����
    private CreatureSpawner spawner; // CreatureSpawner ��ũ��Ʈ ����

    void Start()
    {
        if (myButton != null)
        {
            originalColors = myButton.colors; // ��ư�� ���� ���� ����
            myButton.onClick.AddListener(OnButtonClick); // ��ư Ŭ�� �̺�Ʈ ����
        }

        spawner = FindObjectOfType<CreatureSpawner>();
        UpdateButtonState(); // ��ư ���� ����
    }

    void Update()
    {
        if (spawner != null && myButton != null)
        {
            UpdateButtonState();
        }

        if (isOnCooldown)
        {
            UpdateCooldown();
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

    void PressButton() // ��ư�� ���� ���
    {
        var colors = myButton.colors;
        colors.normalColor = colors.pressedColor;
        myButton.colors = colors;
    }

    void ReleaseButton() // ��ư�� ������ ���
    {
        myButton.colors = originalColors;
    }

    void OnButtonClick() // ��ư�� ���� ��� ��Ÿ�� ����
    {
        if (!isOnCooldown)
        {
            StartCooldown();
        }
    }

    public void StartCooldown() // ��Ÿ�� ����
    {
        isOnCooldown = true;
        cooldownEndTime = Time.time + cooldownTime;

        if (myButton.image != null)
        {
            myButton.image.color = cooldownColor; // ��ư ���� ����
        }
    }

    void UpdateCooldown() //��Ÿ�� ����
    {
        float remainingTime = cooldownEndTime - Time.time; //���� �ð�

        if (remainingTime <= 0) //��Ÿ�� ����
        {
            EndCooldown(); 
        }
        else
        {
            // ��ư ���� ���� ��� ����
            if (myButton.image != null)
            {
                myButton.image.color = Color.Lerp(cooldownColor, activeColor, 1 - (remainingTime / cooldownTime));
            }
        }
    }

    void EndCooldown() //��Ÿ�� ����
    {
        isOnCooldown = false;

        // ��ư ������ ���� �������� ����
        if (myButton.image != null)
        {
            myButton.image.color = activeColor;
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
