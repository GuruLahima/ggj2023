using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollow : MonoBehaviour
{
  // this script loosely follows the player in the x and y axis
  // but it will not follow the player in the z axis

  public Transform target;
  public float smoothSpeed = 0.125f;
  public Vector3 offset;

  void LateUpdate()
  {
    Vector3 desiredPosition = target.position + offset;
    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    transform.position = smoothedPosition;
  }


}
