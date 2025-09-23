using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Essencial para funções de lista

[System.Serializable]
public class PlayerData
{
    public int playerID;
    public Sprite playerIcon;
    public bool isEliminated;
    public Color playerColor;
    public int originalIndex;
    public float score;
    public Sprite leafSprite;

    public PlayerData(int id, Sprite icon, Color color, int index, Sprite leaf)
    {
        playerID = id;
        playerIcon = icon;
        isEliminated = false;
        playerColor = color;
        originalIndex = index;
        score = 0f;
        leafSprite = leaf;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int numberOfPlayers;
    public List<PlayerData> players = new List<PlayerData>();

    // --- VARIÁVEIS PARA O CICLO DE JOGO ---
    [Header("Gerenciamento de Minigames")]
    [Tooltip("Coloque aqui o NOME DE TODAS as cenas de minigame.")]
    public List<string> todosOsMinigames;

    private List<string> minigamesDisponiveis;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Quando o jogo começa, a lista de disponíveis é uma cópia da lista mestre
            ResetarMinigamesDisponiveis();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetupGame(List<Sprite> characterIcons, List<Color> characterColors, List<Sprite> leafSprites)
    {
        players.Clear();
        for (int i = 0; i < characterIcons.Count; i++)
        {
            players.Add(new PlayerData(i + 1, characterIcons[i], characterColors[i], i, leafSprites[i]));
        }
    }

    public void ClearSelections()
    {
        foreach (var player in players)
        {
            player.isEliminated = false;
        }
    }

    public void AwardPoints(List<PlayerData> playerRanking)
    {
        float[] pointsToAward;
        if (numberOfPlayers == 2)
        {
            pointsToAward = new float[] { 0.20f, 0f };
        }
        else // Assume 4 jogadores
        {
            pointsToAward = new float[] { 0.20f, 0.10f, 0.05f, 0f };
        }

        for (int i = 0; i < playerRanking.Count; i++)
        {
            PlayerData rankedPlayer = playerRanking[i];
            PlayerData mainPlayer = players.FirstOrDefault(p => p.playerID == rankedPlayer.playerID);

            if (mainPlayer != null && i < pointsToAward.Length)
            {
                mainPlayer.score += pointsToAward[i];
                mainPlayer.score = Mathf.Clamp01(mainPlayer.score);
            }
        }
    }

    // --- FUNÇÕES DE SORTEIO QUE ESTAVAM FALTANDO ---

    public void ResetarMinigamesDisponiveis()
    {
        // Cria uma nova lista de disponíveis, copiando todos os itens da lista mestre
        minigamesDisponiveis = new List<string>(todosOsMinigames);
    }

    public string SortearProximoMinigame()
    {
        // Se a lista de disponíveis ficou vazia, reseta ela
        if (minigamesDisponiveis.Count == 0)
        {
            Debug.Log("Todos os minigames foram jogados! Resetando a lista.");
            ResetarMinigamesDisponiveis();
        }

        // Sorteia um minigame da lista de disponíveis
        int indiceSorteado = Random.Range(0, minigamesDisponiveis.Count);
        string cenaSorteada = minigamesDisponiveis[indiceSorteado];

        // Remove o minigame sorteado para não repetir
        minigamesDisponiveis.RemoveAt(indiceSorteado);

        return cenaSorteada;
    }
}