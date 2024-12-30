using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public int id;
    public string itemName;
    public string iconPath; 
    public string description;
    public bool isCollected;

    public InventoryItem(int id, string name, string iconPath, string description, bool isCollected)
    {
        this.id = id;
        this.itemName = name;
        this.iconPath = iconPath;
        this.description = description;
        this.isCollected = isCollected;
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(iconPath);
    }
}
