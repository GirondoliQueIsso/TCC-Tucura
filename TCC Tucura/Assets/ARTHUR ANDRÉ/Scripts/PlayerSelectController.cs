using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // MUDANÇA AQUI: Em vez de UMA string, agora temos um ARRAY de strings.
    // No Inspetor da Unity, isso vai aparecer como uma lista onde você pode colocar
    // o nome de todas as suas cenas de minigames.
    [SerializeField] private string[] nomesDasCenasDeMinigames;

    public void SelecionarJogadoresEIniciar(int quantidade)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.numberOfPlayers = quantidade;
            Debug.Log(quantidade + " jogadores selecionados!");
        }
        else
        {
            Debug.LogError("ERRO: GameManager não encontrado na cena!");
            return;
        }

        // --- LÓGICA DO SORTEIO ---

        // Primeiro, verificamos se você realmente colocou alguma cena na lista.
        if (nomesDasCenasDeMinigames.Length == 0)
        {
            Debug.LogError("ERRO: A lista de minigames está vazia! Adicione os nomes das cenas no Inspetor.");
            return; // Para a execução para não dar erro.
        }

        // Sorteamos um número de índice aleatório.
        // Se a lista tem 3 cenas, ele vai sortear 0, 1 ou 2.
        int indiceAleatorio = Random.Range(0, nomesDasCenasDeMinigames.Length);

        // Pegamos o nome da cena que está no índice sorteado.
        string cenaSorteada = nomesDasCenasDeMinigames[indiceAleatorio];

        Debug.Log("Cena sorteada: " + cenaSorteada);

        // Finalmente, carregamos a cena que foi sorteada.
        SceneManager.LoadScene(cenaSorteada);
    }
}