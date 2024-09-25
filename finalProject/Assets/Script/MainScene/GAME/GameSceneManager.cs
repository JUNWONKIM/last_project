using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public PlayerHP player; // �÷��̾��� ü���� �����ϴ� ��ũ��Ʈ
    private CreatureHealth bossHealth; // ���� ü���� �����ϴ� ��ũ��Ʈ

    void Update()
    {
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (player != null && player.hp <= 0) // �÷��̾��� ü���� 0 ������ ���
        {
            LoadWinScene(); // �¸� ������ ��ȯ
        }

        if (bossHealth != null && bossHealth.currentHealth <= 0) // ������ �����ϰ� ü���� 0 ������ ���
        {
            LoadLoseScene(); // �й� ������ ��ȯ
        }
    }

   

    void LoadWinScene()
    {
        //SceneManager.LoadScene("WinScene"); // �¸� ������ ��ȯ
    }

    public void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene"); // �й� �� �̸��� �°� ����
    }
}
