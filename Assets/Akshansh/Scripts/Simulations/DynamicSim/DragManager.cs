using AkshanshKanojia.Controllers.ObjectManager;
using AkshanshKanojia.Inputs.Mobile;
using UnityEngine;
using UnityEngine.Events;

public class DragManager : MobileInputs
{
    public UnityEvent OnCorrectPlaced;
    public UnityEvent OnIncorrectPlace;

    [SerializeField] bool canInteract = true, useTapOffset = true, resetOnWrong = true;
    public Transform targetLocation;
    [SerializeField] float validDist = 2f, zDepth = 10f,resetMoveSpeed =2f;
    [SerializeField] ObjectController objCont;
    [SerializeField] LayerMask raycastLayer;

    Vector3 tempOffset, tempStartPos;

    bool isValid = false;

    public override void Start()
    {
        base.Start();
        objCont.OnMovementEnd += (obj) => { if (obj == gameObject) { EnableInteract(); } };
        OnIncorrectPlace.AddListener(() =>
        {
            if (resetOnWrong)
            {
                DisableInteract();
                objCont.AddEvent(gameObject, tempStartPos, resetMoveSpeed, 
                    false);
            }
        });
    }

    private void UpdatePos(MobileInputManager.TouchData _data)
    {
        var _temp = _data.TouchPosition;
        _temp.z = zDepth;
        _temp = Camera.main.ScreenToWorldPoint(_temp);
        _temp += tempOffset;
        transform.position = _temp;
    }

    public void DisableInteract()
    {
        canInteract = false;
    }
    public void EnableInteract()
    {
        canInteract = true;
    }

    public override void OnTapEnd(MobileInputManager.TouchData _data)
    {
        if (isValid)
        {
            isValid = false;
            if (Vector3.Distance(transform.position, targetLocation.position) < validDist)
            {
                OnCorrectPlaced?.Invoke();
            }
            else
            {
                OnIncorrectPlace?.Invoke();
            }
        }
    }

    public override void OnTapMove(MobileInputManager.TouchData _data)
    {
        if (isValid)
        {
            UpdatePos(_data);
        }
    }

    public override void OnTapped(MobileInputManager.TouchData _data)
    {
        if (!canInteract)
            return;
        tempStartPos = transform.position;
        RaycastHit2D _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(
            _data.TouchPosition), Mathf.Infinity, raycastLayer);
        if (_hit)
        {
            if (_hit.collider.gameObject == gameObject)
            {
                isValid = true;
                if (useTapOffset)
                {
                    var _temp = _data.TouchPosition;
                    _temp.z = zDepth;
                    _temp = Camera.main.ScreenToWorldPoint(_temp);
                    tempOffset = transform.position - _temp;
                }
                UpdatePos(_data);
            }
        }
    }

    public override void OnTapStay(MobileInputManager.TouchData _data)
    {
    }
}