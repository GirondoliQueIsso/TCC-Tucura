using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;

    // Esta fun��o ser� chamada pelo bot�o
    public void IniciarJogo()
    {
        SceneManager.LoadScene("CENA JOGO");
    }
    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }
    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);

    }
    public void Sair()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}


