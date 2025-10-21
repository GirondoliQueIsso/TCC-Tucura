using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controle do Jogador")]
    public int playerID; // Identificador único para cada jogador (1, 2, 3 ou 4)

    // Variáveis de Pulo
    public float forcaPulo = 15f;
    public float jumpCutMultiplier = 0.5f;
    private Rigidbody2D rb;

    // Variáveis para checar se o jogador está no chão
    public Transform peDoJogador;
    public float raioChecagemChao = 0.2f;
    public LayerMask layerDoChao;
    private bool estaNoChao;
    private bool estavaNoChao;

    // Variáveis para controle de pulo triplo
    private int pulosDisponiveis = 3;
    private int pulosMaximos = 3;
    private bool podePular = true;

    private Animator anim;
    private string botaoPulo; // Armazena o nome do botão de pulo (ex: "Jump1", "Jump2")
    private bool isAlive = true;

    // Referência ao Game Controller do minigame
    private GameControllerRolaBosta gameController;

    public void Initialize(int id, GameControllerRolaBosta controller)
    {
        playerID = id;
        gameController = controller;
        botaoPulo = "Jump" + playerID; // Configura o botão de pulo baseado no ID
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) return;

        estaNoChao = Physics2D.OverlapCircle(peDoJogador.position, raioChecagemChao, layerDoChao);

        // DEBUG: Para ver se a checagem do chão está funcionando
        if (Input.GetKeyDown(KeyCode.T)) // Aperte T para testar
        {
            Debug.Log("Player " + playerID + " está no chão? " + estaNoChao);
        }

        if (anim != null)
        {
            anim.SetBool("isJumping", !estaNoChao);
            anim.SetFloat("velocityY", rb.linearVelocity.y);
        }

        if (estaNoChao && !estavaNoChao)
        {
            AudioManagerBesouro.instance.PlaySound("ATERRIZAGEM");
            ResetarPulos();
        }

        // DEBUG: Para ver se o input está sendo detectado
        if (Input.GetButtonDown(botaoPulo))
        {
            Debug.Log("Botão de pulo " + botaoPulo + " foi pressionado para o Player " + playerID);

            if (podePular && pulosDisponiveis > 0)
            {
                Debug.Log("Jogador " + playerID + " VAI PULAR!");
                Pular();
            }
            else
            {
                Debug.Log("Jogador " + playerID + " tentou pular mas não pôde. Condições: podePular=" + podePular + ", pulosDisponiveis=" + pulosDisponiveis);
            }
        }

        if (Input.GetButtonUp(botaoPulo))
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }

        estavaNoChao = estaNoChao;
    }

    private void Pular()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        pulosDisponiveis--;
        AudioManagerBesouro.instance.PlaySound("PULO");
        podePular = false;
        Invoke("HabilitarPulo", 0.1f);
    }

    private void HabilitarPulo()
    {
        podePular = true;
    }

    private void ResetarPulos()
    {
        pulosDisponiveis = pulosMaximos;
        podePular = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo") && isAlive)
        {
            Die();
        }
    }

    // Função para quando o jogador morrer
    private void Die()
    {
        isAlive = false;
        AudioManagerBesouro.instance.PlaySound("MORTE");
        gameObject.SetActive(false); // Desativa o objeto do jogador
        Debug.Log("Jogador " + playerID + " morreu!");

        // Avisa o GameController que este jogador foi eliminado
        if (gameController != null)
        {
            gameController.PlayerDied(playerID);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (peDoJogador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(peDoJogador.position, raioChecagemChao);
        }
    }
}