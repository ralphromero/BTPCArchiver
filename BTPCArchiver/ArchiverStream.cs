using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BTPCArchiver
{
    class ArchiverStream : Stream, IDisposable
    {
        Stream upstream;
        Archiver arch;
        FileArchiver fileArchiver;
        bool fileArchivingEnabled = false;
        string logSource = "BizTalk Server";

        public ArchiverStream(Stream upstream, Archiver arch)
        {
            this.upstream = upstream;
            this.arch = arch;

            fileArchivingEnabled = arch.Enabled;

            if (fileArchivingEnabled)
            {
                try
                {
                    fileArchiver = new FileArchiver(arch.PropertyNS, arch.PropertyName, arch.FilenameExtension, arch.Filepath, arch.MessageID);
                }
                catch (Exception e)
                {
                    fileArchivingEnabled = false;
                    if (!System.Diagnostics.EventLog.SourceExists(logSource))
                        System.Diagnostics.EventLog.CreateEventSource(logSource, "Application");
                    System.Diagnostics.EventLog.WriteEntry(logSource, string.Format("Encountered an error: '{0}' : '{1}'", e.Message, e.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = upstream.Read(buffer, offset, count);
            if (fileArchivingEnabled == true)
            {

                fileArchiver.write(buffer, offset, result);

            }
            return result;
        }

        void IDisposable.Dispose()
        {
            fileArchiver.Dispose();
        }

        public override bool CanRead
        {
            get { return upstream.CanRead; }
        }

        public override bool CanWrite
        {
            get { return upstream.CanWrite; }
        }

        public override void Flush()
        {
            upstream.Flush();
        }

        public override long Length
        {
            get { return upstream.Length; }
        }

        public override long Position
        {
            get
            {
                return upstream.Position;
            }
            set
            {
                upstream.Position = value;
                fileArchiver.Position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return upstream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            upstream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            upstream.Write(buffer, offset, count);
        }

        public String finalOutputFilepath()
        {
            return fileArchiver.FinalFilePath;
        }
    }
}
