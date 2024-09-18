using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Setting : MonoBehaviour
{
    public GameObject settingsPanel;  // ����â �г�
    public GameObject anotherPanel;   // �߰��� �ٸ� �г�
    private bool isPaused = false;
    private bool isAnotherPanelActive = false; // �ٸ� �г��� ����

    void Update()
    {
        // ESC Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isAnotherPanelActive)
            {
                // �ٸ� �г��� ���� ���� �� ESC�� ������ ����â���� ���ư�
                CloseAnotherPanelAndOpenSettings();
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
        anotherPanel.SetActive(false);  // �ٸ� �г� ��Ȱ��ȭ
        Time.timeScale = 0f;  // ���� �Ͻ� ����
        isPaused = true;
        isAnotherPanelActive = false;

        // ��� �Է��� ����
        EventSystem.current.sendNavigationEvents = false;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);  // ����â ��Ȱ��ȭ
        Time.timeScale = 1f;  // ���� �簳
        isPaused = false;
        isAnotherPanelActive = false;

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

    // �ٸ� �г��� ���� ��ư
    public void OnAnotherPanelButtonClick()
    {
        OpenAnotherPanel();
    }

    // �ٸ� �г� ����
    public void OpenAnotherPanel()
    {
        anotherPanel.SetActive(true);  // �ٸ� �г� Ȱ��ȭ
        settingsPanel.SetActive(false);  // ����â ��Ȱ��ȭ
        isAnotherPanelActive = true;
    }

    // �ٸ� �г� �ݰ� ����â���� ���ư���
    public void CloseAnotherPanelAndOpenSettings()
    {
        anotherPanel.SetActive(false);  // �ٸ� �г� ��Ȱ��ȭ
        settingsPanel.SetActive(true);  // ����â �ٽ� Ȱ��ȭ
        isAnotherPanelActive = false;
    }
}
