using UnityEngine;
using System.Collections; // Importante para usar Coroutines

public class GameStartController : MonoBehaviour
{
    // --- VARI�VEIS ---
    public GameObject telaInicialCanvas;
    public GameObject elementosDoJogo;
    // --- NOVA REFER�NCIA ---
    [Tooltip("Arraste o objeto 'ControleDoJogo' aqui")]
    public GameController1_JOGO1 controleDoJogo; // Refer�ncia para o script principal

    private bool jogoIniciado = false; // Trava para evitar m�ltiplos cliques

    void Start()
    {
        // Garante o estado inicial correto
        if (telaInicialCanvas != null) telaInicialCanvas.SetActive(true);
        if (elementosDoJogo != null) elementosDoJogo.SetActive(false);
    }

    void Update()
    {
        // Se a tela inicial estiver ativa e o jogador apertar Espa�o
        if (!jogoIniciado && telaInicialCanvas.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            jogoIniciado = true; // Trava o input
            StartCoroutine(IniciarJogoCoroutine()); // Inicia o jogo de forma segura
        }
    }

    // --- L�GICA ATUALIZADA PARA INICIAR O JOGO ---
    IEnumerator IniciarJogoCoroutine()
    {
        // 1. Desativa a tela inicial
        if (telaInicialCanvas != null)
        {
            telaInicialCanvas.SetActive(false);
        }

        // 2. Ativa os objetos principais do jogo
        if (elementosDoJogo != null)
        {
            elementosDoJogo.SetActive(true);
        }

        // 3. --- PASSO M�GICO ---
        // Espera um �nico frame. Isso "consome" o clique do Espa�o e impede
        // que o SetaPetalaController o detecte no mesmo frame.
        yield return null;

        // 4. Agora que � seguro, manda o ControleDoJogo come�ar a partida.
        // Ele vai criar os �cones e liberar o controle para o jogador 1.
        if (controleDoJogo != null)
        {
            controleDoJogo.ComecarPartida();
        }
        else
        {
            Debug.LogError("Refer�ncia para 'ControleDoJogo' n�o foi definida no GameStartController!");
        }
    }
}