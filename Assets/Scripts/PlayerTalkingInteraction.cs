using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTalkingInteraction : MonoBehaviour
{
  [SerializeField] GameObject thinkingBubble;
  [SerializeField] TextMeshProUGUI thinkingBubbleText;
  float nextAutoDisable;

  private void Update()
  {
    if (nextAutoDisable < Time.time)
    {
      SetBubbleState(false);
    }
    else
    {
      SetBubbleState(true);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var detectable = collision.GetComponent<Detectable>();
    if (detectable is TalkingDetectable talkable)
    {
      if (talkable.IsVisible && !string.IsNullOrEmpty(talkable.TextToBubble))
      {
        thinkingBubbleText.text = talkable.TextToBubble;
        if (talkable.TextDuration > 0)
        {
          nextAutoDisable = Time.time + talkable.TextDuration;
        }
        else
        {
          nextAutoDisable = float.PositiveInfinity;
        }
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    var detectable = collision.GetComponent<Detectable>();
    if (detectable is TalkingDetectable talkable)
    {
      if (talkable.AutoDisableOnLeft)
      {
        nextAutoDisable = Time.time;
      }
      if (talkable.HideOnInteracted)
      {
        talkable.IsVisible = false;
      }
    }
    
  }

  private void SetBubbleState(bool state)
  {
    thinkingBubble.SetActive(state);
  }
}
