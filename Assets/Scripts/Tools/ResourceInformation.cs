using UnityEngine;

public static class ResourceInformation
{
    const string Blocks = "Blocks/";
    public static byte GetBlockId(string blockName)
    {
        GameObject block = Resources.Load(Blocks + blockName) as GameObject;
        byte id = block.GetComponent<Blocks>().index;
        return id;
    }
}
