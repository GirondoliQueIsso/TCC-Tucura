using UnityEngine;
using System.Collections; // Importante para usar Coroutines

public class GameStartController : MonoBehaviour
{
    // --- VARIÁVEIS ---
    public GameObject telaInicialCanvas;
    public GameObject elementosDoJogo;
    // --- NOVA REFERÊNCIA ---
    [Tooltip("Arraste o objeto 'ControleDoJogo' aqui")]
    public GameController1_JOGO1 controleDoJogo; // Referência para o script principal

    private bool jogoIniciado = false; // Trava para evitar múltiplos cliques

    void Start()
    {
        // Garante o estado inicial correto
        if (telaInicialCanvas != null) telaInicialCanvas.SetActive(true);
        if (elementosDoJogo != null) elementosDoJogo.SetActive(false);
    }

    void Update()
    {
        // Se a tela inicial estiver ativa e o jogador apertar Espaço
        if (!jogoIniciado && telaInicialCanvas.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            jogoIniciado = true; // Trava o input
            StartCoroutine(IniciarJogoCoroutine()); // Inicia o jogo de forma segura
        }
    }

    // --- LÓGICA ATUALIZADA PARA INICIAR O JOGO ---
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

        // 3. --- PASSO MÁGICO ---
        // Espera um único frame. Isso "consome" o clique do Espaço e impede
        // que o SetaPetalaController o detecte no mesmo frame.
        yield return null;

        // 4. Agora que é seguro, manda o ControleDoJogo começar a partida.
        // Ele vai criar os ícones e liberar o controle para o jogador 1.
        if (controleDoJogo != null)
        {
            controleDoJogo.ComecarPartida();
        }
        else
        {
            Debug.LogError("Referência para 'ControleDoJogo' não foi definida no GameStartController!");
        }
    }
}