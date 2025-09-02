using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Objetos da Cena")]
    public List<GameObject> todasAsPetalas;
    public List<Transform> pontosDaSet;
    public GameObject mioloAbelhaNormal;
    public GameObject mioloAbelhaIrritada;
    public GameObject setaIndicadora;

    [Header("Abelhas")]
    public GameObject abelhaNormal;
    public GameObject abelhaComRaiva;

    [Header("Efeitos")]
    public Transform mainCameraTransform;
    public float duracaoTremor = 2f;
    public float magnitudeTremor = 0.1f;

    [Header("Sons do Jogo (SFX)")]
    public AudioClip pegarCertaSound;
    public AudioClip pegarErradaSound;
    public AudioClip moverSetaSound;

    [Header("Músicas de Fundo")]
    public AudioClip musicaPrincipal;
    public AudioClip musicaFimDeJogo;

    private GameObject petalaRuim;
    private int indiceDaPetalaSelecionada = 0;
    private bool jogoAcabou = false;
    private bool isGameActive = false;
    private bool ignoreFirstInput = false;

    private AudioManager audioManager;

    void Awake()
    {
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager não encontrado!");
        }
    }

    public void IniciarJogo()
    {
        isGameActive = true;
        ignoreFirstInput = true;
        jogoAcabou = false;

        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        if (abelhaNormal != null) abelhaNormal.SetActive(true);
        if (abelhaComRaiva != null) abelhaComRaiva.SetActive(false);
        if (setaIndicadora != null) setaIndicadora.SetActive(true);

        AtualizarSelecaoVisual();

        if (audioManager != null && musicaPrincipal != null)
        {
            audioManager.PlayMusic(musicaPrincipal, true);
        }
    }

    void Update()
    {
        if (!isGameActive || jogoAcabou) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) MoverSelecao(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoverSelecao(-1);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !ignoreFirstInput)
        {
            ConfirmarSelecao();
        }

        if (ignoreFirstInput) ignoreFirstInput = false;
    }

    void ConfirmarSelecao()
    {
        GameObject petalaEscolhida = todasAsPetalas[indiceDaPetalaSelecionada];

        if (petalaEscolhida == petalaRuim)
        {
            jogoAcabou = true;

            if (audioManager != null && pegarErradaSound != null) audioManager.PlaySFX(pegarErradaSound);
            if (audioManager != null && musicaFimDeJogo != null) audioManager.PlayMusic(musicaFimDeJogo);

            if (abelhaComRaiva != null) abelhaComRaiva.SetActive(true);
            if (abelhaNormal != null) abelhaNormal.SetActive(false);

            if (setaIndicadora != null) setaIndicadora.SetActive(false);

            StartCoroutine(ShakeObject(mainCameraTransform, duracaoTremor, magnitudeTremor));
            StartCoroutine(ShakeObject(abelhaComRaiva.transform, duracaoTremor, magnitudeTremor * 0.5f));
        }
        else
        {
            if (audioManager != null && pegarCertaSound != null) audioManager.PlaySFX(pegarCertaSound);

            petalaEscolhida.SetActive(false);
            MoverSelecao(1);
        }
    }

    private IEnumerator ShakeObject(Transform objectToShake, float duration, float magnitude)
    {
        if (objectToShake == null) yield break;

        Vector3 originalPosition = objectToShake.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            objectToShake.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        objectToShake.position = originalPosition;
    }

    void MoverSelecao(int direcao)
    {
        int proximoIndice = indiceDaPetalaSelecionada;
        for (int i = 0; i < todasAsPetalas.Count; i++)
        {
            proximoIndice += direcao;
            if (proximoIndice >= todasAsPetalas.Count) proximoIndice = 0;
            if (proximoIndice < 0) proximoIndice = todasAsPetalas.Count - 1;
            if (todasAsPetalas[proximoIndice].activeSelf)
            {
                indiceDaPetalaSelecionada = proximoIndice;
                AtualizarSelecaoVisual();
                if (audioManager != null && moverSetaSound != null) audioManager.PlaySFX(moverSetaSound);
                return;
            }
        }
    }

    void AtualizarSelecaoVisual()
    {
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            if (petalaObj.activeSelf) petalaObj.GetComponent<Petala>().Deselecionar();
        }
        GameObject petalaSelecionada = todasAsPetalas[indiceDaPetalaSelecionada];
        petalaSelecionada.GetComponent<Petala>().Selecionar();
        if (setaIndicadora != null)
        {
            if (pontosDaSet != null && pontosDaSet.Count > indiceDaPetalaSelecionada)
            {
                setaIndicadora.transform.position = pontosDaSet[indiceDaPetalaSelecionada].position;
                Vector3 direcaoParaOlhar = (mioloAbelhaNormal.transform.position - setaIndicadora.transform.position).normalized;
                // AQUI ESTÁ A CORREÇÃO FINAL
                setaIndicadora.transform.rotation = Quaternion.FromToRotation(Vector3.left, direcaoParaOlhar);
            }
        }
    }
}