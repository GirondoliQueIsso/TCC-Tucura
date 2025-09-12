using UnityEngine;

public class Petala : MonoBehaviour
{
    private SpriteRenderer mioloRenderer; // Vamos continuar chamando de miolo por clareza
    private SpriteRenderer bordaRenderer;

    void Awake()
    {
        mioloRenderer = GetComponent<SpriteRenderer>(); // Pega o renderer da pétala com borda preta

        Transform bordaTransform = transform.Find("Borda");
        if (bordaTransform != null)
        {
            bordaRenderer = bordaTransform.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Objeto filho 'Borda' não encontrado!", gameObject);
        }

        Deselecionar(); // Garante que a borda colorida comece invisível
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