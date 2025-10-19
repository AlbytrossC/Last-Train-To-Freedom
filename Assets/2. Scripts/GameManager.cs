using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Menu, Playing, Paused}
    public GameState gameState = GameState.Menu;

    public ChangeGameState()
    {
        
    }
}
