﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
  public enum ActionStates
  {
    Waiting,
    WaitingForTarget,
    Running,
    Failed,
    Finished,
  }

  public class ActionResult
  {
    public Action Action;

    public ActionStates State;
  }
}
