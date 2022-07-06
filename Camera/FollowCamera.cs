using Godot;

namespace Starfoxlike.Camera;

public class FollowCamera : Godot.Camera
{
  [Export] public float followSpeed = 3f;
  [Export] public NodePath targetPath = null;
  [Export] public Vector3 offset = Vector3.Zero;

  private Spatial target;

  public override void _Ready()
  {
    base._Ready();

    if (targetPath != null)
    {
      target = GetNode<Spatial>(targetPath);
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    if (target == null) return;

    base._PhysicsProcess(delta);

    var targetPosition = target.GlobalTransform.Translated(offset);
    GlobalTransform = GlobalTransform.InterpolateWith(targetPosition, followSpeed * delta);
    LookAt(target.GlobalTransform.origin, Vector3.Up);
  }
}