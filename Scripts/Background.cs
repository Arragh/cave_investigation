using Godot;

public partial class Background : ParallaxBackground
{
	[Export]
	public ParallaxLayer ParallaxLayer1 { get; set; }

	[Export]
	public ParallaxLayer ParallaxLayer2 { get; set; }

	[Export]
	public ParallaxLayer ParallaxLayer3 { get; set; }

	public override void _Ready()
	{
		ParallaxLayer1.MotionScale = new Vector2(0.5f, 0.5f);
		ParallaxLayer1.MotionMirroring = new Vector2(1152, 0);

		ParallaxLayer2.MotionScale = new Vector2(0.3f, 0.3f);
		ParallaxLayer2.MotionMirroring = new Vector2(1152, 0);

		ParallaxLayer3.MotionScale = new Vector2(0.1f, 0.1f);
		ParallaxLayer3.MotionMirroring = new Vector2(1152, 0);
	}

	public override void _Process(double delta)
	{
		
	}
}
