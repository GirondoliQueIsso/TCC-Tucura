using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // ADICIONADO: Necess�rio para usar TextMeshPro

public class CharacterSelectManager : MonoBehaviour
{
    [Header("CONFIGURA��O")]
    // 1. Arraste aqui TODAS as "Fichas de Personagem" que voc� criou
    public List<CharacterData> listaDePersonagens;
    // 2. Arraste aqui os NOMES de todas as suas cenas de minigames
    public string[] cenasDeMinigames;

    [Header("REFER�NCIAS DA UI")]
    // 3. Arraste aqui os BOT�ES da sua tela, na mesma ordem da lista acima
    public List<Button> botoesDePersonagem;

    // 4. (Opcional) Arraste um objeto de Texto para mostrar de quem � a vez
    // --- MUDAN�A AQUI ---
    public TMP_Text textoDoJogador; // Alterado de "Text" para "TMP_Text"

    private int jogadorAtualIndex = 0;

    void Start()
    {
        // Configura a apar�ncia dos bot�es no in�cio, colocando os �cones dos personagens
        for (int i = 0; i < botoesDePersonagem.Count; i++)
        {
            // P�e o �cone do personagem (da Ficha) no componente Image do bot�o
            botoesDePersonagem[i].GetComponent<Image>().sprite = listaDePersonagens[i].icone;
        }

        // Atualiza o texto para mostrar que � a vez do Jogador 1
        AtualizarTextoDoTurno();
    }

    // Esta fun��o ser� chamada pelo OnClick() de cada bot�o de personagem
    public void EscolherPersonagem(int indiceDoPersonagem)
    {
        // Pega a "Ficha" do personagem escolhido com base no �ndice do bot�o
        CharacterData personagemEscolhido = listaDePersonagens[indiceDoPersonagem];

        // Guarda a escolha na lista do GameManager para usar nas pr�ximas cenas
        GameManager.instance.selectedCharacters.Add(personagemEscolhido);
        Debug.Log("Jogador " + (jogadorAtualIndex + 1) + " escolheu: " + personagemEscolhido.nomeDoPersonagem);

        // Desativa o bot�o para que n�o possa ser escolhido de novo
        botoesDePersonagem[indiceDoPersonagem].interactable = false;

        // Passa a vez para o pr�ximo jogador
        jogadorAtualIndex++;

        // Verifica se todos os jogadores j� fizeram suas escolhas
        if (jogadorAtualIndex >= GameManager.instance.numberOfPlayers)
        {
            // Fim da sele��o, hora de sortear e iniciar o minigame
            IniciarMinigameAleatorio();
        }
        else
        {
            // Se ainda faltam jogadores, apenas atualiza o texto para o pr�ximo
            AtualizarTextoDoTurno();
        }
    }

    private void AtualizarTextoDoTurno()
    {
        // Verifica se voc� arrastou um objeto de texto para o campo no Inspector
        if (textoDoJogador != null)
        {
            // Atualiza o texto na tela
            textoDoJogador.text = "JOGADOR " + (jogadorAtualIndex + 1) + ", ESCOLHA!";
        }
    }

    private void IniciarMinigameAleatorio()
    {
        if (textoDoJogador != null)
        {
            textoDoJogador.text = "SORTEANDO MINIGAME...";
        }

        Debug.Log("Todos escolheram. Sorteando minigame...");

        // Sorteia um n�mero aleat�rio que corresponde a uma posi��o na lista de cenas
        int indiceAleatorio = Random.Range(0, cenasDeMinigames.Length);
        string cenaSorteada = cenasDeMinigames[indiceAleatorio];

        // Carrega a cena do minigame que foi sorteada
        SceneManager.LoadScene(cenaSorteada);
    }
}