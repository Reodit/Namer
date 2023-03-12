using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum DetectDir
{
    Null = -1,
    XDown = 0,
    XUp = 1,
    YDown = 2,
    YUp = 3,
    ZDown = 4,
    ZUp = 5
}

public class Detector
{
    protected int length;
    protected Vector3Int pos;
    protected Vector3Int detectPos;
    protected DetectDir dir;
    DetectManager dtManager;

    public Detector(DetectorBuilder builder)
    {
        this.length = builder.length;
        this.pos = builder.pos;
        this.dir = builder.dir;

        dtManager = DetectManager.GetInstance;
        GetDetectPos();
    }

    public void SetLength(int value)
    {
        value = value < 0 ? 0 : value;
        this.length = value;
    }

    public void SetPosition(Vector3Int value)
    {
        this.pos = value;
    }

    public void SetDir(DetectDir value)
    {
        this.dir = value;
    }

    public bool IsRightPos(bool typeObject)
    {
        if (typeObject)
        {
            if (detectPos.x < 0 || detectPos.x > dtManager.GetMaxX || detectPos.y < 0 || detectPos.y > dtManager.GetMaxY || detectPos.z < 0 || detectPos.z > dtManager.GetMaxZ)
            {
                return false;
            }
            return true;
        }
        else
        {
            if (detectPos.x < 0 || detectPos.x > dtManager.GetTileMaxX || detectPos.y < 0 || detectPos.y > dtManager.GetTileMaxY || detectPos.z < 0 || detectPos.z > dtManager.GetTileMaxZ)
            {
                return false;
            }
            return true;
        }
    }

    protected void GetDetectPos()
    {
        Vector3Int[] dirPos = new Vector3Int[7]
        { Vector3Int.zero, Vector3Int.left, Vector3Int.right, Vector3Int.down, Vector3Int.up, Vector3Int.back, Vector3Int.forward};

        detectPos = this.pos + (dirPos[(int)this.dir + 1] * length);
    }

    public virtual Dictionary<DetectDir, List<GameObject>> GetAdjacentObj()
    {
        Dictionary<DetectDir, List<GameObject>> returnDic = new Dictionary<DetectDir, List<GameObject>>();
        GameObject[,,] objectDatas = dtManager.GetObjectsData();

        GetDetectPos();

        if (objectDatas == null || objectDatas.Length == 0)
        {
            return null;
        }
        if (!IsRightPos(true) || objectDatas.GetLength(1) <= pos.y)
        {
            return null;
        }

        var returnObj = objectDatas[detectPos.x, detectPos.y, detectPos.z];
        if (returnObj == null)
        {
            var strechedDic = dtManager.GetStretchedObjDic;
            if (strechedDic.Keys.Contains(detectPos))
            {
                returnDic.Add(this.dir, new[] { strechedDic[detectPos] }.ToList());
                return returnDic;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.Log(returnObj.name, returnObj.transform);
            returnDic.Add(this.dir, new[] { returnObj }.ToList());
            return returnDic;
        }
    }

    public virtual Dictionary<DetectDir, List<GameObject>> GetAdjacentBlock()
    {
        var returnObjects = GetAdjacentObj();
        if (returnObjects == null)
        {
            GameObject[,,] tileDatas = dtManager.GetTilesData();
            if (tileDatas == null || tileDatas.Length == 0)
            {
                return null;
            }

            if (!IsRightPos(false))
            {
                return null;
            }
            else
            {
                Dictionary<DetectDir, List<GameObject>> returnDic = new Dictionary<DetectDir, List<GameObject>>();
                GameObject returnTile = tileDatas[detectPos.x, detectPos.y, detectPos.z];
                if (returnTile == null)
                {
                    return null;
                }
                else
                {
                    Debug.Log(returnTile.name, returnTile.transform);
                    returnDic.Add(this.dir, new[] { returnTile }.ToList());
                    return returnDic;
                }
            }
        }
        else
        {
            return returnObjects;
        }
    }
}

public class LongObjectDetector : Detector
{
    public Vector3Int scale;

