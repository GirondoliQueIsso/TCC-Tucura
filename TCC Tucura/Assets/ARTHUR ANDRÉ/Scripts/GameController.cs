using UnityEngine;
using System.Collections.Generic; // Necess�rio para usar Listas
using UnityEngine.SceneManagement; // Necess�rio para reiniciar o jogo

public class GameController : MonoBehaviour
{
    [Tooltip("Arraste todos os objetos das p�talas da sua cena para c�.")]
    public List<GameObject> todasAsPetalas;

    [Tooltip("Arraste o objeto com a imagem da abelha normal para c�.")]
    public GameObject mioloAbelhaNormal;

    [Tooltip("Arraste o objeto com a imagem da abelha irritada para c�.")]
    public GameObject mioloAbelhaIrritada;

    private GameObject petalaRuim; // Vari�vel para guardar qual p�tala � a errada.

    // A fun��o Start � chamada uma vez quando o jogo come�a.
    void Start()
    {
        InicializarJogo();
    }

    void InicializarJogo()
    {
        // Garante que no in�cio a abelha normal est� vis�vel e a irritada est� invis�vel.
        mioloAbelhaNormal.SetActive(true);
        mioloAbelhaIrritada.SetActive(false);

        // Escolhe uma p�tala aleatoriamente da lista para ser a p�tala ruim.
        int indiceAleatorio = Random.Range(0, todasAsPetalas.Count);
        petalaRuim = todasAsPetalas[indiceAleatorio];

        // Avisa o script de cada p�tala se ela � a ruim ou n�o.
        foreach (GameObject petalaObj in todasAsPetalas)
        {
            // Pega o componente "Petala" do objeto.
            Petala scriptDaPetala = petalaObj.GetComponent<Petala>();
            // Passa a refer�ncia deste GameController para o script da p�tala.
            scriptDaPetala.gameController = this;

            if (petalaObj == petalaRuim)
            {
                scriptDaPetala.ehPetalaRuim = true;
            }
        }
    }

    // Esta fun��o � chamada pelo script da p�tala quando ela � clicada.
    public void PetalaFoiClicada(GameObject petalaClicada)
    {
        if (petalaClicada == petalaRuim)
        {
            // Se for a p�tala errada, fim de jogo!
            mioloAbelhaNormal.SetActive(false);
            mioloAbelhaIrritada.SetActive(true);
            Debug.Log("Voc� perdeu! A abelha ficou irritada!");
        }
        else
        {
            // Se for uma p�tala boa, ela some.
            petalaClicada.SetActive(false);
            Debug.Log("Ufa, p�tala certa!");
        }
    }
}
