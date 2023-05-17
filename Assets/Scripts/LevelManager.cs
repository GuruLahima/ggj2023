using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  private List<Detectable> AllDetectables = new List<Detectable>();

  #region Singleton
  public static LevelManager Instance;
  private void Awake()
  {
    Instance = this;

    // populate this list
    AllDetectables = FindObjectsOfType<Detectable>(true).ToList();
  }
  #endregion

  public List<Detectable> GetDetectablesOfType(DetectableData detData, bool filterVisible = false)
  {
    return AllDetectables.FindAll((x) => x.DetectableData == detData && (!filterVisible || x.IsVisible));
  }

  public void RestartLevel()
  {
    string currentScene = SceneManager.GetActiveScene().name;
    LoadLevel(currentScene);
  }

  public void LoadLevel(string levelName)
  {
    SceneManager.LoadScene(levelName, LoadSceneMode.Single);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
