using Godot;

namespace Starfoxlike.Player;

public class Player : PathFollow
{
  [Export] private float forwardSpeed = 5f;
  [Export] private float movementSpeed = 18f;
  [Export] private float xBounds = 5f;
  [Export] private float yBounds = 2f;

  private Spatial player;

  public override void _Ready()
  {
    base._Ready();
    
    player = GetNode<Spatial>("Arwing");
  }

  private void SetSpeed(float speed, float delta)
  {
    Offset += speed * delta;
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    var h = Input.GetAxis("left", "right");
    var v = Input.GetAxis("up", "down");

    LocalMove(player, h, v, movementSpeed * delta);
    Roll(player, v, h, 80f, .1f);   // TODO: [Export] values

    SetSpeed(forwardSpeed, delta);
  }

  private static void Roll(Spatial target, float v, float h, float leanLimit, float lerpTime)
  {
    target.Rotation = new Vector3(
      Mathf.LerpAngle(target.Rotation.x, v, lerpTime),
      Mathf.LerpAngle(target.Rotation.y, -h, lerpTime),
      Mathf.LerpAngle(target.Rotation.z, h, lerpTime));
  }

  private void LocalMove(Spatial target, float x, float y, float speed)
  {
    target.Translation += new Vector3(-x, -y, 0).Normalized() * speed;
    ClampPosition(target);
  }

  private void ClampPosition(Spatial target)
  {
    var translation = target.Translation;
    translation.x = Mathf.Clamp(translation.x, -xBounds, xBounds);
    translation.y = Mathf.Clamp(translation.y, -yBounds, yBounds);
    target.Translation = translation;
  }
}