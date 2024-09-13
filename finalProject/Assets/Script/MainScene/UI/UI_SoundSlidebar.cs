using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSlidebar : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // AudioManager���� ���� �� �ҷ�����
        float bgmVolume = AudioManager.instance.GetBGMVolume();
        float sfxVolume = AudioManager.instance.GetSFXVolume();

        // �����̴� �� ����ȭ
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        // �����̴� �� ���� �� AudioManager�� ���� ���� ����
        bgmSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetBGMVolume(bgmSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetSFXVolume(sfxSlider.value); });
    }
}
