using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 5�� �ڿ� �Ѿ��� �ı��մϴ�.
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �÷��̾��� ���
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� �浹�ϸ� �Ѿ��� �ı���
            Destroy(gameObject);
            // ���⿡ �߰����� �÷��̾ ���� ó���� �� �� ����
            // ���� ���, �÷��̾��� ü���� ���ҽ�Ű�� ���� �۾��� ������ �� ����
        }
    }
}
