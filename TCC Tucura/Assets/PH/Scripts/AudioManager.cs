using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Para o controle de cenas

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Singleton Pattern

    [Header("Configura��es de �udio")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.7f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;
    public float fadeDuration = 2.0f; // Dura��o do fade para m�sicas

    [Header("Audio Sources")]
    public AudioSource musicSource1; // Para m�sica de fundo
    public AudioSource musicSource2; // Para transi��es de m�sica de fundo
    public AudioSource sfxSource;    // Para efeitos sonoros

    // Armazenar a m�sica atual para f�cil refer�ncia
    private AudioSource currentMusicSource;
    private AudioSource previousMusicSource;

    private void Awake()
    {
        // Implementa��o do Singleton: Garante que s� haja uma inst�ncia
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroi este novo AudioManager se j� houver um
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mant�m o AudioManager entre as cenas
        }

        // Garante que os AudioSources existam e estejam configurados
        SetupAudioSources();

        // Define o volume inicial
        UpdateOverallVolume();
    }

    void Start()
    {
        // Adiciona um listener para quando a cena for carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Verifica a cena inicial ao iniciar
        CheckInitialSceneMusic();
    }

    void OnDestroy()
    {
        // Remove o listener quando o objeto � destru�do
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SetupAudioSources()
    {
        if (musicSource1 == null) musicSource1 = gameObject.AddComponent<AudioSource>();
        if (musicSource2 == null) musicSource2 = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

        // Configura��es padr�o para as m�sicas de fundo
        musicSource1.loop = true;
        musicSource1.playOnAwake = false;
        musicSource2.loop = true;
        musicSource2.playOnAwake = false;

        // Configura��es padr�o para os efeitos sonoros
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        currentMusicSource = musicSource1; // Come�a com o source 1 como o principal
        previousMusicSource = musicSource2;
    }

    // Chamado quando uma nova cena � carregada
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckInitialSceneMusic();
    }

    // Exemplo de como voc� pode definir a m�sica para diferentes cenas
    void CheckInitialSceneMusic()
    {
        // Substitua pelos nomes reais das suas cenas
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Suponha que voc� tenha AudioClips para cada cena em uma pasta Resources/Audio/Music
        AudioClip menuMusic = Resources.Load<AudioClip>("Audio/Music/MenuMusic"); // Exemplo
        AudioClip gameMusic = Resources.Load<AudioClip>("Audio/Music/GameMusic"); // Exemplo

        if (currentSceneName == "TelainicialCanvas") // Nome da sua cena de menu
        {
            if (menuMusic != null)
            {
                PlayMusic(menuMusic, true); // Toca a m�sica do menu, reinicia se j� estiver tocando
                // Se quiser um volume mais baixo no menu, voc� pode adicionar essa l�gica aqui ou no FadeMusic
            }
        }
        else if (currentSceneName == "GameScene") // Nome da sua cena de jogo (ou outro)
        {
            if (gameMusic != null)
            {
                PlayMusic(gameMusic, true);
            }
        }
        // Adicione mais ifs/else if para outras cenas
    }

    // --- M�todos P�blicos para Outros Scripts Usarem ---

    public void PlayMusic(AudioClip clip, bool restartIfSame = false)
    {
        if (clip == null) return;

        // Se a m�sica atual j� � a que queremos tocar e n�o precisamos reiniciar, n�o faz nada
        if (currentMusicSource.clip == clip && currentMusicSource.isPlaying && !restartIfSame)
        {
            return;
        }

        // Inicia a transi��o
        StartCoroutine(CrossFadeMusic(clip));
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateOverallVolume();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateOverallVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateOverallVolume();
    }

    private void UpdateOverallVolume()
    {
        // Atualiza o volume real dos AudioSources
        musicSource1.volume = musicVolume * masterVolume;
        musicSource2.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume; // SFX usa PlayOneShot, ent�o o volume � passado na chamada
    }

    // --- Coroutine para Transi��o de M�sica (Crossfade) ---
    IEnumerator CrossFadeMusic(AudioClip newClip)
    {
        // Troca os AudioSources para o crossfade
        AudioSource oldSource = currentMusicSource;
        currentMusicSource = (currentMusicSource == musicSource1) ? musicSource2 : musicSource1;
        previousMusicSource = oldSource;

        // Configura a nova m�sica
        currentMusicSource.clip = newClip;
        currentMusicSource.volume = 0f; // Come�a silencioso
        currentMusicSource.Play();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            oldSource.volume = Mathf.Lerp(musicVolume * masterVolume, 0f, progress);
            currentMusicSource.volume = Mathf.Lerp(0f, musicVolume * masterVolume, progress);
            yield return null;
        }

        // Garante que os volumes estejam exatos no final
        oldSource.volume = 0f;
        oldSource.Stop(); // Para a m�sica antiga
        currentMusicSource.volume = musicVolume * masterVolume;
    }
}