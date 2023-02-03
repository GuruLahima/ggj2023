using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject : MonoBehaviour
{
  [SerializeField] float delay;
  [SerializeField] GameObject objectToActivate;

  // Start is called before the first frame update
  void Start()
  {
    Invoke("Activate", delay);
  }

  void Activate()
  {
    objectToActivate.SetActive(true);
  }
}
