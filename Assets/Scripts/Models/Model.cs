using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Classe Model que gera o estado do jogo
public class Model : MonoBehaviour
{
    public event System.Action GameLoaded;
    public event System.Action<Vector2> CharacterPositionChanged;
    public event System.Action EffectsApplied;
    public event System.Action<IGameResult> GameEnded;

    // Atributos do jogador
    private int playerHealth = 100;
    private float startTime;
    private float endTime;
    private int killCount = 0; // Contador de kills

    // Referências a componentes
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public int jumpPower;
    public float stopSpeed = 2f;
    private bool isJumping = false;
    private float horizontalInput;
    private bool arrivedEndGoal = false;
    private Dictionary<Vector2Int, bool> mapCollisions;

    public PlayerAttack PlayerAttack;

    private IAudioManager audioManager;
    private IGameResult gameResultManager;
    private IGameStateManager gameStateManager;

    public Animator PlayerAnimator;

    void Awake()
    {
        mapCollisions = new Dictionary<Vector2Int, bool>(); // Popula com colisões do mapa
        rb = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
        rb.gravityScale = 4;
        jumpPower = 24;

        PlayerAttack = GetComponent<PlayerAttack>();
        audioManager = FindObjectOfType<AudioManager>();
        gameResultManager = FindObjectOfType<GameResult>();
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    // Método para iniciar o jogo
    public void StartGame()
    {
        LoadResources();
        SetupInitialState();
        GameLoaded?.Invoke();
        audioManager.PlaySound("game_start");
        startTime = Time.time; // Regista o tempo de início do jogo
    }

    // Método para carregar recursos
    void LoadResources()
    {
        Debug.Log("Load Resources");
        // Carrega recursos como texturas, modelos, sons, etc.
    }

    // Método para configurar o estado inicial do jogo
    void SetupInitialState()
    {
        Debug.Log("Setup Initial State");
        // Configuração inicial do estado do jogo
    }

    // Método chamado quando uma tecla de movimento é pressionada
    public void UserClickedMovementKey(float horizontalInput)
    {
        this.horizontalInput = horizontalInput;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        CharacterPositionChanged?.Invoke(rb.position);
        PlayerAnimator.SetBool("is_moving", true);
        audioManager.PlaySound("footstep");




    }

    // Método chamado quando a tecla de pulo é pressionada
    public void UserClickedJumpKey()
    {
        if (!isJumping)
        {
            PlayerAnimator.SetBool("is_jumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
            CharacterPositionChanged?.Invoke(rb.position);
            audioManager.PlaySound("jump");
        }
    }

    // Método Update é chamado a cada frame
    void Update()
    {
        getGameCondition();

        if (gameResultManager.result == Result.Won || gameResultManager.result == Result.Died)
        {
            EndGame();
        }

        if (Mathf.Abs(horizontalInput) > 0.01) // Atualiza somente se a entrada for significativa
        {
            // Decaimento exponencial para parada suave
            horizontalInput *= Mathf.Pow(0.5f, Time.deltaTime * stopSpeed);
        }
        else
        {
            horizontalInput = 0; // Zera a entrada para evitar deriva
            PlayerAnimator.SetBool("is_moving", false);
        }
        CharacterPositionChanged?.Invoke(rb.position);
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        CharacterPositionChanged?.Invoke(rb.position);

        AnimatorStateInfo stateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        // verificar estado de animação, se animação acabou, mudar o estado
        if (stateInfo.IsName("Main_still_shooting") && stateInfo.normalizedTime >= 1.0f)
        {
            PlayerAnimator.SetBool("is_shooting", false);
        }


        if (horizontalInput != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(horizontalInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }


    }

    // Método para verificar a condição de fim do jogo
    public void getGameCondition()
    {
        if (playerHealth == 0) {
            gameResultManager.result = Result.Died;
            return;
        }
            
        if (arrivedEndGoal)
        {
            gameResultManager.result = Result.Won;
            return;
        } else
        {
            gameResultManager.result = Result.Won;
            return;
        }
    }

    // Método chamado quando a tecla de ataque é pressionada
    public void UserClickedAttackKey()
    {
        PlayerAttack.ShootProjectile();
        PlayerAnimator.SetBool("is_shooting", true);
        audioManager.PlaySound("attack");
    }

    // Método chamado quando o jogador mata um inimigo
    public void EnemyKilled()
    {
        killCount++; // Incrementa o contador de kills
        gameResultManager.AddScore(10); // Adiciona pontos por kill
    }

    // Método para "matar" o jogador
    private void killPlayer()
    {
        playerHealth = 0;
        EffectsApplied();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
        audioManager.PlaySound("death");
        gameStateManager.SetState("GameOver");

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

    // Método para finalizar o jogo
    public void EndGame()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        // Calcular pontuação com base no tempo, vida restante e kills

        endTime = Time.time; // Registra o tempo de término do jogo
        float timeTaken = endTime - startTime;
        gameResultManager.finalHealth = playerHealth;
        gameResultManager.time = (int)(endTime - startTime);
        gameResultManager.score = killCount * 5;
        gameResultManager.CalculateFinalScore();// Adiciona a pontuação final

        GameEnded?.Invoke(gameResultManager);
        audioManager.PlaySound("game_end");
        gameStateManager.SetState("GameOver");
    }
}
