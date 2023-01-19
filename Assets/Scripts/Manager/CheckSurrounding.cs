using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Update = UnityEngine.PlayerLoop.Update;


public enum Dir
{
    left = 0,
    right,
    down,
    up,
    back,
    forward,
    Null
}


public class CheckSurrounding : MonoBehaviour
{
    public GameObject forwardObjectInfo;
    public GameObject curObjectInfo;

    // TODO 변경 요망
    #region moho's CheckCoding


    [SerializeField] float baseSize = 1f;
    List<List<float>> dots = new List<List<float>>(3);
    bool timeToDetect = false;
    Vector3[] dirs = { Vector3.left, Vector3.right, Vector3.down, Vector3.up, Vector3.back, Vector3.forward };
    void InitArray()
    {
        dots = new List<List<float>>(3);
        dots.Add(new List<float>());
        dots.Add(new List<float>());
        dots.Add(new List<float>());
    }

    public void OnChangedScaleOrPosition(Transform go)
    {
        InitArray();

        float lossyScaleX = go.lossyScale.x;
        int countX = (int)Math.Round(lossyScaleX / baseSize);
        float lossyScaleY = go.lossyScale.y;
        int countY = (int)Math.Round(lossyScaleY / baseSize);
        float lossyScaleZ = go.lossyScale.z;
        int countZ = (int)Math.Round(lossyScaleZ / baseSize);

        float floorX = go.position.x - (lossyScaleX / 2f) - (baseSize / 2f);
        //float floorX = transform.position.x - 0.5f;
        for (int i = 0; i < countX; i++)
        {
            dots[0].Add(floorX + i + 1);
        }

        // 해당 타일은 position 위치가 땅바닥에 있으므로 따로 스케일 값을 뺄 필요가 없음 
        //float floorY = transform.position.y - (lossyScaleY / 2f) - 0.5f;
        float floorY = go.position.y - (baseSize / 2f);
        for (int i = 0; i < countY; i++)
        {
            dots[1].Add(floorY + i + 1);
        }

        float floorZ = go.position.z - (lossyScaleZ / 2f) - (baseSize / 2f);
        //float floorZ = transform.position.z - 0.5f;
        for (int i = 0; i < countZ; i++)
        {
            dots[2].Add(floorZ + i + 1);
        }
    }

    // 검출 코드 --> Transform hit을 반환 
    Transform Detect(float x, float y, float z, Dir dir)
    {
        RaycastHit[] hit = Physics.RaycastAll(new Vector3(x, y, z) - dirs[(int)dir] * 0.01f, dirs[(int)dir], (baseSize))
            .OrderBy(h => h.distance)
            .Where(h => /*h.transform.CompareTag("InteractObj") &&*/ h.transform != this.transform).ToArray();
        Debug.DrawRay(new Vector3(x, y, z) - dirs[(int)dir] * 0.01f, dirs[(int)dir], Color.blue, (baseSize / 2f));
        if (hit.Length != 0)
        {
            foreach (RaycastHit obj in hit)
                Debug.Log("Hit! " + obj.transform.name + " / dir : " + dir.ToString());

            return hit[0].transform;
        }
        return null;
    }

    // 원하는 한 방향 검출하기 --> List<Transform> hits를 반환 
    public List<Transform> GetTransformsAtDirOrNull(GameObject go, Dir dir)
    {
        InitArray();
        OnChangedScaleOrPosition(go.transform);

        List<Transform> hitObjs = new List<Transform>();
        switch ((int)dir)
        {
            case (0):
            case (1):
                for (int i = 0; i < dots[1].Count; i++)
                {
                    for (int l = 0; l < dots[2].Count; l++)
                    {
                        Transform t = Detect(go.transform.position.x + (go.transform.lossyScale.x / 2f) * ((int)dir == 0 ? -1 : 1), dots[1][i], dots[2][l], dir);
                        if (t != null)
                            hitObjs.Add(t);
                    }
                }
                break;
            case (2):
            case (3):
                for (int i = 0; i < dots[0].Count; i++)
                {
                    for (int l = 0; l < dots[2].Count; l++)
                    {
                        // y축은 기본 position이 바닥에 붙어있으므로 바닥위치는 구할 필요 없고, 천장 위치만 스케일을 더해서 계산
                        Transform t = Detect(dots[0][i], go.transform.position.y + (go.transform.lossyScale.y) * ((int)dir - 2), dots[2][l], dir);
                        if (t != null)
                            hitObjs.Add(t);
                    }
                }
                break;
            case (4):
            case (5):
                for (int i = 0; i < dots[0].Count; i++)
                {
                    for (int l = 0; l < dots[1].Count; l++)
                    {
                        Transform t = Detect(dots[0][i], dots[1][l], go.transform.position.z + (go.transform.lossyScale.z / 2f) * ((int)dir == 4 ? -1 : 1), dir);
                        if (t != null)
                            hitObjs.Add(t);
                    }
                }
                break;
            default:
                break;
        }
        return hitObjs.Count == 0 ? null : hitObjs;
    }

