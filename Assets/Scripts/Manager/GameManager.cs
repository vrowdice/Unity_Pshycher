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
    /// 현재 씬 캔버스의 트렌스폼
    /// </summary>
    private Transform m_canvasTrans = null;
    /// <summary>
    /// 스테이지 코드 리스트
    /// </summary>
    private List<int> m_stageCodeList = new List<int>();
    /// <summary>
    /// 스테이지 폴더 위치
    /// </summary>
    private string m_stageFolderPath = "Assets/Scenes/Stage";
    /// <summary>
    /// 현재 스테이지 코드
    /// </summary>
    private int m_stageCode = 0;

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
            SceneManager.sceneLoaded -= SceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //씬 로드하는 경우
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
        FileManager _fileManager = new FileManager();

        m_stageCodeList = _fileManager.GetFileNum(m_stageFolderPath);
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
    /// <param name="argStageCode">스테이지 코드</param>
    public void EnterStage(int argStageCode)
    {
        m_stageCode = argStageCode;
        MoveSceneAsName(argStageCode.ToString());
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
    public List<int> StageCodeList => m_stageCodeList;

    public int StageCode
    {
        get { return m_stageCode; }
        set
        {
            if(m_stageCode <= 0)
            {
                return;
            }

            m_stageCode = value;
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
    /// 스테이지 코드 리스트
    /// </summary>
    List<int> StageCodeList { get; }
    /// <summary>
    /// 현재 씬의 캔버스 트랜스폼
    /// </summary>
    Transform CanvasTrans { get; }
    /// <summary>
    /// 현재 스테이지 코드
    /// </summary>
    int StageCode { get; set; }
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
}

