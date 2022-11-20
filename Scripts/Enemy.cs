using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Enemy : KinematicBody2D
    {
        [Export] public float ShootInterval;
        [Export] public float LookingIndicatorDistance = 30;
        [Export] public PackedScene Bullet;

        private Player _player;
        private Sprite _lookingDirectionIndicator;

        private float _currentTimeToShoot;
        
        public override void _Ready()
        {
            _player = (Player) GetTree().CurrentScene.FindNode("Player");
            _lookingDirectionIndicator = GetNode<Sprite>("LookingDirectionSprite");
            
            _currentTimeToShoot = ShootInterval;
        }

        public override void _Process(float delta)
        {
            if (_currentTimeToShoot <= 0)
            {
                ShootBullet();
                _currentTimeToShoot = ShootInterval;
            }

            _lookingDirectionIndicator.Position = Position.DirectionTo(_player.GlobalPosition) * LookingIndicatorDistance;

            _currentTimeToShoot = Mathf.Max(_currentTimeToShoot - delta, 0);
        }

        private void ShootBullet()
        {
            var bullet = (Bullet) Bullet.Instance();
            GetTree().Root.AddChild(bullet);
            bullet.Position = Position;
            bullet.Init(CollisionLayer, 200, 1200);
            if (_player != null)
            {
                bullet.LookAt(_player.GlobalPosition);
            }
        }
    }
}
