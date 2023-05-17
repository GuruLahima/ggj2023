using UnityEngine;

public class GameProgress : MonoBehaviour
{
  public static GameProgress Instance;

  private void Awake()
  {
    if (!GameProgress.Instance)
    {
      DontDestroyOnLoad(this.gameObject);
      Instance = this;
    }
    else
    {
      gameObject.SetActive(false);
    }
  }

  // im gonna be honest, this is the worst way I have ever "saved" data, but it will do for this game

  public bool BirdLevelPassed = false;
  public bool MotherFirstLevelPassed = false;
  public bool MotherSecondLevelPassed = false;
  public bool GrandpaLevelPassed = false;
}
