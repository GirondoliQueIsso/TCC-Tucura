using UnityEngine;

public class Petala : MonoBehaviour
{
    // Esta variável vai guardar a referência ao nosso controlador do jogo.
    public GameController gameController;
    // Esta variável vai dizer se esta é a pétala "ruim".
    public bool ehPetalaRuim = false;

    // Esta função é chamada automaticamente pelo Unity quando o objeto é clicado.
    void OnMouseDown()
    {
        // Avisa o controlador do jogo que esta pétala específica foi clicada.
        gameController.PetalaFoiClicada(this.gameObject);
    }
}
