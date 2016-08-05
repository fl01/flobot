namespace Flobot.Eliza
{
    public interface ILineSource
    {
        string ReadLine();

        void Close();
    }
}
