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
}
