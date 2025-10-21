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
    public GameObject prefabDoPersonagem; // Campo para o prefab foi adicionado aqui
    public Color corDoJogador;
    public Sprite spriteDaFolha;
}

public class CharacterSelectManager : MonoBehaviour
{
    [Header("CONFIGURAÇÃO DOS PERSONAGENS E CENAS")]
    public List<Personagem> ListaDePersonagens;

    [Header("REFERÊNCIAS DA INTERFACE (UI)")]
    public List<Button> BotoesDePersonagem;
    public TextMeshProUGUI TextoDoJogador;

    private int jogadorAtual = 1;
    private List<Personagem> personagensSelecionados = new List<Personagem>();
    private int numeroDeJogadores;

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

    void ConfigurarBotoes()
    {
        for (int i = 0; i < BotoesDePersonagem.Count; i++)
        {
            int indexDoPersonagem = i;
            BotoesDePersonagem[i].onClick.AddListener(() => SelecionarPersonagem(indexDoPersonagem));
        }
    }

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

    void AtualizarTextoUI()
    {
        TextoDoJogador.text = "Vez do Jogador " + jogadorAtual;
    }

    void IniciarJogo()
    {
        // Listas para coletar todos os dados dos personagens selecionados
        List<Sprite> iconesDosJogadores = new List<Sprite>();
        List<GameObject> prefabsDosJogadores = new List<GameObject>(); // Lista para os prefabs
        List<Color> coresDosJogadores = new List<Color>();
        List<Sprite> folhasDosJogadores = new List<Sprite>();

        foreach (Personagem p in personagensSelecionados)
        {
            iconesDosJogadores.Add(p.iconeDoPersonagem);
            prefabsDosJogadores.Add(p.prefabDoPersonagem); // Adiciona o prefab à lista
            coresDosJogadores.Add(p.corDoJogador);
            folhasDosJogadores.Add(p.spriteDaFolha);
        }

        if (GameManager.instance != null)
        {
            // Envia todos os dados, incluindo a lista de prefabs, para o GameManager
            GameManager.instance.SetupGame(iconesDosJogadores, prefabsDosJogadores, coresDosJogadores, folhasDosJogadores);

            string proximaCena = GameManager.instance.SortearProximoMinigame();
            Debug.Log("Carregando minigame sorteado pelo GameManager: " + proximaCena);
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            Debug.LogError("ERRO CRÍTICO: GameManager não foi encontrado!");
            return;
        }
    }
}