using UnityEngine;

public class MenuInicialController : MonoBehaviour
{
    public GameObject painelDoMenu;
    public GameController gameController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (painelDoMenu.activeSelf)
            {
                painelDoMenu.SetActive(false);
                gameController.IniciarJogo();

                // Adicione esta linha!
                this.enabled = false; // Desativa este script para ele não rodar mais.
            }
        }
    }
}