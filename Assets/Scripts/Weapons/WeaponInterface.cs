using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponInterface: MonoBehaviour
{
    public Sprite idlePlayer;
    public void Pickup(SpriteRenderer player)
    {
        player.sprite = idlePlayer;
    }
    /// <summary>
    /// Returns true if there is a continuous effect with holding the item
    /// </summary>
    /// <returns></returns>
    public abstract bool HasHoldEffect();
    /// <summary>
    /// This weapons basic attack
    /// </summary>
    /// <param name="position">Player transform</param>
    public abstract void BaseAttack(Transform playerTransform);
    /// <summary>
    /// This weapons special attack
    /// </summary>
    /// <param name="playerTransform">Player transform</param>
    public abstract void AdvancedAttack(Transform playerTransform);
    /// <summary>
    /// Effects of item if it has hold effects
    /// </summary>
    /// <param name="player">Player fight script</param>
    public abstract void HoldEffect(PlayerFight player);
}
