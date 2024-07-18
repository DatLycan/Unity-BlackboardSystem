namespace DatLycan.Packages.BlackboardSystem {
    public interface IScholar {
        int GetImportance(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }
}
