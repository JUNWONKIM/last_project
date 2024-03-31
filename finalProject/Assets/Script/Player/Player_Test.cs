using UnityEngine;

public class Player_Test : MonoBehaviour
{
    public float moveSpeed = 5f; // �÷��̾� �̵� �ӵ�

    void Update()
    {
        // ����Ű �Է��� �޾� �̵� ������ ����
        float horizontalInput = Input.GetAxis("Player_Horizontal");
        float verticalInput = Input.GetAxis("Player_Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // �̵� ���⿡ ���� �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
