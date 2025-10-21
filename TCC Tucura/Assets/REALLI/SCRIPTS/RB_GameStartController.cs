using UnityEngine;
using System.Collections;

// CORREÇÃO: Adicionamos ": MonoBehaviour" para que a Unity reconheça o script.
public class RB_GameStartController : MonoBehaviour
{
    [Header("Referências da Cena")]
    [Tooltip("Arraste o objeto 'PainelIntroducao' aqui.")]
    public GameObject telaInicial;

    [Tooltip("Arraste o objeto 'GameplayElementos' que contém os sistemas do jogo.")]
    public GameObject gameplayElementos;

    [Tooltip("Arraste o objeto 'GameController' principal do minigame.")]
    public GameControllerRolaBosta controleDoJogoRB;

    private bool jogoIniciado = false;

    void Start()
    {
        if (telaInicial != null) telaInicial.SetActive(true);
        if (gameplayElementos != null) gameplayElementos.SetActive(false);
    }

    void Update()
    {
        if (!jogoIniciado && telaInicial.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            jogoIniciado = true;
            StartCoroutine(IniciarJogoCoroutine());
        }
    }

    IEnumerator IniciarJogoCoroutine()
    {
        if (telaInicial != null) telaInicial.SetActive(false);
        if (gameplayElementos != null) gameplayElementos.SetActive(true);
        yield return null; // Espera 1 frame

        if (controleDoJogoRB != null) controleDoJogoRB.StartGame();
        else Debug.LogError("Referência para 'ControleDoJogoRB' não foi definida!");
    }
}