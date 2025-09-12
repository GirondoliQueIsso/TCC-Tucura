using UnityEngine;

public class Petala : MonoBehaviour
{
    private SpriteRenderer mioloRenderer; // Vamos continuar chamando de miolo por clareza
    private SpriteRenderer bordaRenderer;

    void Awake()
    {
        mioloRenderer = GetComponent<SpriteRenderer>(); // Pega o renderer da p�tala com borda preta

        Transform bordaTransform = transform.Find("Borda");
        if (bordaTransform != null)
        {
            bordaRenderer = bordaTransform.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Objeto filho 'Borda' n�o encontrado!", gameObject);
        }

        Deselecionar(); // Garante que a borda colorida comece invis�vel
    }

    public void Selecionar(Color corDoJogador)
    {
        if (bordaRenderer != null)
        {
            bordaRenderer.enabled = true; // Mostra a borda colorida
            bordaRenderer.color = corDoJogador; // Pinta a borda
        }
    }

    public void Deselecionar()
    {
        if (bordaRenderer != null)
        {
            bordaRenderer.enabled = false; // Esconde a borda colorida
        }
    }
}