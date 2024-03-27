using UnityEngine;

public class MainCamera_Move : MonoBehaviour
{
    public float moveSpeed = 100f; // ī�޶� �̵� �ӵ�
    public float zoomSpeed = 5000f; // �� ��/�ƿ� �ӵ�
    public float mouseBorderWidth = 10f; // ȭ�� ������ ���콺�� �� ���� ��

    void Update()
    {
        // �����¿� �̵�
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // ���콺 ��ũ���� ���� �� ��/�ƿ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scroll * zoomSpeed * Time.deltaTime, Space.Self);

        // ���콺�� ȭ�� ������ �и� �� �������� �̵�
        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveVector = Vector3.zero;

        if (mousePosition.x < mouseBorderWidth) // �������� �̵�
        {
            moveVector -= transform.right;
        }
        else if (mousePosition.x > Screen.width - mouseBorderWidth) // ���������� �̵�
        {
            moveVector += transform.right;
        }

        if (mousePosition.y < mouseBorderWidth) // �Ʒ��� �̵�
        {
            moveVector += transform.forward;
        }
        else if (mousePosition.y > Screen.height - mouseBorderWidth) // ���� �̵�
        {
            moveVector -= transform.forward;
        }

        transform.Translate(moveVector.normalized * moveSpeed * Time.deltaTime);
    }
}