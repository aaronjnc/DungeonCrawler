using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    private float[] playerPos;
    private float[] playerRot;
    private float[] playerSize;
    private float health;
    public PlayerSave(GameObject player)
    {
        playerPos = new float[] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
        playerRot = new float[] { player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z };
        playerSize = new float[] { player.transform.lossyScale.x, player.transform.lossyScale.y, player.transform.lossyScale.z };
        health = player.GetComponent<PlayerFight>().health;
    }
    public Vector3 GetPlayerPos()
    {
        return new Vector3(playerPos[0], playerPos[1], playerPos[2]);
    }
    public Vector3 GetPlayerRot()
    {
        return new Vector3(playerRot[0], playerRot[1], playerRot[2]);
    }
}
