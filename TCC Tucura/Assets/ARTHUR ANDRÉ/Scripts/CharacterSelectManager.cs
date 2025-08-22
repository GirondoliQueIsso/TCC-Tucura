using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // ADICIONADO: Necessário para usar TextMeshPro

public class CharacterSelectManager : MonoBehaviour
{
    [Header("CONFIGURAÇÃO")]
    // 1. Arraste aqui TODAS as "Fichas de Personagem" que você criou
    public List<CharacterData> listaDePersonagens;
    // 2. Arraste aqui os NOMES de todas as suas cenas de minigames
    public string[] cenasDeMinigames;

    [Header("REFERÊNCIAS DA UI")]
    // 3. Arraste aqui os BOTÕES da sua tela, na mesma ordem da lista acima
    public List<Button> botoesDePersonagem;

    // 4. (Opcional) Arraste um objeto de Texto para mostrar de quem é a vez
    // --- MUDANÇA AQUI ---
    public TMP_Text textoDoJogador; // Alterado de "Text" para "TMP_Text"

    private int jogadorAtualIndex = 0;

    void Start()
    {
        // Configura a aparência dos botões no início, colocando os ícones dos personagens
        for (int i = 0; i < botoesDePersonagem.Count; i++)
        {
            // Põe o ícone do personagem (da Ficha) no componente Image do botão
            botoesDePersonagem[i].GetComponent<Image>().sprite = listaDePersonagens[i].icone;
        }

        // Atualiza o texto para mostrar que é a vez do Jogador 1
        AtualizarTextoDoTurno();
    }

    // Esta função será chamada pelo OnClick() de cada botão de personagem
    public void EscolherPersonagem(int indiceDoPersonagem)
    {
        // Pega a "Ficha" do personagem escolhido com base no índice do botão
        CharacterData personagemEscolhido = listaDePersonagens[indiceDoPersonagem];

        // Guarda a escolha na lista do GameManager para usar nas próximas cenas
        GameManager.instance.selectedCharacters.Add(personagemEscolhido);
        Debug.Log("Jogador " + (jogadorAtualIndex + 1) + " escolheu: " + personagemEscolhido.nomeDoPersonagem);

        // Desativa o botão para que não possa ser escolhido de novo
        botoesDePersonagem[indiceDoPersonagem].interactable = false;

        // Passa a vez para o próximo jogador
        jogadorAtualIndex++;

        // Verifica se todos os jogadores já fizeram suas escolhas
        if (jogadorAtualIndex >= GameManager.instance.numberOfPlayers)
        {
            // Fim da seleção, hora de sortear e iniciar o minigame
            IniciarMinigameAleatorio();
        }
        else
        {
            // Se ainda faltam jogadores, apenas atualiza o texto para o próximo
            AtualizarTextoDoTurno();
        }
    }

    private void AtualizarTextoDoTurno()
    {
        // Verifica se você arrastou um objeto de texto para o campo no Inspector
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

        // Sorteia um número aleatório que corresponde a uma posição na lista de cenas
        int indiceAleatorio = Random.Range(0, cenasDeMinigames.Length);
        string cenaSorteada = cenasDeMinigames[indiceAleatorio];

        // Carrega a cena do minigame que foi sorteada
        SceneManager.LoadScene(cenaSorteada);
    }
}