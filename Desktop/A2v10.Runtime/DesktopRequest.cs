﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Web;

using A2v10.Infrastructure;
using A2v10.Request;
using Newtonsoft.Json;

namespace A2v10.Runtime
{
	public class DesktopRequest
	{
		BaseController _controller = new BaseController();

		public String MimeType { get; private set; }
		public String ContentDisposition { get; private set; }

		const String MIME_JSON   = "application/json";
		const String MIME_HTML   = "text/html";
		const String MIME_SCRIPT = "text/javascript";
		const String MIME_STYLE  = "text/css";

		public Byte[] ProcessRequest(String url, String search, Byte[] post, Boolean postMethod)
		{
			_controller.Host.StartApplication(false); // TODO: ?ADMIN?
			using (_controller.Host.Profiler.BeginRequest(url, null) as IDisposable)
			{
				try
				{
					return ProcessRequestImpl(url, search, post, postMethod);
				}
				catch (Exception /*ex*/)
				{
					throw;
					//WriteExceptionStatus(ex, response);
				}
			}
		}

		Byte[] ProcessRequestImpl(String url, String search, Byte[] post, Boolean postMethod)
		{
			if (url.StartsWith("admin/"))
			{
				url = url.Substring(6);
				_controller.Admin = true;
			}
			_controller.Host.StartApplication(_controller.Admin);
			try
			{
				MimeType = MIME_HTML;
				using (var dr = new DesktopResponse())
				{
					if (String.IsNullOrEmpty(url))
						RenderIndex(dr.Output);
					else if (url.StartsWith("shell/"))
					{
						Shell(url.Substring(6).ToLowerInvariant(), dr.Output, out String shellMime);
						MimeType = shellMime;
					}
					else if (url.StartsWith("_page/"))
						Render(RequestUrlKind.Page, url.Substring(6), search, dr.Output);
					else if (url.StartsWith("_dialog/"))
						Render(RequestUrlKind.Dialog, url.Substring(8), search, dr.Output);
					else if (url.StartsWith("_popup/"))
						Render(RequestUrlKind.Popup, url.Substring(7), search, dr.Output);
					else if (url.StartsWith("_data/"))
					{
						var command = url.Substring(6);
						dr.ContentType = "application/json";
						String jsonData = Encoding.UTF8.GetString(post);
						_controller.Data(command, SetSqlParams, jsonData, dr).Wait();
						MimeType = dr.ContentType;
						if (dr.IsBinaryWrited)
							return dr.GetBytes();
					}
					else if (url.StartsWith("_image/"))
					{
						if (postMethod)
							SaveImage("/" + url, dr.Output);
						else
						{
							var bytes = LoadImage("/" + url, dr);
							MimeType = dr.ContentType;
							return bytes;
						}
					}
					else if (url.StartsWith("_static_image/"))
					{
						var bytes = StaticImage(url.Substring(14).Replace('-', '.'), dr);
						MimeType = dr.ContentType;
						return bytes;
					}
					else if (url.StartsWith("_export/"))
					{
						Export("/" + url, search, dr);
						MimeType = dr.ContentType;
						ContentDisposition = dr.Headers["Content-Disposition"];
						return dr.GetBytes();
					}
					else
						RenderIndex(dr.Output);
					return Encoding.UTF8.GetBytes(dr.Output.ToString());
				}
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
					ex = ex.InnerException;
				// TODO:: /exception
				String msg = $"<div>{ex.Message}</div>";
				return Encoding.UTF8.GetBytes(msg) ;
			}
		}

		// TODO: current user ID and tenantId;
		public Int64 UserId { get { return 50; /*TODO*/ } }
		public Int32 TenantId { get { return 1; } }

		public void SetSqlParams(ExpandoObject prms)
		{
			A2v10.Infrastructure.DynamicHelpers.Set(prms, "UserId", UserId);
			A2v10.Infrastructure.DynamicHelpers.Set(prms, "TenantId", TenantId);
		}

		void Render(RequestUrlKind kind, String path, String search, TextWriter writer)
		{
			ExpandoObject loadPrms = new ExpandoObject();
			loadPrms.Append(HttpUtility.ParseQueryString(search), toPascalCase: true);
			SetSqlParams(loadPrms);
			if (path.StartsWith("app/"))
				_controller.RenderApplicationKind(kind, path, loadPrms, writer).Wait();
			else
				_controller.RenderElementKind(kind, path, loadPrms, writer).Wait();
		}

