using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreationTesting : MonoBehaviour
{
    GameManager manager;
    //CreateWorld create;
    int size;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameController").GetComponent<GameManager>();
        //create = GetComponent<CreateWorld>();
        size = manager.testingsize;
        for(int x = 0; x < size; x++)
        {
            for (int y = 0; y < size;y++)
            {
                if (x!=0 || y!=0)
                {
                    //create.Spawn(new Vector3Int(19 * x, -11 * y, manager.mapz));
                }
            }
        }
    }

}
