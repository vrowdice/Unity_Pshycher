using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    float m_moveSpeed = 5.0f;
    [SerializeField]
    float m_jumpForce = 10.0f;

    [Header("Ground Check Settings")]
    [SerializeField]
    Transform m_groundCheck;
    [SerializeField]
    float m_groundCheckRadius = 0.2f;
    [SerializeField]
    LayerMask m_groundLayer;

    [Header("Common Attak Settings")]
    [SerializeField]
    Transform m_atkPoint;

    [Header("Melee Attack Settings")]
    [SerializeField]
    GameObject m_meleeAtkEffect = null;
    [SerializeField]
    int m_meleeDamage = 1;
    [SerializeField]
    float m_meleeAtkTime = 2.0f;
    [SerializeField]
    float m_meleeInterval = 1.0f;
    [SerializeField]
    float m_meleeAtkRadius = 1.0f;

    [Header("Range Attak Settings")]
    [SerializeField]
    GameObject m_bulletPrefab;
    [SerializeField]
    int m_maxBullet = 3;
    [SerializeField]
    int m_bulletDamage = 1;
    [SerializeField]
    float m_bulletSpeed = 10.0f;
    [SerializeField]
    float m_bulletLifetime = 5.0f;

    private List<Bullet> m_bulletList = new List<Bullet>();
    private Rigidbody2D m_rigidbody;
    private GameObject m_meleeAtkEffectToDestroy = null;
    private bool m_isCanControll = true;
    private bool m_isGrounded;
    private bool m_isCanAttak = true;
    private int m_modeInt = 0;
    private AtkType m_nowAtkMode = new AtkType();

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(m_isCanControll == false)
        {
            return;
        }

        // 이동 로직
        float _moveInput = Input.GetAxis("Horizontal");
        m_rigidbody.velocity = new Vector2(_moveInput * m_moveSpeed, m_rigidbody.velocity.y);

        // 방향 전환
        if (_moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 점프 로직
        m_isGrounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundCheckRadius, m_groundLayer);
        if (m_isGrounded && Input.GetButtonDown("Jump"))
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_jumpForce);
        }

        if(Input.GetKeyDown(KeyCode.M) == true)
        {
            ModeChange();
        }

        if (Input.GetButtonDown("Fire1") == true && m_isCanAttak == true)
        {
            switch (m_nowAtkMode)
            {
                case AtkType.Melee:
                    MeleeAtk();
                    break;
                case AtkType.Range:
                    if(m_bulletList.Count < m_maxBullet)
                    {
                        RangeAtk();
                    }
                    break;
            }
        }
    }

    void IsCanAtkFlagAsTrue()
    {
        m_isCanAttak = true;
    }

    /// <summary>
    /// 모드 변경
    /// </summary>
    void ModeChange()
    {
        m_modeInt++;

        int enumCount = AtkType.GetValues(typeof(AtkType)).Length;
        if(m_modeInt >= enumCount)
        {
            m_modeInt = 0;
        }
        m_nowAtkMode = IntToAtkType(m_modeInt);
    }

    /// <summary>
    /// 근거리 공격
    /// </summary>
    void MeleeAtk()
    {
        if(m_meleeAtkEffect == null)
        {
            return;
        }

        // 근접 공격 효과 생성
        m_meleeAtkEffectToDestroy = Instantiate(m_meleeAtkEffect, m_atkPoint);
        Destroy(m_meleeAtkEffectToDestroy, m_meleeAtkTime);

        // 공격 범위 내의 모든 객체를 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_atkPoint.position, m_meleeAtkRadius);

        // Enemy 태그를 가진 적들에게 데미지 전달
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(m_meleeDamage);
                }
            }
        }

        m_isCanAttak = false;
        Invoke("IsCanAtkFlagAsTrue", m_meleeInterval);
    }

    /// <summary>
    /// 원거리 공격
    /// </summary>
    void RangeAtk()
    {
        if (m_bulletPrefab == null && m_atkPoint == null)
        {
            return;
        }

        GameObject bulletObj = Instantiate(m_bulletPrefab, m_atkPoint.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

        //총알 초기화
        bullet.ResetState(m_bulletDamage, m_bulletLifetime, this);
        //리스트에 총알 추가
        m_bulletList.Add(bullet);

        //총알 가속도 설정
        if (bulletRb != null)
        {
            float direction = transform.localScale.x;
            bulletRb.velocity = new Vector2(direction * m_bulletSpeed, 0);
        }
    }

    /// <summary>
    /// 정수 값을 공격 타입으로 변경
    /// </summary>
    /// <param name="value">정수</param>
    /// <returns>공격 타입</returns>
    AtkType IntToAtkType(int value)
    {
        switch (value)
        {
            case 0:
                return AtkType.Melee;
            case 1:
                return AtkType.Range;
            default:
                return AtkType.Melee;
        }
    }

    /// <summary>
    /// 총알 완전 삭제
    /// 총알에서 불러옴
    /// </summary>
    /// <param name="argBullet">총알 자기 자신</param>
    public void DestroyBullet(Bullet argBullet)
    {
        int _bulletIndex = m_bulletList.IndexOf(argBullet);
        if(_bulletIndex != -1)
        {
            Destroy(m_bulletList[_bulletIndex].gameObject);
            m_bulletList.RemoveAt(_bulletIndex);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(m_groundCheck.position, m_groundCheckRadius);
        }
        if(m_atkPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_atkPoint.position, m_meleeAtkRadius);
        }
    }

    public bool IsCanControll
    {
        get { return m_isCanControll; }
        set
        {
            m_isCanControll = value;
        }
    }
}