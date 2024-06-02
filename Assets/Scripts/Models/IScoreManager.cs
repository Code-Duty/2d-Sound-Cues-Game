// Interface para gerar a pontuação do jogo
public interface IScoreManager
{
    // Método para adicionar pontos à pontuação atual
    void AddScore(int points);

    // Método para obter a pontuação atual
    int GetScore();

    // Método para redefinir a pontuação para zero
    void ResetScore();
}
