using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    private int m_damage = 0;
    private float m_lifetime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_lifetime);
    }

    /// <summary>
    /// 총알의 상태 초기화
    /// </summary>
    /// <param name="argDamage">총알의 데미지</param>
    /// <param name="argLifeTime">총알의 존재 가능한 시간</param>
    public void ResetState(int argDamage, float argLifeTime)
    {
        m_damage = argDamage;
        m_lifetime = argLifeTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController _enemy = other.gameObject.GetComponent<EnemyController>();
        if (_enemy != null && other.gameObject.CompareTag("Enemy"))
        {
            _enemy.TakeDamage(m_damage);

            Destroy(gameObject);
        }
    }
}
