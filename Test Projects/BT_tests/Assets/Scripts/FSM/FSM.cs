using System.Collections.Generic;

using UnityEngine;

namespace AI.FSM {

    /**
     * 
     */
    public interface IState {

        string GetName();

        void OnEntry();

        void OnUpdate();

        void OnExit();

        void OnVisualise();

        FSM GetFSM();
    }

    /**
     * 
     */
    public class FSM {

        private List<IState> States = new List<IState>();

        private IState currentState;

        /*
        public IState GetState() {
            return currentState;
        }
        */

        public void Update() {
            currentState.OnUpdate();
        }

        public void MakeTransition(IState state) {

#if UNITY_EDITOR
            Debug.Log("Transitioning to state \"" + state.GetName() + "\"...");
#endif

            if (currentState != null) {
                currentState.OnExit();
            }
            currentState = state;
            currentState.OnEntry();
        }

        public void Visualise() {
            if (currentState != null) {
                currentState.OnVisualise();
            }
        }
    }
}