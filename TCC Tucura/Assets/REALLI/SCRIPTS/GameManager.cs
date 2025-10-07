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
    public float previousScore; // Guarda a pontuação antes do último minigame

    public PlayerData(int id, Sprite icon, Color color, int index, Sprite leaf)
    {
        playerID = id;
        playerIcon = icon;
        isEliminated = false;
        playerColor = color;
        originalIndex = index;
        score = 0f;
        previousScore = 0f; // Garante que a pontuação anterior também comece em zero
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

    public void SetupGame(List<Sprite> characterIcons, List<Color> characterColors, List<Sprite> leafSprites)
    {
        // Esta função só é chamada no início de uma partida completa,
        // então ela reseta/cria os jogadores do zero.
        players.Clear();
        for (int i = 0; i < characterIcons.Count; i++)
        {
            players.Add(new PlayerData(i + 1, characterIcons[i], characterColors[i], i, leafSprites[i]));
        }
    }

    public void ClearSelections()
    {
        // Reseta o status de "eliminado" de todos os jogadores para o próximo minigame
        foreach (var player in players)
        {
            player.isEliminated = false;
        }
    }

    // A função para adicionar pontos ao final de um minigame
    public void AwardPoints(List<PlayerData> playerRanking)
    {
        // --- LÓGICA ATUALIZADA ---
        // 1. Antes de adicionar os novos pontos, salvamos a pontuação atual de cada jogador
        foreach (var player in players)
        {
            player.previousScore = player.score;
        }

        // 2. Define a tabela de pontos
        float[] pointsToAward;
        if (numberOfPlayers == 2)
        {
            pointsToAward = new float[] { 0.20f, 0f };
        }
        else // Assume 4 jogadores
        {
            pointsToAward = new float[] { 0.20f, 0.10f, 0.05f, 0f };
        }

        // 3. Adiciona os novos pontos
        for (int i = 0; i < playerRanking.Count; i++)
        {
            PlayerData rankedPlayer = playerRanking[i];
            PlayerData mainPlayer = players.FirstOrDefault(p => p.playerID == rankedPlayer.playerID);

            if (mainPlayer != null && i < pointsToAward.Length)
            {
                mainPlayer.score += pointsToAward[i];
                mainPlayer.score = Mathf.Clamp01(mainPlayer.score); // Garante que a pontuação não passe de 1.0 (100%)
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