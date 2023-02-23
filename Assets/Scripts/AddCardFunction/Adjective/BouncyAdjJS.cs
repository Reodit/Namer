using System;
    using System.Collections;
    using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

    public class BouncyAdjJS : MonoBehaviour,IAdjective
    {
        private EAdjective adjectiveName = EAdjective.Bouncy;
        private EAdjectiveType adjectiveType = EAdjectiveType.Repeat;
        private int count = 0;

        #region 통통 꾸밈 카드 멤버변수

        private float currentTime;
        private Vector3 prevPos;
        private Vector3 startPos;

        private bool isBouncy;
        float bounceHeight = 2f;
        float bounceSpeed = 3f;
        float bounciness = 4f;
        private int whenStuck = 1;
        private GameObject orignDownUnder;
        private const float RADIANTTHREESIXTY = 6.283f;
        private const int PEAK = 1;
        private const int CREST = 3;
        private const int PERIOD = 4;

        private float time;
        [Range(0, 1)] public float spHeight;
        [Range(0,1)]public float multiAmount = .2f;

        #endregion

        private void Start()
        {
            var rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            Debug.Log(DetectManager.GetInstance.GetTilesData() == null);
            SetStartPos(gameObject);
            // CheckBouncible(gameObject);
            StartCoroutine(BounceCoroutine(gameObject));
        }

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

            var rb = thisObject.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            SetStartPos(thisObject.gameObject);
            CheckBouncible(thisObject.gameObject);
            thisObject.StartCoroutine(BounceCoroutine(thisObject.gameObject));
            // InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(BounceCoroutine(thisObject.gameObject));
        }

        public void Execute(InteractiveObject thisObject, GameObject player)
        {
            //Debug.Log("Bouncy : this Object -> Player");
        }
        
        public void Execute(InteractiveObject thisObject, InteractiveObject otherObject)
        {
            //Debug.Log("Bouncy : this Object -> other Object");
        }

        public void Abandon(InteractiveObject thisObject)
        {
            
        }
        
        public IAdjective DeepCopy()
        {
            return new BouncyAdj();
        }
        

        void SetStartPos(GameObject targetObj)
        {
            // startPos = targetObj.transform.position;
            startPos=new Vector3(targetObj.transform.position.x, Mathf.RoundToInt(targetObj.transform.position.y),
                targetObj.transform.position.z);
            Debug.Log(startPos);
            prevPos = startPos;
        }

        void CheckBouncible(GameObject targetObj)
        {
            var underObject = DetectManager.GetInstance.GetAdjacentObjectWithDir(targetObj, Dir.down, targetObj.transform.lossyScale);
            var upperObject =
                DetectManager.GetInstance.GetAdjacentObjectWithDir(targetObj, Dir.up, targetObj.transform.lossyScale);
            // if (underObject != null && upperObject == null)
            // {
            //     orignDownUnder = underObject;
            //     isBouncy = true;
            // }
            if (underObject != null)
            {
                orignDownUnder = underObject;
                isBouncy = true;
            }
        }
        /*
         * -------TestCase---------
         * 1. 캐릭터가 아래로 들어갔을때 -> 캐릭터를 박아버리네 
         * 2. 뛰면서 2단계위에 뭐가 있을때
         * 3. 뛰면서 위에 캐릭터 있고 그 위에 물체가 있을때
         * 4. 
         */

        private IEnumerator BounceCoroutine(GameObject obj)
        { 
            // Debug.Log("Hello");
            float time = 0;
            Vector3 prevPosition = obj.transform.position;
            int preRemainder = -1;
            isBouncy = true;

            while (isBouncy)
            {
                time += Time.deltaTime * bounceSpeed;
                
                //sin time 에대한 radian값 을보면된다. 한주기를 나머지 연산 %연산을 쓰데 나우는 디바이더가 주기로된다.
                //마루가 되는 부분이 파이/2
                // time % 
                //주기 2초 
                //마루 0.5초
                //골 1.5초
                //6.283 --> radians 한주기값
                var reminder = Mathf.RoundToInt(PERIOD / (RADIANTTHREESIXTY / bounceHeight) * time);
                float y = (Mathf.Sin(time) * bounceHeight+bounceHeight)/(2*bounceHeight / 1.2f);
                //sin = -1 ~ 1 
                // sin* bounceHeight =  -2 ~ 2
                
                // sin* bounceHeight + bounceHeight = 0~ b2 bounceHeight 
                //최대 값을 1.1 로 만들기 위해 나누기 1.1f 
                //4/x = 1.1 
                // x = 3.64
                // 3.64 = 4 / 1.1 ; --> 4 == 2BounceHight / 1.1 

                    //마루
                    //하강
                #region ChangeTileMap
                    // if (reminder % PERIOD == 0)
                    // {
                    //     Debug.Log("Peak");
                    // var isPlayer = CheckCharacter(obj);    
                    // var tileMap = DetectManager.GetInstance.GetObjectsData();
                    // var underPositionedGameObject = tileMap[Mathf.FloorToInt(obj.transform.position.x),Mathf.FloorToInt(startPos.y),Mathf.FloorToInt(obj.transform.position.z)];
                    // var upperPositionedGameObject = tileMap[Mathf.FloorToInt(obj.transform.position.x),Mathf.FloorToInt(startPos.y+2f),Mathf.FloorToInt(obj.transform.position.z)];
                    // //밑이 널이라서 다시 내려가는 코드
                    // if (underPositionedGameObject == null && !isPlayer)
                    // {
                    //     Debug.Log(1);
                    //     var ChangedVact = new Vector3(obj.transform.position.x, startPos.y+1f, obj.transform.position.z);
                    //     DetectManager.GetInstance.SwapBlockInMap(ChangedVact, startPos);
                    //     DetectAndInteract(obj);
                    // }
                    // //밑이 널이 아니라서 내려가지 않는다. -> 현위치 고수 근데 진자운동의 시작잠을 변경
                    // else if (underPositionedGameObject != null && underPositionedGameObject != obj&& upperPositionedGameObject  == null|| isPlayer)
                    // {
                    //     Debug.Log(2);
                    //     if (underPositionedGameObject)
                    //     {
                    //         var newStartPosition = new Vector3(startPos.x, Mathf.FloorToInt(underPositionedGameObject.transform.position.y+1f),
                    //             startPos.z); // 4,2,2 -> rockobj 존재 한다. 이거 납두어야 하지않나? 그담음에 다시 올라갈떄 어캄?
                    //         startPos = newStartPosition;
                    //         
                    //     }
                    //     else if (isPlayer)
                    //     {
                    //         var newStartPosition = new Vector3(startPos.x,
                    //             Mathf.FloorToInt(isPlayer.transform.position.y + 1f), startPos.z);
                    //         startPos = newStartPosition;
                    //     }
                    // }
                    // //밑이 널이 아닌데 위가 막혀있는경우 -> 고정...
                    // else if (upperPositionedGameObject != null && upperPositionedGameObject != obj && underPositionedGameObject != null && underPositionedGameObject != obj )
                    // {
                    //     Debug.Log(3);
                    //     Debug.Log(underPositionedGameObject, underPositionedGameObject.transform);
                    //     Debug.Log(upperPositionedGameObject, upperPositionedGameObject.transform);
                    //     Debug.Log("Big Problem");
                    //     y = y * whenStuck;
                    // }
                // }
                    //골
                    //상승
                  
                if (reminder % PERIOD == CREST||reminder == 0)
                {
                    var upOrDown = obj.transform.position - startPos;
                    //1 올라간다...
                    //-1 내려간다.. 
                    Debug.Log(upOrDown.normalized);
                    // Debug.Log("Crest");
                    var isPlayer = CheckCharacter(obj);
                    // Debug.Log(isPlayer);
                    var objectsData = DetectManager.GetInstance.GetObjectsData();
                    var tileMap = DetectManager.GetInstance.GetTilesData();

                    //아래 오브젝트
                    // var underPositionedGameObject= objectsData[Mathf.FloorToInt(obj.transform.position.x),
                    //     Mathf.FloorToInt(startPos.y-1f),Mathf.FloorToInt(obj.transform.position.z)];
                    int peckHeight = Mathf.RoundToInt(obj.transform.position.y);
                    if (peckHeight - startPos.y <= .5f)
                    {
                        Debug.Log("4 should be here");
                        var test=DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.down);
                        Debug.Log(obj.transform.position);
                        Debug.Log(obj.transform.position.y);
                        Debug.Log(Mathf.RoundToInt(obj.transform.position.y));
                        Debug.Log(test);
                    }
                        
                    Debug.Log(obj.transform.position);
                    //문제 1. 여기서 현재 물체 위치가 배열상의 위치가 달라서 문제가 발생 --> 다시 배열의 위치를 바꾸어 버린다.
                    // 올라가고 있을때랑, 내려가는 중일 때를 정확히 파악해야한다. 
                    // 파악은 했는데 그다음은? 
                    
                    // var underTileObject = tileMap[Mathf.FloorToInt(obj.transform.position.x),
                    //     Mathf.FloorToInt(startPos.y - 1f), Mathf.FloorToInt(obj.transform.position.z)];
                    //위에 오브젝트
                    // var upperPositionedGameObject= objectsData[Mathf.FloorToInt(obj.transform.position.x),
                    //     Mathf.FloorToInt(startPos.y+1f),Mathf.FloorToInt(obj.transform.position.z)];
                    
                    var underPositionedGameObject = DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.down,obj.transform.lossyScale);
                    var upperPositionedGameObject = DetectManager.GetInstance.GetAdjacentObjectWithDir(obj, Dir.up, obj.transform.lossyScale);
                    
                    if (upperPositionedGameObject != null && upperPositionedGameObject != obj &&
                        underPositionedGameObject != null && underPositionedGameObject != obj)
                    {
                        Debug.Log("Ayy?");
                        whenStuck = 0;
                    }
                    else
                    {
                        whenStuck = 1;
                    }
                    
                    if (peckHeight - startPos.y > .5f)
                    {
                       
                        if (underPositionedGameObject == null && !isPlayer )
                        {
                            
                            Debug.Log(1);
                            // Debug.Log(startPos);
                            // var underTileObject = tileMap[Mathf.FloorToInt(obj.transform.position.x),
                            //          Mathf.FloorToInt(startPos.y - 1f), Mathf.FloorToInt(obj.transform.position.z)];
                            // Debug.Log(underTileObject);
                            
                            var ChangedVact = new Vector3(obj.transform.position.x, Mathf.RoundToInt(startPos.y+1f), obj.transform.position.z);
                            Debug.Log(ChangedVact);
                            Debug.Log(startPos);
                            DetectManager.GetInstance.SwapBlockInMap(ChangedVact, startPos);
                            DetectAndInteract(obj);
                        }
                    //밑이 널이 아니라서 내려가지 않는다. -> 현위치 고수 근데 진자운동의 시작잠을 변경
                    else if (underPositionedGameObject != null && underPositionedGameObject != obj&& upperPositionedGameObject  == null|| isPlayer)
                    {
                        Debug.Log(2);
                       
                        if (underPositionedGameObject)
                        {
                            var newStartPosition = new Vector3(startPos.x, Mathf.FloorToInt(underPositionedGameObject.transform.position.y+1f),
                                startPos.z); // 4,2,2 -> rockobj 존재 한다. 이거 납두어야 하지않나? 그담음에 다시 올라갈떄 어캄?
                            startPos = newStartPosition;
                            
                        }
                        else if (isPlayer)
                        {
                            var newStartPosition = new Vector3(startPos.x,
                                Mathf.FloorToInt(isPlayer.transform.position.y + 1f), startPos.z);
                            startPos = newStartPosition;
                        }
                    }
                    //밑이 널이 아닌데 위가 막혀있는경우 -> 고정...
                    // else if (upperPositionedGameObject != null && upperPositionedGameObject != obj && underPositionedGameObject != null && underPositionedGameObject != obj )
                    // {
                    //     Debug.Log(3);
                    //     Debug.Log(underPositionedGameObject, underPositionedGameObject.transform);
                    //     Debug.Log(upperPositionedGameObject, upperPositionedGameObject.transform);
                    //     Debug.Log("Big Problem");
                    //     whenStuck = 0;
                    // }

                        
                    }
                    else
                    {
                         //밑에 아무것도 없는경우 내려가야한다 -> startPos.y -1f 로변경 + 배열에 물체를 미리 아래로 넣기 
                        if (underPositionedGameObject == null && !isPlayer)
                        {
                            // Debug.Log(peckHeight);
                            // Debug.Log(peckHeight - obj.transform.position.y);
                            // Debug.Log(obj.transform.position);
                            Debug.Log(4);
                            var newStartPosition = new Vector3(startPos.x, startPos.y - 1f, startPos.z);
                            Debug.Log(newStartPosition);
                            Debug.Log(startPos);
                            // Debug.Log(newStartPosition);
                            DetectManager.GetInstance.SwapBlockInMap(startPos,newStartPosition);
                            startPos = newStartPosition;
                        }
                        //진자 운동에 의해 올라가야 하는경우 -> 배열에 위치만 수정 
                        else if (upperPositionedGameObject == null)
                        {
                            Debug.Log(5);
                            var ChangedVact = new Vector3(obj.transform.position.x, startPos.y+1f, obj.transform.position.z);
                            Debug.Log(ChangedVact);
                            Debug.Log(startPos);
                            DetectManager.GetInstance.SwapBlockInMap(startPos, ChangedVact);
                        
                            DetectAndInteract(obj);
                        }
                    // 밑에 뭐가 있고 위에도 뭐가 있는경우 그자리 고정 배열체인지 X  startPos X
                    // else if (upperPositionedGameObject != null&& upperPositionedGameObject != obj && ((underPositionedGameObject != null && underPositionedGameObject != obj) || underTileObject != null))
                    // {
                    //     Debug.Log(6);
                    //     var a = objectsData[Mathf.FloorToInt(obj.transform.position.x),
                    //         Mathf.FloorToInt(startPos.y + 1f), Mathf.FloorToInt(obj.transform.position.z)];
                    //     var b = objectsData[Mathf.FloorToInt(obj.transform.position.x),
                    //         Mathf.FloorToInt(startPos.y), Mathf.FloorToInt(obj.transform.position.z)];
                    //     Debug.Log(a, a.transform);
                    //     Debug.Log(b, b.transform);
                    //     Debug.Log(startPos);
                    //     Debug.Log(isPlayer);
                    //     // Debug.Log("2");    
                    //     whenStuck = 0;
                    //     // Debug.Log(upperPositionedGameObject, upperPositionedGameObject.transform);
                    //     // startPos = new Vector3(startPos.x, targetPositionGameObject.transform.position.y - 1f,
                    //     //     startPos.z);
                    // }                    
                    }
                    // var newPos = Vector3.Lerp(obj.transform.position, startPos + new Vector3(0, y, 0), Time.deltaTime * bounciness);
                }
                    
               
                    

                #endregion
              
                //위로올라갈때
                /*
                 * 1. 이전위치 현위치 비교
                 * 현위치 > 이전위치 = 상승
                 * 
                 * 이전위치 < 현위치 = 하락
                 * 
                 */

                #region 현위치 전위치 비교로 타일맵에서 가져오는코드
                // if (obj.transform.position.y > prevPosition.y)
                // {
                //     var tileMap = DetectManager.GetInstance.GetObjectsData();
                //     var targetPositionGameObject= tileMap[Mathf.FloorToInt(obj.transform.position.x),Mathf.FloorToInt(startPos.y+1f),Mathf.FloorToInt(obj.transform.position.z)];
                //     if (targetPositionGameObject == null)
                //     {
                //         var ChangedVact = new Vector3(obj.transform.position.x, startPos.y+1f, obj.transform.position.z);
                //         DetectManager.GetInstance.SwapBlockInMap(startPos, ChangedVact);
                //
                //         DetectAndInteract(obj);
                //     }
                //     // SyncTileMap(obj,startPos.y+1f);
                // }
                // else if (obj.transform.position.y < prevPosition.y)
                // {
                //     var tileMap = DetectManager.GetInstance.GetObjectsData();
                //     var targetPositionGameObject= tileMap[Mathf.FloorToInt(obj.transform.position.x),Mathf.FloorToInt(startPos.y),Mathf.FloorToInt(obj.transform.position.z)];
                //     if (targetPositionGameObject == null)
                //     {
                //         var ChangedVact = new Vector3(obj.transform.position.x, startPos.y+1f, obj.transform.position.z);
                //         DetectManager.GetInstance.SwapBlockInMap(ChangedVact, startPos);
                //         DetectAndInteract(obj);
                //     }
                //     // SyncTileMap(obj,startPos.y);
                // }
                    #endregion
               
                #region Region ForLoops
                // y 축 0 인버전
                // var origin = new Vector3(obj.transform.position.x -(obj.transform.lossyScale.x*0.45f), obj.transform.position.y,
                //     obj.transform.position.z-(obj.transform.lossyScale.z*0.45f));
                //검출 시작 포인트
                // y 축에 .5f 더해준 버전
                // var origin = new Vector3(obj.transform.position.x -(obj.transform.lossyScale.x*0.45f), obj.transform.position.y+(obj.transform.position.y*.5f),
                //     obj.transform.position.z-(obj.transform.lossyScale.z*0.45f));
                // var origin = new Vector3(obj.transform.position.x -(obj.transform.lossyScale.x*0.45f), obj.transform.position.y+.5f,
                //     obj.transform.position.z-(obj.transform.lossyScale.z*0.45f));
                // // 포룹에서 i 와 j 를 더해주는 밸류 
                // float xIncrementPoint = obj.transform.lossyScale.x *.11f;
                // float zIncrementpoint = obj.transform.lossyScale.z *.11f;
                // for (float i = 0; i < obj.transform.lossyScale.x*.75f; i+=xIncrementPoint)
                // {
                //     for (float j = 0; j < obj.transform.lossyScale.z * .75f; j += zIncrementpoint)
                //     {
                //         // RaycastHit hit;
                //         // var rayPoint = new Vector3(origin.x + i, origin.y, origin.z + j);
                //         // // Debug.Log(rayPoint);
                //         // //짧은 길이의 레이
                //         //
                //         // //1. 레이를 쏜다.
                //         // if (Physics.Raycast(rayPoint, Vector3.down, out hit, .5f))
                //         // {
                //         //     var downUnder = hit.transform.gameObject;
                //         //     Debug.Log(hit.transform.gameObject,hit.transform);
                //         //     // 스타트 포지션 바꾸는 코드
                //         //     if (downUnder != null && downUnder != orignDownUnder)
                //         //     {
                //         //                
                //         //         orignDownUnder = downUnder;
                //         //             
                //         //         prevStartPos = startPos;
                //         //         Debug.Log($"{i} : {j} - {downUnder}",downUnder.transform);
                //         //         Debug.Log(startPos);
                //         //         //2. 검출 되면 검출된 오브젝트를 기준으로 startPos를 다시 세팅한다. 
                //         //         startPos = new Vector3(startPos.x, Mathf.FloorToInt(hit.transform.position.y + 1f),
                //         //             startPos.z);
                //         //         test = true;
                //         //         Debug.Log(startPos);
                //         //         // yield return this.StartCoroutine(BounceCoroutine(obj));
                //         //         // yield break;
                //         //         //3. 포룹 나가기
                //         //         break;
                //         //         //coroutine을 다시 불러야 하구 -> no need 
                //         //
                //         //         // yield break;
                //         //     }
                //         // }
                //         // else
                //         // {
                //         // 1-2 레이가 검출되지 않은 경우 레이를 길게 쏜다.
                //         // 긴 레이를 위한 힛
                //         RaycastHit longHit;
                //         var longRayPoint = new Vector3(origin.x + i, origin.y, origin.z + j);
                //         // // 맵전체 높이의 레이 길이를 발사하는 레이
                //         // //character height is .9f
                //         if (obj.transform.position.y - startPos.y >= 1f)
                //         {
                //             // Debug.Log("yo");
                //
                //             #region RayCastAll
                //             // 하나레이를 쏘면 안됨
                //             // var hits = Physics.RaycastAll(longRayPoint, Vector3.down, 1f);
                //             // if (hits.Length == 0)
                //             // {
                //             //     Debug.Log("???");
                //             //     startPos = new Vector3(startPos.x,
                //             //         Mathf.FloorToInt(orignDownUnder.transform.position.y - 1f), startPos.z);
                //             // }
                //             //
                //             // foreach (var item in hits)
                //             // {
                //             //     if (item.transform.gameObject == obj) continue;
                //             //     GameObject downUnder = item.transform.gameObject;
                //             //     if (orignDownUnder != downUnder)
                //             //     {
                //             //         orignDownUnder = downUnder;
                //             //         startPos = new Vector3(startPos.x,
                //             //             Mathf.FloorToInt(downUnder.transform.position.y + 1f), startPos.z);
                //             //     }
                //             // }
                //
                //             #endregion
                //
                //             #region Raycast
                //             if (Physics.Raycast(longRayPoint, Vector3.down, out longHit,DetectManager.GetInstance.GetMaxY))
                //             {
                //                 // Debug.Log("yo2");
                //                 GameObject downUnder = longHit.transform.gameObject;
                //                 if (downUnder != obj)
                //                 {
                //                     if (orignDownUnder == null) orignDownUnder = downUnder;
                //                     if (orignDownUnder != null && orignDownUnder != downUnder)
                //                     {
                //                         orignDownUnder = downUnder;
                //                         // Debug.Log(transform.position);
                //                         // Debug.Log(downUnder.transform.gameObject, downUnder.transform);
                //                         // Debug.Log(downUnder.transform.position);
                //                         // Debug.Log(startPos);
                //                         startPos = new Vector3(startPos.x,
                //                             Mathf.FloorToInt(downUnder.transform.position.y + 1f), startPos.z);
                //                         // Debug.Log(startPos);
                //                         test = true;
                //                         break;
                //                     }
                //                 }
                //                
                //             }
                //             // else
                //             // {
                //             //     Debug.Log(startPos);
                //             //     startPos = new Vector3(startPos.x,
                //             //         Mathf.FloorToInt(orignDownUnder.transform.position.y-1f), startPos.z);
                //             //     // yield return new WaitForSeconds(.5f);
                //             // }
                //             
                //
                //             #endregion
                //             
                //
                //             // if (longHit.transform.gameObject != obj && downUnder != orignDownUnder)
                //             // {
                //             //     // if(downUnder.gameObject.tag == "Player") GameManager.GetInstance.SetTimeScale(0);
                //             //     orignDownUnder = downUnder;
                //             //     // if (startPos.y - Mathf.FloorToInt(longHit.transform.position.y) >= 2)
                //             //     // {
                //             //     // Debug.Log(startPos);
                //             //     // Debug.Log(downUnder.transform.position.y +1f);
                //             //     // Debug.Log(Mathf.CeilToInt(downUnder.transform.position.y +1f));
                //             //     // Debug.Log(Mathf.FloorToInt(downUnder.transform.position.y +1f));
                //             //     // Debug.Log(Mathf.RoundToInt(downUnder.transform.position.y +1f));
                //             //     // 2. 바닥에 검출된거 기준으로 스타트 포지션 다시 수정
                //             //     Debug.Log($"{i} : {j} - {downUnder}",downUnder.transform);
                //             //     Debug.Log(startPos);
                //             //     startPos = new Vector3(startPos.x, Mathf.FloorToInt(downUnder.transform.position.y +1f), startPos.z);
                //             //     // InteractionSequencer.GetInstance.CoroutineQueue.Enqueue(BounceCoroutine(obj)); // 무한 루프
                //             //     // yield break;
                //             //     Debug.Log(startPos);
                //             //     test = true;
                //             //     break;
                //             //     // }
                //             // }
                //         }
                //         
                //         if (test) break;
                //     }
                // }
                //

                #endregion

             
                // 어느정도 올라가면
                y = y * whenStuck;
                obj.transform.position = Vector3.Lerp(obj.transform.position, startPos + new Vector3(0, y, 0), Time.deltaTime * bounciness);
                preRemainder = reminder;

                #region 움직인 후 타일맵에 경신하는 코드
                // if(obj.transform.position.y >= startPos.y+1f)
                // {
                //     if (Mathf.FloorToInt(obj.transform.position.y) != Mathf.FloorToInt(prevPos.y))
                //     {
                //         Vector3 currentPos = new Vector3(obj.transform.position.x, Mathf.FloorToInt(obj.transform.position.y),obj.transform.position.z);
                //         Debug.Log(prevPos);
                //         UpdateTileMap(prevPos, currentPos);
                //         prevPos = currentPos;
                //         // 프리브를 바꾸어주고
                //         Debug.Log(prevPos);
                //         Debug.Log("------------------");
                //     }
                //     // UpdateTileMap(obj);
                // }
                //
                // if (obj.transform.position.y <= startPos.y + 0.1f)
                // {
                //      // Debug.Log("hey");
                //         var currentPos=new Vector3(obj.transform.position.x, Mathf.FloorToInt(obj.transform.position.y),
                //             obj.transform.position.z);
                //
                //     if (currentPos != prevPos)
                //     {
                //         UpdateTileMap(prevPos,currentPos);
                //         Debug.Log(prevPos);
                //         prevPos = currentPos;
                //         Debug.Log(prevPos);
                //     }
                //    
                // }
                

                    #endregion
               
                    
                yield return new WaitForEndOfFrame();
            }
        }

      
        

        void DetectAndInteract(GameObject targetObject)
        {
            List<GameObject> changedObjs = new List<GameObject>();
            changedObjs.Add(targetObject);
            DetectManager.GetInstance.StartDetector(changedObjs);
            // var objectsData = DetectManager.GetInstance.GetObjectsData();
            // if (gameObject.GetComponentInChildren<ParticleSystem>())
            // {
            //     if (gameObject.GetComponentInChildren<ParticleSystem>().isPlaying)
            //     {
            //         var a = objectsData[Mathf.RoundToInt(targetObject.transform.position.x), Mathf.RoundToInt(startPos.y)
            //             , Mathf.RoundToInt(targetObject.transform.position.z)];
            //         var b = objectsData[Mathf.RoundToInt(targetObject.transform.position.x), Mathf.RoundToInt(startPos.y+1f)
            //             , Mathf.RoundToInt(targetObject.transform.position.z)];
            //         Debug.Log(a);
            //         Debug.Log(b);
            //     }
            // }
           
        }

        GameObject CheckCharacter(GameObject thisObject )
        {
           
                float maxDistance = DetectManager.GetInstance.GetMaxY;
                var originPoint = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y+(thisObject.transform.position.y*multiAmount), thisObject.transform.position.z);
                RaycastHit hit;
                //스피어 케스트로 캐릭터 검출
                var hits = Physics.SphereCast(originPoint,thisObject.transform.lossyScale.x*.4f,Vector3.down, out hit,maxDistance);
                if (hits)
                {
                   
                    if (hit.transform.gameObject != thisObject&&hit.transform.gameObject.tag =="Player")
                    {
                        return hit.transform.gameObject;
                        //여기서 캐릭터만 검출하기 return true or false
                        // if(orignDownUnder == null)
                        //     orignDownUnder = hit.transform.gameObject;
                        // if (orignDownUnder != null && orignDownUnder != hit.transform.gameObject)
                        // {
                        //     orignDownUnder = hit.transform.gameObject;
                        //     startPos = new Vector3(startPos.x, Mathf.FloorToInt(hit.transform.position.y + 1f), startPos.z);
                        // }
                    }
                    
                }

            return null;

        }
        [ContextMenu(("TestSPhereCast"))]
        public void TestSphereCast()
        {
            CheckCharacter(gameObject);
        }

        //콜리젼 을  활용해서 스타트 포스를 바꾸기 위한 테스트
        // private void OnCollisionEnter(Collision collision)
        // {
        //     Debug.Log(startPos);
        //     // Debug.Log(collision.gameObject.transform.position.y);
        //     // Debug.Log(Mathf.FloorToInt(collision.gameObject.transform.position.y + 1f));
        //     startPos = new Vector3(startPos.x, Mathf.FloorToInt(collision.gameObject.transform.position.y), startPos.z);
        //     Debug.Log(startPos);
        // }


        /*
         * 1. 크기만큼 y축으로 올라가따가 내려가따가
         * 1. check if it can go up
         * 2. make it go up and down continuously
         * 
         * 2. 캐릭터가  위에 올라갈수 있음 
         */
        
    }