using UnityEngine;

// Classe ScoreManager que implementa a interface IScoreManager
public class ScoreManager : MonoBehaviour, IScoreManager
{
    // Variável para armazenar a pontuação
    private int score;

    // Adiciona pontos à pontuação
    public void AddScore(int points)
    {
        score += points;
    }

    // Retorna a pontuação atual
    public int GetScore()
    {
        return score;
    }

    // Redefine a pontuação para zero
    public void ResetScore()
    {
        score = 0;
    }
}
