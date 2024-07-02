using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public GameObject itemGameobjet;

    public abstract void Use();
}
