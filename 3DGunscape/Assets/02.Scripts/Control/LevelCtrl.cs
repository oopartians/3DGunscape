using UnityEngine;
using System.Collections;

public class LevelCtrl : MonoBehaviour
{
    public int Level = 1;
    public int Exp = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Gain exp and level up
    public void GainExp(int exp)
    {
        Exp += exp;
        LevelUp();
    }

    // Required Exp is 30, 40, 50, ...
    int GetRequiredExpToLvlUp()
    {
        return 20 + 10*Level;
    }

    void LevelUp()
    {
        while (Exp > GetRequiredExpToLvlUp())
        {
            Exp -= GetRequiredExpToLvlUp();
            Level++;
        }
    }
}
