using Godot;
using System;

public class Player : PathFollow
{
  [Export] private float forwardSpeed = 5f;
  [Export] private float movementSpeed = 18f;
  [Export] private float lookSpeed = 360f;
  [Export] private float xBounds = 5f;
  [Export] private float yBounds = 2f;

  private Spatial player;
  private Camera camera;

  public override void _Ready()
  {
    base._Ready();
    
    player = GetNode<Spatial>("Arwing");
    camera = GetNode<Camera>("Camera");
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

    LocalMove(h, v, movementSpeed, delta);
    Roll(player, v, h, 80f, .1f);

    SetSpeed(forwardSpeed, delta);
  }

  private void Roll(Spatial target, float v, float h, float leanLimit, float lerpTime)
  {
    target.Rotation = new Vector3(
      Mathf.LerpAngle(target.Rotation.x, v, lerpTime),
      Mathf.LerpAngle(target.Rotation.y, -h, lerpTime),
      Mathf.LerpAngle(target.Rotation.z, -h * leanLimit, lerpTime));
  }

  private void LocalMove(float x, float y, float speed, float delta)
  {
    player.Translation += new Vector3(-x, -y, 0) * speed * delta;
    ClampPosition();
  }

  private void ClampPosition()
  {
    var translation = player.Translation;
    translation.x = Mathf.Clamp(translation.x, -xBounds, xBounds);
    translation.y = Mathf.Clamp(translation.y, -yBounds, yBounds);
    player.Translation = translation;
  }
}
