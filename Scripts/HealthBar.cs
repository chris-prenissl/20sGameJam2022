using Godot;

namespace TwentySecondGameJam2022.Scripts
{
	public partial class HealthBar : Node2D
	{
		[Export] public NodePath HealthIndicatorPath;
		private Sprite2D _healthIndicator;

		private int _maxHealth = 10;

		public override void _Ready()
		{
			_healthIndicator = GetNode<Sprite2D>(HealthIndicatorPath);
			
			_healthIndicator.Scale = new Vector2(1, _healthIndicator.Scale.y);
			Visible = false;
		}

		public void SetMaxHealth(int maxHealth)
		{
			_maxHealth = maxHealth;
		}
		

		public void SetHealth(int health)
		{
			_healthIndicator.Scale = new Vector2( (float) health/_maxHealth , _healthIndicator.Scale.y);
			if (health < _maxHealth)
			{
				Visible = true;
			}
		}
	}
}
