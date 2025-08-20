using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A 'inst�ncia' est�tica nos permite acessar este script de qualquer outro lugar
    // de forma f�cil, apenas usando "GameManager.instance".
    public static GameManager instance;

    // Esta � a vari�vel que vai guardar o n�mero de jogadores.
    public int numberOfPlayers;

    // A fun��o Awake � chamada antes de qualquer outra fun��o Start.
    private void Awake()
    {
        // Este � um padr�o de projeto chamado "Singleton". Ele garante que S� EXISTA
        // UM objeto com este script no jogo inteiro.
        if (instance == null)
        {
            // Se n�o existe nenhuma inst�ncia do GameManager, esta se torna a inst�ncia principal.
            instance = this;

            // Esta � a linha mais importante: ela impede que este objeto seja destru�do
            // quando uma nova cena � carregada.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // CORRE��O AQUI: O asterisco foi removido do coment�rio.
            // Se um GameManager j� existe (por exemplo, se voc� voltou para o menu),
            // este novo objeto � destru�do para evitar duplicatas.
            Destroy(gameObject);
        }
    }
}