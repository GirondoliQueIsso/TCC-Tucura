using UnityEngine;

public class PlayerInsectController : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    public KeyCode teclaDeFuga;
    public float forcaDeFuga = 25f; // Já comece com um valor mais alto

    [Header("Referências")]
    public Transform luz;

    private Rigidbody2D rb;
    private bool podeSeMover = true;

    // NOVO: Contador para cliques rápidos
    private int cliquesParaAplicar = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update é melhor para capturar inputs rápidos
    void Update()
    {
        if (podeSeMover && Input.GetKeyDown(teclaDeFuga))
        {
            Debug.Log("TECLA DE FUGA PRESSIONADA!");

            // Em vez de aplicar a força aqui, nós apenas contamos o clique
            cliquesParaAplicar++;
        }
    }

    // FixedUpdate é melhor para aplicar forças
    void FixedUpdate()
    {
        // Se tivermos algum clique para aplicar...
        if (cliquesParaAplicar > 0)
        {
            // Calculamos a direção para longe da luz
            Vector2 direcaoFuga = ((Vector2)transform.position - (Vector2)luz.position).normalized;

            // Aplicamos uma força TOTAL baseada em quantos cliques foram dados
            rb.AddForce(direcaoFuga * forcaDeFuga * cliquesParaAplicar, ForceMode2D.Impulse);

            // Resetamos o contador
            cliquesParaAplicar = 0;
        }
    }

    public void PararMovimento()
    {
        podeSeMover = false;
        rb.linearVelocity = Vector2.zero;
    }
}