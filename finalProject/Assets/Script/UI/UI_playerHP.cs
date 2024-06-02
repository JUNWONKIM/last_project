using UnityEngine;
using UnityEngine.UI;

public class UI_playerHP : MonoBehaviour
{
    public Text playerHPText; // UI Text ���

    private PlayerHP playerHP; // �÷��̾��� HP�� �����ϴ� ��ũ��Ʈ

    void Start()
    {
        // �÷��̾� ������Ʈ���� PlayerHP ��ũ��Ʈ�� ������
        playerHP = FindObjectOfType<PlayerHP>();

        // ���� PlayerHP ��ũ��Ʈ�� ã�� �� ������ ������ ����ϰ� ��ũ��Ʈ�� ��Ȱ��ȭ
        if (playerHP == null)
        {
            Debug.LogError("PlayerHP ��ũ��Ʈ�� ã�� �� �����ϴ�!");
            enabled = false;
        }
    }

    void Update()
    {
        // UI Text ��ҿ� �÷��̾��� HP�� ǥ��
        if (playerHPText != null && playerHP != null)
        {
            playerHPText.text = "HP: " + playerHP.hp.ToString(); // HP ���� ���ڿ��� ��ȯ�Ͽ� ǥ��
        }
    }
}
