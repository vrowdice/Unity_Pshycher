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

    [Header("Shooting Settings")]
    [SerializeField]
    GameObject m_bulletPrefab;
    [SerializeField]
    Transform m_firePoint;
    [SerializeField]
    int m_bulletDamage = 1;
    [SerializeField]
    float m_bulletSpeed = 10.0f;
    [SerializeField]
    float m_bulletLifetime = 5.0f;

    private Rigidbody2D m_rigidbody;
    private bool m_isGrounded;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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

        // 총알 발사 로직
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (m_bulletPrefab != null && m_firePoint != null)
        {
            GameObject bulletObj = Instantiate(m_bulletPrefab, m_firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();

            bullet.ResetState(m_bulletDamage, m_bulletLifetime);
            if (bulletRb != null)
            {
                float direction = transform.localScale.x;
                bulletRb.velocity = new Vector2(direction * m_bulletSpeed, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (m_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_groundCheck.position, m_groundCheckRadius);
        }
    }
}