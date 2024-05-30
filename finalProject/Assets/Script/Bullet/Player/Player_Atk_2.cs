using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Atk_2 : MonoBehaviour
{
    public GameObject objectToSpawn; // ��ȯ�� ������Ʈ ������
    public float lifetime = 1f; // ������Ʈ�� ������������ �ð�
    public float fixedYPosition = 2.5f; // ������ Y �� ��ġ

    private void Start()
    {
        // ������Ʈ�� lifetime �ð� �Ŀ� ������� �ϰ� ���ο� ������Ʈ�� ��ȯ
        StartCoroutine(DestroyAndSpawn());
    }

    private IEnumerator DestroyAndSpawn()
    {
        // lifetime �ð� ���� ���
        yield return new WaitForSeconds(lifetime);

        // ���� ��ġ���� Y���� ������ ���·� ���ο� ������Ʈ ��ȯ
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = fixedYPosition;

        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // ���� ������Ʈ�� �ı�
        Destroy(gameObject);
    }
}
