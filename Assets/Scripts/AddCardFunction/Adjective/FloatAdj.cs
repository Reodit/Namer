using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAdj : IAdjective
{
    private EAdjective adjectiveName = EAdjective.Float;
    private EAdjectiveType adjectiveType = EAdjectiveType.Normal;
    private int count = 0;
    #region 둥둥 멤버변수
    private float currentTime;
    private float movingSpeed = 1f;
    private float length = 0.08f;
    private float speed = 0.8f;
    GameObject startObj;
    Vector3 childStartPos;
    GameObject groundObj;
    #endregion
    public EAdjective GetAdjectiveName()
    {
        return adjectiveName;
    }

    public EAdjectiveType GetAdjectiveType()
    {
        return adjectiveType;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int addCount)
    {
        this.count += addCount;
    }

    public void Execute(InteractiveObject thisObject)
    {
        //Debug.Log("this is Execute");
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(FloatObj(thisObject.gameObject));
    }

    public void Execute(InteractiveObject thisObject, GameObject player)
    {
        //Debug.Log("Float : this Object -> Player");
    }

    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
    {
        //Debug.Log("Float : this Object -> other Object");
    }

    public void Abandon(InteractiveObject thisObject)
    {
        InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(GravityOn(thisObject.gameObject));
    }

    public IAdjective DeepCopy()
    {
        return new FloatAdj();
    }

    IEnumerator FloatObj(GameObject obj)
    {
        //Debug.Log("This is FloatCoroutineStart");
        obj.transform.position = Vector3Int.RoundToInt(obj.transform.position);
        if (DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.up, Mathf.RoundToInt(obj.transform.lossyScale.y)) == null)
        {
            //바로 밑에 있는 타일 검사해서 있으면 전 과정 돌리기
            //바로 밑에 타일이 없으면 올라가는 코루틴 pass 둥둥 이펙트만 살리기
            //Float 2번 들어갈때의 로직이 없는 상태 - 만들어야함

            RaycastHit hit;
            if (Physics.Raycast(obj.transform.position+new Vector3(0,0.5f,0), Vector3.down, out hit))
            {
                groundObj = hit.collider.gameObject;
            }

            var rb = obj.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            currentTime = 0;
            if (obj != null) yield return null;
            Vector3 startPos = obj.transform.position;
            startObj = DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.down, 1);
            childStartPos = obj.transform.GetChild(0).localPosition;

            if (startObj != null)
            {
                DetectManager.GetInstance.SwapBlockInMap(startObj.transform.position + Vector3.up, startObj.transform.position + Vector3.up + Vector3.up);

                while (obj != null && obj.GetComponent<InteractiveObject>().CheckAdjective(adjectiveName) && currentTime < movingSpeed)
                {
                    //Debug.Log("FloatStart");
                    currentTime += Time.deltaTime;
                    obj.transform.localPosition = Vector3.Lerp(startPos, startObj.transform.position + Vector3.up + Vector3.up, currentTime / movingSpeed);
                    yield return InteractionSequencer.GetInstance.WaitUntilPlayerInteractionEnd(this);
                    //yield return null;
                }

                DetectManager.GetInstance.StartDetector();

                yield return new WaitForSeconds(0.2f);
                if (obj != null) yield return null;
            }

            else if (obj.GetComponent<InteractiveObject>().CheckAdjective(adjectiveName))
            {
                DetectManager.GetInstance.SwapBlockInMap(startPos, startPos + Vector3.up);

                while (obj != null && obj.GetComponent<InteractiveObject>().CheckAdjective(adjectiveName) && currentTime < movingSpeed)
                {
                    currentTime += Time.deltaTime;
                    obj.transform.localPosition = Vector3.Lerp(startPos, startPos + Vector3.up, currentTime / movingSpeed);
                    yield return InteractionSequencer.GetInstance.WaitUntilPlayerInteractionEnd(this);
                    //yield return null;
                }
            }

            float yPos = startPos.y + 1;
            //Debug.Log(yPos);
            obj.transform.localPosition = new Vector3(startPos.x, yPos, startPos.z);

            Vector3 currentPos = obj.transform.GetChild(0).localPosition;

            while (obj != null && obj.GetComponent<InteractiveObject>().CheckAdjective(adjectiveName))
            {
                if (obj.GetComponent<InteractiveObject>().CheckAdjective(EAdjective.Bouncy) || !obj.GetComponent<InteractiveObject>().abandonBouncy)
                {
                    //Debug.Log("BouncyMethod");
                    int counting = obj.GetComponent<InteractiveObject>().Adjectives[(int)EAdjective.Float].GetCount();
                    if (obj.GetComponent<InteractiveObject>().floatDone != counting)
                    {
                        obj.GetComponent<InteractiveObject>().floatDone = counting;
                    }
                }
                else
                {
                    //Debug.Log("floateffect" + obj.name);
                    currentTime += Time.deltaTime * speed;

                    obj.transform.GetChild(0).
                    localPosition = new Vector3(obj.transform.GetChild(0).localPosition.x, currentPos.y + Mathf.Sin(currentTime) * length, obj.transform.GetChild(0).localPosition.z);
                }
                yield return InteractionSequencer.GetInstance.WaitUntilPlayerInteractionEnd(this);
            }
            if (obj != null)
                Abandon(obj.GetComponent<InteractiveObject>());
        }


        else if (DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.up).GetComponent<InteractiveObject>())
        {
            yield break;
        }
        //Debug.Log("coroutineStop");
    }

    IEnumerator GravityOn(GameObject gameObject)
    {
        Vector3 startPos = gameObject.transform.position;
        if (gameObject != null)
        {
            //abandon 시 mesh의 위치를 되돌리는 코드
            gameObject.transform.GetChild(0).localPosition = childStartPos;

            if (!gameObject.GetComponent<InteractiveObject>().CheckAdjective(EAdjective.Bouncy))
            {
                //Debug.Log("name : " + gameObject.name);
                // Debug.Log("Count : " + gameObject.GetComponent<InteractiveObject>().CheckCountAdjective(adjectiveName));

                if (gameObject.GetComponent<InteractiveObject>().CheckCountAdjective(adjectiveName) < 1)
                {
                    //Debug.Log("Count1 : " + count);
                    var rb = gameObject.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }

                else if (gameObject.GetComponent<InteractiveObject>().CheckCountAdjective(adjectiveName) < 2)
                {
                    //Debug.Log("Count2 : " + count);
                    //Debug.Log("name1 : " + gameObject.name);
                    //var rb = gameObject.GetComponent<Rigidbody>();
                    //Debug.Log("wndfuf : " + rb.useGravity + " : " + rb.isKinematic);

                    DetectManager.GetInstance.SwapBlockInMap(gameObject.transform.position, gameObject.transform.position - Vector3.up);
                    currentTime = 0;
                    while (gameObject != null && gameObject.GetComponent<InteractiveObject>().CheckAdjective(adjectiveName) && currentTime < movingSpeed)
                    {
                        currentTime += Time.deltaTime;
                        gameObject.transform.localPosition = Vector3.Lerp(startPos, startPos - Vector3.up, currentTime / movingSpeed);
                        yield return InteractionSequencer.GetInstance.WaitUntilPlayerInteractionEnd(this);
                    }
                }
               
                
            }
        }
        yield return null;
    }
}
