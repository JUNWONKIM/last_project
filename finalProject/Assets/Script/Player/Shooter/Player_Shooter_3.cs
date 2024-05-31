using System.Collections;
using UnityEngine;

public class Player_Shooter_3 : MonoBehaviour
{
    public static Player_Shooter_3 instance;

    public GameObject swordPrefab; // Į ������
    public float summonInterval = 5f; // Į ��ȯ ����
    public float distanceFromPlayer = 30f; // �÷��̾�κ����� �Ÿ�
    public int swordNum = 0;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // ���� ���ݸ��� Į�� ��ȯ�ϴ� �ڷ�ƾ ����
        InvokeRepeating("SummonSword", 0f, summonInterval);
    }

    void SummonSword()
    {
        // �÷��̾� ���ӿ�����Ʈ ��������
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {

            if (swordNum == 1)
            {
                // �÷��̾� ��ġ�� Į�� ��ȯ ��ġ ���
                Vector3 summonPosition = player.transform.position + new Vector3(distanceFromPlayer, 0f, 0f);
                Quaternion summonRotation = Quaternion.Euler(90f, 90f, 0f);

                // Į ��ȯ
                Instantiate(swordPrefab, summonPosition, summonRotation);
            }

            if (swordNum == 2)
            {
                // �÷��̾� ��ġ�� Į�� ��ȯ ��ġ ���
                Vector3 summonPosition = player.transform.position + new Vector3(distanceFromPlayer, 0f, 0f);
                Quaternion summonRotation = Quaternion.Euler(90f, 90f, 0f);
                Vector3 summonPosition2 = player.transform.position + new Vector3(-distanceFromPlayer, 0f, 0f);
                Quaternion summonRotation2 = Quaternion.Euler(90f, 90f, 0f);

                // Į ��ȯ
                Instantiate(swordPrefab, summonPosition, summonRotation);
                Instantiate(swordPrefab, summonPosition2, summonRotation2);
            }

            if (swordNum == 3)
            {
                // ���� ���� ����
                float angleInterval = 360f / 3f;
                float currentAngle = 0f;

                for (int i = 0; i < swordNum; i++)
                {
                    // �÷��̾� ��ġ�� Į�� ��ȯ ��ġ ���
                    float x = distanceFromPlayer * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
                    float z = distanceFromPlayer * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
                    Vector3 summonPosition = player.transform.position + new Vector3(x, 0f, z);
                    Quaternion summonRotation = Quaternion.Euler(90f, 90f, 0f);

                    // Į ��ȯ
                    Instantiate(swordPrefab, summonPosition, summonRotation);

                    // ���� Į�� ��ȯ�ϱ� ���� ���� ����
                    currentAngle += angleInterval;
                }

            }
        }
    }
    public void IncreaseSwordNum()
    {

        swordNum++;
        Debug.Log("Į ���� : " + swordNum);

    }
}
