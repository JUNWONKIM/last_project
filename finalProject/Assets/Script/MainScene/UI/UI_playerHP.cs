using UnityEngine;
using UnityEngine.UI;

public class UI_playerHP : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHP playerHP;

    void Start()
    {
        // �ִ� ü���� �����̴��� �ִ� ������ ����
        healthSlider.maxValue = playerHP.max_hp;
        // ���� ü���� �����̴��� �ʱ� ������ ����
        healthSlider.value = playerHP.hp;
    }

    void Update()
    {
        // �� �����Ӹ��� �����̴��� ���� �÷��̾��� ���� ü������ ������Ʈ
        healthSlider.value = playerHP.hp;
    }
}
