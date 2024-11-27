using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 돈 아이템 저장, 다음 씬에 데이터 이동 등을 담당합니다
/// 단독으로 실행이 가능해야합니다
/// 라운드 데이터 리스트의 인덱스는 스크립터블 오브젝트 안의 라운드 데이터 인덱스와 일치해야합니다
/// 
/// 부가 기능
/// 
/// 경고 패널 생성 기능
/// 사운드 관리 기능
/// </summary>
public class GameManager : MonoBehaviour, IGameManager
{
    /// <summary>
    /// 자기 자신
    /// </summary>
    static GameManager g_gameManager = null;

    /// <summary>
    /// 경고 오브젝트 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_alertObjPrefeb = null;
    /// <summary>
    /// 옵션 매니저
    /// </summary>
    [SerializeField]
    OptionManager m_optionManager = null;

    /// <summary>
    /// 스테이지 폴더 위치
    /// </summary>
    private const string m_stageFolderPath = "Assets/Scenes/Stage/";

    /// <summary>
    /// 현재 씬 캔버스의 트렌스폼
    /// </summary>
    private Transform m_canvasTrans = null;
    /// <summary>
    /// 스테이지 코드 리스트
    /// 현재는 레벨의 개념이 없음
    /// </summary>
    private List<List<int>> m_stageIndexList = new List<List<int>>();
    /// <summary>
    /// 스테이지 클리어 리스트
    /// 최종 클리어 한 스테이지를 표시
    /// </summary>
    private List<int> m_stageClearList = new List<int>();
    /// <summary>
    /// 현재 레벨 인덱스
    /// 스테이지 위의 개념
    /// </summary>
    private int m_levelIndex = 0;
    /// <summary>
    /// 현재 스테이지 인덱스
    /// </summary>
    private int m_stageIndex = 0;

    /// <summary>
    /// 돈
    /// 
    /// 저장 필요
    /// </summary>
    private long m_money = 10000;

    private void Awake()
    {
        //싱글톤 세팅
        if (g_gameManager == null)
        {
            g_gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //씬 로드하는 경우
        SceneManager.sceneLoaded -= SceneLoaded;
        SceneManager.sceneLoaded += SceneLoaded;

        //씬 이름 불러와서 리스트로 참조
        FileManager _fileManager = new FileManager();
        m_stageIndexList.Add(_fileManager.GetFileNum(m_stageFolderPath + m_levelIndex.ToString()));

        //클리어 한 스테이지 초기화
        for(int i = 0; i < m_stageIndexList.Count; i++)
        {
            m_stageClearList.Add(-1);
        }
    }

    /// <summary>
    /// 씬이 로드 되었을 때
    /// gamedatamanager 단독 사용 권장
    /// </summary>
    /// <param name="scene">씬</param>
    /// <param name="mode">모드</param>
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            m_canvasTrans = GameObject.Find("Canvas").transform;

            if(m_canvasTrans != null)
            {
                Instantiate(m_optionManager);
            }
        }
        catch
        {
            m_canvasTrans = null;
            Debug.Log("no canvas");
        }
    }

    /// <summary>
    /// 이름으로 씬 이동
    /// </summary>
    /// <param name="argStr">이동할 씬의 이름</param>
    public void MoveSceneAsName(string argStr)
    {
        SceneManager.LoadScene(argStr);
    }

    /// <summary>
    /// 스테이지 입장
    /// </summary>
    /// <param name="argStageIndex">스테이지 코드</param>
    public void EnterStage(int argStageIndex)
    {
        StageIndex = argStageIndex;
        MoveSceneAsName(argStageIndex.ToString());
    }

    /// <summary>
    /// 현재 스테이지 클리어
    /// </summary>
    public void ClearStage(bool argIsNextStage)
    {
        m_stageClearList[m_levelIndex] += 1;

        if (argIsNextStage == true)
        {
            EnterStage(StageIndex + 1);
        }
        else
        {
            MoveSceneAsName("SelectStage");
        }
    }

    /// <summary>
    /// 경고 패널 생성
    /// </summary>
    public void Alert(string argAlertStr)
    {
        if (m_canvasTrans != null)
        {
            Instantiate(m_alertObjPrefeb, m_canvasTrans).GetComponent<AlertPanel>().Alert(argAlertStr);
        }
    }

    public static GameManager Instance => g_gameManager;
    public Transform CanvasTrans => m_canvasTrans;
    public List<int> StageIndexList => m_stageIndexList[m_levelIndex];
    public int ClearStageCount => m_stageClearList[m_levelIndex];

    public int LevelIndex
    {
        get { return m_levelIndex; }
        set
        {
            if (m_levelIndex <= 0)
            {
                return;
            }

            m_levelIndex = value;
        }
    }
    public int StageIndex
    {
        get { return m_stageIndex; }
        set
        {
            if(m_stageIndex <= 0)
            {
                return;
            }

            m_stageIndex = value;
        }
    }
    public long Money
    {
        get { return m_money; }
        set
        {
            m_money = value;
            if (m_money <= 0)
            {
                m_money = 0;
            }
        }
    }
}

public interface IGameManager
{
    /// <summary>
    /// 현재 레벨의 스테이지 인덱스 리스트 반환
    /// </summary>
    List<int> StageIndexList { get; }
    /// <summary>
    /// 현재 씬의 캔버스 트랜스폼
    /// </summary>
    Transform CanvasTrans { get; }
    /// <summary>
    /// 클리어한 스테이지
    /// </summary>
    int ClearStageCount { get; }
    /// <summary>
    /// 현재 레벨 인덱스
    /// </summary>
    int LevelIndex { get; set; }
    /// <summary>
    /// 현재 스테이지 코드
    /// </summary>
    int StageIndex { get; set; }
    /// <summary>
    /// 돈
    /// </summary>
    long Money { get; set; }
    /// <summary>
    /// 스테이지 입장
    /// </summary>
    void EnterStage(int argStageCode);
    /// <summary>
    /// 이름으로 씬 이동
    /// </summary>
    void MoveSceneAsName(string argStr);
    /// <summary>
    /// 경고 패널 생성
    /// </summary>
    void Alert(string argAlertStr);
    /// <summary>
    /// 스테이지 클리어
    /// </summary>
    void ClearStage(bool argIsNextStage);
}