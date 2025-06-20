using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Video;
using System;

public class MenuEventSystemHandler : MonoBehaviour
{
    [Header("Reference")]
    public List<Selectable> selectables = new List<Selectable>();
    [SerializeField] protected Selectable _firstSelected;
    [SerializeField] protected ScenesToLoad gameStart;

    [Header("Controls")]
    [SerializeField] protected InputActionReference _navigateActionReference;
    [SerializeField] protected InputActionReference _cancelActionReference;

    [Header("Animations")]
    [SerializeField] protected float _selectedAnimationScale = 1.1f;
    [SerializeField] protected float _selectedAnimationDuration = 0.2f;
    [SerializeField] protected List<GameObject> _animationExlusions = new List<GameObject>();


    [Header("Sound")]
    public UnityEvent SoundEvent;
    [Header("Video")]
    [SerializeField] protected VideoPlayer videoPlayer;

    protected Dictionary<Selectable, Vector3> _originalScales = new Dictionary<Selectable, Vector3>();

    protected Selectable _lastSelected;

    protected Tween _scaleUpTween;
    protected Tween _scaleDownTween;
    public virtual void OnEnable()
    {
        _navigateActionReference.action.performed += OnNavigate;
       for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].transform.localScale = _originalScales[selectables[i]];
        }
       StartCoroutine(SelectAfterDelay());
        videoPlayer.loopPointReached += VideoEnded;
        _cancelActionReference.action.performed += SkipVideo;
       
    }
    protected virtual IEnumerator SelectAfterDelay()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
    }
    public virtual void OnDisable()
    {
        _navigateActionReference.action.performed -= OnNavigate;
        if (_scaleUpTween != null)
        {
            _scaleUpTween.Kill();
        }
        if (_scaleDownTween != null)
        {
            _scaleDownTween.Kill();
        }
        videoPlayer.loopPointReached -= VideoEnded;
        _cancelActionReference.action.performed -= SkipVideo;
    }

    private void SkipVideo(InputAction.CallbackContext context)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            SceneController.Instance.LoadSceneSet(gameStart);
        }
        
    }

    void VideoEnded(VideoPlayer vp)
    {
        SceneController.Instance.LoadSceneSet(gameStart);
    }
    public virtual void Awake()
    {

       foreach (var selectable in selectables)
        {
           
            AddSelectableListener(selectable);
            _originalScales.Add(selectable, selectable.transform.localScale);
        }
    }

    protected virtual void AddSelectableListener(Selectable selectable)
    {
        EventTrigger trigger =selectable.gameObject.GetComponent<EventTrigger>();
        if (trigger==null)
        {
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry SelectEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Select
        };
        trigger.triggers.Add(SelectEntry);
        SelectEntry.callback.AddListener(OnSelect);
        EventTrigger.Entry DeselectEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.Deselect
        };
        DeselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(DeselectEntry);

        EventTrigger.Entry PointerEnterEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        PointerEnterEntry.callback.AddListener(OnPointerEnter);
        trigger.triggers.Add(PointerEnterEntry);
        EventTrigger.Entry PointerExitEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit
        };
        PointerExitEntry.callback.AddListener(OnPointerExit);
        trigger.triggers.Add(PointerExitEntry);
    }
    public void OnSelect(BaseEventData eventData)
    {
        SoundEvent?.Invoke();
        _lastSelected = eventData.selectedObject.GetComponent<Selectable>();
        if (_animationExlusions.Contains(_lastSelected.gameObject))
        {
            return;
        }
        Vector3 newScale = eventData.selectedObject.transform.localScale * _selectedAnimationScale;
        _scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _selectedAnimationDuration).SetUpdate(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (_animationExlusions.Contains(eventData.selectedObject))
        {
            return;
        }
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        _scaleDownTween = eventData.selectedObject.transform.DOScale(_originalScales[sel], _selectedAnimationDuration).SetUpdate(true);
    }
    public void OnPointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            Selectable sel = pointerEventData.pointerEnter.GetComponentInParent<Selectable>();
            if (sel == null)
            {
                sel = pointerEventData.pointerEnter.GetComponentInChildren<Selectable>();
            }
            pointerEventData.selectedObject = sel.gameObject;
        }
    }
    public void OnPointerExit(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = null;
        }
    }
    protected virtual void OnNavigate(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null && _lastSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelected.gameObject);
        }
    }
}
