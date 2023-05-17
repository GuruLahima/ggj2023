using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.U2D;

public class JumpAbility : Ability
{
  #region public fields
  [Header(" params")]
  public float jumpTime = 2.0f;
  public CinemachineVirtualCamera mainCamera;
  [Header("References")]
  public Collider2D playerCollider;
  public SpriteRenderer playerSprite;
  public SpriteRenderer playerShadow;
  public Rigidbody2D rb;
  #endregion


  #region exposed fields

  [Header("Jumping vfx params")]
  [SerializeField] float cameraSizeAtMaxJumpHeight;
  [SerializeField] Ease cameraScaleEaseUp;
  [SerializeField] Ease cameraScaleEaseDown;
  [SerializeField] Vector3 spriteSizeAtMaxJumpHeight;
  [SerializeField] Ease spriteScaleEaseUp;
  [SerializeField] Ease spriteScaleEaseDown;
  [SerializeField] Vector3 shadowSizeAtMaxJumpHeight;
  [SerializeField] Ease shadowScaleEaseUp;
  [SerializeField] Ease shadowScaleEaseDown;

  [SerializeField] Vector3 shadowOffsetAtMaxJumpHeight;
  #endregion


  #region private fields
  // public Cinemachine.CinemachinePixelPerfect cinemachinePixelPerfectComponent;
  public PixelPerfectCamera pixelPerfectComponent;

  private IEnumerator jumpCoroutine = null;
  #endregion


  #region MonoBehaviour callbacks

  private void Update()
  {
    if (MainCharacterController.gameIsPaused) { return; }
    if (IsActive)
      if (Input.GetKeyDown(KeyCode.Space) && jumpCoroutine == null)
      {
        jumpCoroutine = Jump(jumpTime);
        StartCoroutine(jumpCoroutine);
      }
  }
  #endregion


  #region public methods

  #endregion


  #region private methods
  private IEnumerator Jump(float time)
  {
    Debug.Log("Jump start");
    float cameraOriginalSize = mainCamera.m_Lens.OrthographicSize;
    // animate viewport size to simulate jumping
    Tween jumpTween = DOTween.To(() => mainCamera.m_Lens.OrthographicSize, x => mainCamera.m_Lens.OrthographicSize = x, cameraOriginalSize * cameraSizeAtMaxJumpHeight, time / 2).SetEase(cameraScaleEaseUp).OnComplete(() =>
    {
      // restore viewport size
      DOTween.To(() => mainCamera.m_Lens.OrthographicSize, x => mainCamera.m_Lens.OrthographicSize = x, cameraOriginalSize, time / 2).SetEase(cameraScaleEaseDown);
    });

    // float camOrigRefResX = pixelPerfectComponent.refResolutionX;
    // float camOrigRefResY = pixelPerfectComponent.refResolutionY;
    // Tween cameraTweenX = DOTween.To(() => (int)pixelPerfectComponent.refResolutionX, x => pixelPerfectComponent.refResolutionX = (int)x, camOrigRefResX * cameraSizeAtMaxJumpHeight, time / 2).SetEase(cameraScaleEaseUp).OnComplete(() =>
    // {
    //   // restore viewport size
    //   DOTween.To(() => (int)pixelPerfectComponent.refResolutionX, x => pixelPerfectComponent.refResolutionX = (int)x, camOrigRefResX, time / 2).SetEase(cameraScaleEaseDown);
    // });


    // Tween cameraTweenY = DOTween.To(() => (int)pixelPerfectComponent.refResolutionY, x => pixelPerfectComponent.refResolutionY = (int)x, camOrigRefResY * cameraSizeAtMaxJumpHeight, time / 2).SetEase(cameraScaleEaseUp).OnComplete(() =>
    // {
    //   // restore viewport size
    //   DOTween.To(() => (int)pixelPerfectComponent.refResolutionY, x => pixelPerfectComponent.refResolutionY = (int)x, camOrigRefResY, time / 2).SetEase(cameraScaleEaseDown);
    // });




    // animate player sprite size to simulate jumping
    Vector3 originalSpriteSize = playerSprite.transform.localScale;
    Tween spriteTweenDown = null;
    Tween spriteTweenUp = playerSprite.transform.DOScale(spriteSizeAtMaxJumpHeight, time / 2).SetEase(spriteScaleEaseUp).OnComplete(() =>
    {
      // restore player sprite size
      spriteTweenDown = playerSprite.transform.DOScale(originalSpriteSize, time / 2).SetEase(spriteScaleEaseDown);
    });
    Vector3 originalShadowSize = playerShadow.transform.localScale;
    Tween shadowTweenDown = null;
    Tween shadowTweenUp = playerShadow.transform.DOScale(shadowSizeAtMaxJumpHeight, time / 2).SetEase(shadowScaleEaseUp).OnComplete(() =>
    {
      // restore player sprite size
      shadowTweenDown = playerShadow.transform.DOScale(originalShadowSize, time / 2).SetEase(shadowScaleEaseDown);
    });

    Vector3 originalShadowOffset = playerShadow.transform.localPosition;
    Tween shadowOffsetTweenDown = null;
    Tween shadowOffsetTweenUp = playerShadow.transform.DOLocalMove(shadowOffsetAtMaxJumpHeight, time / 2).SetEase(shadowScaleEaseUp).OnComplete(() =>
    {
      // restore player sprite size
      shadowOffsetTweenDown = playerShadow.transform.DOLocalMove(originalShadowOffset, time / 2).SetEase(shadowScaleEaseDown);
    });

    // disable player collision during jump
    playerCollider.enabled = false;


    yield return spriteTweenUp.WaitForCompletion();
    yield return spriteTweenDown.WaitForCompletion();

    // restore player collision
    playerCollider.enabled = true;

    StopCoroutine(jumpCoroutine);
    jumpCoroutine = null;
    Debug.Log("Jump end");

  }

  #endregion


  #region networking code

  #endregion

}
