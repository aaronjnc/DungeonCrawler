using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class InteractableTile : MonoBehaviour
{
    GameManager manager;
    public abstract void Interact();
}
