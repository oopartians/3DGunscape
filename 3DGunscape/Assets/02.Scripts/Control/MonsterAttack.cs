using UnityEngine;
using System.Collections;

public class MonsterAttack : MonoBehaviour
{
    private Transform Target;
    public float Damage;
    private float AttackDistance;

    // Use this for initialization
    void Start()
    {
        Target = PlayerWeapons.player.transform;
        AttackDistance = GetComponent<MonsterCtrl>().AttackDistance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackTarget()
    {
        float distance = Vector3.Distance(Target.position, gameObject.transform.position);

        if (distance < AttackDistance)
        {
            Target.BroadcastMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver);
            Target.BroadcastMessage("Direction", transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}
