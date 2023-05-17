using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEntity : MonoBehaviour
{
  public enum MoveDirection
  {
    Up,
    Down,
    Right,
    Left
  };

  [SerializeField] MoveDirection direction;
  [SerializeField] Vector2 movementOffset;
  [SerializeField] float movespeed;

  private Vector2 startPosition;
  private Vector2 endPosition;

  private void Start()
  {
    SetPositions();
  }

  private void SetPositions()
  {
    startPosition = transform.position;
    endPosition = transform.position;
    
    if (direction == MoveDirection.Right)
    {
      startPosition.x -= movementOffset.x;
      endPosition.x += movementOffset.y;
    }
    else if (direction == MoveDirection.Left)
    {
      startPosition.x += movementOffset.x;
      endPosition.x -= movementOffset.y;
    }
    else if (direction == MoveDirection.Up)
    {
      startPosition.y -= movementOffset.x;
      endPosition.y += movementOffset.y;
    }
    else if (direction == MoveDirection.Down)
    {
      startPosition.y += movementOffset.x;
      endPosition.y -= movementOffset.y;
    }

  }

  private void Update()
  {
    transform.Translate(GetDirection(direction) * movespeed * Time.deltaTime);

    
    if (Vector2.Distance(transform.position, endPosition) < 0.3f)
    {
      transform.position = startPosition;
    }
  }

  private Vector2 GetDirection(MoveDirection dir)
  {
    return dir switch
    {
      MoveDirection.Up => Vector2.up,
      MoveDirection.Down => Vector2.down,
      MoveDirection.Right => Vector2.right,
      MoveDirection.Left => Vector2.left,
      _ => throw new System.NotImplementedException(),
    };
  }

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    Gizmos.DrawLine(startPosition, endPosition);
  }

  [NaughtyAttributes.Button("Recalcuate")]
  private void RecalculatePositions()
  {
    SetPositions();
  }
#endif
}
