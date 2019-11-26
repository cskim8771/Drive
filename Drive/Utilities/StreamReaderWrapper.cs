using System.Diagnostics.CodeAnalysis;
using Drive.Utilities.Interface;
using System.IO;

namespace Drive.Utilities
{
    [ExcludeFromCodeCoverage]
    public class StreamReaderWrapper : IStreamReader
    {
        private StreamReader _streamReader;

        public void CreateReader(string filePath) => _streamReader = new StreamReader(filePath);

        public bool FileExists(string filePath) => File.Exists(filePath);
        
        public string ReadLine()
        { 
            return _streamReader.ReadLine();
        } 
        
        public void SetPosition(int position) => _streamReader.BaseStream.Position = position;

        public bool EndOfStream => _streamReader.EndOfStream;
    }
}
