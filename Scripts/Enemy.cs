using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Enemy : KinematicBody2D
    {
        [Export] public float ShootInterval;
        [Export] public PackedScene Bullet;

        private Player _player;

        private float _currentTimeToSpawn;
        
        public override void _Ready()
        {
            _player = (Player) GetTree().CurrentScene.FindNode("Player");
            _currentTimeToSpawn = ShootInterval;
        }

        public override void _Process(float delta)
        {
            if (_currentTimeToSpawn <= 0)
            {
                ShootBullet();
                _currentTimeToSpawn = ShootInterval;
            }

            _currentTimeToSpawn = Mathf.Max(_currentTimeToSpawn - delta, 0);
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
