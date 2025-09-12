using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // Coloque aqui o nome exato da sua cena de seleção de personagem
    [SerializeField] private string characterSelectSceneName = "CenaSelecaoPersonagem";

    // Esta função deve ser chamada pelos seus botões de 2, 3, 4 jogadores
    public void SelecionarJogadores(int quantidade)
    {
        // Usamos 'GameManager.instance' como no seu script original
        if (GameManager.instance != null)
        {
            // Define o número de jogadores
            GameManager.instance.numberOfPlayers = quantidade;

            // Limpa dados de uma partida anterior (chama a função que já existia no seu GameManager)
            GameManager.instance.ClearSelections();

            Debug.Log(quantidade + " jogadores selecionados!");
        }
        else
        {
            Debug.LogError("ERRO: GameManager não encontrado na cena!");
            return;
        }

        // Carrega a próxima cena
        SceneManager.LoadScene(characterSelectSceneName);
    }
}