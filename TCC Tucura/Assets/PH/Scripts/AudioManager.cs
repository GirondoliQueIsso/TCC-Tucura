// AudioManager.cs
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton: Uma forma de garantir que só existe um AudioManager no jogo todo
    // e que ele seja fácil de acessar.
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("Este AudioSource é para a música de fundo.")]
    public AudioSource musicSource;

    [Tooltip("Este AudioSource é para os efeitos sonoros (SFX).")]
    public AudioSource sfxSource;

    private float originalMusicVolume;

    void Awake()
    {
        // Lógica do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Faz o AudioManager sobreviver à troca de cenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Guarda o volume original da música para restaurá-lo depois
        if (musicSource != null)
        {
            originalMusicVolume = musicSource.volume;
        }
    }

    // Toca uma música de fundo
    public void PlayMusic(AudioClip musicClip, bool loop)
    {
        if (musicSource == null || musicSource.clip == musicClip) return;

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // Toca um efeito sonoro uma única vez
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(sfxClip);
    }

    // A função MÁGICA: Inicia a corrotina para diminuir e voltar o som
    public void DuckMusic(AudioClip sfxToPlay)
    {
        if (sfxToPlay == null) return;
        StartCoroutine(DuckMusicCoroutine(sfxToPlay));
    }

    private IEnumerator DuckMusicCoroutine(AudioClip sfxToPlay)
    {
        // 1. Abaixa o volume da música
        yield return StartCoroutine(FadeMusic(0.2f, 0.5f)); // Abaixa para 20% do volume em 0.5s

        // 2. Toca o efeito sonoro de game over
        PlaySFX(sfxToPlay);

        // 3. Espera o efeito sonoro terminar
        yield return new WaitForSeconds(sfxToPlay.length);

        // 4. Volta o volume da música ao normal
        yield return StartCoroutine(FadeMusic(originalMusicVolume, 0.5f)); // Volta ao volume original em 0.5s
    }

    // Corrotina que faz o fade (mudança suave de volume)
    private IEnumerator FadeMusic(float targetVolume, float duration)
    {
        float currentTime = 0;
        float startVolume = musicSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }
}