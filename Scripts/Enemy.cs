using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Enemy : KinematicBody2D
    {
        private enum EnemyState
        {
            Idle,
            Moving
        }
        
        [Export] public int Health;
        [Export] public float ShootInterval;
        [Export] public float LookingIndicatorDistance = 30;
        [Export] public float MinDistanceToPlayer = 30;
        [Export] public int Speed = 100;
        [Export] public int BulletSpeed = 200;
        [Export] public int BulletMaxRange = 1200;
        [Export] public PackedScene Bullet;

        [Export] public NodePath BodySpritePath;
        [Export] public NodePath HitDetectionAreaPath;
        [Export] public NodePath LookingDirectionSpritePath;
        [Export] public NodePath HealthBarPath;

        private Player _player;
        private AnimatedSprite _bodySprite;
        private Area2D _hitDetectionArea;
        private Sprite _lookingDirectionIndicator;
        private HealthBar _healthBar;

        private EnemyState _state;
        private Vector2 _velocity;
        private float _currentTimeToShoot;
        
        public override void _Ready()
        {
            _state = EnemyState.Idle;
            _player = (Player) GetTree().CurrentScene.FindNode(nameof(Player));

            _bodySprite = GetNode<AnimatedSprite>(BodySpritePath);
            _hitDetectionArea = GetNode<Area2D>(HitDetectionAreaPath);
            _lookingDirectionIndicator = GetNode<Sprite>(LookingDirectionSpritePath);
            _healthBar = GetNode<HealthBar>(HealthBarPath);
            
            _healthBar.SetMaxHealth(Health);
            _currentTimeToShoot = ShootInterval;

            _hitDetectionArea.Connect("area_entered", this, nameof(GotHit));
            SetState(EnemyState.Idle);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_currentTimeToShoot <= 0)
            {
                ShootBullet();
                _currentTimeToShoot = ShootInterval;
            }

            var directionToPlayer = Position.DirectionTo(_player.GlobalPosition);

            _lookingDirectionIndicator.Position = directionToPlayer * LookingIndicatorDistance;
            
            if (Position.DistanceTo(_player.GlobalPosition) > MinDistanceToPlayer)
            {
                SetState(EnemyState.Moving);
                _velocity = directionToPlayer * Speed;
                _velocity = MoveAndSlide(_velocity);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
            
            _currentTimeToShoot = Mathf.Max(_currentTimeToShoot - delta, 0);
        }

        private void SetState(EnemyState state)
        {
            if (_state != state)
            {
                _state = state;
                _bodySprite.Animation = state.ToString().ToLower();
            }
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

        private void GotHit(Area2D area)
        {
            var bullet = (Bullet) area;

            if (bullet.OwnerLayer == CollisionLayer) return;
            
            _healthBar.SetHealth(--Health);
            bullet.QueueFree();
            
            if (Health <= 0)
            {
                QueueFree();
            }
        }
    }
}
