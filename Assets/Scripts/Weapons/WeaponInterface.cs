using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponInterface<T, I>
{
    /// <summary>
    /// Returns true if there is a continuous effect with holding the item
    /// </summary>
    /// <returns></returns>
    bool HasHoldEffect();
    /// <summary>
    /// This weapons basic attack
    /// </summary>
    /// <param name="position">Player transform</param>
    void BaseAttack(T playerTransform);
    /// <summary>
    /// This weapons special attack
    /// </summary>
    /// <param name="playerTransform">Player transform</param>
    void AdvancedAttack(T playerTransform);
    /// <summary>
    /// Effects of item if it has hold effects
    /// </summary>
    /// <param name="player">Player fight script</param>
    void HoldEffect(I player);
}
