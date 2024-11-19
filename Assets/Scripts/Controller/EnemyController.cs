using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int m_health = 3;

    [Header("Damage Settings")]
    [SerializeField]
    private GameObject m_hitEffectPrefab;

    // 스테이지 매니저를 참조하는 변수
    private StageManager m_stageManager;

    void Start()
    {
        m_stageManager = FindObjectOfType<StageManager>();
    }

    // 적이 파괴될 때 호출되는 메서드
    void OnDestroy()
    {
        if (m_stageManager != null)
        {
            m_stageManager.RemoveEnemy(gameObject);
        }
    }

    /// <summary>
    /// 총알에 맞을 때 호출됩니다.
    /// </summary>
    /// <param name="damage">받는 피해량</param>
    public void TakeDamage(int argDamage)
    {
        m_health -= argDamage;

        // 피격 효과 생성 (옵션)
        if (m_hitEffectPrefab != null)
        {
            Instantiate(m_hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // 체력이 0 이하라면 적 제거
        if (m_health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적 제거 로직
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }
}