		void RenderIndex(TextWriter writer)
		{
			// TODO: userName
			String userName = "User Name";
			String locale = "uk"; // TODO: ctrl.CurrentLang
			String theme = "site"; // TODO: ctrl.Host.Theme
			var prms = new Dictionary<String, String>
				{
					{ "$(RootUrl)", String.Empty },
					{ "$(HelpUrl)", _controller.Host.HelpUrl },
					{ "$(PersonName)", userName },
					{ "$(Theme)", theme },
					{ "$(Build)", _controller.Host.AppBuild },
					{ "$(Locale)", locale },
					{ "$(Minify)", _controller.IsDebugConfiguration ? String.Empty : "min." },
					{ "$(Description)", _controller.Host.AppDescription }
				};
			_controller.Layout(writer, prms);
		}

		public void Shell(String url, TextWriter writer, out String mimeType)
		{
			mimeType = MIME_SCRIPT;
			switch (url)
			{
				case "appstyles":
					_controller.GetAppStyleConent(writer);
					mimeType = MIME_STYLE;
					break;
				case "appscripts":
					_controller.GetAppScriptConent(writer);
					mimeType = MIME_STYLE;
					break;
				case "script":
					try
					{
						_controller.ShellScript(null, SetSqlParams, userAdmin: false, bAdmin: false, writer: writer).Wait();
						mimeType = MIME_SCRIPT;
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
							ex = ex.InnerException;
						writer.Write($"alert('{ex.Message.EncodeJs()}')");
					}
					break;
				case "trace":
					String json = _controller.Host.Profiler.GetJson();
					mimeType = MIME_JSON;
					writer.Write(json);
					break;
			}
		}


		Byte[] LoadImage(String url, HttpResponseBase response)
		{
			try
			{
				AttachmentInfo info = _controller.DownloadAttachment(url, SetSqlParams).Result;
				if (info == null)
					return null;
				response.ContentType = info.Mime;
				if (info.Stream == null)
					return null;
				return info.Stream;
			}
			catch (Exception ex)
			{
				return GetImageException(ex, response);
			}
		}

		Byte[] StaticImage(String url, HttpResponseBase response)
		{
			try
			{
				AttachmentInfo info = _controller.StaticImage(url);
				if (info == null || info.Stream == null)
					return null;
				response.ContentType = info.Mime;
				return info.Stream;
			}
			catch (Exception ex)
			{
				return GetImageException(ex, response);
			}
		}

		Byte[] GetImageException(Exception ex, HttpResponseBase response)
		{
			response.ContentType = "image/png";
			if (ex.InnerException != null)
				ex = ex.InnerException;
			var b = new Bitmap(380, 30);
			var g = Graphics.FromImage(b);
			g.FillRectangle(Brushes.LavenderBlush, new Rectangle(0, 0, 380, 30));
			g.DrawString(ex.Message, SystemFonts.SmallCaptionFont, Brushes.DarkRed, 5, 5, StringFormat.GenericTypographic);
			g.Save();
			using (var ms = new MemoryStream())
			{
				b.Save(ms, ImageFormat.Png);
				ms.Seek(0, SeekOrigin.Begin);
				return ms.GetBuffer();
			}
		}

		String SaveImage(String url, TextWriter writer)
		{
			throw new InvalidOperationException();
		}

		public Byte[] UploadFiles(String url, String files)
		{
			try
			{
				var fileColl = new SimpleHttpFileCollection(files);
				MimeType = MIME_JSON;
				var list = _controller.SaveAttachments(TenantId, "/" + url, fileColl, UserId).Result;
				var rval = new ExpandoObject();
				rval.Set("status", "OK");
				rval.Set("ids", list);
				String result = JsonConvert.SerializeObject(rval, JsonHelpers.StandardSerializerSettings);
				return Encoding.UTF8.GetBytes(result);
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
					ex = ex.InnerException;
				// TODO:: /exception
				String msg = $"<div>{ex.Message}</div>";
				return Encoding.UTF8.GetBytes(msg);
			}
		}

		void WriteExceptionStatus(Exception ex, HttpResponseBase response)
		{
			response.ContentType = "clr/error+server";
			if (ex.InnerException != null)
				ex = ex.InnerException;
			response.Write(Encoding.UTF8.GetBytes(ex.Message));
		}

		void Export(String path, String search, HttpResponseBase response)
		{
			// HTTP GET
			try
			{
				ExpandoObject loadPrms = new ExpandoObject();
				loadPrms.Append(_controller.CheckPeriod(HttpUtility.ParseQueryString(search)), toPascalCase: true);
				SetSqlParams(loadPrms);
				_controller.Export(path, TenantId, UserId, loadPrms, response).Wait();
			}
			catch (Exception ex)
			{
				WriteExceptionStatus(ex, response);
			}
		}

	}
}
