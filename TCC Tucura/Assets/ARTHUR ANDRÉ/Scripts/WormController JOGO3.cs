using UnityEngine;
using System.Collections.Generic;

public class WormController : MonoBehaviour
{
    [Header("Identidade")]
    public int playerID;
    public int score = 0;

    [Header("Configurações")]
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;
    public KeyCode actionKey;
    public GameObject bodyPrefab;
    public int initialBodySize = 5;
    public float bodySpacing = 0.5f;

    private int turnDirection = 1;
    private Rigidbody2D rb;
    private bool isDead = false;
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionsHistory = new List<Vector3>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GrowWorm(initialBodySize);
    }

    void Update()
    {
        if (isDead) return;
        if (Input.GetKeyDown(actionKey))
        {
            turnDirection *= -1;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        transform.Rotate(0, 0, -turnDirection * turnSpeed * Time.fixedDeltaTime);
        rb.linearVelocity = transform.up * moveSpeed;

        positionsHistory.Insert(0, transform.position);

        int index = 0;
        foreach (var part in bodyParts)
        {
            if (part != null)
            {
                int targetIndex = Mathf.Min(Mathf.FloorToInt((index + 1) * bodySpacing / Time.fixedDeltaTime), positionsHistory.Count - 1);
                if (targetIndex >= 0)
                {
                    part.transform.position = positionsHistory[targetIndex];
                }
            }
            index++;
        }

        if (positionsHistory.Count > (bodyParts.Count + 1) * 20)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        WormGameManager.Instance.ReportCollision(this, other.gameObject);
    }

    public void GrowWorm(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = (bodyParts.Count > 0) ? bodyParts[bodyParts.Count - 1].transform.position : transform.position;
            GameObject newPart = Instantiate(bodyPrefab, spawnPosition, transform.rotation);
            newPart.GetComponent<BodySegmentController>().ownerID = this.playerID;
            bodyParts.Add(newPart);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        foreach (var part in bodyParts)
        {
            if (part != null) Destroy(part);
        }
        Destroy(gameObject);
    }
}