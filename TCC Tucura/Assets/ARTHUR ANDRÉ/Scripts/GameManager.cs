using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A 'instância' estática nos permite acessar este script de qualquer outro lugar
    // de forma fácil, apenas usando "GameManager.instance".
    public static GameManager instance;

    // Esta é a variável que vai guardar o número de jogadores.
    public int numberOfPlayers;

    // A função Awake é chamada antes de qualquer outra função Start.
    private void Awake()
    {
        // Este é um padrão de projeto chamado "Singleton". Ele garante que SÓ EXISTA
        // UM objeto com este script no jogo inteiro.
        if (instance == null)
        {
            // Se não existe nenhuma instância do GameManager, esta se torna a instância principal.
            instance = this;

            // Esta é a linha mais importante: ela impede que este objeto seja destruído
            // quando uma nova cena é carregada.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // CORREÇÃO AQUI: O asterisco foi removido do comentário.
            // Se um GameManager já existe (por exemplo, se você voltou para o menu),
            // este novo objeto é destruído para evitar duplicatas.
            Destroy(gameObject);
        }
    }
}