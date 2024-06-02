using System;
using System.Collections.Generic;
using UnityEngine;

// Classe Model que gera o estado do jogo
public class Model : MonoBehaviour
{
    // Eventos para notificar mudanças no estado do jogo
    public event Action GameLoaded;
    public event Action<Vector2> CharacterPositionChanged;
    public event Action EffectsApplied;
    public event Action<int> GameEnded;

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
    private IScoreManager scoreManager;
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
        scoreManager = FindObjectOfType<ScoreManager>();
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
        int gameResult = VerifyEndingCondition();

        if (gameResult > 0)
        {
            EndGame(gameResult);
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
    public int VerifyEndingCondition()
    {
        if (playerHealth == 0)
            return 1;

        if (arrivedEndGoal)
            return 2;
        else
            return 0;
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
        scoreManager.AddScore(10); // Adiciona pontos por kill
    }

    // Método para "matar" o jogador
    private void killPlayer()
    {
        playerHealth = 0;
        EffectsApplied();
        audioManager.PlaySound("death");
        gameStateManager.SetState("GameOver");
    }

    // Método chamado quando ocorre uma colisão 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
        PlayerAnimator.SetBool("is_jumping", false);

        if (collision.gameObject.CompareTag("Instakill"))
        {
            killPlayer();
        }
    }

    // Método para finalizar o jogo
    public void EndGame(int gameResult)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GameEnded?.Invoke(gameResult);
        audioManager.PlaySound("game_end");
        gameStateManager.SetState("GameOver");

        // Calcular pontuação com base no tempo, vida restante e kills
        endTime = Time.time; // Registra o tempo de término do jogo
        float timeTaken = endTime - startTime;
        int finalScore = CalculateScore(timeTaken, playerHealth, killCount);
        scoreManager.AddScore(finalScore); // Adiciona a pontuação final
    }

    // Método para calcular a pontuação com base no tempo, vida restante e kills
    private int CalculateScore(float timeTaken, int playerHealth, int killCount)
    {
        int baseScore = 1000; // Pontuação base
        int timePenalty = Mathf.FloorToInt(timeTaken) * 10; // Penalidade por tempo
        int healthBonus = playerHealth * 5; // Bônus por vida restante
        int killBonus = killCount * 10; // Bônus por kills
        return baseScore - timePenalty + healthBonus + killBonus;
    }
}
