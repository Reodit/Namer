using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdjective
{
    public Adjective GetName(); 
    public int GetCount();
    public void SetCount(int addCount);
    
    public void Execute(InteractiveObject thisObject);
    public void Execute(InteractiveObject thisObject, GameObject player);
    public void Execute(InteractiveObject thisObject, InteractiveObject otherObject);
}
