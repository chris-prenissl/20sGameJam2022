using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Bullet : Area2D
    {
        private int _maxRange = 1200;
        private int _speed = 750;

        public uint OwnerLayer;
        
        private float _travelledDistance;

        public override void _Ready()
        {
            Connect("body_entered", this, nameof(HitWall));
        }

        public override void _PhysicsProcess(float delta)
        {
            var distance = _speed * delta;
            var motion = Transform.x * _speed * delta;

            Position += motion;
            _travelledDistance += distance;
            if (_travelledDistance > _maxRange)
            {
                QueueFree();
            }
        }

        public void Init(uint ownerLayer, int speed, int maxRange)
        {
            OwnerLayer = ownerLayer;
            _speed = speed;
            _maxRange = maxRange;
        }

        public void HitWall(Node body)
        {
            QueueFree();
        }
    }
}
