using System.IO;

namespace Drive.Utilities.Interface
{
    public interface IStreamReader
    {
        void CreateReader(string filePath);

        bool FileExists(string filePath);

        string ReadLine();
        void SetPosition(int position);
        bool EndOfStream { get; }
    }
}
