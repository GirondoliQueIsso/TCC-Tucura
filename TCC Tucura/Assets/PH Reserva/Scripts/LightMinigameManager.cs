using UnityEngine;
using TMPro; // Adicione esta linha se for usar TextMeshPro para UI

public class LightMinigameManager : MonoBehaviour
{
    [Header("Configurações do Jogo")]
    public Transform luz;
    public float forcaAtracao = 2f;
    public float raioDeMorte = 0.5f;

    [Header("Referências")]
    public PlayerInsectController[] jogadores; // Coloque todos os seus jogadores aqui
    public GameObject zonaSeguraVisual; // O objeto com o anel
    public TextMeshProUGUI textoStatus; // Um texto na tela para mostrar o vencedor
    private float raioDeVitoria;

    void Start()
    {
        // LÊ o tamanho do círculo para definir o raio de vitória
        raioDeVitoria = zonaSeguraVisual.transform.localScale.x / 2f;

        Debug.Log("Raio de Vitória definido pelo círculo visual: " + raioDeVitoria);

        if (textoStatus != null) textoStatus.text = "FUJA DA LUZ!";
    }

    void FixedUpdate()
    {
        foreach (PlayerInsectController jogador in jogadores)
        {
            if (jogador != null && jogador.enabled)
            {
                Vector2 direcaoAtracao = (luz.position - jogador.transform.position).normalized;
                jogador.GetComponent<Rigidbody2D>().AddForce(direcaoAtracao * forcaAtracao);

                float distancia = Vector2.Distance(jogador.transform.position, luz.position);

                if (distancia > raioDeVitoria)
                {
                    Vitoria(jogador);
                }

                if (distancia < raioDeMorte)
                {
                    Derrota(jogador);
                }
            }
        }
    }

    void Vitoria(PlayerInsectController jogadorVencedor)
    {
        if (textoStatus != null) textoStatus.text = $"{jogadorVencedor.name} VENCEU!";

        foreach (PlayerInsectController jogador in jogadores)
        {
            if (jogador != null)
            {
                jogador.PararMovimento();
            }
        }
    }

    void Derrota(PlayerInsectController jogadorPerdedor)
    {
        Debug.Log($"{jogadorPerdedor.name} foi sugado pela luz!");
        Destroy(jogadorPerdedor.gameObject);
    }

    // --- CORREÇÃO AQUI ---
    // A função OnDrawGizmos deve estar aqui, no final do script, sozinha.
    void OnDrawGizmos()
    {
        if (luz != null)
        {
            // Tenta ler o raio de vitória do tamanho do objeto visual para mostrar no editor também
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(luz.position, raioDeVitoria);
            }
            else // Se o jogo não estiver rodando, lê diretamente da escala para preview
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(luz.position, zonaSeguraVisual.transform.localScale.x / 2f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(luz.position, raioDeMorte);
        }
    }
}