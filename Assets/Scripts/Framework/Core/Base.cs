using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {
    private AppFacade m_Facade;
    private SoundManager m_SoundMgr;
    private TimerManager m_TimerMgr;
    private ObjectPoolManager m_ObjectPoolMgr;
    private FrameManager m_FrameMgr;
    private SendPlatformManager m_PlatformMgr;
    private ReceiveManager m_ReceiveMgr;
    private MaterialsManager m_MaterialsMgr;

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    protected void RegisterMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RegisterViewCommand(view, messages.ToArray());
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    protected void RemoveMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RemoveViewCommand(view, messages.ToArray());
    }

    protected AppFacade facade {
        get {
            if (m_Facade == null) {
                m_Facade = AppFacade.Instance;
            }
            return m_Facade;
        }
    }

    protected SoundManager SoundMgr
    {
        get {
            if (m_SoundMgr == null) {
                m_SoundMgr = facade.GetManager<SoundManager>(ManagerName.SoundMgr);
            }
            return m_SoundMgr;
        }
    }

    protected TimerManager TimerMgr
    {
        get {
            if (m_TimerMgr == null) {
                m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.TimerMgr);
            }
            return m_TimerMgr;
        }
    }

    protected ObjectPoolManager ObjectPoolMgr
    {
        get {
            if (m_ObjectPoolMgr == null) {
                m_ObjectPoolMgr = facade.GetManager<ObjectPoolManager>(ManagerName.ObjectPoolMgr);
            }
            return m_ObjectPoolMgr;
        }
    }

    protected FrameManager FrameMgr
    {
        get
        {
            if (m_FrameMgr == null)
            {
                m_FrameMgr = facade.GetManager<FrameManager>(ManagerName.FrameMgr);
            }
            return m_FrameMgr;
        }
    }

    protected SendPlatformManager PlatformMgr
    {
        get
        {
            if (m_PlatformMgr == null)
            {
                m_PlatformMgr = facade.GetManager<SendPlatformManager>(ManagerName.PlatformMgr);
            }
            return m_PlatformMgr;
        }
    }

    protected ReceiveManager ReceiveMgr
    {
        get
        {
            if (m_ReceiveMgr == null)
            {
                m_ReceiveMgr = facade.GetManager<ReceiveManager>(ManagerName.ReceiveMgr);
            }
            return m_ReceiveMgr;
        }
    }

    protected MaterialsManager MaterialsMgr
    {
        get
        {
            if (m_MaterialsMgr == null)
            {
                m_MaterialsMgr = facade.GetManager<MaterialsManager>(ManagerName.MaterialsMgr);
            }
            return m_MaterialsMgr;
        }
    }
}
