namespace ChakraHost.Hosting
{
    using System;
    using System.Runtime.Serialization;

#pragma warning disable IDE0049 // Use framework type
	/// <summary>
	///     An exception returned from the Chakra engine.
	/// </summary>
	[Serializable]
    public class JavaScriptException : Exception
    {
        /// <summary>
        /// The error code.
        /// </summary>
        private readonly JavaScriptErrorCode code;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        public JavaScriptException(JavaScriptErrorCode code) :
            this(code, "A fatal exception has occurred in a JavaScript runtime")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        /// <param name="message">The error message.</param>
        public JavaScriptException(JavaScriptErrorCode code, string message) :
            base(message)
        {
            this.code = code;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptException"/> class. 
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected JavaScriptException(string message, Exception innerException) :
            base(message, innerException)
        {
            if (message != null)
            {
                code = (JavaScriptErrorCode) base.HResult;
            }
        }

        /*
        /// <summary>
        ///     Serializes the exception information.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("code", (uint)code);
        }
        */
        /// <summary>
        ///     Gets the error code.
        /// </summary>
        public JavaScriptErrorCode ErrorCode
        {
            get { return code; }
        }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
#pragma warning restore IDE0049
}