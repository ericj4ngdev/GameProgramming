using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1, Idle = 0, Attack, }
public class LongEnemy : Enemy
{
    // Pursuit : 추적
    [Header("Pursuit")]
    [SerializeField] private float targetRecognitionRange = 8;  // 인식 및 공격 범위 (이 범위 안에 들어오면 Attack" 상태로 변경)
    [SerializeField] private float pursuitLimitRange = 10;      // 추적 범위 (이 범위 바깥으로 나가면 "Wander" 상태로 변경)

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRate = 1;
    private Vector3 moveDirection = Vector3.zero;

    private EnemyState enemyState = EnemyState.None;    // 현재 적 행동
    private float lastAttackTime = 0;                   // 공격 주기 계산용 변수 
    

    [SerializeField] private Player target;                           // 적의 공격 대상(플레이어)

    public override void TakeDamage()
    {
        // bool isDead;
        Debug.Log("Enemy Damaged");
        gameObject.SetActive(false);
        WaveSpawner.Instance.leftMoster--;
        WaveSpawner.Instance.tLeftMonster.text = WaveSpawner.Instance.leftMoster.ToString();
        // 점수 추가
        // 남은 몬스터 수 줄기
        // Destroy(gameObject);        // 나중에 체력에 따른 제거 조건 다르게 하기
    }

    private void Start()
    {
        target = FindObjectOfType<Player>();        // 플레이어 인식
        ChangeState(EnemyState.Idle);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        // 쫓아가면서 공격
        if (distance <= targetRecognitionRange)
        {
            ChangeState(EnemyState.Attack);
        }
        // 더 멀어지면 추격하지 않는다. 
        else if (distance >= pursuitLimitRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // 현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없기 때문에 return
        if (enemyState == newState) return;
        // 이전에 재생중이던 상태 종료
        StopCoroutine(enemyState.ToString());
        // 현재 적의 상태를 newState로 설정
        enemyState = newState;
        // 새로운 상태 재생
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            // 대기상태일 때, 하는 행동
            // 타겟과의 거리에 따라 행동 선태개(배회, 추격, 원거리 공격)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {            
            LookRotationToTarget();         // 타겟 방향을 계속 주시
            MoveToTarget();                 // 타겟 방향을 계속 이동
            // 타겟과의 거리에 따라 행동 선택 (원거리 공격 / 정지)
            CalculateDistanceToTargetAndSelectState();
            if (Time.time - lastAttackTime > attackRate)
            {
                // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;

                // 발사체 생성
                Instantiate(projectilePrefab, projectileSpawnPoint.position,
                    projectileSpawnPoint.rotation);
            }
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {        
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // 내 위치        
        transform.rotation = Quaternion.LookRotation(to - from);            // 바로 돌기
    }
    private void MoveToTarget()
    {
        Vector3 to = target.transform.position; // 목표 위치
        Vector3 from = transform.position;      // 내 위치
        moveDirection = (to - from).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    private void OnDrawGizmos()
    {        
        // 목표 인식 및 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        // 추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);
    }
}