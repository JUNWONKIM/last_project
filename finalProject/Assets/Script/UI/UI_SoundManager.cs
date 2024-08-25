using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // ��������� ����� AudioSource
    public AudioClip defaultClip;    // �⺻ ������� Ŭ��
    public AudioClip bossClip;       // ���� ������� Ŭ��
    public bool loop = true;         // ������� ���� ����
    [Range(0f, 1f)] public float volume = 0.5f; // ������� ���� (0.0 ~ 1.0)

    private PlayerHP playerHP;       // PlayerHP �ν��Ͻ�
    private bool isBossMusicPlaying = false; // ���� ������ ��� ������ ����

    void Start()
    {
        // AudioSource�� �������� ���� ���, ������Ʈ�� ������
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �ʱ� ����
        audioSource.loop = loop;
        audioSource.volume = volume;

        // �⺻ ������� ���
        if (defaultClip != null)
        {
            PlayMusic(defaultClip);
        }

        // PlayerHP �ν��Ͻ��� ã�Ƽ� ����
        playerHP = FindObjectOfType<PlayerHP>();
    }

    void Update()
    {
        // �����̳� ���� ������ �ǽð����� ����Ǿ��� �� �ݿ�
        audioSource.volume = volume;
        audioSource.loop = loop;

        // �÷��̾��� HP ���¿� ���� ������� ����
        if (playerHP != null)
        {
            if (playerHP.hp <= playerHP.max_hp * 0.3f && !isBossMusicPlaying)
            {
                // HP�� �ִ� HP�� 30% ������ �� ���� �������� ����
                PlayMusic(bossClip);
                isBossMusicPlaying = true;
            }
            else if (playerHP.hp > playerHP.max_hp * 0.3f && isBossMusicPlaying)
            {
                // HP�� �ٽ� 30% �̻����� ���ƿ��� �⺻ �������� ����
                PlayMusic(defaultClip);
                isBossMusicPlaying = false;
            }
        }
    }

    // ������� ��� �Լ�
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // ���� ��� ���� ������ ���߰� ���ο� �������� ��ü
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    // ������� �Ͻ����� �Լ�
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // ������� �ٽ� ��� �Լ�
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    // ������� ���� �Լ�
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
