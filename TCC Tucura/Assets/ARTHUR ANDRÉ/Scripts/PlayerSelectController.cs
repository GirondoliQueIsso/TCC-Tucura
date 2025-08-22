using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectController : MonoBehaviour
{
    // Coloque aqui o nome exato da sua cena de seleção de personagem
    [SerializeField] private string characterSelectSceneName = "CenaSelecaoPersonagem";

    public void SelecionarJogadores(int quantidade)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.numberOfPlayers = quantidade;

            // CORREÇÃO AQUI: Usando o novo nome da função do GameManager
            GameManager.instance.ClearSelections();

            Debug.Log(quantidade + " jogadores selecionados!");
        }
        else
        {
            Debug.LogError("ERRO: GameManager não encontrado na cena!");
            return;
        }

        SceneManager.LoadScene(characterSelectSceneName);
    }
}