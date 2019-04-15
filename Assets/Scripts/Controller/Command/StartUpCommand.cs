using UnityEngine;
using System.Collections;

public class StartUpCommand : ControllerCommand {

    public override void Execute(IMessage message) {

        //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager<GameManager>(ManagerName.GameMgr);
        AppFacade.Instance.AddManager<SoundManager>(ManagerName.SoundMgr);
        AppFacade.Instance.AddManager<TimerManager>(ManagerName.TimerMgr);
        AppFacade.Instance.AddManager<ObjectPoolManager>(ManagerName.ObjectPoolMgr);    
        AppFacade.Instance.AddManager<FrameManager>(ManagerName.FrameMgr);       
        AppFacade.Instance.AddManager<SendPlatformManager>(ManagerName.PlatformMgr);
        AppFacade.Instance.AddManager<ReceiveManager>(ManagerName.ReceiveMgr);
        AppFacade.Instance.AddManager<MaterialsManager>(ManagerName.MaterialsMgr);
    }
}