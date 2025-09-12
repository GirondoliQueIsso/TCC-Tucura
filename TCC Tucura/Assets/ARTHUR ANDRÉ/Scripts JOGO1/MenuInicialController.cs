using UnityEngine;

public class MenuInicialController : MonoBehaviour
{
    public GameObject painelDoMenu;
    //public GameController gameController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (painelDoMenu.activeSelf)
            {
                painelDoMenu.SetActive(false);
                GameController1_JOGO1.Instance.ComecarPartida();

                // Adicione esta linha!
                this.enabled = false; // Desativa este script para ele não rodar mais.
            }
        }
    }
}