using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configuração do Objeto")]
    public GameObject obstaclePrefab;

    [Header("Configuração Inicial")]
    public float velocidadeInicial = 5f;
    public float spawnRateInicial = 2f;

    [Header("Aumento de Dificuldade")]
    public float tempoParaAumentarDificuldade = 10f; // A cada 10 segundos, o jogo fica mais difícil
    public float incrementoDeVelocidade = 0.5f;
    public float reducaoDoSpawnRate = 0.1f;

    [Header("Limites de Dificuldade")]
    public float velocidadeMaxima = 15f;
    public float spawnRateMinimo = 0.5f;

    // Variáveis de controle
    private float velocidadeAtual;
    private float spawnRateAtual;
    private float cronometroDificuldade;
    private float spawnTimer;

    void Start()
    {
        // Define os valores iniciais
        velocidadeAtual = velocidadeInicial;
        spawnRateAtual = spawnRateInicial;
        ResetSpawnTimer();
    }

    void Update()
    {
        // Contagem regressiva para criar um novo obstáculo
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnObstaculo();
            ResetSpawnTimer();
        }

        // Contagem regressiva para aumentar a dificuldade geral do jogo
        cronometroDificuldade += Time.deltaTime;
        if (cronometroDificuldade >= tempoParaAumentarDificuldade)
        {
            AumentarDificuldade();
            cronometroDificuldade = 0; // Reseta o cronômetro de dificuldade
        }
    }

    void SpawnObstaculo()
    {
        // Cria um novo obstáculo
        GameObject novoObstaculo = Instantiate(obstaclePrefab, transform.position, transform.rotation);

        // Pega o script do obstáculo e define sua velocidade
        MovimentoObstaculo scriptObstaculo = novoObstaculo.GetComponent<MovimentoObstaculo>();
        if (scriptObstaculo != null)
        {
            scriptObstaculo.velocidade = velocidadeAtual;
        }
    }

    void AumentarDificuldade()
    {
        // Aumenta a velocidade, respeitando o limite máximo
        velocidadeAtual += incrementoDeVelocidade;
        velocidadeAtual = Mathf.Min(velocidadeAtual, velocidadeMaxima); // Garante que não passe do máximo

        // Diminui o tempo de spawn (mais bolas), respeitando o limite mínimo
        spawnRateAtual -= reducaoDoSpawnRate;
        spawnRateAtual = Mathf.Max(spawnRateAtual, spawnRateMinimo); // Garante que não passe do mínimo

        Debug.Log("Dificuldade Aumentada! Velocidade: " + velocidadeAtual + " | Spawn Rate: " + spawnRateAtual);
    }

    void ResetSpawnTimer()
    {
        // Usa o spawnRateAtual, que muda com o tempo
        spawnTimer = spawnRateAtual;
    }
}