    // 6방향 모두 검출하기 --> List<Transform> hits를 반환 
    public List<Transform> GetTransformsAroundObjectOrNull(GameObject go)
    {
        List<Transform> result = new List<Transform>();

        // 6방향으로 검출 
        for (int i = 0; i < 6; i++)
        {
            List<Transform> transforms = GetTransformsAtDirOrNull(go, (Dir)i);
            bool isNull = (transforms == null);
            if (!isNull)
            {
                foreach (Transform t in transforms)
                    if (t != null) result.Add(t);
            }
        }

        //SetTimeTrue();
        return result.Count == 0 ? null : result;
    }



    #endregion
    public Dir objDir;



    //Find All object In scene Problecm is it collects Rock also
    // public List<GameObject> FindAllInteractObjectsInScene()
    // {
    //     List<GameObject> allObjectInScene = new List<GameObject>();
    //
    //     // Get all objects in the scene
    //     GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
    //     // Loop through all objects in the scene
    //     for (int i = 0; i < allObjects.Length; i++)
    //     {
    //         if (allObjects[i].tag == "InteractObj")
    //         {
    //             allObjectInScene.Add(allObjects[i]);
    //         }
    //     }
    //     // foreach (var item in allObjectInScene)
    //     // {
    //     //     print(item.name);
    //     // }
    //
    //     return allObjectInScene;
    // }
    //
    //
    //
    //
    // public List<GameObject> FindNeighborsObjects(GameObject originObject, float checkDistance)
    // {
    //     List<GameObject> newNeighbors = new List<GameObject>();
    //     GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
    //
    //     for (int i = 0; i < allObjects.Length; i++)
    //     {
    //         float distance = Vector3.Distance(originObject.transform.position, allObjects[i].transform.position);
    //
    //         if (distance <= originObject.GetComponent<Renderer>().bounds.extents.magnitude + checkDistance)
    //         {
    //             newNeighbors.Add(allObjects[i]);
    //         }
    //     }
    //
    //   
    //     return newNeighbors;
    // }



    // check neighboring gameobject using rigidbody.sweeptest returns dictionary<object dictionary[enum], gameobject>
    public Dictionary<Dir, GameObject[]> CheckNeighboursObjectsUsingSweepTest(GameObject checkingObject, float sweepDistance=.5f)
    {
         var rb = CheckRigidBody(checkingObject);
            Dictionary<Dir, GameObject[]> objsDict = new Dictionary<Dir, GameObject[]>();
            
            //앞쪽
            var forwardObj= rb.SweepTestAll(Vector3.forward, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.forward, forwardObj);
    
            //뒤쪽
            var backObj = rb.SweepTestAll(Vector3.back, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.back,backObj);
            
            //왼쪽
            var leftObj = rb.SweepTestAll(Vector3.left, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.left,leftObj);
            
            //오른쪽
            var rightObj = rb.SweepTestAll(Vector3.right, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.right,rightObj);
            
            //위쪽
            var upObj = rb.SweepTestAll(Vector3.up, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.up,upObj);
            
            //아래쪽
            var downObj = rb.SweepTestAll(Vector3.down, sweepDistance).Select(x=>x.transform.gameObject).ToArray();
            objsDict.Add(Dir.down,downObj);
            
            return objsDict;
    }
    //CheckRigidBody() it check's rigid body and if it's not exist than make new one 
    //WARNING!!: iskinematic = ture, useGravity = false
    public Rigidbody CheckRigidBody(GameObject go)
    {
        if (go.TryGetComponent(out Rigidbody rigid)) return rigid;
        else
        {
            var rb =go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            return rb;
        }
    }

