using UnityEngine;
using System.Collections;

public class MonsterCtrl : MonoBehaviour
{
    // States
    public enum EMonsterState {Idle, Trace, Attack, Die}
    public EMonsterState MonsterState = EMonsterState.Idle;
    
    // Components
    private Transform _monsterTransform;
    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    // Distances
    public float TraceDistance = 20.0f;
    public float AttackDistance = 2.0f;

    // Life
    private bool _isDie = false;

    // Blood Effect
    public GameObject _bloodEffect;
    public GameObject _bloodDecal;


    // Use this for initialization
    void Start () {
	    // Get transform
        _monsterTransform = this.gameObject.GetComponent<Transform>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // Get component
        _navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
        _animator = this.gameObject.GetComponent<Animator>();
        // Coroutines
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!_isDie)
        {
            yield return new WaitForSeconds(0.2f);

            // Check distance
            float distance = Vector3.Distance(_playerTransform.position, _monsterTransform.position);

            if (distance <= AttackDistance)
            {
                MonsterState = EMonsterState.Attack;
            }
            else if (distance <= TraceDistance)
            {
                MonsterState = EMonsterState.Trace;
            }
            else
            {
                MonsterState = EMonsterState.Idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!_isDie)
        {
            switch (MonsterState)
            {
                case EMonsterState.Idle:
                    _navMeshAgent.Stop();
                    _animator.SetBool("IsTrace", false);
                    _animator.SetBool("IsAttack", false);
                    break;

                case EMonsterState.Trace:
                    // Set target
                    _navMeshAgent.Resume();
                    _navMeshAgent.SetDestination(_playerTransform.position);
//                    _navMeshAgent.destination = _playerTransform.position;
                    _animator.SetBool("IsTrace", true);
                    _animator.SetBool("IsAttack", false);
                    break;

                case EMonsterState.Attack:
                    _navMeshAgent.Stop();
                    _animator.SetBool("IsAttack", true);
                    break;
            }

            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator CreateBloodEffect(Vector3 position)
    {
        // Blood Effect
        GameObject blood1 = (GameObject) Instantiate(_bloodEffect, position, Quaternion.identity);
        Destroy(blood1, 2.0f);

        // Blood Decal
        Vector3 decalPosition = _monsterTransform.position + Vector3.up*0.01f;
        // Random
        Quaternion decalRotation = Quaternion.Euler(0, Random.Range(0,360), 0);

        // Decal Prefab
        GameObject blood2 = (GameObject) Instantiate(_bloodDecal, decalPosition, decalRotation);
        // Random position
        float scale = Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = new Vector3(scale, 1, scale);

        // Last for 5 secs
        Destroy(blood2, 5.0f);

        yield return null;
    }

    // EnemyDamageReceiver calls this method
    public void Die()
    {
        StopAllCoroutines();
        _isDie = true;
        MonsterState = EMonsterState.Die;
        _navMeshAgent.Stop();
        _animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider collider in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            collider.enabled = false;
        }
    }

    // EnemyDamageReceiver calls this method
    public void GotHit()
    {
        // BloodEffect is appearing at little higher and closer position to player
        Vector3 middle = _monsterTransform.position;
        middle.y += _monsterTransform.lossyScale.y; // Higher
        middle += (_playerTransform.position - _monsterTransform.position).normalized; // Closer

        StartCoroutine(this.CreateBloodEffect(middle));
        _animator.SetTrigger("IsHit");
    }
}
