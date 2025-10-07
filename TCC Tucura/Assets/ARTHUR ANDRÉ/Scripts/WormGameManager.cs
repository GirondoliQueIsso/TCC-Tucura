using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WormGameManager : MonoBehaviour
{
    public static WormGameManager Instance;

    [Header("Configurações do Jogo")]
    public GameObject starPrefab;
    public Rect spawnArea;
    public int scoreToWin = 3;
    public float starSpawnDelay = 3f;

    [Header("Gerenciamento de Jogadores")]
    public List<WormController> players;

    [Header("Interface (UI)")]
    public List<TextMeshProUGUI> scoreTexts;
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;

    private HashSet<WormController> wormsToKill = new HashSet<WormController>();
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (winnerPanel != null) winnerPanel.SetActive(false);
        UpdateScoreUI();
        Invoke("SpawnStar", starSpawnDelay);
    }

    // --- ALTERAÇÃO 1: CHAMADA REMOVIDA DAQUI ---
    void LateUpdate()
    {
        if (wormsToKill.Count > 0)
        {
            foreach (WormController worm in wormsToKill)
            {
                if (worm != null)
                {
                    worm.Die();
                }
            }
            wormsToKill.Clear();
            // A linha "CheckForLastWormStanding();" foi removida daqui.
        }
    }

    public void SpawnStar()
    {
        if (isGameOver || starPrefab == null) return;
        float randomX = Random.Range(spawnArea.x, spawnArea.x + spawnArea.width);
        float randomY = Random.Range(spawnArea.y, spawnArea.y + spawnArea.height);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);
        Instantiate(starPrefab, randomPosition, Quaternion.identity);
    }

    public void ReportCollision(WormController initiatorWorm, GameObject collidedWith)
    {
        if (isGameOver || string.IsNullOrEmpty(collidedWith.tag)) return;

        switch (collidedWith.tag)
        {
            case "Parede":
                wormsToKill.Add(initiatorWorm);
                break;

            case "Star":
                Destroy(collidedWith);
                initiatorWorm.score++;
                initiatorWorm.GrowWorm(1);
                UpdateScoreUI();

                if (initiatorWorm.score >= scoreToWin)
                {
                    EndGame(initiatorWorm);
                }
                else
                {
                    Invoke("SpawnStar", starSpawnDelay);
                }
                break;

            case "Player":
                WormController otherWorm = collidedWith.GetComponent<WormController>();
                if (otherWorm != null)
                {
                    wormsToKill.Add(initiatorWorm);
                    wormsToKill.Add(otherWorm);
                }
                break;

            case "Body":
                BodySegmentController segment = collidedWith.GetComponent<BodySegmentController>();
                if (segment != null && segment.ownerID != initiatorWorm.playerID)
                {
                    wormsToKill.Add(initiatorWorm);
                }
                break;
        }
    }

    void UpdateScoreUI()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (i < scoreTexts.Count && players[i] != null && scoreTexts[i] != null)
            {
                scoreTexts[i].text = "P" + (i + 1) + ": " + players[i].score;
            }
        }
    }

    // --- ALTERAÇÃO 2: FUNÇÃO ESVAZIADA ---
    // Esta função não faz mais nada. O jogo não acaba mais por eliminação.
    void CheckForLastWormStanding()
    {
        // Vazio
    }

    void EndGame(WormController winner)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (winnerText != null) winnerText.text = "JOGADOR " + winner.playerID + " VENCEU!";
        if (winnerPanel != null) winnerPanel.SetActive(true);

        // Para todas as minhocas restantes, mesmo que não estejam na lista original
        WormController[] allWorms = FindObjectsOfType<WormController>();
        foreach (var worm in allWorms)
        {
            if (worm != null)
            {
                worm.enabled = false;
                Rigidbody2D rb = worm.GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(spawnArea.x + spawnArea.width / 2, spawnArea.y + spawnArea.height / 2, 0), new Vector3(spawnArea.width, spawnArea.height, 0));
    }
}