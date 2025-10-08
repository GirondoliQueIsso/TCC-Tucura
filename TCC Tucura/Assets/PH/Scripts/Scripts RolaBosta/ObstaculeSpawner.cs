using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configura��o do Objeto")]
    public GameObject obstaclePrefab;

    // NOVO: Refer�ncias para os pontos de spawn
    [Header("Pontos de Spawn")]
    public Transform spawnPointDireita;
    public Transform spawnPointEsquerda;

    [Header("Configura��o Inicial")]
    public float velocidadeInicial = 5f;
    public float spawnRateInicial = 2f;

    [Header("Aumento de Dificuldade")]
    public float tempoParaAumentarDificuldade = 10f;
    public float incrementoDeVelocidade = 0.5f;
    public float reducaoDoSpawnRate = 0.1f;

    [Header("Limites de Dificuldade")]
    public float velocidadeMaxima = 15f;
    public float spawnRateMinimo = 0.5f;

    // Vari�veis de controle
    private float velocidadeAtual;
    private float spawnRateAtual;
    private float cronometroDificuldade;
    private float spawnTimer;

    void Start()
    {
        velocidadeAtual = velocidadeInicial;
        spawnRateAtual = spawnRateInicial;
        ResetSpawnTimer();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnObstaculo();
            ResetSpawnTimer();
        }

        cronometroDificuldade += Time.deltaTime;
        if (cronometroDificuldade >= tempoParaAumentarDificuldade)
        {
            AumentarDificuldade();
            cronometroDificuldade = 0;
        }
    }

    // M�TODO ALTERADO PARA ESCOLHER O LADO
    void SpawnObstaculo()
    {
        // Garante que os pontos de spawn foram configurados no Inspector
        if (spawnPointDireita == null || spawnPointEsquerda == null)
        {
            Debug.LogError("Pontos de spawn n�o foram definidos no Inspector!");
            return;
        }

        Transform pontoDeSpawn;
        float direcaoMovimento;

        // Sorteia um n�mero: se for menor que 0.5, nasce na esquerda, sen�o, na direita
        if (Random.Range(0f, 1f) < 0.5f)
        {
            // Nasce na esquerda e se move para a direita (velocidade positiva)
            pontoDeSpawn = spawnPointEsquerda;
            direcaoMovimento = 1f;
        }
        else
        {
            // Nasce na direita e se move para a esquerda (velocidade negativa)
            pontoDeSpawn = spawnPointDireita;
            direcaoMovimento = -1f;
        }

        // Cria o obst�culo no ponto de spawn escolhido
        GameObject novoObstaculo = Instantiate(obstaclePrefab, pontoDeSpawn.position, pontoDeSpawn.rotation);

        // Pega o script do obst�culo e define sua velocidade com a dire��o correta
        MovimentoObstaculo scriptObstaculo = novoObstaculo.GetComponent<MovimentoObstaculo>();
        if (scriptObstaculo != null)
        {
            scriptObstaculo.velocidade = velocidadeAtual * direcaoMovimento;
        }
    }

    void AumentarDificuldade()
    {
        velocidadeAtual = Mathf.Min(velocidadeAtual + incrementoDeVelocidade, velocidadeMaxima);
        spawnRateAtual = Mathf.Max(spawnRateAtual - reducaoDoSpawnRate, spawnRateMinimo);
        Debug.Log("Dificuldade Aumentada! Velocidade: " + velocidadeAtual + " | Spawn Rate: " + spawnRateAtual);
    }

    void ResetSpawnTimer()
    {
        spawnTimer = spawnRateAtual;
    }
}
