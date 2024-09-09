using Godot;

public partial class PlayerAnimationHandler : Node
{
    private AnimatedSprite2D _animatedSprite;

    public PlayerAnimationHandler(AnimatedSprite2D animatedSprite)
    {
        _animatedSprite = animatedSprite;
    }

    public void SetPlayerAnimation(string animationName)
    {
        GD.Print("Set player animation to " + animationName);
        _animatedSprite.Play(animationName);
    }
}