    private int countX = 0;
    private int countY = 0;
    private int countZ = 0;

    public LongObjectDetector(DetectorBuilder builder)
        : base(builder)
    {
        this.scale = builder.scale;
    }

    public void SetScale(Vector3Int scale)
    {
        int x = scale.x < 1 ? 1 : scale.x;
        int y = scale.y < 1 ? 1 : scale.y;
        int z = scale.z < 1 ? 1 : scale.z;

        this.scale = scale;
    }

    private void SetCount()
    {
        switch (dir)
        {
            case (DetectDir.XDown):
                countY = scale.y;
                countZ = scale.z;
                break;
            case (DetectDir.XUp):
                countY = scale.y;
                countZ = scale.z;
                length += scale.x;
                break;
            case (DetectDir.YDown):
                countX = scale.x;
                countZ = scale.z;
                break;
            case (DetectDir.YUp):
                countX = scale.x;
                countZ = scale.z;
                length += scale.y;
                break;
            case (DetectDir.ZDown):
                countX = scale.x;
                countY = scale.y;
                break;
            case (DetectDir.ZUp):
                countX = scale.x;
                countY = scale.y;
                length += scale.z;
                break;
            default:
                break;
        }
    }

    public override Dictionary<DetectDir, List<GameObject>> GetAdjacentObj()
    {
        Dictionary<DetectDir, List<GameObject>> returnDic = new Dictionary<DetectDir, List<GameObject>>();
        returnDic.Add(this.dir, new List<GameObject>());

        SetCount();

        Vector3Int originPos = new Vector3Int(this.pos.x, this.pos.y, this.pos.z);
        for (int x = 0; x <= countX; x++)
        {
            for (int y = 0; y <= countY; y++)
            {
                for (int z = 0; z <= countZ; z++)
                {
                    this.pos += new Vector3Int(x, y, z);
                    base.GetDetectPos();
                    var objs = base.GetAdjacentObj()[this.dir];
                    if (objs == null)
                    {
                        continue;
                    }
                    returnDic[this.dir].Add(objs[0]);
                }
            }
        }

        this.pos = originPos;
        return returnDic;
    }

    public override Dictionary<DetectDir, List<GameObject>> GetAdjacentBlock()
    {
        Dictionary<DetectDir, List<GameObject>> returnDic = new Dictionary<DetectDir, List<GameObject>>();
        returnDic.Add(this.dir, new List<GameObject>());

        SetCount();

        Vector3Int originPos = new Vector3Int(this.pos.x, this.pos.y, this.pos.z);
        for (int x = 0; x <= countX; x++)
        {
            for (int y = 0; y <= countY; y++)
            {
                for (int z = 0; z <= countZ; z++)
                {
                    this.pos += new Vector3Int(x, y, z);
                    base.GetDetectPos();
                    var objs = base.GetAdjacentBlock()[this.dir];
                    if (objs == null)
                    {
                        continue;
                    }
                    returnDic[this.dir].Add(objs[0]);
                }
            }
        }

        this.pos = originPos;
        return returnDic;
    }
}

public class DetectorBuilder
{
    public int length;
    public Vector3Int pos;
    public DetectDir dir;
    public Vector3Int scale;

    public DetectorBuilder()
    {
        length = 0;
        pos = Vector3Int.zero;
        dir = DetectDir.Null;
        scale = Vector3Int.one;
    }

    public Detector BuildDetector()
    {
        if (scale == Vector3Int.one)
        {
            return new Detector(this);
        }
        else
        {
            return new LongObjectDetector(this);
        }
    }

    public DetectorBuilder SetLength(int length)
    {
        this.length = length;
        return this;
    }

    public DetectorBuilder SetPosition(Vector3Int pos)
    {
        this.pos = pos;
        return this;
    }

    public DetectorBuilder SetScale(Vector3Int scale)
    {
        this.scale = scale;
        return this;
    }

    public DetectorBuilder SetDir(DetectDir dir)
    {
        this.dir = dir;
        return this;
    }
}
