public enum Result
{
    Playing,
    Won,
    Died
}


// Interface para gerar o resultado final do jogo
public interface IGameResult
{
    int finalHealth { get; set; }
    int score { get; set; }
    int time { get; set; }
    Result result { get; set; }


    // Método para adicionar pontos à pontuação atual
    void AddScore(int points);

    int CalculateFinalScore();

    // Método para redefinir a pontuação para zero
    void ResetScore();
}
