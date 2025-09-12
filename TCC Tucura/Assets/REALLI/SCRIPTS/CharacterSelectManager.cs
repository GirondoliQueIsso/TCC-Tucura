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
    [Header("CONFIGURA��O DOS PERSONAGENS E CENAS")]
    public List<Personagem> ListaDePersonagens;
    public List<string> CenasDeMinigames; // <<-- DECLARA��O ESTAVA FALTANDO AQUI

    [Header("REFER�NCIAS DA INTERFACE (UI)")]
    public List<Button> BotoesDePersonagem;
    public TextMeshProUGUI TextoDoJogador;

    // --- VARI�VEIS INTERNAS ---
    private int jogadorAtual = 1;
    private List<Personagem> personagensSelecionados = new List<Personagem>(); // <<-- DECLARA��O ESTAVA FALTANDO AQUI
    private int numeroDeJogadores;

    // A fun��o Start est� correta
    void Start()
    {
        if (GameManager.instance != null)
        {
            numeroDeJogadores = GameManager.instance.numberOfPlayers;
        }
        else
        {
            Debug.LogWarning("GameManager n�o encontrado. Usando 2 jogadores como padr�o para teste.");
            numeroDeJogadores = 2;
        }

        ConfigurarBotoes();
        AtualizarTextoUI();
    }

    // A fun��o ConfigurarBotoes est� correta
    void ConfigurarBotoes()
    {
        for (int i = 0; i < BotoesDePersonagem.Count; i++)
        {
            int indexDoPersonagem = i;
            BotoesDePersonagem[i].onClick.AddListener(() => SelecionarPersonagem(indexDoPersonagem));
        }
    }

    // A fun��o SelecionarPersonagem est� correta
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

    // A fun��o AtualizarTextoUI est� correta
    void AtualizarTextoUI()
    {
        TextoDoJogador.text = "Vez do Jogador " + jogadorAtual;
    }

    // A fun��o IniciarJogo foi corrigida
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
            Debug.LogError("ERRO CR�TICO: GameManager n�o foi encontrado!");
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
            Debug.LogError("ERRO: A lista 'CenasDeMinigames' est� vazia!");
        }
    }
}