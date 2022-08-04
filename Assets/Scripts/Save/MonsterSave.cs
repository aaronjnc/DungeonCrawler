using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSave
{
    private float[] monsterPos;
    private float[] monsterRot;
    private byte id;
    public MonsterSave(GameObject monster)
    {
        MonsterInfo monsterInfo = monster.GetComponent<MonsterInfo>();
        id = monsterInfo.id;
        monsterPos = new float[] { monster.transform.position.x, monster.transform.position.y, monster.transform.position.z };
        monsterRot = new float[] { monster.transform.rotation.x, monster.transform.rotation.y, monster.transform.rotation.z };
    }
    public Vector3 GetPosition()
    {
        return new Vector3(monsterPos[0], monsterPos[1], monsterPos[2]);
    }
    public Vector3 GetRotation()
    {
        return new Vector3(monsterRot[0], monsterRot[1], monsterRot[2]);
    }
    public byte GetID()
    {
        return id;
    }
}
