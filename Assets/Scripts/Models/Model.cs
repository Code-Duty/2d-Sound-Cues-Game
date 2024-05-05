using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Model : MonoBehaviour
{
    public event System.Action GameLoaded;
    public event System.Action<Vector2> CharacterPositionChanged;
    public event System.Action EffectsApplied;
    public event System.Action<int> GameEnded;

    private int playerHealth = 100;

    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public int jumpPower;
    public float stopSpeed = 2f; // The factor of deceleration
    private bool isJumping = false;
    private float horizontalInput;
    private bool arrivedEndGoal = false;
    private Dictionary<Vector2Int, bool> mapCollisions; // Simplified for example purposes

    void Awake()
    {
        mapCollisions = new Dictionary<Vector2Int, bool>(); // Populate with map collisions
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 4;
        jumpPower = 24;
    }

    public void StartGame()
    {
        LoadResources();
        SetupInitialState();
        GameLoaded?.Invoke();
    }

    void LoadResources()
    {
        UnityEngine.Debug.Log("Load Resources");
        // Load resources such as textures, models, sounds, etc.
    }

    void SetupInitialState()
    {
        UnityEngine.Debug.Log("Setup Initial State");
        // Initial setup of the game state
    }

    public void UserClickedMovementKey(float horizontalInput)
    {
        this.horizontalInput = horizontalInput;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        CharacterPositionChanged?.Invoke(rb.position);
    }

    public void UserClickedJumpKey()
    {
        if (!isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
            CharacterPositionChanged?.Invoke(rb.position);
        }
    }

    void Update()
    {
        int gameResult = VerifyEndingCondition();

        if (gameResult > 0)
        {
            EndGame(gameResult);
        }

        if (Mathf.Abs(horizontalInput) > 0.01) // Only update if input is significant
        {
            // Exponential decay for smooth stopping
            horizontalInput *= Mathf.Pow(0.5f, Time.deltaTime * stopSpeed);
        }
        else
        {
            horizontalInput = 0; // Zero out the input to prevent drift
        }
        CharacterPositionChanged?.Invoke(rb.position);
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        CharacterPositionChanged?.Invoke(rb.position);
    }

    public int VerifyEndingCondition()
    {
        if(playerHealth == 0)
            return 1;

        if (arrivedEndGoal)
            return 2;
        else
            return 0;
    }

    public void UserClickedAttackKey()
    {
        // Attack logic here
    }

    private void killPlayer()
    {
        playerHealth = 0;
        EffectsApplied();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;

        if (collision.gameObject.CompareTag("Instakill"))
        {
            killPlayer();
        }

        
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Finish"))
        {
            arrivedEndGoal = true;
        }
    }

    public void EndGame(int gameResult)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GameEnded?.Invoke(gameResult);
    }
}
