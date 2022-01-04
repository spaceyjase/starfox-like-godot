using Godot;

public class FollowCamera : Camera
{
  [Export] private float minDistance = 2.0f;
  [Export] private float maxDistance = 4.0f;
  [Export] private float angleV = 0.0f;
  [Export] private float height = 1.5f;
  
  public override void _Ready()
  {
    base._Ready();

    SetAsToplevel(true);
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);

    var target = ((Spatial)GetParent()).GlobalTransform.origin;
    var pos = GlobalTransform.origin;

    var fromTarget = pos - target;

    if (fromTarget.Length() < minDistance)
    {
	    fromTarget = fromTarget.Normalized() * minDistance;
    }
    else if (fromTarget.Length() > maxDistance)
    {
	    fromTarget = fromTarget.Normalized() * maxDistance;
    }

    fromTarget.y = height;

    pos = target + fromTarget;

    LookAtFromPosition(pos, target, Vector3.Up);
	
		// Turn a little up or down
	  var t = Transform;
	  t.basis = new Basis(t.basis[0], Mathf.Deg2Rad(angleV)) * t.basis;
		Transform = t;
  }
}
