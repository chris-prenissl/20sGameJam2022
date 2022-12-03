using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public partial class Player : CharacterBody2D
    {
        [Export] public int Speed = 200;
        [Export] public int DashSpeed = 400;
        [Export] public float DashTime = 0.5f;
        [Export] public float DashCoolDownTime = 1;
        [Export] public int BulletSpeed = 800;
        [Export] public int BulletMaxRange = 1200;
        [Export] public PackedScene Bullet;
        [Export] public float ShootingIndicatorDistance;

        private Sprite2D _shootingIndicator;

        private float _currentDashWaitingTime;
        private float _currentDashTime;
        private bool _collided;

        public override void _Ready()
        {
            _shootingIndicator = GetNode<Sprite2D>("ShootingIndicatorSprite");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_currentDashTime <= 0)
            {
                Velocity = new Vector2(
                    Input.GetAxis("move_left", "move_right"),
                    Input.GetAxis("move_up", "move_down"));
                Velocity = Velocity.Normalized() * Speed;
            }
            
            _shootingIndicator.Position = Position.DirectionTo(GetGlobalMousePosition()) * ShootingIndicatorDistance;
            
            _collided = MoveAndSlide();

            if (_currentDashTime > 0)
            {
                var destinationValue = Velocity.Normalized() * Speed;
                Velocity = Velocity.Lerp(destinationValue, (float) delta);
                _currentDashTime = Mathf.Max(_currentDashTime - (float) delta, 0);
            }

            _currentDashWaitingTime = Mathf.Max(_currentDashWaitingTime - (float) delta, 0);
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
            var bullet = (Bullet) Bullet.Instantiate();
            GetTree().Root.AddChild(bullet);
            bullet.Position = Position;
            bullet.Init(CollisionLayer, BulletSpeed, BulletMaxRange);
            bullet.LookAt(_shootingIndicator.GlobalPosition);
        }

        private void Dash()
        {
            Velocity = Velocity.Normalized() * DashSpeed;
            _currentDashTime = DashTime;
            _currentDashWaitingTime = DashTime + DashCoolDownTime;
        }
    }
}
