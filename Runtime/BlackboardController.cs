using UnityEngine;

namespace DatLycan.Packages.BlackboardSystem {
    public class BlackboardController : MonoBehaviour {
        [SerializeField] private BlackboardObject blackboardObject;
        [SerializeField, Tooltip("Update the Chancellors loop. \n-> Calls Scholars Execute() method.")] public bool updateChancellor = true;
        
        private readonly Blackboard blackboard = new();
        private readonly Chancellor chancellor = new();

        public Blackboard GetBlackboard() => blackboard;

        public void RegisterScholar(IScholar scholar) => chancellor.RegisterScholar(scholar);
        public void UnregisterScholar(IScholar scholar) => chancellor.UnregisterScholar(scholar);

        private void Awake() => blackboardObject.SetValuesOnBlackboard(blackboard);
        
        private void Update() {
            if (!updateChancellor) return;
            chancellor.BlackboardIteration(blackboard).ForEach(a => a());
        } 
    }
}
