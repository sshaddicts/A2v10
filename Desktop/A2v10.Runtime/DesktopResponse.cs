﻿// Copyright © 2015-2019 Alex Kukhtin. All rights reserved.

using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace A2v10.Runtime
{
	public class DesktopResponse : HttpResponseBase, IDisposable
	{

		public override String ContentType { get; set; }
		public override NameValueCollection Headers => _headers;

		private MemoryStream _stream;
		public override Stream OutputStream
		{
			get
			{
				if (_stream == null)
					_stream = new MemoryStream();
				return _stream;
			}
		}

		TextWriter _textWriter;
		public override TextWriter Output
		{
			get
			{
				if (_textWriter == null)
					_textWriter = new StringWriter();
				return _textWriter;
			}
		}

		BinaryWriter _binaryWriter;
		public BinaryWriter BinaryWriter
		{
			get
			{
				if (_binaryWriter == null)
					_binaryWriter = new BinaryWriter(OutputStream);
				return _binaryWriter;
			}
		}


		public void Dispose()
		{
			if (_textWriter != null)
				_textWriter.Close();
			_textWriter = null;
			if (_binaryWriter != null)
				_binaryWriter.Close();
			_binaryWriter = null;
			if (_stream != null)
				_stream.Close();
			_stream = null;
		}

		private readonly NameValueCollection _headers = new NameValueCollection();


		public override void BinaryWrite(Byte[] buffer)
		{
			BinaryWriter.Write(buffer);
		}

		public Boolean IsBinaryWrited => _stream != null;

		public Byte[] GetBytes()
		{
			if (_stream != null)
				return _stream.GetBuffer();
			return null;
		}
	}
}
