using UnityEngine;

public class Petala : MonoBehaviour
{
    // Esta vari�vel vai guardar a refer�ncia ao nosso controlador do jogo.
    public GameController gameController;
    // Esta vari�vel vai dizer se esta � a p�tala "ruim".
    public bool ehPetalaRuim = false;

    // Esta fun��o � chamada automaticamente pelo Unity quando o objeto � clicado.
    void OnMouseDown()
    {
        // Avisa o controlador do jogo que esta p�tala espec�fica foi clicada.
        gameController.PetalaFoiClicada(this.gameObject);
    }
}
