namespace TestTarget.Cqrs
{
    public interface IQueryDispatcher
    {
        TResult Query<TResult>(IQuery<TResult> query);
    }
}