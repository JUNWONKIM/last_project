using UnityEngine;
using UnityEngine.UI;

public class UI_Cursor : MonoBehaviour
{
    public Sprite validCursorSprite; // ��ȯ ���� ���� ���콺 Ŀ�� ��������Ʈ
    public Sprite invalidCursorSprite; // ��ȯ �Ұ� ���� ���콺 Ŀ�� ��������Ʈ
    public CreatureSpawner creatureSpawner; // CreatureSpawner ��ũ��Ʈ ����
    public UI_Setting uiSetting; // UI_Setting ��ũ��Ʈ ����

    public Vector2 cursorOffset = new Vector2(-2f, -10f); // Ŀ�� �̹��� ������ (�������� �̵�)

    private Image cursorImage;
    private RectTransform rectTransform;

    void Start()
    {
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // �⺻ �ý��� Ŀ�� �����

        // Ŀ�� �̹����� ��Ŀ�� �߾����� ����
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    void LateUpdate()
    {
        if (uiSetting != null && uiSetting.IsSettingsPanelActive())
        {
            cursorImage.sprite = validCursorSprite;
            UpdateCursorPosition(); // Ŀ�� ��ġ�� ������Ʈ
        }
        else
        {
            UpdateCursor(); // �Ϲ� ��忡�� Ŀ�� ������Ʈ
        }
    }

    void UpdateCursor()
    {
        UpdateCursorPosition();

        // ����Ʈ ������ ���콺 ��ġ�� ���� �������� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 worldCursorPosition = hit.point;

            // Ŀ�� ��ġ�� spawnRange �ȿ� �ִ��� Ȯ��
            bool isValidSpawnPosition = creatureSpawner.IsWithinSpawnRange(worldCursorPosition);

            // ��ȿ�� Ŀ�� ���¿� ���� ��������Ʈ ���� (���� �ٲٱ�)
            cursorImage.sprite = isValidSpawnPosition ? invalidCursorSprite : validCursorSprite;
        }
    }

    void UpdateCursorPosition()
    {
        // ���콺 ��ġ�� ���� Ŀ�� ��ġ ������Ʈ
        Vector2 cursorPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out cursorPosition
        );
        cursorPosition += cursorOffset; // ������ ����
        rectTransform.anchoredPosition = cursorPosition;
    }
}
