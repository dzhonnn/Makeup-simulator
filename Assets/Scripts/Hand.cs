using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    private const float Duration = 1f;
    private const float MaxDistanceToFace = 1f;
    [SerializeField] private Vector3 defaultPosition = new Vector3(0, -10f, 0);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private SpriteRenderer grabbedTool;

    private Vector3 targetPosition;
    private bool isInputActive = false;

    private Action touchEnded;
    private Vector3 makeupReadyPosition = Vector3.zero;
    private MakeupData makeupData;

    void OnEnable()
    {
        touchEnded += CheckPosition;
    }

    void OnDisable()
    {
        touchEnded -= CheckPosition;
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (GetTouchPosition(out Vector3 touchPosition))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10f));
        }

        if (isInputActive)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private bool GetTouchPosition(out Vector3 touchPosition)
    {
        touchPosition = defaultPosition;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEnded?.Invoke();
                // touchPosition = Input.GetTouch(0).position;
                return false;
            }
        }

        return false;
    }

    private void SetTool(MakeupData _makeupData)
    {
        makeupData = _makeupData;

        if (makeupData.makeupType == MakeupType.Lipstick)
        {
            makeupData.makeupTool.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        makeupData.makeupTool.DeactivateTool();
        grabbedTool.sprite = makeupData.makeupTool.GetComponent<Image>().sprite;
    }

    private void ReleaseTool()
    {
        if (makeupData.makeupType == MakeupType.Cream)
        {
            makeupData.makeupTool.gameObject.SetActive(true);
        }
        makeupData.makeupTool.ActivateTool();
        grabbedTool.sprite = null;

        makeupData = null;

        targetPosition = defaultPosition;
    }

    private void ApplyToolTipColor(GameObject paletteButton)
    {
        if (!grabbedTool) return;

        if (makeupData.makeupType == MakeupType.Lipstick) return;

        Texture2D paletteButtonTex = paletteButton.GetComponent<Image>().sprite.texture;

        int x = paletteButtonTex.width / 2;
        int y = paletteButtonTex.height / 2;

        grabbedTool.material.DOColor(paletteButtonTex.GetPixel(x, y), "_Color", Duration / 2f);
    }

    private void ResetToolTipColor()
    {
        if (!grabbedTool) return;

        if (makeupData.makeupType == MakeupType.Lipstick) return;

        grabbedTool.material.DOColor(Color.white, "_Color", Duration / 2f);
    }

    private void CheckPosition()
    {
        Vector3 facePosition = Game.Instance.Character.Face.position;
        float distance = Vector2.Distance(transform.position, facePosition);

        if (distance < MaxDistanceToFace && makeupData != null)
        {
            MakeupAnimation();
        }
        else if (makeupData != null)
        {
            targetPosition = makeupReadyPosition;
        }
        else
        {
            targetPosition = defaultPosition;
        }
    }

    private void ApplyMakeup()
    {
        switch (makeupData.makeupType)
        {
            case MakeupType.Blush:
                Game.Instance.MakeupApplier.ApplyBlush(makeupData.makeupSprite);
                break;
            case MakeupType.Lipstick:
                Game.Instance.MakeupApplier.ApplyLipstick(makeupData.makeupSprite);
                break;
            case MakeupType.Eyeshadows:
                Game.Instance.MakeupApplier.ApplyEyeshadows(makeupData.makeupSprite);
                break;
        }
    }

    private void MakeupAnimation()
    {
        DisableInput();

        Vector3 facePosition = Game.Instance.Character.Face.position;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(facePosition - makeupData.toolOffset, Duration))
            .SetEase(Ease.OutSine);

        float offset = 0.5f;

        Vector3 faceLeftPosition = facePosition + Vector3.left * offset;
        faceLeftPosition -= makeupData.toolOffset;
        faceLeftPosition += makeupData.faceOffset;

        Vector3 faceRightPosition = facePosition + Vector3.right * offset;
        faceRightPosition -= makeupData.toolOffset;
        faceRightPosition += makeupData.faceOffset;

        sequence.Append(transform.DOMove(faceLeftPosition, Duration / 2f))
            .SetEase(Ease.OutSine);

        sequence.Append(transform.DOMove(faceRightPosition, Duration / 2f))
            .SetEase(Ease.OutSine);

        sequence.JoinCallback(ResetToolTipColor);

        if (makeupData.makeupType == MakeupType.Cream)
        {
            sequence.AppendCallback(() => Game.Instance.MakeupApplier.RemoveAcne());
        }
        else
        {
            sequence.AppendCallback(() => ApplyMakeup());
        }

        ReleaseToolAnimation(sequence);
    }

    private void ReleaseToolAnimation(Sequence sequence)
    {
        sequence.AppendCallback(ResetToolTipColor);

        sequence.Append(transform.DOMove(makeupData.makeupTool.transform.position, Duration))
                .SetEase(Ease.OutSine);

        sequence.Join(grabbedTool.transform.DORotate(Vector3.zero, Duration / 2f))
            .SetEase(Ease.OutSine);

        sequence.AppendCallback(() => ReleaseTool());

        sequence.onComplete += EnableInput;
    }

    public void GrabToolAnimation(Transform paletteButton, MakeupData _makeupData)
    {
        DisableInput();

        Sequence sequence = DOTween.Sequence();

        if (makeupData != null)
        {
            ReleaseToolAnimation(sequence);

            return;
        }

        // Move hand to tool
        sequence.Append(transform.DOMove(_makeupData.makeupTool.transform.position, Duration)
            .SetEase(Ease.OutSine));

        // Grabbing tool
        sequence.AppendCallback(() => SetTool(_makeupData));

        sequence.Join(grabbedTool.transform.DORotate(_makeupData.toolRotation, Duration / 2f))
            .SetEase(Ease.OutSine);

        if (_makeupData.makeupType != MakeupType.Lipstick)
        {
            PaletteAnimation(paletteButton, _makeupData, sequence);
        }
        else
        {
            LipstickAnimation(sequence);
        }
    }

    private void LipstickAnimation(Sequence sequence)
    {
        sequence.Append(transform.DOMove(Vector3.zero, Duration));

        makeupReadyPosition = Vector3.zero;

        sequence.onComplete += () => targetPosition = makeupReadyPosition;
        sequence.onComplete += EnableInput;
    }

    private void PaletteAnimation(Transform paletteButton, MakeupData _makeupData, Sequence sequence)
    {
        // Move hand to color palette
        sequence.Append(transform.DOMove(paletteButton.position - _makeupData.toolOffset, Duration / 2f)
            .SetEase(Ease.InSine));

        float offset = 0.2f;

        Vector3 paletteLeftPosition = paletteButton.position + Vector3.left * offset;
        paletteLeftPosition -= _makeupData.toolOffset;

        Vector3 paletteRightPosition = paletteButton.position + Vector3.right * offset;
        paletteRightPosition -= _makeupData.toolOffset;

        // Move left right
        sequence.Append(transform.DOMove(paletteLeftPosition, .25f)
                .SetEase(Ease.InOutSine));

        sequence.Append(transform.DOMove(paletteRightPosition, .25f)
                .SetEase(Ease.InOutSine));

        sequence.JoinCallback(() => ApplyToolTipColor(paletteButton.gameObject));

        makeupReadyPosition = Vector3.zero;

        sequence.onComplete += () => targetPosition = makeupReadyPosition;
        sequence.onComplete += EnableInput;
    }

    public void CreamAnimation(Transform cream)
    {
        DisableInput();

        Sequence sequence = DOTween.Sequence();

        if (makeupData != null)
        {
            ReleaseToolAnimation(sequence);

            return;
        }

        sequence.Append(transform.DOMove(cream.position, Duration))
            .SetEase(Ease.OutSine);

        Image creamImage = cream.gameObject.GetComponent<Image>();

        sequence.AppendCallback(() =>
        {
            grabbedTool.sprite = creamImage.sprite;
            creamImage.gameObject.SetActive(false);
            makeupData = new MakeupData(MakeupType.Cream, cream.gameObject.GetComponent<MakeupTool>());
        });

        Vector3 facePosition = Game.Instance.Character.Face.transform.position;
        Vector3 creamPosition = cream.position;

        Vector3 creamApplyPosition = new Vector3(-1.62f, 2.49f, 0);

        sequence.Join(transform.DOMove(creamApplyPosition, Duration))
            .SetEase(Ease.OutSine);

        makeupReadyPosition = creamApplyPosition;

        sequence.onComplete += () => targetPosition = makeupReadyPosition;
        sequence.onComplete += EnableInput;
    }

    public void DisableInput()
    {
        isInputActive = false;
    }

    public void EnableInput()
    {
        isInputActive = true;
    }
}
