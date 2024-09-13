using UnityEngine;
using UnityEngine.UI;

public class UI_selectCreature : MonoBehaviour
{
    public Button myButton;
    public int buttonIndex; // �� ��ư�� �ε��� (1���� 4����)
    public Image cooldownImage; // ��Ÿ�� ǥ�ÿ� �̹���
    public float cooldownTime = 5f; // ��ư�� ��Ÿ�� �ð�
    public Color activeColor = Color.white; // ��ư Ȱ��ȭ ����
    public Color cooldownColor = Color.red; // ��ư ��Ÿ�� ����

    private ColorBlock originalColors;
    private bool isOnCooldown = false;
    private float cooldownEndTime;
    private CreatureSpawner spawner;

    void Start()
    {
        if (myButton != null)
        {
            // ��ư�� ���� ���� ����
            originalColors = myButton.colors;
            // ��ư Ŭ�� �̺�Ʈ ����
            myButton.onClick.AddListener(OnButtonClick);
        }

        // ��Ÿ�� �̹��� ����
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }
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

    void PressButton()
    {
        var colors = myButton.colors;
        colors.normalColor = colors.pressedColor;
        myButton.colors = colors;
    }

    void ReleaseButton()
    {
       
            myButton.colors = originalColors;
        
    }

    void OnButtonClick()
    {
        if (!isOnCooldown)
        {
            StartCooldown();
        }
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        cooldownEndTime = Time.time + cooldownTime;

        // ��ư ���� ����
        if (myButton.image != null)
        {
            myButton.image.color = cooldownColor;
        }

        // ��Ÿ�� �̹��� �ʱ�ȭ
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f;
        }
    }

    void UpdateCooldown()
    {
        float remainingTime = cooldownEndTime - Time.time;

        if (remainingTime <= 0)
        {
            EndCooldown();
        }
        else
        {
            // ��Ÿ�� �̹��� ������Ʈ
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = remainingTime / cooldownTime;
            }

            // ��ư ���� ��� ����
            if (myButton.image != null)
            {
                myButton.image.color = Color.Lerp(cooldownColor, activeColor, 1 - (remainingTime / cooldownTime));
            }
        }
    }

    void EndCooldown()
    {
        isOnCooldown = false;

        // ��ư ���� �������
        if (myButton.image != null)
        {
            myButton.image.color = activeColor;
        }

        // ��Ÿ�� �̹��� �ʱ�ȭ
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }
    }

    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }
}
