using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
