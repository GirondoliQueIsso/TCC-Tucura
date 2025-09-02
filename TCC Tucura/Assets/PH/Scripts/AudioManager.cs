// AudioManager.cs
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton: Uma forma de garantir que s� existe um AudioManager no jogo todo
    // e que ele seja f�cil de acessar.
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("Este AudioSource � para a m�sica de fundo.")]
    public AudioSource musicSource;

    [Tooltip("Este AudioSource � para os efeitos sonoros (SFX).")]
    public AudioSource sfxSource;

    private float originalMusicVolume;

    void Awake()
    {
        // L�gica do Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Faz o AudioManager sobreviver � troca de cenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Guarda o volume original da m�sica para restaur�-lo depois
        if (musicSource != null)
        {
            originalMusicVolume = musicSource.volume;
        }
    }

    // Toca uma m�sica de fundo
    public void PlayMusic(AudioClip musicClip, bool loop)
    {
        if (musicSource == null || musicSource.clip == musicClip) return;

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // Toca um efeito sonoro uma �nica vez
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(sfxClip);
    }

    // A fun��o M�GICA: Inicia a corrotina para diminuir e voltar o som
    public void DuckMusic(AudioClip sfxToPlay)
    {
        if (sfxToPlay == null) return;
        StartCoroutine(DuckMusicCoroutine(sfxToPlay));
    }

    private IEnumerator DuckMusicCoroutine(AudioClip sfxToPlay)
    {
        // 1. Abaixa o volume da m�sica
        yield return StartCoroutine(FadeMusic(0.2f, 0.5f)); // Abaixa para 20% do volume em 0.5s

        // 2. Toca o efeito sonoro de game over
        PlaySFX(sfxToPlay);

        // 3. Espera o efeito sonoro terminar
        yield return new WaitForSeconds(sfxToPlay.length);

        // 4. Volta o volume da m�sica ao normal
        yield return StartCoroutine(FadeMusic(originalMusicVolume, 0.5f)); // Volta ao volume original em 0.5s
    }

    // Corrotina que faz o fade (mudan�a suave de volume)
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