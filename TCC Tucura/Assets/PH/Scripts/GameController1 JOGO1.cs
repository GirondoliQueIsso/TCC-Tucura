using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Objetos da Cena")]
    public List<GameObject> todasAsPetalas;
    public List<Transform> pontosDaSeta;
    public GameObject mioloAbelhaNormal;
    public GameObject mioloAbelhaIrritada;
    public GameObject setaIndicadora;

    [Header("Sons do Jogo (SFX)")]
    public AudioClip pegarCertaSound;  // Som ao pegar a pétala certa
    public AudioClip pegarErradaSound; // Som ao pegar a pétala errada
    public AudioClip moverSetaSound;     // Som ao mover a seta

    [Header("Músicas de Fundo")]
    public AudioClip musicaPrincipal;    // Música para tocar durante o jogo
    public AudioClip musicaFimDeJogo;    // Música para tocar quando a abelha irritada aparece

    private GameObject petalaRuim;
    private int indiceDaPetalaSelecionada = 0;
    private bool jogoAcabou = false;
    private bool isGameActive = false;
    private bool ignoreFirstInput = false;

    // Referência ao AudioManager
    private AudioManager audioManager;

    void Awake()
    {
        // Obtém a instância do AudioManager para poder usá-lo
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager não encontrado! Verifique se ele existe na cena e está configurado corretamente.");
        }
    }

    // Esta função é chamada para começar ou recomeçar o jogo
    public void IniciarJogo()
    {
        isGameActive = true;
        ignoreFirstInput = true;
        jogoAcabou = false; // Reseta o estado de "fim de jogo"

        // Escolhe uma pétala aleatória para ser a "ruim"
        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        // Configura o estado visual inicial dos objetos
        if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(false);
        if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(true);
        if (setaIndicadora != null) setaIndicadora.SetActive(true);

        AtualizarSelecaoVisual();

        // Toca a música principal do jogo usando o AudioManager
        if (audioManager != null && musicaPrincipal != null)
        {
            // O 'true' força a música a reiniciar caso ela já esteja tocando
            audioManager.PlayMusic(musicaPrincipal, true);
        }
    }

    void Update()
    {
        // Se o jogo não estiver ativo ou já tiver acabado, não faz nada
        if (!isGameActive || jogoAcabou)
        {
            return;
        }

        // Captura os inputs do jogador para mover a seta
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverSelecao(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverSelecao(-1);
        }

        // Captura o input para confirmar a escolha da pétala
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !ignoreFirstInput)
        {
            ConfirmarSelecao();
        }

        // Ignora o primeiro input para evitar seleção acidental
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
            // Lógica para navegar circularmente pela lista de pétalas
            proximoIndice += direcao;
            if (proximoIndice >= todasAsPetalas.Count) proximoIndice = 0;
            if (proximoIndice < 0) proximoIndice = todasAsPetalas.Count - 1;

            // Move a seleção apenas para uma pétala que ainda está ativa
            if (todasAsPetalas[proximoIndice].activeSelf)
            {
                indiceDaPetalaSelecionada = proximoIndice;
                AtualizarSelecaoVisual();

                // Toca o som de movimento da seta
                if (audioManager != null && moverSetaSound != null)
                {
                    audioManager.PlaySFX(moverSetaSound);
                }
                return; // Sai da função após encontrar a próxima pétala ativa
            }
        }
    }

    void AtualizarSelecaoVisual()
    {
        // Deseleciona todas as pétalas
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            if (petalaObj.activeSelf)
            {
                petalaObj.GetComponent<Petala>().Deselecionar();
            }
        }

        // Seleciona a pétala atual
        GameObject petalaSelecionada = todasAsPetalas[indiceDaPetalaSelecionada];
        petalaSelecionada.GetComponent<Petala>().Selecionar();

        // Atualiza a posição e rotação da seta indicadora
        if (setaIndicadora != null)
        {
            if (pontosDaSeta != null && pontosDaSeta.Count > indiceDaPetalaSelecionada)
            {
                setaIndicadora.transform.position = pontosDaSeta[indiceDaPetalaSelecionada].position;
                Vector3 direcaoParaOlhar = (mioloAbelhaNormal.transform.position - setaIndicadora.transform.position).normalized;
                setaIndicadora.transform.rotation = Quaternion.FromToRotation(Vector3.left, direcaoParaOlhar);
            }
        }
    }

    void ConfirmarSelecao()
    {
        GameObject petalaEscolhida = todasAsPetalas[indiceDaPetalaSelecionada];

        // Verifica se a pétala escolhida é a errada (Fim de Jogo)
        if (petalaEscolhida == petalaRuim)
        {
            jogoAcabou = true;

            // Toca o som de seleção errada
            if (audioManager != null && pegarErradaSound != null)
            {
                audioManager.PlaySFX(pegarErradaSound);
            }

            // Inicia a TRANSIÇÃO para a música de fim de jogo
            if (audioManager != null && musicaFimDeJogo != null)
            {
                audioManager.PlayMusic(musicaFimDeJogo);
            }

            // Ativa a abelha irritada e desativa os outros elementos
            if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(true);
            if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(false);
            if (setaIndicadora != null) setaIndicadora.SetActive(false);
        }
        else // Se a pétala escolhida é a certa
        {
            // Toca o som de seleção correta
            if (audioManager != null && pegarCertaSound != null)
            {
                audioManager.PlaySFX(pegarCertaSound);
            }

            // Desativa a pétala e move a seleção para a próxima
            petalaEscolhida.SetActive(false);
            MoverSelecao(1);
        }
    }
}