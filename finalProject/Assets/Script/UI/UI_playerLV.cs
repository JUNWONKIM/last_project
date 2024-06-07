using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_playerLV : MonoBehaviour
{
    public Text levelText; // �ؽ�Ʈ UI ���
    public PlayerLV playerLV; // PlayerLV ��ũ��Ʈ

    void Start()
    {
        // �ʱ� �ؽ�Ʈ ����
        UpdateLevelText();
    }

    void Update()
    {
        // �� �����Ӹ��� �ؽ�Ʈ ������Ʈ
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        levelText.text = "Lv :  " + playerLV.LV;
    }
}
