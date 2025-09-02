using UnityEngine;

public class SetaPetalaController : MonoBehaviour
{
    public Transform[] petalas; // Array com as pétalas (posições)
    public AudioClip moveSound; // Som ao mover a seta

    private int indexAtual = 0; // Índice da pétala atual
    private AudioSource audioSource;

    void Start()
    {
        // Pega ou adiciona o AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Som 2D

        // Inicializa posição da seta
        if (petalas.Length > 0)
        {
            transform.position = petalas[indexAtual].position;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverSeta(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverSeta(-1);
        }
    }

    void MoverSeta(int direcao)
    {
        indexAtual += direcao;

        // Loop circular
        if (indexAtual >= petalas.Length)
            indexAtual = 0;
        else if (indexAtual < 0)
            indexAtual = petalas.Length - 1;

        transform.position = petalas[indexAtual].position;

        // Toca o som
        audioSource.PlayOneShot(moveSound);
    }
}
