using UnityEngine;

// Esta linha mágica permite criar "fichas de personagem" direto no menu do Unity
[CreateAssetMenu(fileName = "NovoPersonagem", menuName = "Tucura/Dados de Personagem")]
public class CharacterData : ScriptableObject
{
    public string nomeDoPersonagem;
    public Sprite icone; // Imagem para a tela de seleção (nosso placeholder)
    public GameObject prefabDoPersonagem; // O boneco que será usado no minigame (pode deixar em branco por agora)
}