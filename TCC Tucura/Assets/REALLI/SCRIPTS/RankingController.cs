using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Essencial para usar Coroutines
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RankingController : MonoBehaviour
{
    [Header("Referências dos Painéis")]
    public GameObject painel2Jogadores;
    public GameObject painel4Jogadores;

    [Header("Prefab do Placar")]
    public GameObject placarJogadorPrefab;

    [Header("Configurações da Animação")]
    [Tooltip("Quanto tempo (em segundos) a animação de preenchimento deve durar.")]
    public float duracaoDaAnimacao = 1.5f;

    // Lista para guardar referências aos placares criados
    private List<GameObject> placaresInstanciados = new List<GameObject>();

    void Start()
    {
        if (GameManager.instance != null)
        {
            ConfigurarPainel();
            // Primeiro, criamos os placares com os valores ANTIGOS
            PrepararPlacarInicial();
            // Depois, iniciamos a animação para os valores NOVOS
            StartCoroutine(AnimarPlacar());
        }
        else
        {
            Debug.LogError("ERRO CRÍTICO: GameManager não foi encontrado!");
        }
    }

    void ConfigurarPainel()
    {
        // ... (esta função continua a mesma)
    }

    // NOVA FUNÇÃO: Apenas cria os placares e os ajusta para a pontuação ANTERIOR
    void PrepararPlacarInicial()
    {
        Transform containerDoPlacar = GameManager.instance.numberOfPlayers == 2 ?
            painel2Jogadores.transform.Find("Container") :
            painel4Jogadores.transform.Find("Container");

        if (containerDoPlacar == null) return;

        foreach (Transform child in containerDoPlacar) Destroy(child.gameObject);

        List<PlayerData> players = GameManager.instance.players;

        foreach (PlayerData player in players)
        {
            GameObject placarObj = Instantiate(placarJogadorPrefab, containerDoPlacar);

            // Guarda a referência ao placar criado
            placaresInstanciados.Add(placarObj);

            Image fillImage = placarObj.transform.Find("FillImage")?.GetComponent<Image>();
            TextMeshProUGUI percentageText = placarObj.GetComponentInChildren<TextMeshProUGUI>();

            if (fillImage != null && percentageText != null)
            {
                fillImage.sprite = player.leafSprite;
                fillImage.color = Color.white;

                // Define o preenchimento e o texto para a PONTUAÇÃO ANTIGA
                fillImage.fillAmount = player.previousScore;
                percentageText.text = (player.previousScore * 100).ToString("F0") + "%";
            }
        }
    }

    // NOVA FUNÇÃO: A Corrotina que anima o preenchimento
    IEnumerator AnimarPlacar()
    {
        float tempoDecorrido = 0f;
        List<PlayerData> players = GameManager.instance.players;

        // Pega os valores iniciais e finais de cada jogador
        float[] valoresIniciais = new float[players.Count];
        float[] valoresFinais = new float[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            valoresIniciais[i] = players[i].previousScore;
            valoresFinais[i] = players[i].score;
        }

        // Loop da animação
        while (tempoDecorrido < duracaoDaAnimacao)
        {
            // Incrementa o tempo
            tempoDecorrido += Time.deltaTime;
            // Calcula o progresso da animação (de 0 a 1)
            float progresso = Mathf.Clamp01(tempoDecorrido / duracaoDaAnimacao);

            // Atualiza cada placar
            for (int i = 0; i < placaresInstanciados.Count; i++)
            {
                // Interpola suavemente entre o valor inicial e o final
                float valorAtual = Mathf.Lerp(valoresIniciais[i], valoresFinais[i], progresso);

                Image fillImage = placaresInstanciados[i].transform.Find("FillImage")?.GetComponent<Image>();
                TextMeshProUGUI percentageText = placaresInstanciados[i].GetComponentInChildren<TextMeshProUGUI>();

                if (fillImage != null && percentageText != null)
                {
                    fillImage.fillAmount = valorAtual;
                    percentageText.text = (valorAtual * 100).ToString("F0") + "%";
                }
            }

            // Espera até o próximo frame
            yield return null;
        }

        // Garante que no final, os valores estejam 100% corretos
        for (int i = 0; i < placaresInstanciados.Count; i++)
        {
            Image fillImage = placaresInstanciados[i].transform.Find("FillImage")?.GetComponent<Image>();
            TextMeshProUGUI percentageText = placaresInstanciados[i].GetComponentInChildren<TextMeshProUGUI>();
            if (fillImage != null && percentageText != null)
            {
                fillImage.fillAmount = valoresFinais[i];
                percentageText.text = (valoresFinais[i] * 100).ToString("F0") + "%";
            }
        }
    }

    // ... (suas funções ProximoMinigame e VoltarAoMenuPrincipal continuam as mesmas) ...
}