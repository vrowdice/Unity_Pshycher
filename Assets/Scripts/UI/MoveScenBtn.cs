using UnityEngine;

public class MoveScenBtn : MonoBehaviour
{
    /// <summary>
    /// 클릭 시
    /// </summary>
    /// <param name="argScenName">이동할 씬 이름</param>
    public void Click(string argScenName)
    {
        GameManager.Instance.MoveSceneAsName(argScenName);
    }
}
