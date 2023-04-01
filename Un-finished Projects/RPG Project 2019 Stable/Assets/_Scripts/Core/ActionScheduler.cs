﻿using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action) //Dependancy Inversion -- Call the CURRENT actions' Cancel()
        {
            if (currentAction == action) return;

            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }
    }
}