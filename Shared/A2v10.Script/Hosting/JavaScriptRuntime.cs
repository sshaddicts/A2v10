﻿namespace ChakraHost.Hosting
{
	using A2v10.Script;
	using System;
	using System.Threading;

	/// <summary>
	///     A Chakra runtime.
	/// </summary>
	/// <remarks>
	///     <para>
	///     Each Chakra runtime has its own independent execution engine, JIT compiler, and garbage 
	///     collected heap. As such, each runtime is completely isolated from other runtimes.
	///     </para>
	///     <para>
	///     Runtimes can be used on any thread, but only one thread can call into a runtime at any 
	///     time.
	///     </para>
	///     <para>
	///     NOTE: A JavaScriptRuntime, unlike other objects in the Chakra hosting API, is not 
	///     garbage collected since it contains the garbage collected heap itself. A runtime will 
	///     continue to exist until Dispose is called.
	///     </para>
	/// </remarks>
	public partial struct JavaScriptRuntime : IDisposable
	{
		/// <summary>
		/// The handle.
		/// </summary>
		private IntPtr handle;

		/// <summary>
		///     Gets a value indicating whether the runtime is valid.
		/// </summary>
		public Boolean IsValid
		{
			get { return handle != IntPtr.Zero; }
		}

		/// <summary>
		///     Gets the current memory usage for a runtime.
		/// </summary>
		/// <remarks>
		///     Memory usage can be always be retrieved, regardless of whether or not the runtime is active
		///     on another thread.
		/// </remarks>
		public UIntPtr MemoryUsage
		{
			get
			{
				NativeMethods.ThrowIfError(NativeMethods.JsGetRuntimeMemoryUsage(this, out UIntPtr memoryUsage));
				return memoryUsage;
			}
		}

		/// <summary>
		///     Gets or sets the current memory limit for a runtime.
		/// </summary>
		/// <remarks>
		///     The memory limit of a runtime can be always be retrieved, regardless of whether or not the 
		///     runtime is active on another thread.
		/// </remarks>
		public UIntPtr MemoryLimit
		{
			get
			{
				NativeMethods.ThrowIfError(NativeMethods.JsGetRuntimeMemoryLimit(this, out UIntPtr memoryLimit));
				return memoryLimit;
			}

			set
			{
				NativeMethods.ThrowIfError(NativeMethods.JsSetRuntimeMemoryLimit(this, value));
			}
		}

		/// <summary>
		///     Gets or sets a value indicating whether script execution is disabled in the runtime.
		/// </summary>
		public Boolean Disabled
		{
			get
			{
				NativeMethods.ThrowIfError(NativeMethods.JsIsRuntimeExecutionDisabled(this, out Boolean isDisabled));
				return isDisabled;
			}

			set
			{
				NativeMethods.ThrowIfError(value
										? NativeMethods.JsDisableRuntimeExecution(this)
										: NativeMethods.JsEnableRuntimeExecution(this));
			}
		}

		/// <summary>
		///     Creates a new runtime.
		/// </summary>
		/// <param name="attributes">The attributes of the runtime to be created.</param>
		/// <param name="version">The version of the runtime to be created.</param>
		/// <param name="threadServiceCallback">The thread service for the runtime. Can be null.</param>
		/// <returns>The runtime created.</returns>
		public static JavaScriptRuntime Create(JavaScriptRuntimeAttributes attributes, JavaScriptRuntimeVersion version, JavaScriptThreadServiceCallback threadServiceCallback)
		{
			NativeMethods.ThrowIfError(NativeMethods.JsCreateRuntime(attributes, threadServiceCallback, out JavaScriptRuntime handle));
			return handle;
		}

		/// <summary>
		///     Creates a new runtime.
		/// </summary>
		/// <param name="attributes">The attributes of the runtime to be created.</param>
		/// <param name="version">The version of the runtime to be created.</param>
		/// <returns>The runtime created.</returns>
		public static JavaScriptRuntime Create(JavaScriptRuntimeAttributes attributes, JavaScriptRuntimeVersion version)
		{
			return Create(attributes, version, null);
		}

		/// <summary>
		///     Creates a new runtime.
		/// </summary>
		/// <returns>The runtime created.</returns>
		public static JavaScriptRuntime Create()
		{
			return Create(JavaScriptRuntimeAttributes.None, JavaScriptRuntimeVersion.VersionEdge, null);
		}

		/// <summary>
		///     Disposes a runtime.
		/// </summary>
		/// <remarks>
		///     Once a runtime has been disposed, all resources owned by it are invalid and cannot be used.
		///     If the runtime is active (i.e. it is set to be current on a particular thread), it cannot 
		///     be disposed.
		/// </remarks>
		public void Dispose()
		{
			if (IsValid)
			{
				NativeMethods.ThrowIfError(NativeMethods.JsDisposeRuntime(this));
			}

			handle = IntPtr.Zero;
		}

		/// <summary>
		///     Performs a full garbage collection.
		/// </summary>
		public void CollectGarbage()
		{
			NativeMethods.ThrowIfError(NativeMethods.JsCollectGarbage(this));
		}

		/// <summary>
		///     Sets a memory allocation callback for specified runtime
		/// </summary>
		/// <remarks>
		///     <para>
		///     Registering a memory allocation callback will cause the runtime to call back to the host 
		///     whenever it acquires memory from, or releases memory to, the OS. The callback routine is
		///     called before the runtime memory manager allocates a block of memory. The allocation will
		///     be rejected if the callback returns false. The runtime memory manager will also invoke the
		///     callback routine after freeing a block of memory, as well as after allocation failures. 
		///     </para>
		///     <para>
		///     The callback is invoked on the current runtime execution thread, therefore execution is 
		///     blocked until the callback completes.
		///     </para>
		///     <para>
		///     The return value of the callback is not stored; previously rejected allocations will not
		///     prevent the runtime from invoking the callback again later for new memory allocations.
		///     </para>
		/// </remarks>
		/// <param name="callbackState">
		///     User provided state that will be passed back to the callback.
		/// </param>
		/// <param name="allocationCallback">
		///     Memory allocation callback to be called for memory allocation events.
		/// </param>
		public void SetMemoryAllocationCallback(IntPtr callbackState, JavaScriptMemoryAllocationCallback allocationCallback)
		{
			NativeMethods.ThrowIfError(NativeMethods.JsSetRuntimeMemoryAllocationCallback(this, callbackState, allocationCallback));
		}

		/// <summary>
		///     Sets a callback function that is called by the runtime before garbage collection.
		/// </summary>
		/// <remarks>
		///     <para>
		///     The callback is invoked on the current runtime execution thread, therefore execution is 
		///     blocked until the callback completes.
		///     </para>
		///     <para>
		///     The callback can be used by hosts to prepare for garbage collection. For example, by 
		///     releasing unnecessary references on Chakra objects.
		///     </para>
		/// </remarks>
		/// <param name="callbackState">
		///     User provided state that will be passed back to the callback.
		/// </param>
		/// <param name="beforeCollectCallback">The callback function being set.</param>
		public void SetBeforeCollectCallback(IntPtr callbackState, JavaScriptBeforeCollectCallback beforeCollectCallback)
		{
			NativeMethods.ThrowIfError(NativeMethods.JsSetRuntimeBeforeCollectCallback(this, callbackState, beforeCollectCallback));
		}

		/// <summary>
		///     Creates a script context for running scripts.
		/// </summary>
		/// <remarks>
		///     Each script context has its own global object that is isolated from all other script 
		///     contexts.
		/// </remarks>
		/// <returns>The created script context.</returns>
		public JavaScriptContext CreateContext()
		{
			NativeMethods.ThrowIfError(NativeMethods.JsCreateContext(this, out JavaScriptContext reference));
			return reference;
		}
	}
}
