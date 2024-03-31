using UnityEngine;

public class MainCamera_Move : MonoBehaviour
{
    public float moveSpeed = 100f; // ī�޶� �̵� �ӵ�
    public float zoomSpeed = 5000f; // �� ��/�ƿ� �ӵ�
    public float mouseBorderWidth = 10f; // ȭ�� ������ ���콺�� �� ���� ��

    void Update()
    {
        // ws Ű�� ���� �յ� �̵�
        float verticalInput = Input.GetAxis("Vertical");
        // ad Ű�� ���� �¿� �̵�
        float horizontalInput = Input.GetAxis("Horizontal");

        // �̵� ���� ���
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // ī�޶��� ������ ����Ͽ� �̵� ���� ���
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; // ī�޶� 45�� ��￩�� �����Ƿ� y ���� �̵��� ����
        right.y = 0;

        Vector3 move = forward * verticalInput + right * horizontalInput;
        move.Normalize(); // ����ȭ

        // �̵� ���� ���
        Vector3 movement = move * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // ���콺 ��ũ���� ���� �� ��/�ƿ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * -zoomSpeed * Time.deltaTime, 0, Space.World);


        // ���콺�� ȭ�� ������ �и� �� �������� �̵�
       /* Vector3 mousePosition = Input.mousePosition;
        Vector3 moveVector = Vector3.zero;

        if (mousePosition.x < mouseBorderWidth) // �������� �̵�
        {
            moveVector -= right;
        }
        else if (mousePosition.x > Screen.width - mouseBorderWidth) // ���������� �̵�
        {
            moveVector += right;
        }

        if (mousePosition.y < mouseBorderWidth) // �Ʒ��� �̵�
        {
            moveVector -= forward;
        }
        else if (mousePosition.y > Screen.height - mouseBorderWidth) // ���� �̵�
        {
            moveVector += forward;
        }

        transform.Translate(moveVector.normalized * moveSpeed * Time.deltaTime, Space.World);*/
    }
}
