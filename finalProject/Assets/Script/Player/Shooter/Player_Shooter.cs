using System.Collections.Generic;
using UnityEngine;

public class Player_Shooter : MonoBehaviour
{
    public GameObject projectilePrefab; // �߻�ü �������� �Ҵ��� ����
    public float fireInterval = 1f; // �߻� ����
    public float fireInterval_slow = 2f;
    public float detectionRange = 100f; // ���� Ž���� ����
    public float projectileSpeed = 100f;
    private float lastFireTime; // ������ �߻� �ð�

  

    void Start()
    {
        // LineRenderer ������Ʈ �߰�
     
    }

    void Update()
    {
        // ���� �������� ���� ����� ���� Ž���ϰ� �߻�ü�� �߻�
        if (Time.time - lastFireTime > fireInterval)
        {
            FireProjectile();
            lastFireTime = Time.time;
        }

    }

    void FireProjectile()
    {
        // ���� ����� ���� Ž��
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        
        List<GameObject> allCreatures = new List<GameObject>();
        allCreatures.AddRange(creatures);


        GameObject closestCreature = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject creature in allCreatures)
        {
            float distance = Vector3.Distance(transform.position, creature.transform.position);
            if (distance < closestDistance && distance <= detectionRange)
            {
                closestCreature = creature;
                closestDistance = distance;
            }
        }

        // �߻�ü�� �߻��� ���� �ִ� ��� �߻�
        if (closestCreature != null)
        {
            Vector3 targetDirection = closestCreature.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, rotation);

            // �߻�ü�� �̵� �ӵ� �ο�
            Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                // �߻��� ���� �������� �߻�ü�� �̵���Ŵ
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }
        }

        CheckForSlowObjects();
    }


    // Ž�� ������ �÷��̾��� �߽��� ����ٴϵ��� ������Ʈ�ϴ� �Լ�
 
 
    private void CheckForSlowObjects()
    {
        // �ֺ��� �ִ� ��� ���� ������Ʈ�� �����ɴϴ�.
        GameObject[] slowObjects = GameObject.FindGameObjectsWithTag("Slow");

        // �ֺ��� Slow �±׸� ���� ������Ʈ�� �ִ��� Ȯ���մϴ�.
        if (slowObjects.Length > 0)
        {
            // Slow �±׸� ���� ������Ʈ�� �����ϸ� �̵� �ӵ��� ���ҽ�ŵ�ϴ�.
            fireInterval = fireInterval_slow; // �̵� �ӵ��� 50%�� ���Դϴ�.
        }
        else
        {
            // Slow �±׸� ���� ������Ʈ�� �������� ������ ���� �̵� �ӵ��� �����մϴ�.
           fireInterval = 1f; // �̵� �ӵ��� 100%�� �����մϴ�.
        }
    }
}
