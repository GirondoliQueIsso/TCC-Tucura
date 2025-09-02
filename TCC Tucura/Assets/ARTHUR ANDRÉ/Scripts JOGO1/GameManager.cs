using UnityEngine;
using System.Collections.Generic; // Adicione esta linha para poder usar "List<T>"

public class GameManager : MonoBehaviour
{
    // A 'instância' estática (Singleton) permite acessar este GameManager
    // de qualquer outro script de forma fácil, usando "GameManager.instance".
    public static GameManager instance;

    // Esta variável vai guardar a quantidade de jogadores (2 ou 4)
    // que foi escolhida na tela de seleção.
    public int numberOfPlayers;

    // --- MUDANÇA PRINCIPAL ---
    // Em vez de guardar apenas o GameObject, agora guardamos a "ficha" completa
    // do personagem (que contém o nome, ícone e o futuro prefab).
    // Esta lista será preenchida na tela de seleção de personagem.
    public List<CharacterData> selectedCharacters = new List<CharacterData>();

    // A função Awake é chamada antes de qualquer função Start, garantindo
    // que nossa instância esteja pronta desde o início.
    private void Awake()
    {
        // Lógica do Singleton: garante que SÓ EXISTA UM GameManager no jogo inteiro.
        if (instance == null)
        {
            // Se não existe nenhuma instância, esta se torna a instância principal.
            instance = this;

            // Esta é a linha mais importante: impede que este objeto (o GameManager)
            // seja destruído quando uma nova cena é carregada.
            // Assim, mantemos as informações (nº de jogadores, personagens escolhidos) entre as cenas.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se um GameManager já existe (por exemplo, se você voltou para o menu principal),
            // este novo que tentou ser criado é destruído para evitar duplicatas e erros.
            Destroy(gameObject);
        }
    }

    // Função pública para limpar a lista de personagens escolhidos.
    // Isso é útil para chamar quando um novo jogo começa (ao sair do menu),
    // garantindo que as escolhas da partida anterior sejam apagadas.
    public void ClearSelections()
    {
        selectedCharacters.Clear();
    }
}