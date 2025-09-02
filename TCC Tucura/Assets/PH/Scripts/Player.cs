using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // A variável 'velocidade' não é mais usada, mas não tem problema deixar ela aqui.
    public float velocidade = 7f;
    public float forcaPulo = 15f;
    public float jumpCutMultiplier = 0.5f; // Permite ajustar o "corte" do pulo no Inspector
    private Rigidbody2D rb;

    // Variáveis para checar se o jogador está no chão
    public Transform peDoJogador;
    public float raioChecagemChao = 0.2f;
    public LayerMask layerDoChao;
    private bool estaNoChao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Checa se o jogador está tocando o chão
        estaNoChao = Physics2D.OverlapCircle(peDoJogador.position, raioChecagemChao, layerDoChao);

        // Pulo com Altura Variável
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            // Aplica a força total do pulo quando o botão é pressionado
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }

        if (Input.GetButtonUp("Jump"))
        {
            // Se o jogador soltar o botão enquanto ainda está subindo...
            if (rb.linearVelocity.y > 0)
            {
                // ...corta a velocidade vertical
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}