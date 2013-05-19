using System;

namespace Gate.Adapters.AspNet.Integration {
    [Serializable]
    public class CrossAppDomainResponseFile {
        public string Path { get; private set; }
        public long Offset { get; private set; }
        public long Length { get; private set; }

        public CrossAppDomainResponseFile(string path, long offset, long length) {
            Path = path;
            Offset = offset;
            Length = length;
        }
    }
}