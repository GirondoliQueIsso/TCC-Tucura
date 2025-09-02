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
    public AudioClip pegarCertaSound;  // Som ao pegar a p�tala certa
    public AudioClip pegarErradaSound; // Som ao pegar a p�tala errada
    public AudioClip moverSetaSound;     // Som ao mover a seta

    [Header("M�sicas de Fundo")]
    public AudioClip musicaPrincipal;    // M�sica para tocar durante o jogo
    public AudioClip musicaFimDeJogo;    // M�sica para tocar quando a abelha irritada aparece

    private GameObject petalaRuim;
    private int indiceDaPetalaSelecionada = 0;
    private bool jogoAcabou = false;
    private bool isGameActive = false;
    private bool ignoreFirstInput = false;

    // Refer�ncia ao AudioManager
    private AudioManager audioManager;

    void Awake()
    {
        // Obt�m a inst�ncia do AudioManager para poder us�-lo
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager n�o encontrado! Verifique se ele existe na cena e est� configurado corretamente.");
        }
    }

    // Esta fun��o � chamada para come�ar ou recome�ar o jogo
    public void IniciarJogo()
    {
        isGameActive = true;
        ignoreFirstInput = true;
        jogoAcabou = false; // Reseta o estado de "fim de jogo"

        // Escolhe uma p�tala aleat�ria para ser a "ruim"
        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        // Configura o estado visual inicial dos objetos
        if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(false);
        if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(true);
        if (setaIndicadora != null) setaIndicadora.SetActive(true);

        AtualizarSelecaoVisual();

        // Toca a m�sica principal do jogo usando o AudioManager
        if (audioManager != null && musicaPrincipal != null)
        {
            // O 'true' for�a a m�sica a reiniciar caso ela j� esteja tocando
            audioManager.PlayMusic(musicaPrincipal, true);
        }
    }

    void Update()
    {
        // Se o jogo n�o estiver ativo ou j� tiver acabado, n�o faz nada
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

        // Captura o input para confirmar a escolha da p�tala
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !ignoreFirstInput)
        {
            ConfirmarSelecao();
        }

        // Ignora o primeiro input para evitar sele��o acidental
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
            // L�gica para navegar circularmente pela lista de p�talas
            proximoIndice += direcao;
            if (proximoIndice >= todasAsPetalas.Count) proximoIndice = 0;
            if (proximoIndice < 0) proximoIndice = todasAsPetalas.Count - 1;

            // Move a sele��o apenas para uma p�tala que ainda est� ativa
            if (todasAsPetalas[proximoIndice].activeSelf)
            {
                indiceDaPetalaSelecionada = proximoIndice;
                AtualizarSelecaoVisual();

                // Toca o som de movimento da seta
                if (audioManager != null && moverSetaSound != null)
                {
                    audioManager.PlaySFX(moverSetaSound);
                }
                return; // Sai da fun��o ap�s encontrar a pr�xima p�tala ativa
            }
        }
    }

    void AtualizarSelecaoVisual()
    {
        // Deseleciona todas as p�talas
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            if (petalaObj.activeSelf)
            {
                petalaObj.GetComponent<Petala>().Deselecionar();
            }
        }

        // Seleciona a p�tala atual
        GameObject petalaSelecionada = todasAsPetalas[indiceDaPetalaSelecionada];
        petalaSelecionada.GetComponent<Petala>().Selecionar();

        // Atualiza a posi��o e rota��o da seta indicadora
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

        // Verifica se a p�tala escolhida � a errada (Fim de Jogo)
        if (petalaEscolhida == petalaRuim)
        {
            jogoAcabou = true;

            // Toca o som de sele��o errada
            if (audioManager != null && pegarErradaSound != null)
            {
                audioManager.PlaySFX(pegarErradaSound);
            }

            // Inicia a TRANSI��O para a m�sica de fim de jogo
            if (audioManager != null && musicaFimDeJogo != null)
            {
                audioManager.PlayMusic(musicaFimDeJogo);
            }

            // Ativa a abelha irritada e desativa os outros elementos
            if (mioloAbelhaIrritada != null) mioloAbelhaIrritada.SetActive(true);
            if (mioloAbelhaNormal != null) mioloAbelhaNormal.SetActive(false);
            if (setaIndicadora != null) setaIndicadora.SetActive(false);
        }
        else // Se a p�tala escolhida � a certa
        {
            // Toca o som de sele��o correta
            if (audioManager != null && pegarCertaSound != null)
            {
                audioManager.PlaySFX(pegarCertaSound);
            }

            // Desativa a p�tala e move a sele��o para a pr�xima
            petalaEscolhida.SetActive(false);
            MoverSelecao(1);
        }
    }
}