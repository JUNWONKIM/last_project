using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_startbutton : MonoBehaviour
{
    public Text startText; // Text UI ��ҿ� ���� ����
    public UI_Setting uiSetting; // UI_Setting ��ũ��Ʈ�� ���� ����

    void Start()
    {
        startText.text = "Ű�� ���� �����ϼ���";
    }

    void Update()
    {
        // ����â�� Ȱ��ȭ���� �ʾ��� ���� Ű���� �Է��� ó��
        if (uiSetting != null && !uiSetting.IsSettingsPanelActive())
        {
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainScene(); // Ű���� �Է��� �����Ǹ� ���� �� �ε�
            }
        }
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene"); // MainScene�� ���� ������ ���ԵǾ� �ִ��� Ȯ��
    }
}
