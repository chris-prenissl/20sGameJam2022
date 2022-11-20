using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Player : KinematicBody2D
    {
        [Export] public int Speed = 200;
        [Export] public int DashSpeed = 400;
        [Export] public float DashTime = 0.5f;
        [Export] public float DashCoolDownTime = 1;
        [Export] public int BulletSpeed = 800;
        [Export] public int BulletMaxRange = 1200;
        [Export] public PackedScene Bullet;
        [Export] public float ShootingIndicatorDistance;

        private Sprite _shootingIndicator;

        private Vector2 _velocity;
        private float _currentDashWaitingTime;
        private float _currentDashTime;

        public override void _Ready()
        {
            _shootingIndicator = GetNode<Sprite>("ShootingIndicatorSprite");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_currentDashTime <= 0)
            {
                _velocity.y = Input.GetAxis("move_up", "move_down");
                _velocity.x = Input.GetAxis("move_left", "move_right");
                _velocity = _velocity.Normalized() * Speed;
            }
            
            _shootingIndicator.Position = Position.DirectionTo(GetGlobalMousePosition()) * ShootingIndicatorDistance;
            
            _velocity = MoveAndSlide(_velocity);

            if (_currentDashTime > 0)
            {
                var destinationValue = _velocity.Normalized() * Speed;
                _velocity = _velocity.LinearInterpolate(destinationValue, delta);
                _currentDashTime = Mathf.Max(_currentDashTime - delta, 0);
            }

            _currentDashWaitingTime = Mathf.Max(_currentDashWaitingTime - delta, 0);
        }

        public override void _Input(InputEvent @event)
        {
            if (_currentDashTime <= 0 && @event.IsActionPressed("shoot"))
            {
                ShootBullet();
            }

            if (_currentDashWaitingTime <= 0 && @event.IsActionPressed("dash"))
            {
                Dash();
            }
        }

        private void ShootBullet()
        {
            var bullet = (Bullet) Bullet.Instance();
            GetTree().Root.AddChild(bullet);
            bullet.Position = Position;
            bullet.Init(CollisionLayer, BulletSpeed, BulletMaxRange);
            bullet.LookAt(_shootingIndicator.GlobalPosition);
        }

        private void Dash()
        {
            _velocity = _velocity.Normalized() * DashSpeed;
            _currentDashTime = DashTime;
            _currentDashWaitingTime = DashTime + DashCoolDownTime;
        }
    }
}
