using UnityEngine;

public class Player : MonoBehaviour
{
    // Variáveis de Pulo
    public float forcaPulo = 15f;
    public float jumpCutMultiplier = 0.5f;
    private Rigidbody2D rb;

    // Variáveis para checar se o jogador está no chão
    public Transform peDoJogador;
    public float raioChecagemChao = 0.2f;
    public LayerMask layerDoChao;
    private bool estaNoChao;
    private bool estavaNoChao; // Armazena o estado do chão do frame anterior

    // Variáveis para controle de pulo triplo
    private int pulosDisponiveis = 3;
    private int pulosMaximos = 3;
    private bool podePular = true;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Checa o estado atual do chão
        estaNoChao = Physics2D.OverlapCircle(peDoJogador.position, raioChecagemChao, layerDoChao);

        // Lógica de Animação
        if (anim != null)
        {
            anim.SetBool("isJumping", !estaNoChao);
            anim.SetFloat("velocityY", rb.linearVelocity.y);
        }

        // 2. Compara o estado atual com o anterior para detectar a aterrissagem
        if (estaNoChao && !estavaNoChao)
        {
            AudioManagerBesouro.instance.PlaySound("ATERRIZAGEM");
            ResetarPulos();
        }

        // Lógica de Pulo
        if (Input.GetButtonDown("Jump") && podePular && pulosDisponiveis > 0)
        {
            Pular();
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        }

        // 3. Atualiza o estado anterior do chão no final
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
        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            AudioManagerBesouro.instance.PlaySound("MORTE");
            gameObject.SetActive(false);
            Debug.Log("Jogador morreu!");
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
