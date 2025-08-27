using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // MUDAN�A AQUI: Em vez de UMA string, agora temos um ARRAY de strings.
    // No Inspetor da Unity, isso vai aparecer como uma lista onde voc� pode colocar
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
            Debug.LogError("ERRO: GameManager n�o encontrado na cena!");
            return;
        }

        // --- L�GICA DO SORTEIO ---

        // Primeiro, verificamos se voc� realmente colocou alguma cena na lista.
        if (nomesDasCenasDeMinigames.Length == 0)
        {
            Debug.LogError("ERRO: A lista de minigames est� vazia! Adicione os nomes das cenas no Inspetor.");
            return; // Para a execu��o para n�o dar erro.
        }

        // Sorteamos um n�mero de �ndice aleat�rio.
        // Se a lista tem 3 cenas, ele vai sortear 0, 1 ou 2.
        int indiceAleatorio = Random.Range(0, nomesDasCenasDeMinigames.Length);

        // Pegamos o nome da cena que est� no �ndice sorteado.
        string cenaSorteada = nomesDasCenasDeMinigames[indiceAleatorio];

        Debug.Log("Cena sorteada: " + cenaSorteada);

        // Finalmente, carregamos a cena que foi sorteada.
        SceneManager.LoadScene(cenaSorteada);
    }
}