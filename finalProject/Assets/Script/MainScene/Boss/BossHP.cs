using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossHP : MonoBehaviour
{
    public float maxHP = 1; // �ִ� ü��
    public float currentHP; // ���� ü��
    private Animator animator; // Creature�� �ִϸ����� ������Ʈ
    private bool isDead = false; // ���� �׾����� ����
    private Rigidbody rb;

    // ���� �� ����� �Ҹ� ���� ����
    public AudioClip deathSound; // �״� �Ҹ� Ŭ��
    private AudioSource audioSource; // ����� �ҽ�

    void Start()
    {
        currentHP = maxHP; // ������ �� �ִ� ü������ ����
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        audioSource = GetComponent<AudioSource>(); // AudioSource ������Ʈ ��������

        // AudioSource ����
        if (audioSource != null)
        {
            audioSource.spatialBlend = 0f; // 2D �Ҹ��� ����
        }
    }

    // �������� �Ծ��� �� ȣ��Ǵ� �Լ�
    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            currentHP -= amount; // ��������ŭ ü�� ����

            if (currentHP <= 0)
            {
                Die(); // ü���� 0 �����̸� ��� ó��
            }
        }
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetBool("isDie", true);
        }

        // �״� �Ҹ� ���
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.volume = 1f;
            audioSource.Play();
        }

        // Rigidbody ���� ��ȣ�ۿ� ��Ȱ��ȭ �� ������ ����
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll; // ��� ���� ������ �� ȸ�� ����

        // NavMeshAgent ��Ȱ��ȭ (������ NavMesh�� ����ϰ� �ִٸ�)
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        gameObject.tag = "Untagged";

        PlayerLV.IncrementCreatureDeathCount();
        isDead = true;

        // ������ ������ ���� �й� ������ ��ȯ
        SceneManager.LoadScene("LoseScene"); // �й� �� �̸����� ���� �ʿ�

        // �ִϸ��̼��� ��Ȯ�� ���̸� ��������
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float dieAnimationLength = clipInfo[0].clip.length;

        Destroy(gameObject, dieAnimationLength);
    }
}
