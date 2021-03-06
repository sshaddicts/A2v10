﻿// Copyright © 2015-2017 Alex Kukhtin. All rights reserved.


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Markup;

namespace A2v10.Xaml
{
	[Serializable]
	public class ResourceDictionary : Dictionary<String, Object>
	{
		public ResourceDictionary()
		{

		}

		protected ResourceDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	public abstract class RootContainer : Container, IUriContext, IDisposable
	{
		#region IUriContext
		public Uri BaseUri { get; set; }
		#endregion

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(Boolean dispoising)
		{
			if (dispoising)
				OnDispose();
		}

		#endregion
		protected ResourceDictionary _resources;

		public ResourceDictionary Resources
		{
			get
			{
				if (_resources == null)
					_resources = new ResourceDictionary();
				return _resources;
			}
			set
			{
				_resources = value;
			}
		}

		public Object FindResource(String key)
		{
			if (_resources == null)
				return null;
			if (_resources.TryGetValue(key, out Object resrc))
				return resrc;
			return null;
		}

		internal Styles Styles { get; set; }
	}
}
