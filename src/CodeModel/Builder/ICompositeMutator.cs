namespace CodeModel.Builder
{
    public interface ICompositeMutator : IMutator
    {
        void Mutate(CodeModelBuilder bulder);
    }
}