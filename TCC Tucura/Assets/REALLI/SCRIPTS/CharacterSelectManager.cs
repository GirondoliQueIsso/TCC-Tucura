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
    public Color corDoJogador;
    public Sprite spriteDaFolha; // O sprite da folha colorida correspondente
}

public class CharacterSelectManager : MonoBehaviour
{
    [Header("CONFIGURA��O DOS PERSONAGENS E CENAS")]
    public List<Personagem> ListaDePersonagens;


    [Header("REFER�NCIAS DA INTERFACE (UI)")]
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
            Debug.LogWarning("GameManager n�o encontrado. Usando 2 jogadores como padr�o para teste.");
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
        // Esta parte de coletar os dados dos jogadores est� perfeita e continua a mesma
        List<Sprite> iconesDosJogadores = new List<Sprite>();
        List<Color> coresDosJogadores = new List<Color>();
        List<Sprite> folhasDosJogadores = new List<Sprite>();

        foreach (Personagem p in personagensSelecionados)
        {
            iconesDosJogadores.Add(p.iconeDoPersonagem);
            coresDosJogadores.Add(p.corDoJogador);
            folhasDosJogadores.Add(p.spriteDaFolha);
        }

        // A l�gica de sorteio agora � feita aqui dentro
        if (GameManager.instance != null)
        {
            // 1. Envia os dados dos jogadores para o GameManager
            GameManager.instance.SetupGame(iconesDosJogadores, coresDosJogadores, folhasDosJogadores);

            // --- L�GICA DE SORTEIO ATUALIZADA E CENTRALIZADA ---
            // 2. Pede ao GameManager para sortear o pr�ximo minigame da lista dele
            string proximaCena = GameManager.instance.SortearProximoMinigame();

            // 3. Carrega a cena que o GameManager escolheu
            Debug.Log("Carregando minigame sorteado pelo GameManager: " + proximaCena);
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            Debug.LogError("ERRO CR�TICO: GameManager n�o foi encontrado!");
            // O return aqui n�o � estritamente necess�rio, mas � uma boa pr�tica
            return;
        }

        // A l�gica de sorteio antiga que estava aqui embaixo foi REMOVIDA.
    }
}