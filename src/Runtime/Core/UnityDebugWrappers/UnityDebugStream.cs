using Cysharp.Text;
using UnityEngine;
using System.IO;
using System;

namespace Nk7.Logger
{
    internal sealed class UnityDebugStream : Stream
    {
        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => default;

        public override long Position { get; set; }

        private Utf16ValueStringBuilder _buffer;
        private readonly byte _endOfString;

        public UnityDebugStream()
        {
            _buffer = ZString.CreateStringBuilder();
            _endOfString = (byte)'\n';
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            if (_buffer.Length == 0)
            {
                return;
            }

            ReadOnlySpan<char> span = _buffer.AsSpan();
            char prefix = span[1];

            ReadOnlySpan<char> messageSpan = span.Slice(4);
            int end = messageSpan.Length;

            while (end > 0 && char.IsWhiteSpace(messageSpan[end - 1]))
            {
                end--;
            }

            if (end > 0)
            {
                string message = messageSpan.Slice(0, end).ToString();

                switch (prefix)
                {
                    case UnityDebugFormatter.INFORMATION_PREFIX:
                    case UnityDebugFormatter.DEBUG_PREFIX:
                    case UnityDebugFormatter.TRACE_PREFIX:
                        Debug.Log(message);
                        break;

                    case UnityDebugFormatter.CRITICAL_PREFIX:
                    case UnityDebugFormatter.ERROR_PREFIX:
                        Debug.LogError(message);
                        break;

                    case UnityDebugFormatter.WARNING_PREFIX:
                        Debug.LogWarning(message);
                        break;
                }
            }

            _buffer.Clear();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                byte b = buffer[offset + i];

                if (b == _endOfString)
                {
                    Flush();
                }
                else
                {
                    _buffer.Append((char)b);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Flush();
                _buffer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
