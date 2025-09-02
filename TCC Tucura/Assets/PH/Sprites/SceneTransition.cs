using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class SceneTransitionExplodeDOTween : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject spritePrefab;

    [Header("Quantidade e Tempo")]
    public int spriteCount = 30;
    public float transitionDuration = 1.2f;

    [Header("Animação")]
    public Vector3 scaleTarget = new Vector3(1.5f, 1.5f, 1f);
    public float scaleDuration = 0.35f;
    public float preExplosionDelay = 0.15f;

    [Header("Explosão")]
    public float explosionDistance = 8f;
    public float explosionDuration = 0.6f;
    public float rotationRange = 180f;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        DOTween.Init();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(PlayTransition(sceneName));
    }

    IEnumerator PlayTransition(string sceneName)
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.aspect * halfHeight;

        for (int i = 0; i < spriteCount; i++)
        {
            float x = Random.Range(-halfWidth, halfWidth);
            float y = Random.Range(-halfHeight, halfHeight);
            Vector3 spawnPos = new Vector3(x, y, 0);

            GameObject s = Instantiate(spritePrefab, spawnPos, Quaternion.identity);
            s.transform.localScale = Vector3.zero;

            // Crescer suavemente
            s.transform.DOScale(scaleTarget, scaleDuration)
                .SetEase(Ease.OutBack);

            // Explosão após um pequeno delay
            Vector2 dir = Random.insideUnitCircle.normalized;
            Vector3 targetPos = spawnPos + (Vector3)(dir * explosionDistance);
            float rotation = Random.Range(-rotationRange, rotationRange);

            s.transform.DOMove(targetPos, explosionDuration)
                .SetDelay(preExplosionDelay)
                .SetEase(Ease.OutCubic);

            s.transform.DORotate(Vector3.forward * rotation, explosionDuration, RotateMode.LocalAxisAdd)
                .SetDelay(preExplosionDelay)
                .SetEase(Ease.OutCubic);

            Destroy(s, transitionDuration + 0.2f);
        }

        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene(sceneName);
    }
}
