using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Zone")]
public class RecruitZone : ScriptableObject
{
    public string zoneName;

    public float[] dist = new float[4];

    public int cost;

    public int summonCount;

}
