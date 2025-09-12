using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int playerID;
    public Sprite playerIcon;
    public bool isEliminated;
    public Color playerColor;
    public int originalIndex; // Guarda o índice original da tela de seleção (0-5)

    // O construtor agora também recebe o índice original
    public PlayerData(int id, Sprite icon, Color color, int index)
    {
        playerID = id;
        playerIcon = icon;
        isEliminated = false;
        playerColor = color;
        originalIndex = index;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int numberOfPlayers;
    public List<PlayerData> players = new List<PlayerData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetupGame(List<Sprite> characterIcons, List<Color> characterColors)
    {
        players.Clear();
        for (int i = 0; i < characterIcons.Count; i++)
        {
            // Agora, ao criar o jogador, passamos 'i' como o seu índice original
            players.Add(new PlayerData(i + 1, characterIcons[i], characterColors[i], i));
        }
    }

    public void ClearSelections()
    {
        players.Clear();
    }
}