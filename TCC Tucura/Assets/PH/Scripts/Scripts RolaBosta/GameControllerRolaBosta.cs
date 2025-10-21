using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameControllerRolaBosta : MonoBehaviour
{
    public static GameControllerRolaBosta Instance;

    [Header("Configuração da Cena")]
    public List<Transform> spawnPoints;

    [Header("Configuração da UI")]
    public GameObject containerIconesJogadores;
    public GameObject prefabIconeJogador;
    public GameObject prefabIconeEspacoVazio;

    [Header("Ajuste de Posição e Tamanho da UI")]
    public int padding2Jogadores = 250;
    public int padding4Jogadores = 0;
    public float tamanhoDosIcones = 1f;

    private List<PlayerData> jogadores;
    private Dictionary<int, GameObject> uiIcones = new Dictionary<int, GameObject>();
    private int playersAlive;
    private List<PlayerData> rankingFinal = new List<PlayerData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.players.Any())
        {
            jogadores = GameManager.instance.players;
            GameManager.instance.ClearSelections();
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado! Criando jogadores de teste.");
            jogadores = new List<PlayerData>
            {
                new PlayerData(1, null, null, Color.red, 0, null),
                new PlayerData(2, null, null, Color.blue, 1, null)
            };
        }
        // A linha StartGame() foi removida daqui, como planejado.
    }

    // CORREÇÃO: A função agora é 'public' para que outros scripts possam chamá-la.
    public void StartGame()
    {
        CriarIconesUI();
        SpawnPlayers();
        playersAlive = jogadores.Count;
    }

    void CriarIconesUI()
    {
        uiIcones.Clear();
        foreach (Transform child in containerIconesJogadores.transform)
        {
            Destroy(child.gameObject);
        }

        List<int> ordemVisualDesejada = new List<int> { 3, 2, 1, 4 };

        foreach (int slotID in ordemVisualDesejada)
        {
            PlayerData jogadorNesteSlot = jogadores.FirstOrDefault(p => p.playerID == slotID);
            if (jogadorNesteSlot != null)
            {
                GameObject iconeObj = Instantiate(prefabIconeJogador, containerIconesJogadores.transform);
                iconeObj.transform.localScale = new Vector3(tamanhoDosIcones, tamanhoDosIcones, 1f);
                Image iconeImg = iconeObj.GetComponent<Image>();
                if (iconeImg != null)
                {
                    if (jogadorNesteSlot.playerIcon != null) iconeImg.sprite = jogadorNesteSlot.playerIcon;
                    iconeImg.color = Color.white;
                }
                uiIcones.Add(jogadorNesteSlot.playerID, iconeObj);
            }
            else
            {
                GameObject placeholderObj = Instantiate(prefabIconeEspacoVazio, containerIconesJogadores.transform);
                placeholderObj.transform.localScale = new Vector3(tamanhoDosIcones, tamanhoDosIcones, 1f);
            }
        }

        HorizontalLayoutGroup layoutGroup = containerIconesJogadores.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            RectOffset newPadding = new RectOffset(layoutGroup.padding.left, layoutGroup.padding.right, layoutGroup.padding.top, layoutGroup.padding.bottom);
            if (jogadores.Count == 2) newPadding.left = padding2Jogadores;
            else newPadding.left = padding4Jogadores;
            layoutGroup.padding = newPadding;
        }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < jogadores.Count; i++)
        {
            if (i >= spawnPoints.Count) continue;
            PlayerData dadosDoJogador = jogadores[i];
            GameObject prefabParaInstanciar = dadosDoJogador.playerPrefab;
            if (prefabParaInstanciar == null) continue;
            GameObject playerObj = Instantiate(prefabParaInstanciar, spawnPoints[i].position, Quaternion.identity);
            Player playerScript = playerObj.GetComponent<Player>();
            playerScript.Initialize(dadosDoJogador.playerID, this);
        }
    }

    public void PlayerDied(int playerID)
    {
        PlayerData eliminatedPlayer = jogadores.FirstOrDefault(p => p.playerID == playerID);
        if (eliminatedPlayer != null && !eliminatedPlayer.isEliminated)
        {
            eliminatedPlayer.isEliminated = true;
            rankingFinal.Insert(0, eliminatedPlayer);
        }

        if (uiIcones.ContainsKey(playerID))
        {
            Image iconeImg = uiIcones[playerID].GetComponent<Image>();
            if (iconeImg != null) iconeImg.color = Color.grey;
        }

        playersAlive--;
        if (playersAlive <= 1) EndGame();
    }

    void EndGame()
    {
        PlayerData winner = jogadores.FirstOrDefault(p => !p.isEliminated);
        if (winner != null) rankingFinal.Insert(0, winner);
        if (GameManager.instance != null) GameManager.instance.AwardPoints(rankingFinal);
        Invoke("LoadRankingScene", 2f);
    }

    void LoadRankingScene()
    {
        SceneManager.LoadScene("CenaRanking");
    }
}