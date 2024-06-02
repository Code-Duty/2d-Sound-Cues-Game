using UnityEngine;

public class GameResult : MonoBehaviour, IGameResult
{
    // Variáveis para armazenar os valores
    private int _score;
    private int _finalHealth;
    private int _time;
    private Result _result;

    // Implementação das propriedades da interface
    int IGameResult.finalHealth { get => _finalHealth; set => _finalHealth = value; }
    int IGameResult.score { get => _score; set => _score = value; }
    int IGameResult.time { get => _time; set => _time = value; }
    Result IGameResult.result { get => _result; set => _result = value; }

    // Adiciona pontos à pontuação
    public void AddScore(int points)
    {
        _score += points;
    }

    // Calcula a pontuação final
    public int CalculateFinalScore()
    {
        int baseScore = 1000; // Pontuação base
        int timePenalty = Mathf.FloorToInt(_time) * 10; // Penalidade por tempo
        int healthBonus = _finalHealth * 5; // Bônus por vida restante
        int killBonus = _score * 10; // Bônus por kills
        return baseScore - timePenalty + healthBonus + killBonus;
    }

    // Redefine a pontuação para zero
    public void ResetScore()
    {
        _score = 0;
    }
}