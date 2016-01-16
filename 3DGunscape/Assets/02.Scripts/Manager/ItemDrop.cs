using UnityEngine;
using System.Collections;

public class ItemDrop : MonoBehaviour
{
    public DropItemInfo[] Items;

    // Use this for initialization
	void Start ()
	{
        Debug.Log("Item : " + Items.Length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [System.Serializable]
    public struct DropItemInfo
    {
        public GameObject Item;
        public float Chance;
    }

    public void Drop(Vector3 position)
    {
        float dropRate = Random.Range(0f, 1f);
        foreach (DropItemInfo itemInfo in Items)
        {
            float chance = itemInfo.Chance;
            Debug.Log("dropRate : " + dropRate + " chance : " + chance);
            if (dropRate < chance)
            {
                Instantiate(itemInfo.Item, position, Quaternion.identity);
                return;
            }
            dropRate -= chance;
        }
    }
}
