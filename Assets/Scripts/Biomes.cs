using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biomes : MonoBehaviour
{
    public int biomeID;
    public string biomeName;
    public List<Blocks> biomeBlocks = new List<Blocks>();
    public float chance;
    public List<Blocks> floorBlocks = new List<Blocks>();
    public Blocks baseFloor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
