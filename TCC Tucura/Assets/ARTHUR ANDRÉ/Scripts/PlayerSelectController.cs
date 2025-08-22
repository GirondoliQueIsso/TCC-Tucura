using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // Coloque aqui o nome exato da sua cena de sele��o de personagem
    [SerializeField] private string characterSelectSceneName = "CenaSelecaoPersonagem";

    public void SelecionarJogadores(int quantidade)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.numberOfPlayers = quantidade;

            // CORRE��O AQUI: Usando o novo nome da fun��o do GameManager
            GameManager.instance.ClearSelections();

            Debug.Log(quantidade + " jogadores selecionados!");
        }
        else
        {
            Debug.LogError("ERRO: GameManager n�o encontrado na cena!");
            return;
        }

        SceneManager.LoadScene(characterSelectSceneName);
    }
}