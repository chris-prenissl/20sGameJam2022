using Godot;

namespace TwentySecondGameJam2022.Scripts
{
    public class Player : KinematicBody2D
    {
        [Export] public int Speed = 200;

        private Vector2 _velocity;

        public override void _PhysicsProcess(float delta)
        {
            _velocity.y = Input.GetAxis("move_up", "move_down");
            _velocity.x = Input.GetAxis("move_left", "move_right");
            _velocity = _velocity.Normalized() * Speed;
            _velocity = MoveAndSlide(_velocity);
        }
    }
}
