using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameController1_JOGO1 : MonoBehaviour
{
    public static GameController1_JOGO1 Instance;

    [Header("Objetos da Cena")]
    public GameObject elementosDoJogo;
    public List<GameObject> TodasAsPetalas;
    public List<Transform> PontosDaSet;
    public GameObject MioloAbelhaNormal;
    public SetaPetalaController SetaIndicadora;

    [Header("Abelhas")]
    public GameObject AbelhaNormal;
    public GameObject AbelhaComRaiva;

    [Header("Efeitos")]
    public Transform MainCameraTransform;
    public float DuracaoTremor = 2f;
    public float MagnitudeTremor = 0.1f;

    [Header("Sons do Jogo (SFX)")]
    public AudioClip PegarCertaSound;
    public AudioClip PegarErradaSound;
    public AudioClip MoverSetaSound;

    [Header("M�sicas de Fundo")]
    public AudioClip musicaDaPartida;
    public AudioClip musicaDoVencedor;

    [Header("Configura��es da UI de Turnos")]
    public TextMeshProUGUI textoJogadorDaVez;
    public GameObject containerIconesJogadores;
    public GameObject prefabIconeJogador;

    private List<PlayerData> jogadores;
    private List<GameObject> uiIcones = new List<GameObject>();
    private int jogadorAtualIndex = 0;
    private int petalaRuimIndex = -1;
    private int indicePetalaAtual = 0;
    private bool podeControlarSeta = false;
    private bool ignorarPrimeiroInput = false;
    private int ultimoJogadorQueIniciouRodada = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.players.Any())
        {
            jogadores = GameManager.instance.players;
        }
        else
        {
            Debug.LogWarning("GameManager n�o encontrado! Criando jogadores de teste.");
            // Exemplo de como seria com o novo construtor para teste
            jogadores = new List<PlayerData>
            {
                new PlayerData(1, null, Color.red, 0),
                new PlayerData(2, null, Color.blue, 1)
            };
        }
        elementosDoJogo.SetActive(false);
    }

    public void ComecarPartida()
    {
        ignorarPrimeiroInput = true;
        elementosDoJogo.SetActive(true);
        CriarIconesUI();
        IniciarRodada();

        if (AudioManager.Instance != null && musicaDaPartida != null)
        {
            AudioManager.Instance.PlayMusic(musicaDaPartida, true);
        }
    }

    void Update()
    {
        if (!podeControlarSeta) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) MoverSelecao(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoverSelecao(-1);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (ignorarPrimeiroInput)
            {
                ignorarPrimeiroInput = false;
            }
            else
            {
                ConfirmarSelecao();
            }
        }
    }

    public void ConfirmarSelecao()
    {
        podeControlarSeta = false;

        if (indicePetalaAtual == petalaRuimIndex)
        {
            if (AudioManager.Instance != null && PegarErradaSound != null)
            {
                AudioManager.Instance.DuckMusic(PegarErradaSound);
            }
            AbelhaNormal?.SetActive(false);
            AbelhaComRaiva?.SetActive(true);
            SetaIndicadora?.gameObject.SetActive(false);
            StartCoroutine(ShakeObject(MainCameraTransform, DuracaoTremor, MagnitudeTremor));
            StartCoroutine(ShakeObject(AbelhaComRaiva.transform, DuracaoTremor, MagnitudeTremor * 0.5f));
            StartCoroutine(ProcessoDeEliminacao());
        }
        else
        {
            if (AudioManager.Instance != null && PegarCertaSound != null)
            {
                AudioManager.Instance.PlaySFX(PegarCertaSound);
            }
            TodasAsPetalas[indicePetalaAtual].SetActive(false);
            ProximoJogador();
        }
    }

    void MoverSelecao(int direcao)
    {
        int proximoIndice = indicePetalaAtual;
        for (int i = 0; i < TodasAsPetalas.Count; i++)
        {
            proximoIndice += direcao;
            if (proximoIndice >= TodasAsPetalas.Count) proximoIndice = 0;
            if (proximoIndice < 0) proximoIndice = TodasAsPetalas.Count - 1;

            if (TodasAsPetalas[proximoIndice].activeSelf)
            {
                indicePetalaAtual = proximoIndice;
                AtualizarSelecaoVisual();

                if (AudioManager.Instance != null && MoverSetaSound != null)
                {
                    AudioManager.Instance.PlaySFX(MoverSetaSound);
                }
                return;
            }
        }
    }

    void AnunciarVencedor()
    {
        PlayerData vencedor = jogadores.FirstOrDefault(p => !p.isEliminated);
        if (vencedor != null)
        {
            textoJogadorDaVez.text = "Jogador " + vencedor.playerID + " VENCEU!";
            elementosDoJogo.SetActive(false);

            if (AudioManager.Instance != null && musicaDoVencedor != null)
            {
                AudioManager.Instance.PlayMusic(musicaDoVencedor, false);
            }
        }
    }

    #region L�GICA DE TURNOS E RODADAS
    void CriarIconesUI()
    {
        foreach (var jogador in jogadores)
        {
            GameObject iconeObj = Instantiate(prefabIconeJogador, containerIconesJogadores.transform);
            Image iconeImg = iconeObj.GetComponentInChildren<Image>();
            if (iconeImg != null && jogador.playerIcon != null)
            {
                iconeImg.sprite = jogador.playerIcon;
            }
            uiIcones.Add(iconeObj);
        }
    }

    void IniciarRodada()
    {
        foreach (GameObject p in TodasAsPetalas)
        {
            p.SetActive(true);
        }
        petalaRuimIndex = Random.Range(0, TodasAsPetalas.Count);
        Debug.Log("P�tala Ruim da rodada: " + petalaRuimIndex);

        // --- L�GICA DE SELE��O DO PRIMEIRO JOGADOR DA RODADA ---

        // Procura o pr�ximo jogador para come�ar a rodada, a partir do �ltimo que come�ou
        int proximoIndexInicial = (ultimoJogadorQueIniciouRodada + 1) % jogadores.Count;
        int jogadorInicial = proximoIndexInicial;

        // Loop para encontrar o pr�ximo jogador v�lido (n�o eliminado)
        while (jogadores[jogadorInicial].isEliminated)
        {
            jogadorInicial = (jogadorInicial + 1) % jogadores.Count;

            // Se demos uma volta completa e n�o achamos ningu�m, algo est� errado (mas isso evita um loop infinito)
            if (jogadorInicial == proximoIndexInicial) break;
        }

        // Define o jogador encontrado como o jogador atual e guarda a informa��o
        jogadorAtualIndex = jogadorInicial;
        ultimoJogadorQueIniciouRodada = jogadorAtualIndex;

        IniciarTurno();
    }

    void IniciarTurno()
    {
        textoJogadorDaVez.text = "Vez do Jogador " + jogadores[jogadorAtualIndex].playerID;

        // --- L�GICA DE DESTAQUE ATUALIZADA E MAIS SEGURA ---
        for (int i = 0; i < jogadores.Count; i++) // Usamos jogadores.Count para garantir
        {
            // Garante que temos um �cone para este jogador
            if (i < uiIcones.Count && uiIcones[i] != null)
            {
                // Pega a imagem da borda (o componente Image no objeto pai do prefab)
                Image bordaImg = uiIcones[i].GetComponent<Image>();

                if (bordaImg != null)
                {
                    if (jogadores[i].isEliminated)
                    {
                        bordaImg.color = Color.grey;
                        uiIcones[i].transform.localScale = Vector3.one;
                    }
                    else if (i == jogadorAtualIndex)
                    {
                        bordaImg.color = jogadores[i].playerColor;
                        uiIcones[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    }
                    else
                    {
                        bordaImg.color = Color.white;
                        uiIcones[i].transform.localScale = Vector3.one;
                    }
                }
            }
        }

        MoverParaPrimeiraPetalaDisponivel();
        podeControlarSeta = true;
    }

    void ProximoJogador()
    {
        int proximoJogador = (jogadorAtualIndex + 1) % jogadores.Count;
        while (jogadores[proximoJogador].isEliminated)
        {
            proximoJogador = (proximoJogador + 1) % jogadores.Count;
        }
        jogadorAtualIndex = proximoJogador;
        IniciarTurno();
    }

    IEnumerator ProcessoDeEliminacao()
    {
        jogadores[jogadorAtualIndex].isEliminated = true;
        Image iconeImg = uiIcones[jogadorAtualIndex].GetComponentInChildren<Image>();
        if (iconeImg != null)
        {
            iconeImg.color = Color.grey;
        }
        textoJogadorDaVez.text = "Jogador " + jogadores[jogadorAtualIndex].playerID + " foi eliminado!";
        yield return new WaitForSeconds(DuracaoTremor);
        AbelhaNormal.SetActive(true);
        AbelhaComRaiva.SetActive(false);
        if (SetaIndicadora != null) SetaIndicadora.gameObject.SetActive(true);
        if (jogadores.Count(p => !p.isEliminated) > 1)
        {
            IniciarRodada();
        }
        else
        {
            AnunciarVencedor();
        }
    }
    #endregion

    #region L�GICA DE JOGABILIDADE
    void AtualizarSelecaoVisual()
    {
        foreach (GameObject petalaObj in TodasAsPetalas)
        {
            if (petalaObj.activeSelf)
            {
                petalaObj.GetComponent<Petala>().Deselecionar();
            }
        }

        GameObject petalaSelecionada = TodasAsPetalas[indicePetalaAtual];

        // --- ESTA � A MUDAN�A PRINCIPAL QUE CORRIGE O ERRO ---
        PlayerData jogadorDaVez = jogadores[jogadorAtualIndex];
        petalaSelecionada.GetComponent<Petala>().Selecionar(jogadorDaVez.playerColor);

        if (SetaIndicadora != null && PontosDaSet.Count > indicePetalaAtual)
        {
            Transform pontoAlvo = PontosDaSet[indicePetalaAtual];
            SetaIndicadora.transform.position = pontoAlvo.position;
            Vector3 direcaoParaOlhar = (MioloAbelhaNormal.transform.position - SetaIndicadora.transform.position).normalized;
            float angulo = Mathf.Atan2(direcaoParaOlhar.y, direcaoParaOlhar.x) * Mathf.Rad2Deg;
            SetaIndicadora.transform.rotation = Quaternion.Euler(0, 0, angulo + 180);
        }
    }

    void MoverParaPrimeiraPetalaDisponivel()
    {
        for (int i = 0; i < TodasAsPetalas.Count; i++)
        {
            if (TodasAsPetalas[i].activeSelf)
            {
                indicePetalaAtual = i;
                AtualizarSelecaoVisual();
                return;
            }
        }
    }

    private IEnumerator ShakeObject(Transform objectToShake, float duration, float magnitude)
    {
        if (objectToShake == null) yield break;
        Vector3 originalPosition = objectToShake.position;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            objectToShake.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        objectToShake.position = originalPosition;
    }
    #endregion
}