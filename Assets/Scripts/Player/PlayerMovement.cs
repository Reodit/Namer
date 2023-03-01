using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    #region components
    public Rigidbody rb;
    public PlayerEntity playerEntity;
    private PlayerEffetct pe;
    #endregion

    [SerializeField] private GameObject interactObj;
    public GameObject addCardTarget;
    private VirtualJoystick virtualJoystick;
    [SerializeField] public float moveSpeed;
    [SerializeField] public int rotateSpeed;
    public Vector3 pInputVector;
    private Dir targetDir;
    private int objscale;
    private Rigidbody climbRb;
    private float duration = 0f;

    [SerializeField] [Range(0.1f, 5f)] private float rootmotionSpeed;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        #region Component Init
        rb = GetComponent<Rigidbody>();
        playerEntity = GetComponent<PlayerEntity>();
        GameObject vrJoystick = GameObject.Find("IngameCanvas").transform.Find("VirtualJoystick").gameObject;
        vrJoystick.SetActive(true);
        virtualJoystick = vrJoystick.GetComponent<VirtualJoystick>();
        pe = GetComponent<PlayerEffetct>();
        #endregion

        #region KeyAction Init
        GameManager.GetInstance.KeyAction += MoveKeyInput;
        GameManager.GetInstance.KeyAction += PlayInteraction;
        #if UNITY_ANDROID
        GameManager.GetInstance.KeyAction -= PlayInteraction;
        #endif
        #endregion
        
        #region Init Variable
        GameManager.GetInstance.localPlayerMovement = this;
        rootmotionSpeed = 1f;
        moveSpeed = 3f;
        rotateSpeed = 10;
        GameManager.GetInstance.isPlayerDoAction = false;
        GameManager.GetInstance.isPlayerCanInput = true;
        #endregion
    }
    
    public void MoveKeyInput()
    {
        pInputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }
    
    public void PlayInteraction()
    {
        if (Input.GetKeyDown(GameManager.GetInstance.interactionKey))
        {
            CheckInteraction();
        }
        #if UNITY_ANDROID
        CheckInteraction();
        #endif
    }

    private void Update()
    {
        if (GameManager.GetInstance.isPlayerDoAction)
        { 
            duration += Time.deltaTime;

            if (duration > 4)
            {
                GameManager.GetInstance.isPlayerDoAction = false;
            }
        }
        
        else
        {
            duration = 0f;
        }

        if (rb)
        {
            DetectManager.GetInstance.CheckCharacterCurrentTile(this.gameObject);
            DetectManager.GetInstance.CheckForwardObj(this.gameObject);
            interactObj = DetectManager.GetInstance.forwardObjectInfo;
        }

        else
        {
            Init();
        }
        
        // TODO 하드 코딩 제거
        if (UIManager.GetInstance.ingameCanvas)
        {
            if (GameManager.GetInstance.isPlayerCanInput &&
                !GameManager.GetInstance.isPlayerDoAction &&
                interactObj &&
                interactObj.CompareTag("InteractObj") &&
                interactObj.TryGetComponent<InteractiveObject>(out InteractiveObject io))
            {
                targetDir = DetectManager.GetInstance.objDir;
                if (io.Adjectives[(int)EAdjective.Win] != null ||
                    io.Adjectives[(int)EAdjective.Obtainable] != null ||
                    io.Adjectives[(int)EAdjective.Climbable] != null ||
                    io.Adjectives[(int)EAdjective.Movable] != null)
                {
                    UIManager.GetInstance.ingameCanvas.TryGetComponent<IngameCanvasController>(
                        out IngameCanvasController ic);
                    ic.InteractionBtnOn();
                }
            }

            else
            {
                if (UIManager.GetInstance.ingameCanvas.TryGetComponent<IngameCanvasController>(out IngameCanvasController ic))
                {
                    ic.InteractionBtnOff();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (pInputVector == Vector3.zero)
        {
            pInputVector = virtualJoystick.vInputVector;
        }
        
        //Player Move : Idle = Stop
        if (pInputVector == Vector3.zero)
        {
            if (playerEntity.currentStates == PlayerStates.Teeter)
            {
                return;
            }
        }
        
        if (transform.position.y < -3f)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            GameManager.GetInstance.ResetCurrentLvl();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            return;
        }

        if (GameManager.GetInstance.isPlayerCanInput && !GameManager.GetInstance.isPlayerDoAction)
        {
            if (Physics.Raycast(rb.position + pInputVector * (Time.deltaTime * moveSpeed) + Vector3.up * 0.2f, Vector3.down, 20f))
            {
                rb.MovePosition(rb.position + pInputVector * (Time.deltaTime * moveSpeed));
                playerEntity.pAnimator.SetFloat("scalar", pInputVector.magnitude);
                playerEntity.ChangeState(PlayerStates.Move);

                if (pInputVector != Vector3.zero)
                {
                    rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(pInputVector),
                        Time.deltaTime * rotateSpeed);
                }
            }
            
            else
            {
                // 절벽에서 떨어질거 같은 모션 추가
                playerEntity.ChangeState(PlayerStates.Teeter);
                if (pInputVector != Vector3.zero)
                {
                    rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(pInputVector),
                        Time.deltaTime * rotateSpeed);
                }
            }
        }
    }

    private void CheckInteraction()
    {
        if (GameManager.GetInstance.isPlayerCanInput && 
            !GameManager.GetInstance.isPlayerDoAction &&
            interactObj &&
            interactObj.CompareTag("InteractObj"))
        {
            // TODO 하드 코딩 제거
            // :: Queuing 중 Delay주는 부분에서 버그 발생하여 잠시 하드 코딩으로 임시 처리합니다.
            {
                targetDir = DetectManager.GetInstance.objDir;
                InteractionSequencer.GetInstance.playerActionTargetObject =
                    interactObj.GetComponent<InteractiveObject>();
                if (InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Win] != null)
                {
                    InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Win].Execute(
                        InteractionSequencer.GetInstance.playerActionTargetObject, this.gameObject);
                    return;
                }

                if (InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Obtainable] != null)
                {
                    InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Obtainable].Execute(
                        InteractionSequencer.GetInstance.playerActionTargetObject, this.gameObject);
                    return;
                }

                if (InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Movable] != null)
                {
                    InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Movable].Execute(
                        InteractionSequencer.GetInstance.playerActionTargetObject, this.gameObject);
                    return;
                }

                if (InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Climbable] != null)
                {
                    climbRb = InteractionSequencer.GetInstance.playerActionTargetObject.GetComponent<Rigidbody>();
                    var targetTransform = InteractionSequencer.GetInstance
                        .playerActionTargetObject.transform;
                    objscale = (int)targetTransform.localScale.y
                               - ((int)transform.position.y - (int)targetTransform.position.y);

                    InteractionSequencer.GetInstance.playerActionTargetObject.Adjectives[(int)EAdjective.Climbable].Execute(
                        InteractionSequencer.GetInstance.playerActionTargetObject, this.gameObject);
                    return;
                }
            }
        }
    }
    
    // 방향만 맞춰주면 되는 경우
    public IEnumerator SetRotationBeforeInteraction()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        float targetRotationY = 0;
        //float curPlayerRoationY = transform.eulerAngles.y;
        switch (targetDir)
        {
            case Dir.right :
                targetRotationY = 90;
                break;
            case Dir.down :
                targetRotationY = 180;
                break;
            case Dir.left :
                targetRotationY = 270;
                break;
            case Dir.up :
                targetRotationY = 0;
                break;
            default :
                Debug.LogError("잘못된 타겟 방향값입니다...!");
                break;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, targetRotationY, 0));
        
        // TODO Lerp Lotation 하기 (예정)
        // if (this.transform.rotation.y < 0)
        // {
        //     curPlayerRoationY += 360;
        // }
        //float destinationRotationY = targetRotationY - curPlayerRoationY;
        
        //float rotationTime = 0;

        // while (rotationTime < 1 )
        // {
        //     rotationTime += Time.deltaTime;
        //     if (rotationTime > 1f)
        //     {
        //         rotationTime = 1;
        //     }
        //     
        //     yield return new WaitForEndOfFrame();
        // }
        
        yield return new WaitForFixedUpdate();
    }
    
    
    // 포지션도 맞춰줘야 할 경우
    public IEnumerator SetPositionBeforeInteraction(Transform targetObjTransform)
    {
        // TODO 포지션까지 맞춰줘야 하는 경우 고민 (예정)
        yield return new WaitForFixedUpdate();
    }

    #region Animation Rootmotion
    public IEnumerator PushRootmotion()
    {
        yield return SetRotationBeforeInteraction();

        Vector3 targetPos = Vector3.zero;
        
        switch (targetDir)
        {
            case Dir.right :
                targetPos = Vector3.right;
                break;
            case Dir.down :
                targetPos = Vector3.back;
                break;
            case Dir.left :
                targetPos = Vector3.left;
                break;
            case Dir.up :
                targetPos = Vector3.forward;
                break;
            default :
                Debug.LogError("잘못된 타겟 방향값입니다...!");
                break;
        }
        var curPos = transform.position;
        Vector3 destinationPos = targetPos + curPos;
        float moveTime = 0;
        
        while (moveTime < 1)
        {
            moveTime += Time.deltaTime * rootmotionSpeed; 
            transform.position = Vector3.Lerp(curPos, destinationPos, moveTime + 0.1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        GameManager.GetInstance.isPlayerDoAction = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        playerEntity.pAnimator.SetFloat("scalar", 0);
    }

    public IEnumerator ClimbRootmotion()
    {
        yield return SetRotationBeforeInteraction();
        // var position = transform.position;
        // position = new Vector3((float)Math.Round(position.x, 1),
        //     (float)Math.Round(position.y, 1), (float)Math.Round(position.z, 1));
        // transform.position = position;
        //
        // // TODO 애니메이션 폴리싱 예정 (오브젝트 모양이 제각각이라 일단 일괄적으로 하드코딩 처리)
        // {
        //     Vector3 targetPos = Vector3.zero;
        //     var curPos = position;
        //     climbRb.constraints = RigidbodyConstraints.FreezeAll;
        //     rb.constraints = RigidbodyConstraints.FreezeAll;
        //     switch (targetDir)
        //     {
        //         case Dir.right:
        //             targetPos = Vector3.right;
        //             break;
        //         case Dir.down:
        //             targetPos = Vector3.back;
        //             break;
        //         case Dir.left:
        //             targetPos = Vector3.left;
        //             break;
        //         case Dir.up:
        //             targetPos = Vector3.forward;
        //             break;
        //         default:
        //             Debug.LogError("잘못된 타겟 방향값입니다...!");
        //             break;
        //     }
        //
        //     float moveTime = 0;
        //     Vector3 target1 = new Vector3(curPos.x, curPos.y + objscale * 0.5f, curPos.z);
        //     yield return new WaitForSeconds(1f);
        //     while (moveTime < 1)
        //     {
        //         moveTime += Time.deltaTime * rootmotionSpeed;
        //         if (transform.position.y > target1.y)
        //         {
        //             yield return null;
        //         }
        //         else
        //         {
        //             transform.position = Vector3.Lerp(curPos, target1, moveTime);
        //         }
        //
        //         yield return null;
        //     }
        //
        //     transform.position = target1;
        //     moveTime = 0;
        //     curPos = transform.position;
        //
        //     while (moveTime < 1)
        //     {
        //         moveTime += Time.deltaTime * rootmotionSpeed;
        //         transform.position = Vector3.Lerp(curPos, curPos + targetPos * 0.5f, moveTime);
        //         yield return null;
        //     }
        //
        //     curPos = transform.position;
        //     moveTime = 0;
        //     Vector3 target2 = new Vector3(curPos.x, curPos.y + objscale * 0.5f + 0.05f, curPos.z);
        //
        //     while (moveTime < 1f)
        //     {
        //         moveTime += Time.deltaTime * rootmotionSpeed;
        //         if (transform.position.y > target2.y)
        //         {
        //             yield return null;
        //         }
        //         else
        //         {
        //             transform.position = Vector3.Lerp(curPos, target2, moveTime);
        //         }
        //
        //         yield return null;
        //     }
        //     yield return new WaitForSeconds(0.2f);
        // }
        yield return StartCoroutine(pe.Fadeout());
        transform.position = new Vector3(interactObj.transform.position.x, transform.position.y, interactObj.transform.position.z) + Vector3.up * objscale;
        transform.localScale = Vector3.one;
        
        GameManager.GetInstance.isPlayerDoAction = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        climbRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        playerEntity.pAnimator.SetFloat("scalar", 0);
    }

    public IEnumerator AddcardRootmotion()
    {
        float angle = Mathf.Atan2(transform.position.x - addCardTarget.transform.position.x, transform.position.z - addCardTarget.transform.position.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle + 180,0f);
        
        playerEntity.pAnimator.SetFloat("scalar", 0);
        yield return new WaitForSeconds(0.1f);
        
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    #endregion

    #region AnimationEvent Function
    public void PushRootmotionEvent()
    {
        StartCoroutine(PushRootmotion());    
    }

    public void ClimbRootmotionEvent()
    {
        StartCoroutine(ClimbRootmotion());
    }
    #endregion
}