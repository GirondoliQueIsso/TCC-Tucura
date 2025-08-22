using UnityEngine;
using UnityEngine.UI; // Importe esta linha para trabalhar com UI

public class GameStartController : MonoBehaviour
{
    // Variável para o Canvas da tela inicial
    public GameObject telaInicialCanvas;

    // Variável para o Animator do botão "PRONTO"
    public Animator prontoButtonAnimator;

    // Variável para os objetos do jogo principal (que devem aparecer depois)
    public GameObject elementosDoJogo;

    void Start()
    {
        // Garante que a tela inicial está ativa e o jogo não
        if (telaInicialCanvas != null)
        {
            telaInicialCanvas.SetActive(true);
        }
        if (elementosDoJogo != null)
        {
            elementosDoJogo.SetActive(false);
        }
    }

    void Update()
    {
        // Se a tela inicial estiver ativa, verifique o input do espaço
        if (telaInicialCanvas.activeSelf)
        {
            // Quando o jogador APERTA a tecla Espaço
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Ativa a animação de "pressionado" do botão
                if (prontoButtonAnimator != null)
                {
                    // "IsPressed" é um parâmetro que vamos criar no Animator
                    prontoButtonAnimator.SetBool("IsPressed", true);
                }
            }

            // Quando o jogador SOLTA a tecla Espaço
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Volta a animação do botão para o estado normal
                if (prontoButtonAnimator != null)
                {
                    prontoButtonAnimator.SetBool("IsPressed", false);
                }

                // Inicia o jogo
                StartGame();
            }
        }
    }

    void StartGame()
    {
        // Desativa a tela inicial
        if (telaInicialCanvas != null)
        {
            telaInicialCanvas.SetActive(false);
        }

        // Ativa os objetos do jogo principal
        if (elementosDoJogo != null)
        {
            elementosDoJogo.SetActive(true);
        }
    }
}
