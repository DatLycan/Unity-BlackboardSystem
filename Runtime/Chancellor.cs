using System;
using System.Collections.Generic;
using DatLycan.Packages.Utils;

namespace DatLycan.Packages.BlackboardSystem {
    public class Chancellor {
        private List<IScholar> scholars = new();

        public void RegisterScholar(IScholar scholar) {
            Preconditions.CheckNotNull(scholar);
            scholars.Add(scholar);
        }

        public void UnregisterScholar(IScholar scholar) {
            Preconditions.CheckNotNull(scholar);
            scholars.Remove(scholar);
        }

        public List<Action> BlackboardIteration(Blackboard blackboard) {
            IScholar bestScholar = null;
            int highestInsistence = 0;

            foreach (IScholar scholar in scholars) {
                int insistence = scholar.GetImportance(blackboard);
                if (insistence > highestInsistence) {
                    highestInsistence = insistence;
                    bestScholar = scholar;
                }
            }
            
            bestScholar?.Execute(blackboard);
            
            List<Action> actions = new(blackboard.PassedActions);
            blackboard.ClearActions();
            
            return actions;
        }
    }
}
