namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainModel
    {
        public float OperationCount { get; private set; }
        public float CompletedOperations { get; private set; }
        
        public void SetOperationCount(float operationCount) => OperationCount = operationCount;
        
        public void CompleteOperation() => CompletedOperations++;
    }
}