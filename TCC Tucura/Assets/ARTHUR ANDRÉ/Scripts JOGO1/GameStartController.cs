using UnityEngine;
using UnityEngine.UI; // Importe esta linha para trabalhar com UI

public class GameStartController : MonoBehaviour
{
    // Vari�vel para o Canvas da tela inicial
    public GameObject telaInicialCanvas;

    // Vari�vel para o Animator do bot�o "PRONTO"
    public Animator prontoButtonAnimator;

    // Vari�vel para os objetos do jogo principal (que devem aparecer depois)
    public GameObject elementosDoJogo;

    void Start()
    {
        // Garante que a tela inicial est� ativa e o jogo n�o
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
        // Se a tela inicial estiver ativa, verifique o input do espa�o
        if (telaInicialCanvas.activeSelf)
        {
            // Quando o jogador APERTA a tecla Espa�o
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Ativa a anima��o de "pressionado" do bot�o
                if (prontoButtonAnimator != null)
                {
                    // "IsPressed" � um par�metro que vamos criar no Animator
                    prontoButtonAnimator.SetBool("IsPressed", true);
                }
            }

            // Quando o jogador SOLTA a tecla Espa�o
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Volta a anima��o do bot�o para o estado normal
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
