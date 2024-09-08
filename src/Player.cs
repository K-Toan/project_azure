using Godot;

public partial class Player : CharacterBody2D
{
    // move
    [Export]
    public float MoveSpeed { get; set; } = 200f;

    [Export]
    private bool _isMoving = false;
    private float Acceleration = 18f;

    [Export]
    private Vector2 _moveVelocity;

    // dash
    [Export]
    public float DashSpeed { get; set; } = 720f;

    [Export]
    public float DashDuration { get; set; } = 0.2f;

    [Export]
    public float DashCooldown { get; set; } = 2f;

    [Export]
    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashTime = 0f;
    private Vector2 _dashVelocity;

    // other variables
    [Export]
    private Vector2 _inputDirection;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        GetInput();
        HandleDash(delta);
        HandleMovement(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_isDashing)
        {
            Velocity = _dashVelocity * DashSpeed;
        }
        else
        {
            Velocity = _moveVelocity;
        }

        MoveAndSlide();
    }

    private void GetInput()
    {
        // move
        _inputDirection = Input.GetVector("left", "right", "up", "down").Normalized();
        if(_inputDirection != Vector2.Zero)
        {
            _isMoving = true;
        }

        // perform dash
        if (Input.IsActionJustPressed("dash") && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            _dashTime = DashDuration;
            _dashVelocity = _inputDirection;
        }
    }

    private void HandleMovement(double delta)
    {
        // lerp move velocity
        _moveVelocity = _moveVelocity.Lerp(_inputDirection * MoveSpeed, Acceleration * (float)delta);
    }

    private void HandleDash(double delta)
    {
        // dash cooldown
        if (_dashTime > 0)
        {
            _dashTime -= (float)delta;
        }
        // reset dash
        else
        {
            _isDashing = false;
            _canDash = true;
        }
    }
}
