using Godot;
using System;

public partial class Player : CharacterBody2D, IDamageable
{
    // move
    [Export]
    public float MoveSpeed { get; set; } = 180f;

    [Export]
    public float Acceleration { get; set; } = 18f;

    private float _currentSpeed = 0f;
    private Vector2 _moveVelocity;
    private Vector2 _moveDirection;
    private Vector2 _lastMoveDirection;

    // dash
    [Export]
    public float DashSpeed { get; set; } = 720f;

    [Export]
    public float DashDuration { get; set; } = 0.2f;

    [Export]
    public float DashCooldown { get; set; } = 1f;

    private bool _canDash = true;
    private float _dashTimer = 0f;
    private float _dashCooldownTimer = 0f;
    private Vector2 _dashDirection;

    // attack
    private bool _canAttack = true;

    // animation
    private float x = 0;
    private float y = 0;
    private string animation = "idle";
    private string direction = "down";

    // other variables
    private PlayerStateHandler _playerStateHandler;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _playerStateHandler = new PlayerStateHandler();

        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _Process(double delta)
    {
        GetInput();
        //HandleAttack();
        HandleDash(delta);
        HandleMove(delta);
        HandleAnimation();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    private void GetInput()
    {
        // move direction input
        _moveDirection = Input.GetVector("left", "right", "up", "down").Normalized();
        if (_moveDirection != Vector2.Zero)
        {
            _lastMoveDirection = _moveDirection;
        }

        // dash
        if (Input.IsActionJustPressed("dash"))
        {
            Dash();
        }
        // attack
        else if (Input.IsActionJustPressed("mouse1") && _canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!_canAttack ||
            _playerStateHandler.GetCurrentState() == PlayerState.Attack)
            return;

        //_playerStateHandler.SetPlayerState(PlayerState.Attack);
    }

    private void Dash()
    {
        if (!_canDash ||
            _dashCooldownTimer > 0 ||
            _playerStateHandler.GetCurrentState() == PlayerState.Dash)
        {
            return;
        }

        _playerStateHandler.SetPlayerState(PlayerState.Dash);
        _canDash = false;
        _dashTimer = DashDuration;
        _dashCooldownTimer = DashCooldown;
        _dashDirection = _lastMoveDirection;
    }

    private void HandleMove(double delta)
    {
        if (_playerStateHandler.GetCurrentState() == PlayerState.Dash || _playerStateHandler.GetCurrentState() == PlayerState.Attack)
            return;

        // lerp move velocity
        _moveVelocity = _moveVelocity.Lerp(_moveDirection * MoveSpeed, Acceleration * (float)delta);
        _currentSpeed = _moveVelocity.Length();

        // set CharacterBody2D velocity
        Velocity = _moveVelocity;

        if (_currentSpeed <= 0.1f)
        {
            _playerStateHandler.SetPlayerState(PlayerState.Idle);
        }
        else if (_currentSpeed > 0.1f && _currentSpeed <= 0.8f)
        {
            _playerStateHandler.SetPlayerState(PlayerState.Walk);
        }
        else
        {
            _playerStateHandler.SetPlayerState(PlayerState.Run);
        }
    }

    private void HandleDash(double delta)
    {
        if (_playerStateHandler.GetCurrentState() == PlayerState.Dash)
        {
            // perform dash 
            if (_dashTimer > 0)
            {
                _dashTimer -= (float)delta;
                Velocity = _dashDirection * DashSpeed;
            }
            // reset dash
            else
            {
                Velocity = Vector2.Zero;
                _playerStateHandler.SetPlayerState(PlayerState.Idle);
            }
        }

        // cooldown dash
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= (float)delta;
        }
        else
        {
            _canDash = true;
        }
    }

    private void HandleAttack()
    {

    }

    private void HandleAnimation()
    {
        switch (_playerStateHandler.GetCurrentState())
        {
            case PlayerState.Idle:
                x = _lastMoveDirection.X;
                y = _lastMoveDirection.Y;
                animation = "idle";
                break;

            case PlayerState.Walk:
                x = _lastMoveDirection.X;
                y = _lastMoveDirection.Y;
                animation = "walk";
                break;

            case PlayerState.Run:
                x = _lastMoveDirection.X;
                y = _lastMoveDirection.Y;
                animation = "run";
                break;

            case PlayerState.Dash:
                x = _dashDirection.X;
                y = _dashDirection.Y;
                animation = "dash";
                break;

            case PlayerState.Attack:
                break;

            default:
                return;
        }

        if (x > 0)
        {
            direction = "side";
            _animatedSprite.FlipH = false;
        }
        else if (x < 0)
        {
            direction = "side";
            _animatedSprite.FlipH = true;
        }
        else
        {
            if (y < 0)
            {
                direction = "up";
            }
            else
            {
                direction = "down";
            }
        }

        _animatedSprite.Play(animation + "_" + direction);
    }

    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
