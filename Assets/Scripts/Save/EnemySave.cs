using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySave
{
    private float[] enemyPos;
    private float[] enemyRot;
    private byte id;
    public EnemySave(GameObject enemy)
    {
        EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();
        id = enemyInfo.id;
        enemyPos = new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z };
        enemyRot = new float[] { enemy.transform.rotation.x, enemy.transform.rotation.y, enemy.transform.rotation.z };
    }
    public Vector3 GetPosition()
    {
        return new Vector3(enemyPos[0], enemyPos[1], enemyPos[2]);
    }
    public Vector3 GetRotation()
    {
        return new Vector3(enemyRot[0], enemyRot[1], enemyRot[2]);
    }
    public byte GetID()
    {
        return id;
    }
}
