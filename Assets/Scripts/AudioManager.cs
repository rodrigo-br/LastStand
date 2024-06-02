using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip enemyShootClip;
    [SerializeField] AudioClip playerShootClip;
    [SerializeField] AudioClip[] enemySounds;
    [SerializeField] AudioClip[] enemyDieClips;
    [SerializeField] AudioClip[] playerDieClips;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    AudioSource myAudioSource;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        myAudioSource.Play();
    }

    private void PlayClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayEnemySoundClip()
    {
        PlayClip(enemySounds[Random.Range(0, enemySounds.Length)]);
    }

    public void PlayPlayerDieClip()
    {
        PlayClip(playerDieClips[Random.Range(0, playerDieClips.Length)]);
    }

    public void PlayEnemyDieClip()
    {
        PlayClip(enemyDieClips[Random.Range(0, enemyDieClips.Length)]);
    }

    public void PlayPlayerShootClip()
    {
        PlayClip(playerShootClip);
    }

    public void PlayEnemyShootClip()
    {
        PlayClip(enemyShootClip);
    }
}
