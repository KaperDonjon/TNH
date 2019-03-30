using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public string Key;
    public int Num;
    public string SubKey;
    public int SubNum;
    public List<string> Ley;
    public List<int> Lum;

    public static Mark F_MarkObjWithKey(GameObject obj, string Key)
    {
        Mark n1 = obj.AddComponent<Mark>();
        n1.Key = Key;
        return n1;
    }
}
