using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("First Setting")]
    /// <summary>
    /// 플래이어 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_playerPrefeb = null;
    /// <summary>
    /// 스킬 패널 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_abilityPanelPrefeb = null;

    [Header("Stage Setting")]
    /// <summary>
    /// 목적지에 도달하면 스테이지 클리어
    /// </summary>
    [SerializeField]
    bool m_isReachDestination = false;
    /// <summary>
    /// 목적지
    /// </summary>
    [SerializeField]
    Transform m_destination;
    /// <summary>
    /// 모든 적을 죽이면 스테이지 클리어
    /// </summary>
    [SerializeField]
    bool m_isKillAllEnemy = false;

    /// <summary>
    /// 게임 매니저 인터페이스
    /// </summary>
    private IGameManager m_gameManager = null;
    /// <summary>
    /// 플래이어 컨트롤러
    /// </summary>
    private PlayerController m_playerController = null;
    /// <summary>
    /// 적 리스트
    /// </summary>
    private List<GameObject> m_enemyList = new List<GameObject>();
    /// <summary>
    /// 플레이어 스폰 포인트
    /// </summary>
    private Transform m_playerSpawnPosition = null;

    // Start is called before the first frame update
    void Start()
    {
        //인터페이스 지정
        m_gameManager = GameManager.Instance;

        //플래이어 스폰 포인트 찾아서 숨기기
        m_playerSpawnPosition = GameObject.Find("PlayerSpawnPosition").transform;
        m_playerSpawnPosition.GetComponent<SpriteRenderer>().enabled = false;

        //플래이어 스폰 포인트에 플래이어 생성
        m_playerPrefeb = Instantiate(m_playerPrefeb);
        m_playerPrefeb.transform.position = m_playerSpawnPosition.position;
        m_playerController = m_playerPrefeb.GetComponent<PlayerController>();

        //능력 패널 생성
        m_abilityPanelPrefeb = Instantiate(m_abilityPanelPrefeb, m_gameManager.CanvasTrans);
        m_abilityPanelPrefeb.SetActive(false);

        // Enemy 태그를 가진 모든 적을 가져오기
        GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        m_enemyList = new List<GameObject>(_enemies);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) == true)
        {
            if (m_playerController.IsCanControll == true)
            {
                m_playerController.IsCanControll = false;
                m_abilityPanelPrefeb.SetActive(true);
            }
            else
            {
                m_playerController.IsCanControll = true;
                m_abilityPanelPrefeb.SetActive(false);
            }
        }

        if (m_isReachDestination == true && m_destination != null)
        {
            CheckDestination();
        }
    }

    /// <summary>
    /// 적이 삭제될 시
    /// 적이 스스로 호출함
    /// </summary>
    /// <param name="enemy">적 정보</param>
    public void RemoveEnemy(GameObject enemy)
    {
        if (m_enemyList.Contains(enemy))
        {
            m_enemyList.Remove(enemy);
        }

        if (m_isKillAllEnemy == true)
        {
            if (m_enemyList.Count <= 0)
            {
                m_gameManager.ClearStage();
            }
        }
    }

    /// <summary>
    /// 플레이어가 목적지에 도달했는지 확인
    /// </summary>
    void CheckDestination()
    {
        if (m_playerPrefeb != null)
        {
            float distance = Vector3.Distance(m_playerPrefeb.transform.position, m_destination.position);
            if (distance < 1.0f)
            {
                m_gameManager.ClearStage();
            }
        }
    }
}
