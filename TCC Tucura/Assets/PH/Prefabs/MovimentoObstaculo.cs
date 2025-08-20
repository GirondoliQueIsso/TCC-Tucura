using UnityEngine;

public class MovimentoObstaculo : MonoBehaviour
{
    public float velocidade = 5f; // Velocidade de movimento da bola
    private Rigidbody2D rb;
    private Vector2 direcao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Decide aleatoriamente se a bola come�a indo para a direita ou para a esquerda
        if (Random.value > 0.5f)
        {
            direcao = Vector2.right;
        }
        else
        {
            direcao = Vector2.left;
        }

        // Destr�i a bola depois de um tempo para n�o poluir a cena
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        // Move o objeto usando a f�sica. Como ele � Kinematic, definimos a velocidade diretamente.
        rb.linearVelocity = direcao * velocidade;
    }
}