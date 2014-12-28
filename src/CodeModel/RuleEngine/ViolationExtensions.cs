namespace CodeModel.RuleEngine
{
    public static class ViolationExtensions
    {
        public static bool HasSourceLocation(this Violation @this)
        {
            var x = @this as IViolationWithSourceLocation;

            return x != null && x.SourceLocation.HasValue;
        }
    }
}