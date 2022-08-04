using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Parent class for all interactbales
/// </summary>
public abstract class InteractableTile : MonoBehaviour
{
    /// <summary>
    /// method called when interacting with object
    /// </summary>
    public abstract void Interact();
}
