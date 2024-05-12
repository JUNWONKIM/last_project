using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    public int maxHealth = 1; // �ִ� ü��
    private int currentHealth; // ���� ü��
    private Animator animator; // Creature�� �ִϸ����� ������Ʈ
    private bool isDead = false; // ���� �׾����� ����
    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth; // ������ �� �ִ� ü������ ����
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
    }

    // �������� �Ծ��� �� ȣ��Ǵ� �Լ�
    public void TakeDamage(int amount)
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
        // Creature�� �ִϸ����Ϳ��� �״� �ִϸ��̼��� ����϶�� �˸�
        animator.SetBool("isDie", true);
        gameObject.tag = "Untagged";
        // �� �� �Ŀ� �±׸� �����Ͽ� �ٸ� ��ũ��Ʈ���� ������ �ν����� ���ϰ� ��


        // ���� �׾����� ǥ��
        isDead = true;

        // ���� ��ġ�� ������Ű�� ȸ���� ����
        rb.velocity = Vector3.zero;

        float dieAnimationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Destroy(gameObject, dieAnimationLength);
    }

   
}
