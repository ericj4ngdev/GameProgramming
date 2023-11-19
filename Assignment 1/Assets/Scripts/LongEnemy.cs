using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1, Idle = 0, Attack, }
public class LongEnemy : Enemy
{
    // Pursuit : ����
    [Header("Pursuit")]
    [SerializeField] private float targetRecognitionRange = 8;  // �ν� �� ���� ���� (�� ���� �ȿ� ������ Attack" ���·� ����)
    [SerializeField] private float pursuitLimitRange = 10;      // ���� ���� (�� ���� �ٱ����� ������ "Wander" ���·� ����)

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackRate = 1;
    private Vector3 moveDirection = Vector3.zero;

    private EnemyState enemyState = EnemyState.None;    // ���� �� �ൿ
    private float lastAttackTime = 0;                   // ���� �ֱ� ���� ���� 
    

    [SerializeField] private Player target;                           // ���� ���� ���(�÷��̾�)

    public override void TakeDamage()
    {
        // bool isDead;
        Debug.Log("Enemy Damaged");
        gameObject.SetActive(false);
        WaveSpawner.Instance.leftMoster--;
        WaveSpawner.Instance.tLeftMonster.text = WaveSpawner.Instance.leftMoster.ToString();
        // ���� �߰�
        // ���� ���� �� �ٱ�
        // Destroy(gameObject);        // ���߿� ü�¿� ���� ���� ���� �ٸ��� �ϱ�
    }

    private void Start()
    {
        target = FindObjectOfType<Player>();        // �÷��̾� �ν�
        ChangeState(EnemyState.Idle);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        // �Ѿư��鼭 ����
        if (distance <= targetRecognitionRange)
        {
            ChangeState(EnemyState.Attack);
        }
        // �� �־����� �߰����� �ʴ´�. 
        else if (distance >= pursuitLimitRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // ���� ������� ���¿� �ٲٷ��� �ϴ� ���°� ������ �ٲ� �ʿ䰡 ���� ������ return
        if (enemyState == newState) return;
        // ������ ������̴� ���� ����
        StopCoroutine(enemyState.ToString());
        // ���� ���� ���¸� newState�� ����
        enemyState = newState;
        // ���ο� ���� ���
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            // �������� ��, �ϴ� �ൿ
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���°�(��ȸ, �߰�, ���Ÿ� ����)
            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {            
            LookRotationToTarget();         // Ÿ�� ������ ��� �ֽ�
            MoveToTarget();                 // Ÿ�� ������ ��� �̵�
            // Ÿ�ٰ��� �Ÿ��� ���� �ൿ ���� (���Ÿ� ���� / ����)
            CalculateDistanceToTargetAndSelectState();
            if (Time.time - lastAttackTime > attackRate)
            {
                // ���� �ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ���� �ð� ����
                lastAttackTime = Time.time;

                // �߻�ü ����
                Instantiate(projectilePrefab, projectileSpawnPoint.position,
                    projectileSpawnPoint.rotation);
            }
            yield return null;
        }
    }

    private void LookRotationToTarget()
    {        
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);  // ��ǥ ��ġ
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);      // �� ��ġ        
        transform.rotation = Quaternion.LookRotation(to - from);            // �ٷ� ����
    }
    private void MoveToTarget()
    {
        Vector3 to = target.transform.position; // ��ǥ ��ġ
        Vector3 from = transform.position;      // �� ��ġ
        moveDirection = (to - from).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    private void OnDrawGizmos()
    {        
        // ��ǥ �ν� �� ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        // ���� ����
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);
    }
}