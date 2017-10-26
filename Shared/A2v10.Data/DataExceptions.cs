﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2v10.Data
{
	public class DataLoaderException : Exception
	{
		public DataLoaderException(String message)
			:base(message)
		{
		}
	}

	public class DataWriterException : Exception
	{
		public DataWriterException(String message)
			:base(message)
		{
		}
	}

    public class DataDynamicException: Exception
    {
        public DataDynamicException(String message)
			:base(message)
		{
        }
    }
}
