using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Player : KinematicBody2D
    {
        [Export] public int Speed = 200;
        [Export] public PackedScene Bullet;
        [Export] public float ShootingIndicatorDistance;

        private Sprite _shootingIndicator;

        private Vector2 _velocity;

        public override void _Ready()
        {
            _shootingIndicator = GetNode<Sprite>("ShootingIndicatorSprite");
        }

        public override void _PhysicsProcess(float delta)
        {
            _velocity.y = Input.GetAxis("move_up", "move_down");
            _velocity.x = Input.GetAxis("move_left", "move_right");
            _velocity = _velocity.Normalized() * Speed;

            _shootingIndicator.Position = Position.DirectionTo(GetGlobalMousePosition()) * ShootingIndicatorDistance;
            
            _velocity = MoveAndSlide(_velocity);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("shoot"))
            {
                ShootBullet();
            }
        }

        private void ShootBullet()
        {
            var bullet = (Bullet) Bullet.Instance();
            GetTree().Root.AddChild(bullet);
            bullet.Position = Position;
            bullet.LookAt(_shootingIndicator.GlobalPosition);
        }
    }
}
