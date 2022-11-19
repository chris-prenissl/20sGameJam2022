using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Bullet : Area2D
    {
        [Export] public float MaxRange = 1200;
        [Export] public float Speed = 750;
        
        private float _travelledDistance;

        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnBulletHit));
        }

        public override void _PhysicsProcess(float delta)
        {
            var distance = Speed * delta;
            var motion = Transform.x * Speed * delta;

            Position += motion;
            _travelledDistance += distance;
            if (_travelledDistance > MaxRange)
            {
                QueueFree();
            }
        }

        public void OnBulletHit(Area2D area)
        {
            if (area.CollisionLayer != 1)
            {
                QueueFree();
            }
        }
    }
}
