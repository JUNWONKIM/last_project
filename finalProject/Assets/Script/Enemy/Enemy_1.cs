using UnityEngine;

public class Enemy_1 : MonoBehaviour
{
    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    private Transform player; // �÷��̾��� ��ġ

   

    void Start()
    {
        // �÷��̾� ���� ������Ʈ�� ã�� Ʈ�������� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player").transform;

     
    }

    void Update()
    {
        // �÷��̾ ���� �̵�
        transform.LookAt(player);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
