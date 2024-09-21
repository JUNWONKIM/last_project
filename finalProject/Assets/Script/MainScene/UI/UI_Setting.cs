using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Setting : MonoBehaviour
{
    public GameObject settingsPanel;  // ����â �г�
    public GameObject keyInfoPanel;   // �߰��� Ű ���� �г�
    private bool isPaused = false;
    private bool isKeyInfoPanelActive = false; // Ű ���� �г��� ����

    void Update()
    {
        // ESC Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isKeyInfoPanelActive)
            {
                // Ű ���� �г��� ���� ���� �� ESC�� ������ ����â���� ���ư�
                CloseKeyInfoPanelAndOpenSettings();
            }
            else if (isPaused)
            {
                // ����â�� ���� ���� �� ESC�� ������ ������ �簳
                ResumeGame();
            }
            else
            {
                // ������ �Ͻ��������� ���� ���¿����� ESC�� ������ ����â�� ����
                PauseGame();
            }
        }
    }

    public void ToggleSettingsPanel()
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

    void PauseGame()
    {
        settingsPanel.SetActive(true);  // ����â Ȱ��ȭ
        keyInfoPanel.SetActive(false);  // Ű ���� �г� ��Ȱ��ȭ
        Time.timeScale = 0f;  // ���� �Ͻ� ����
        isPaused = true;
        isKeyInfoPanelActive = false;

        // ��� �Է��� ����
        EventSystem.current.sendNavigationEvents = false;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);  // ����â ��Ȱ��ȭ
        Time.timeScale = 1f;  // ���� �簳
        isPaused = false;
        isKeyInfoPanelActive = false;

        // �Է� �̺�Ʈ�� �ٽ� Ȱ��ȭ
        EventSystem.current.sendNavigationEvents = true;
    }

    public void QuitGame()
    {
        Application.Quit();  // ���� ����
    }

    public bool IsSettingsPanelActive()
    {
        return settingsPanel.activeSelf;  // ����â�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
    }

    // ���� �г� ��� ��ư
    public void OnSettingsButtonClick()
    {
        ToggleSettingsPanel();  // ��ư���� ����â ���
    }

    // Ű ���� �г��� ���� ��ư
    public void OnKeyInfoPanelButtonClick()
    {
        OpenKeyInfoPanel();
    }

    // Ű ���� �г� ����
    public void OpenKeyInfoPanel()
    {
        keyInfoPanel.SetActive(true);  // Ű ���� �г� Ȱ��ȭ
        settingsPanel.SetActive(false);  // ����â ��Ȱ��ȭ
        isKeyInfoPanelActive = true;
    }

    // Ű ���� �г� �ݰ� ����â���� ���ư���
    public void CloseKeyInfoPanelAndOpenSettings()
    {
        keyInfoPanel.SetActive(false);  // Ű ���� �г� ��Ȱ��ȭ
        settingsPanel.SetActive(true);  // ����â �ٽ� Ȱ��ȭ
        isKeyInfoPanelActive = false;
    }

    // ���� ���� ��ư �Լ�
    public void OnQuitButtonClick()
    {
        QuitGame();  // ���� ����
    }
}
