using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class Personagem
{
    public string nome;
    public Sprite iconeDoPersonagem;
    public Color corDoJogador; // Adicionamos o campo para a cor
}

public class CharacterSelectManager : MonoBehaviour
{
    [Header("CONFIGURAÇÃO DOS PERSONAGENS E CENAS")]
    public List<Personagem> ListaDePersonagens;
    public List<string> CenasDeMinigames; // <<-- DECLARAÇÃO ESTAVA FALTANDO AQUI

    [Header("REFERÊNCIAS DA INTERFACE (UI)")]
    public List<Button> BotoesDePersonagem;
    public TextMeshProUGUI TextoDoJogador;

    // --- VARIÁVEIS INTERNAS ---
    private int jogadorAtual = 1;
    private List<Personagem> personagensSelecionados = new List<Personagem>(); // <<-- DECLARAÇÃO ESTAVA FALTANDO AQUI
    private int numeroDeJogadores;

    // A função Start está correta
    void Start()
    {
        if (GameManager.instance != null)
        {
            numeroDeJogadores = GameManager.instance.numberOfPlayers;
        }
        else
        {
            Debug.LogWarning("GameManager não encontrado. Usando 2 jogadores como padrão para teste.");
            numeroDeJogadores = 2;
        }

        ConfigurarBotoes();
        AtualizarTextoUI();
    }

    // A função ConfigurarBotoes está correta
    void ConfigurarBotoes()
    {
        for (int i = 0; i < BotoesDePersonagem.Count; i++)
        {
            int indexDoPersonagem = i;
            BotoesDePersonagem[i].onClick.AddListener(() => SelecionarPersonagem(indexDoPersonagem));
        }
    }

    // A função SelecionarPersonagem está correta
    public void SelecionarPersonagem(int indexDoPersonagem)
    {
        Personagem personagemEscolhido = ListaDePersonagens[indexDoPersonagem];
        personagensSelecionados.Add(personagemEscolhido);
        BotoesDePersonagem[indexDoPersonagem].interactable = false;
        BotoesDePersonagem[indexDoPersonagem].GetComponent<Image>().color = Color.gray;

        if (personagensSelecionados.Count >= numeroDeJogadores)
        {
            TextoDoJogador.text = "Sorteando minigame...";
            IniciarJogo();
        }
        else
        {
            jogadorAtual++;
            AtualizarTextoUI();
        }
    }

    // A função AtualizarTextoUI está correta
    void AtualizarTextoUI()
    {
        TextoDoJogador.text = "Vez do Jogador " + jogadorAtual;
    }

    // A função IniciarJogo foi corrigida
    void IniciarJogo()
    {
        List<Sprite> iconesDosJogadores = new List<Sprite>();
        List<Color> coresDosJogadores = new List<Color>();

        foreach (Personagem p in personagensSelecionados)
        {
            iconesDosJogadores.Add(p.iconeDoPersonagem);
            coresDosJogadores.Add(p.corDoJogador);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.SetupGame(iconesDosJogadores, coresDosJogadores);
        }
        else
        {
            Debug.LogError("ERRO CRÍTICO: GameManager não foi encontrado!");
            return;
        }

        if (CenasDeMinigames.Count > 0)
        {
            int indiceSorteado = Random.Range(0, CenasDeMinigames.Count);
            string cenaSorteada = CenasDeMinigames[indiceSorteado];
            SceneManager.LoadScene(cenaSorteada);
        }
        else
        {
            Debug.LogError("ERRO: A lista 'CenasDeMinigames' está vazia!");
        }
    }
}