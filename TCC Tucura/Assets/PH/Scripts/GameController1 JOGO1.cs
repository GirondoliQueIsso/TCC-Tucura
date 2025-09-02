using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("Objetos da Cena")]
    public List<GameObject> todasAsPetalas;
    public List<Transform> pontosDaSeta;
    public GameObject mioloAbelhaNormal;
    public GameObject mioloAbelhaIrritada;
    public GameObject setaIndicadora;

    public AudioSource audioSource;
    public AudioClip pegarCertaSound; // Som ao mover a seta
    public AudioClip pegarErradaSound; // Som ao mover a seta

    private GameObject petalaRuim;
    private int indiceDaPetalaSelecionada = 0;
    private bool jogoAcabou = false;
    private bool isGameActive = false;
    private bool ignoreFirstInput = false;

    public void IniciarJogo()
    {
        isGameActive = true;
        ignoreFirstInput = true;

        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(false);
        if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(true);
        if (setaIndicadora != null) setaIndicadora.SetActive(true);

        AtualizarSelecaoVisual();
    }

    void Update()
    {
        if (!isGameActive || jogoAcabou)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverSelecao(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverSelecao(-1);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !ignoreFirstInput)
        {
            ConfirmarSelecao();
        }

        if (ignoreFirstInput)
        {
            ignoreFirstInput = false;
        }
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
                return;
            }
        }
    }

    void AtualizarSelecaoVisual()
    {
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            if (petalaObj.activeSelf)
            {
                petalaObj.GetComponent<Petala>().Deselecionar();
            }
        }

        GameObject petalaSelecionada = todasAsPetalas[indiceDaPetalaSelecionada];
        petalaSelecionada.GetComponent<Petala>().Selecionar();

        if (setaIndicadora != null)
        {
            if (pontosDaSeta != null && pontosDaSeta.Count > indiceDaPetalaSelecionada)
            {
                // Posição continua a mesma
                setaIndicadora.transform.position = pontosDaSeta[indiceDaPetalaSelecionada].position;

                // --- ROTAÇÃO INTELIGENTE (AQUI ESTÁ A MUDANÇA) ---
                Vector3 direcaoParaOlhar = (mioloAbelhaNormal.transform.position - setaIndicadora.transform.position).normalized;
                setaIndicadora.transform.rotation = Quaternion.FromToRotation(Vector3.left, direcaoParaOlhar);
            }
        }
    }

    void ConfirmarSelecao()
    {
        GameObject petalaEscolhida = todasAsPetalas[indiceDaPetalaSelecionada];
        if (petalaEscolhida == petalaRuim)
        {
            jogoAcabou = true;
            audioSource.PlayOneShot(pegarErradaSound);
            if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(true);
            if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(false);
            if (setaIndicadora != null) setaIndicadora.SetActive(false);
        }
        else
        {
            audioSource.PlayOneShot(pegarCertaSound);
            petalaEscolhida.SetActive(false);
            MoverSelecao(1);
        }
    }
}