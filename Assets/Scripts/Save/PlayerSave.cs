using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    [Tooltip("Position of player")]
    private float[] playerPos;
    [Tooltip("Rotation of player")]
    private float[] playerRot;
    [Tooltip("Size of player object")]
    private float[] playerSize;
    [Tooltip("Player health")]
    private float health;
    /// <summary>
    /// Creates new PlayerSave object
    /// </summary>
    /// <param name="player"></param>
    public PlayerSave(GameObject player)
    {
        playerPos = new float[] { player.transform.position.x, player.transform.position.y, player.transform.position.z };
        playerRot = new float[] { player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z };
        playerSize = new float[] { player.transform.lossyScale.x, player.transform.lossyScale.y, player.transform.lossyScale.z };
        health = player.GetComponent<PlayerFight>().health;
    }
    /// <summary>
    /// Returns player position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPos()
    {
        return new Vector3(playerPos[0], playerPos[1], playerPos[2]);
    }
    /// <summary>
    /// Returns player rotation
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerRot()
    {
        return new Vector3(playerRot[0], playerRot[1], playerRot[2]);
    }
    /// <summary>
    /// Returns player health
    /// </summary>
    /// <returns></returns>
    public float GetPlayerHealth()
    {
        return health;
    }
}