    //CheckNeighborsWithCollider() check neighboring gameobject with physics.overlapbox which requires collider 
    
  
    public Dictionary<Dir, List<GameObject>> CheckNeighborsWithCollider(GameObject checkingGameObject)
    {
        Dictionary<Dir, List<GameObject>> neigborObjectDict = new Dictionary<Dir, List<GameObject>>();
        for (int i = 0; i <= (int)Dir.back; i++)
        {
            neigborObjectDict.Add((Dir)i, new List<GameObject>());
        }
        var colider = checkingGameObject.GetComponent<Collider>().bounds.center;
        var colliders = Physics.OverlapBox(colider, transform.localScale/2);
        foreach (var item in colliders)
        {
            if(item.gameObject == null) continue;
            // right side
            if ((item.transform.position - checkingGameObject.transform.position).normalized.x > 0 &&
                (item.transform.position - checkingGameObject.transform.position).normalized.y >= 0)
            {
                neigborObjectDict[Dir.right].Add(item.gameObject);
                
            }
            //left side
            if ((item.transform.position - checkingGameObject.transform.position).normalized.x < 0 &&
                (item.transform.position - checkingGameObject.transform.position).normalized.y > 0)
            {
                neigborObjectDict[Dir.left].Add(item.gameObject);
            }
            //front
            if ((item.transform.position - checkingGameObject.transform.position).normalized.z > 0 &&
                (item.transform.position - checkingGameObject.transform.position).normalized.y > 0)
            {
                neigborObjectDict[Dir.forward].Add(item.gameObject);
            }
            //back
            if ((item.transform.position - checkingGameObject.transform.position).normalized.z < 0 &&
                (item.transform.position - checkingGameObject.transform.position).normalized.y > 0)
            {
                neigborObjectDict[Dir.back].Add(item.gameObject);
            }
            //down
            if ((item.transform.position - checkingGameObject.transform.position).normalized.y < 0
                && (item.transform.position - checkingGameObject.transform.position).normalized.x == 0
                && (item.transform.position - checkingGameObject.transform.position).normalized.z == 0)
            {
                neigborObjectDict[Dir.down].Add(item.gameObject);
            }
            //Up
            if ((item.transform.position - checkingGameObject.transform.position).normalized.y > 0
                && (item.transform.position - checkingGameObject.transform.position).normalized.x == 0
                && (item.transform.position - checkingGameObject.transform.position).normalized.z == 0)
            {
                neigborObjectDict[Dir.up].Add(item.gameObject);
            }
        }
    
        return neigborObjectDict;
    }



    #region CharacterCheck
    //CheckCharacterCurrentTile() it cast a ray to buttom of an GameObject
    public void CheckCharacterCurrentTile(GameObject character)
    {
        RaycastHit hit;
        if (Physics.Raycast(character.transform.position, Vector3.down, out hit, 2f))
        {
            if (!forwardObjectInfo)
            {
                curObjectInfo = hit.transform.gameObject;
            }
        }
    }

    //CheckForwardObj() it checks what is infront of gameobject
    public void CheckForwardObj(GameObject gameObject)
    {
        RaycastHit IORay;
        Vector3 fwd = gameObject.transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position, fwd, Color.green);

        if (Physics.Raycast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), fwd, out IORay, 0.5f))
        {
            if (curObjectInfo.GetComponent<BoxCollider>().bounds.center.x - IORay.transform.position.x == -1 &&
                curObjectInfo.GetComponent<BoxCollider>().bounds.center.z - IORay.transform.position.z == 0)
            {
                forwardObjectInfo = IORay.transform.gameObject;
                objDir = Dir.right;
            }

            else if (curObjectInfo.GetComponent<BoxCollider>().bounds.center.x - IORay.transform.position.x == 0 &&
                     curObjectInfo.GetComponent<BoxCollider>().bounds.center.z - IORay.transform.position.z == 1)
            {
                forwardObjectInfo = IORay.transform.gameObject;
                objDir = Dir.down;
            }

            else if (curObjectInfo.GetComponent<BoxCollider>().bounds.center.x - IORay.transform.position.x == 1 &&
                     curObjectInfo.GetComponent<BoxCollider>().bounds.center.z - IORay.transform.position.z == 0)
            {
                forwardObjectInfo = IORay.transform.gameObject;
                objDir = Dir.left;
            }

            else if (curObjectInfo.GetComponent<BoxCollider>().bounds.center.x - IORay.transform.position.x == 0 &&
                     curObjectInfo.GetComponent<BoxCollider>().bounds.center.z - IORay.transform.position.z == -1)
            {
                forwardObjectInfo = IORay.transform.gameObject;
                objDir = Dir.up;
            }
        }

        else
        {
            forwardObjectInfo = null;
            objDir = Dir.Null;
        }
    }

    #endregion
   
}

