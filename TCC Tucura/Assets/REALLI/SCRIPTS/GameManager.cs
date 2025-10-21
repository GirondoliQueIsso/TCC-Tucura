using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int playerID;
    public Sprite playerIcon;
    public GameObject playerPrefab; // Prefab do personagem foi adicionado aqui
    public bool isEliminated;
    public Color playerColor;
    public int originalIndex;
    public float score;
    public Sprite leafSprite;
    public float previousScore;

    // O construtor foi atualizado para receber o prefab
    public PlayerData(int id, Sprite icon, GameObject prefab, Color color, int index, Sprite leaf)
    {
        playerID = id;
        playerIcon = icon;
        playerPrefab = prefab; // Atribuição do prefab
        isEliminated = false;
        playerColor = color;
        originalIndex = index;
        score = 0f;
        previousScore = 0f;
        leafSprite = leaf;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int numberOfPlayers;
    public List<PlayerData> players = new List<PlayerData>();

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
            ResetarMinigamesDisponiveis();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Função atualizada para receber a lista de prefabs dos personagens
    public void SetupGame(List<Sprite> characterIcons, List<GameObject> characterPrefabs, List<Color> characterColors, List<Sprite> leafSprites)
    {
        players.Clear();
        for (int i = 0; i < characterIcons.Count; i++)
        {
            // Adiciona o novo PlayerData com o prefab correspondente
            players.Add(new PlayerData(i + 1, characterIcons[i], characterPrefabs[i], characterColors[i], i, leafSprites[i]));
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
        foreach (var player in players)
        {
            player.previousScore = player.score;
        }

        float[] pointsToAward;
        if (numberOfPlayers == 2)
        {
            pointsToAward = new float[] { 0.20f, 0f };
        }
        else
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

    public void ResetarMinigamesDisponiveis()
    {
        minigamesDisponiveis = new List<string>(todosOsMinigames);
    }

    public string SortearProximoMinigame()
    {
        if (minigamesDisponiveis.Count == 0)
        {
            Debug.Log("Todos os minigames foram jogados! Resetando a lista.");
            ResetarMinigamesDisponiveis();
        }

        int indiceSorteado = Random.Range(0, minigamesDisponiveis.Count);
        string cenaSorteada = minigamesDisponiveis[indiceSorteado];
        minigamesDisponiveis.RemoveAt(indiceSorteado);
        return cenaSorteada;
    }
}