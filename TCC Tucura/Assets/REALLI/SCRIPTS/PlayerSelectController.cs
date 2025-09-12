using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // Coloque aqui o nome exato da sua cena de sele��o de personagem
    [SerializeField] private string characterSelectSceneName = "CenaSelecaoPersonagem";

    // Esta fun��o deve ser chamada pelos seus bot�es de 2, 3, 4 jogadores
    public void SelecionarJogadores(int quantidade)
    {
        // Usamos 'GameManager.instance' como no seu script original
        if (GameManager.instance != null)
        {
            // Define o n�mero de jogadores
            GameManager.instance.numberOfPlayers = quantidade;

            // Limpa dados de uma partida anterior (chama a fun��o que j� existia no seu GameManager)
            GameManager.instance.ClearSelections();

            Debug.Log(quantidade + " jogadores selecionados!");
        }
        else
        {
            Debug.LogError("ERRO: GameManager n�o encontrado na cena!");
            return;
        }

        // Carrega a pr�xima cena
        SceneManager.LoadScene(characterSelectSceneName);
    }
}