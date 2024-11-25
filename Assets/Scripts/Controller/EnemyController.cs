using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private int m_health = 3;

    [Header("Damage Settings")]
    [SerializeField]
    private GameObject m_hitEffectPrefab;
    [SerializeField]
    private float m_flashDuration = 0.2f;

    [Header("View Settings")]
    [SerializeField]
    private GameObject m_viewSprite = null;

    // 스테이지 매니저를 참조하는 변수
    private StageManager m_stageManager;
    private SpriteRenderer m_viewSpriteRenderer;
    private Color m_originalColor;

    void Start()
    {
        m_stageManager = FindObjectOfType<StageManager>();
        m_viewSpriteRenderer = m_viewSprite.GetComponent<SpriteRenderer>();
        m_originalColor = m_viewSpriteRenderer.color;
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

        // 적이 데미지를 받았을 때 빨갛게 변하도록 Coroutine 호출
        StartCoroutine(FlashRed());

        // 체력이 0 이하라면 적 제거
        if (m_health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적이 빨갛게 변했다가 원래 색으로 돌아오는 함수
    /// </summary>
    private IEnumerator FlashRed()
    {
        // 빨간색으로 변경
        m_viewSpriteRenderer.color = Color.red;

        // 지정된 시간 동안 빨갛게 유지
        yield return new WaitForSeconds(m_flashDuration);

        // 원래 색으로 복원
        m_viewSpriteRenderer.color = m_originalColor;
    }

    /// <summary>
    /// 적 제거 로직
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }
}
