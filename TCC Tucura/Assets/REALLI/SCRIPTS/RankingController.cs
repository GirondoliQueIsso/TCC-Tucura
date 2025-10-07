using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Essencial para carregar cenas

public class RankingController : MonoBehaviour
{
    [Header("Referências dos Painéis")]
    public GameObject painel2Jogadores;
    public GameObject painel4Jogadores;

    [Header("Prefab do Placar")]
    public GameObject placarJogadorPrefab;

    void Start()
    {
        if (GameManager.instance != null)
        {
            ConfigurarPainel();
            ExibirPlacar();
        }
        else
        {
            Debug.LogError("ERRO CRÍTICO: GameManager não foi encontrado na CenaRanking! Voltando ao menu...");
            // Como medida de segurança, se o GameManager não existir, volta para o menu.
            SceneManager.LoadScene("MENU");
        }
    }

    void ConfigurarPainel()
    {
        int numJogadores = GameManager.instance.numberOfPlayers;
        painel2Jogadores.SetActive(numJogadores == 2);
        painel4Jogadores.SetActive(numJogadores != 2);
    }

    void ExibirPlacar()
    {
        Transform containerDoPlacar = GameManager.instance.numberOfPlayers == 2 ?
            painel2Jogadores.transform.Find("Container") :
            painel4Jogadores.transform.Find("Container");

        if (containerDoPlacar == null)
        {
            Debug.LogError("O GameObject 'Container' não foi encontrado como filho do painel ativo!");
            return;
        }

        foreach (Transform child in containerDoPlacar)
        {
            Destroy(child.gameObject);
        }

        List<PlayerData> players = GameManager.instance.players;

        foreach (PlayerData player in players)
        {
            GameObject placarObj = Instantiate(placarJogadorPrefab, containerDoPlacar);

            Image fillImage = placarObj.transform.Find("FillImage")?.GetComponent<Image>();
            TextMeshProUGUI percentageText = placarObj.GetComponentInChildren<TextMeshProUGUI>();

            if (fillImage != null && percentageText != null)
            {
                fillImage.sprite = player.leafSprite;
                fillImage.color = Color.white;
                fillImage.fillAmount = player.score;
                percentageText.text = (player.score * 100).ToString("F0") + "%";
            }
        }
    }

    // --- FUNÇÕES PARA OS BOTÕES ---

    // Conecte esta função ao seu botão "Próximo Jogo" ou "Continuar"
    public void ProximoMinigame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ClearSelections();
            string proximaCena = GameManager.instance.SortearProximoMinigame();
            SceneManager.LoadScene(proximaCena);
        }
    }

    // Conecte esta função ao seu botão "Sair" ou "Menu Principal"
    public void VoltarAoMenuPrincipal()
    {
        // Destrói a instância do GameManager para que uma nova partida comece do zero
        // quando o jogador voltar para o menu.
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }

        // Carrega a cena do Menu (certifique-se de que o nome "MENU" está correto)
        SceneManager.LoadScene("MENU");
    }
}