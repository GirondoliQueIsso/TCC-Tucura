using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManagerBesouro : MonoBehaviour
{
    public static AudioManagerBesouro instance;

    public Sound[] sounds;
    public AudioSource sfxSource;
    public AudioSource backgroundMusicSource; // Opcional, para música de fundo

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string soundName)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("Som: " + soundName + " não encontrado!");
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }
}