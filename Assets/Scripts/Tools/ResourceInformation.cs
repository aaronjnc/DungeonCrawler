using UnityEngine;

public static class ResourceInformation
{
    [Tooltip("Folder location of blocks")]
    const string Blocks = "Blocks/";
    /// <summary>
    /// Returns ID of block name
    /// </summary>
    /// <param name="blockName"></param>
    /// <returns></returns>
    public static byte GetBlockId(string blockName)
    {
        GameObject block = Resources.Load(Blocks + blockName) as GameObject;
        byte id = block.GetComponent<Blocks>().index;
        return id;
    }
}
