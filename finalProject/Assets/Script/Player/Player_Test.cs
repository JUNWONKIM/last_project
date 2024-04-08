using UnityEngine;

public class Player_Test : MonoBehaviour
{
    public float moveSpeed = 100f; // �÷��̾� �̵� �ӵ�

    void Update()
    {
        // ����Ű �Է��� �޾� �̵� ������ ����
        float horizontalInput = Input.GetAxis("Player_Horizontal");
        float verticalInput = Input.GetAxis("Player_Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // �÷��̾ �ٶ� ������ ����
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
        }

        // �̵� ���⿡ ���� �̵�
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
