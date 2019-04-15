using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

public class View : Base, IView {
    public virtual void OnMessage(IMessage message) {
    }
}
