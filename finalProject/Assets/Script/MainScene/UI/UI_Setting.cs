using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // EventSystem�� ����ϱ� ���� �߰�

public class UI_Setting : MonoBehaviour
{
    public GameObject settingsPanel; // ����â �г�
    private bool isPaused = false;

    void Update()
    {
        // ESC Ű�� ������ ����â�� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // ����â �ݱ�
            }
            else
            {
                PauseGame(); // ����â ����
            }
        }
    }

    void PauseGame()
    {
        settingsPanel.SetActive(true); // ����â Ȱ��ȭ
        Time.timeScale = 0f; // ���� �Ͻ� ����
        isPaused = true;

        // ��� �Է��� ����
        EventSystem.current.sendNavigationEvents = false;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false); // ����â ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �簳
        isPaused = false;

        // �Է� �̺�Ʈ�� �ٽ� Ȱ��ȭ
        EventSystem.current.sendNavigationEvents = true;
    }

    public void QuitGame()
    {
        Application.Quit(); // ���� ����
    }

    public bool IsSettingsPanelActive()
    {
        return settingsPanel.activeSelf; // ����â�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
    }

}
