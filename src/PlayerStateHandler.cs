using Godot;

public partial class PlayerStateHandler : Node
{
    private PlayerState _currentState;

    public PlayerStateHandler()
    {
        _currentState = PlayerState.Idle;
    }

    public void SetPlayerState(PlayerState newState)
    {
        if (_currentState != newState)
        {
            GD.Print("Player state is set to " + newState.ToString());
            _currentState = newState;
        }
    }

    public PlayerState GetCurrentState()
    {
        return _currentState;
    }
}
