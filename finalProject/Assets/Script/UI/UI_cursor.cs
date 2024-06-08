using UnityEngine;
using UnityEngine.UI;

public class UI_cursor : MonoBehaviour
{
    public Sprite validCursorSprite; // ��ȯ ���� ���� ���콺 Ŀ�� ��������Ʈ
    public Sprite invalidCursorSprite; // ��ȯ �Ұ� ���� ���콺 Ŀ�� ��������Ʈ
    public CreatureSpawner creatureSpawner; // CreatureSpawner ��ũ��Ʈ ����

    private Image cursorImage;
    private RectTransform rectTransform;

    void Start()
    {
        cursorImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        Cursor.visible = false; // �⺻ �ý��� Ŀ�� �����
    }

    void Update()
    {
        UpdateCursor();
    }

    void UpdateCursor()
    {
        // ���콺 ��ġ�� ���� Ŀ�� ��ġ ������Ʈ
        Vector2 cursorPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out cursorPosition
        );
        rectTransform.anchoredPosition = cursorPosition;

        // ����Ʈ ������ ���콺 ��ġ�� ���� �������� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 worldCursorPosition = hit.point;

            // Ŀ�� ��ġ�� spawnRange �ȿ� �ִ��� Ȯ��
            bool isValidSpawnPosition = creatureSpawner.IsWithinSpawnRange(worldCursorPosition);

            // ��ȿ�� Ŀ�� ���¿� ���� ��������Ʈ ����
            cursorImage.sprite = isValidSpawnPosition ? invalidCursorSprite : validCursorSprite;
        }
    }
}
