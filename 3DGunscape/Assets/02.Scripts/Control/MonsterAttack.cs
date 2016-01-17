using UnityEngine;
using System.Collections;

public class MonsterAttack : MonoBehaviour
{
    private Transform Target;
    public float Damage;

    // Use this for initialization
    void Start()
    {
        Target = PlayerWeapons.player.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackTarget()
    {
        Target.BroadcastMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver);
        Target.BroadcastMessage("Direction", transform, SendMessageOptions.DontRequireReceiver);
    }
}
