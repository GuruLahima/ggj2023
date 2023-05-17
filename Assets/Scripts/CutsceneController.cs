using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class CutsceneController : MonoBehaviour
{
  public static CutsceneController Instance;

  private void Awake()
  {
    Instance = this;
  }

  [InputAxis]
  [SerializeField] string skipButton;
  [SerializeField] TextMeshProUGUI leftText;
  [SerializeField] TextMeshProUGUI rightText;
  [SerializeField] Image leftImage;
  [SerializeField] Image rightImage;
  [SerializeField] GameObject cutscenePanel;
  [SerializeField] float animationSpeed;
  [SerializeField] Color colorWhenSpeaking;
  [SerializeField] Color colorWhenNotSpeaking;
  [SerializeField] TextMeshProUGUI leftName;
  [SerializeField] TextMeshProUGUI rightName;
  [SerializeField] bool playOnStartOfScene;
  [SerializeField] List<CutsceneData> startOfSceneCutscenes = new List<CutsceneData>();
  Queue<CutsceneData> cutscenes = new Queue<CutsceneData>();

  Coroutine _lastSequence;
  Coroutine _lastAnimateText;
  bool skipped = false;

  private void Start()
  {
    cutscenes = new Queue<CutsceneData>(startOfSceneCutscenes);
    if (playOnStartOfScene)
    {
      PlayCutscene(cutscenes.Dequeue());
    }
  }

  public void PlayCutscene(CutsceneData cutscene)
  {
    if (cutscene)
    {
      skipped = false;
      if (_lastSequence != null)
      {
        StopCoroutine(_lastSequence);
      }
      StopAllCoroutines();
      _lastSequence = StartCoroutine(CutsceneSequence(cutscene));
    }
  }

  public void StopCutscenes()
  {
    skipped = false;
    if (_lastSequence != null)
    {
      StopCoroutine(_lastSequence);
    }
    StopAllCoroutines();
  }

  private void Update()
  {
    if (Input.GetButtonDown(skipButton))
    {
      skipped = true;
    }
  }

  IEnumerator CutsceneSequence(CutsceneData cutscene)
  {
    cutscene.OnCutsceneStarted?.Invoke();
    cutscenePanel.SetActive(true);
    for (int i = 0; i < cutscene.Scenarios.Count; i++)
    {
      leftName.text = cutscene.Scenarios[i].LeftName;
      rightName.text = cutscene.Scenarios[i].RightName;
      if (cutscene.Scenarios[i].LeftImage)
      {
        leftImage.sprite = cutscene.Scenarios[i].LeftImage;
        leftImage.gameObject.SetActive(true);
        leftImage.color = (cutscene.Scenarios[i].Allignment == CutsceneScenario.ScenarioTextAllignment.Left) ? colorWhenSpeaking : colorWhenNotSpeaking;
      }
      else
      {
        leftImage.gameObject.SetActive(false);
      }
      if (cutscene.Scenarios[i].RightImage)
      {
        rightImage.sprite = cutscene.Scenarios[i].RightImage;
        rightImage.gameObject.SetActive(true);
        rightImage.color = (cutscene.Scenarios[i].Allignment == CutsceneScenario.ScenarioTextAllignment.Right) ? colorWhenSpeaking : colorWhenNotSpeaking;
      }
      else
      {
        rightImage.gameObject.SetActive(false);
      }
      for (int j = 0; j < cutscene.Scenarios[i].ListOfTexts.Count; j++)
      {
        cutscene.Scenarios[i].OnScenarioStarted?.Raise();
        if (_lastAnimateText != null)
        {
          StopCoroutine(_lastAnimateText);
        }
        if (cutscene.Scenarios[i].Allignment == CutsceneScenario.ScenarioTextAllignment.Left)
        {
          _lastAnimateText = StartCoroutine(AnimateSentence(leftText, cutscene.Scenarios[i].ListOfTexts[j]));
          leftText.gameObject.SetActive(true);
          rightText.gameObject.SetActive(false);
        }
        else
        {
          _lastAnimateText = StartCoroutine(AnimateSentence(rightText, cutscene.Scenarios[i].ListOfTexts[j]));
          rightText.gameObject.SetActive(true);
          leftText.gameObject.SetActive(false);
        }
        if (cutscene.Scenarios[i].waitForInput)
        {
          while (skipped == false)
          {
            yield return null;
          }
        }
        else if (cutscene.Scenarios[i].waitForEvent)
        {
          while (cutscene.Scenarios[i].waitForEvent.WasReaisedThisFrame())
          {
            yield return null;
          }
        }
        else
        {
          yield return new WaitForSeconds(cutscene.Scenarios[i].talkingInterval);

        }
        skipped = false;
        cutscene.Scenarios[i].OnScenarioEnded?.Raise();
      }
    }
    cutscenePanel.SetActive(false);
    cutscene.OnCutsceneEnded?.Invoke();
    if (cutscenes.Count > 1)
      PlayCutscene(cutscenes.Dequeue());

  }

  IEnumerator AnimateSentence(TextMeshProUGUI field, string text)
  {
    field.text = "";
    for (int i = 0; i < text.Length; i++)
    {
      field.text += text[i];
      yield return new WaitForSeconds(animationSpeed);
    }
  }
}
