namespace TestTarget.Cqrs
{
    public interface IQuery<TResult>
    {
    }

    public class GetUser : IQuery<string>
    {
        public GetUser(string userName)
        {            
        }
    }
}