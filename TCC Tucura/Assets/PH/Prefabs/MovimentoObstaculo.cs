using UnityEngine;

// Garante que o objeto sempre terá um Rigidbody2D
[RequireComponent(typeof(Rigidbody2D))]
public class MovimentoObstaculo : MonoBehaviour
{
    // Esta variável será pública para que o Spawner possa alterá-la.
    // O Spawner definirá um valor POSITIVO para mover para a direita,
    // e um valor NEGATIVO para mover para a esquerda.
    public float velocidade;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate é o lugar certo para cálculos de física
    void FixedUpdate()
    {
        // Aplica a velocidade diretamente no eixo X, 
        // mantendo qualquer velocidade vertical que o objeto possa ter (como gravidade).
        rb.linearVelocity = new Vector2(velocidade, rb.linearVelocity.y);
    }

    // É uma boa prática destruir o objeto quando ele sai da tela
    void Update()
    {
        // Se a posição X (em valor absoluto) for muito grande, o objeto está fora da tela
        if (Mathf.Abs(transform.position.x) > 15f) // Ajuste este valor se sua tela for maior
        {
            Destroy(gameObject);
        }
    }
}