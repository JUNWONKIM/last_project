using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    public float maxHealth = 1; // �ִ� ü��
    public float currentHealth; // ���� ü��
    private Animator animator; // Creature�� �ִϸ����� ������Ʈ
    private bool isDead = false; // ���� �׾����� ����
    private Rigidbody rb;

    // ���� �� ����� �Ҹ� ���� ����
    public AudioClip deathSound; // �״� �Ҹ� Ŭ��
    private AudioSource audioSource; // ����� �ҽ�

    void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ ����
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
            currentHealth -= amount; // ��������ŭ ü�� ����

            if (currentHealth <= 0)
            {
                Die(); // ü���� 0 �����̸� ��� ó��
            }
        }
    }

    void Die()
    {
        if (animator != null)
        {
            // Creature�� �ִϸ����Ϳ��� �״� �ִϸ��̼��� ����϶�� �˸�
            animator.SetBool("isDie", true);
        }

        // �״� �Ҹ� ���
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.volume = 1f; // ������ �ʿ信 �°� ����
            audioSource.Play(); // �״� �Ҹ� ���
        }

        gameObject.tag = "Untagged";
        // �� �� �Ŀ� �±׸� �����Ͽ� �ٸ� ��ũ��Ʈ���� ������ �ν����� ���ϰ� ��

        PlayerLV.IncrementCreatureDeathCount();
        // ���� �׾����� ǥ��
        isDead = true;

        // ���� ��ġ�� ������Ű�� ȸ���� ����
        rb.velocity = Vector3.zero;

        // �ִϸ��̼��� ���̿� ���� ������Ʈ�� �ı�
        float dieAnimationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Destroy(gameObject, dieAnimationLength);
    }
}
