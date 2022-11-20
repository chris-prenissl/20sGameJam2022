using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Enemy : KinematicBody2D
    {
        [Export] public int Health;
        [Export] public float ShootInterval;
        [Export] public float LookingIndicatorDistance = 30;
        [Export] public int BulletSpeed = 200;
        [Export] public int BulletMaxRange = 1200;
        [Export] public PackedScene Bullet;

        private Player _player;
        private Area2D _hitDetectionArea;
        private Sprite _lookingDirectionIndicator;

        private float _currentTimeToShoot;
        
        public override void _Ready()
        {
            _player = (Player) GetTree().CurrentScene.FindNode("Player");
            _hitDetectionArea = GetNode<Area2D>("HitDetectionArea");
            _lookingDirectionIndicator = GetNode<Sprite>("LookingDirectionSprite");
            
            _currentTimeToShoot = ShootInterval;

            _hitDetectionArea.Connect("area_entered", this, nameof(Hit));
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
            bullet.Init(CollisionLayer, BulletSpeed, BulletMaxRange);
            if (_player != null)
            {
                bullet.LookAt(_player.GlobalPosition);
            }
        }

        private void Hit(Area2D area)
        {
            var bullet = (Bullet) area;

            if (bullet.OwnerLayer == CollisionLayer) return;
            
            Health--;
            bullet.QueueFree();
            
            if (Health <= 0)
            {
                QueueFree();
            }
        }
    }
}
