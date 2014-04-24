﻿//----------------------------------------------------------------------
// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
// Apache License 2.0
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//----------------------------------------------------------------------

using System;
using System.Net;

namespace Microsoft.IdentityModel.Clients.ActiveDirectory
{
#if ADAL_WINRT
    class ActiveDirectoryAuthenticationException : Exception
#else
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception type thrown when an error occurs during token acquisition.
    /// </summary>
    [Serializable]
    public class ActiveDirectoryAuthenticationException : Exception
#endif
    {
        /// <summary>
        ///  Initializes a new instance of the exception class.
        /// </summary>
        public ActiveDirectoryAuthenticationException()
            : base(ActiveDirectoryAuthenticationErrorMessage.Unknown)
        {
            this.ErrorCode = ActiveDirectoryAuthenticationError.Unknown;
        }

        /// <summary>
        ///  Initializes a new instance of the exception class with a specified
        ///  error code.
        /// </summary>
        /// <param name="errorCode">The error code returned by the service or generated by client. This is the code you can rely on for exception handling.</param>
        public ActiveDirectoryAuthenticationException(string errorCode)
            : base(ExceptionHelper.GetErrorMessage(errorCode))
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///  Initializes a new instance of the exception class with a specified
        ///  error code and error message.
        /// </summary>
        /// <param name="errorCode">The error code returned by the service or generated by client. This is the code you can rely on for exception handling.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ActiveDirectoryAuthenticationException(string errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///  Initializes a new instance of the exception class with a specified
        ///  error code and a reference to the inner exception that is the cause of
        ///  this exception.
        /// </summary>
        /// <param name="errorCode">The error code returned by the service or generated by client. This is the code you can rely on for exception handling.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. It may especially contain the actual error message returned by the service.</param>
        public ActiveDirectoryAuthenticationException(string errorCode, Exception innerException)
            : base(ExceptionHelper.GetErrorMessage(errorCode), innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        ///  Initializes a new instance of the exception class with a specified
        ///  error code, error message and a reference to the inner exception that is the cause of
        ///  this exception.
        /// </summary>
        /// <param name="errorCode">The error code returned by the service or generated by client. This is the code you can rely on for exception handling.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. It may especially contain the actual error message returned by the service.</param>
        public ActiveDirectoryAuthenticationException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
            WebException exception = innerException as WebException;
            if (exception != null)
            {
                var response = (HttpWebResponse)(exception.Response);
                if (response != null)
                {
                    this.InnerStatusCode = (int)response.StatusCode;
                }
            }
        }

        /// <summary>
        /// Gets the error code returned by the service or generated by client. This is the code you can rely on for exception handling.
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        /// Gets the status code returned from http layer. This status code is either the HttpStatusCode in the inner WebException response or
        /// NavigateError Event Status Code in browser based flow (See http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int InnerStatusCode { get; set; }

#if ADAL_WINRT
#else
        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected ActiveDirectoryAuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ErrorCode = info.GetString("ErrorCode");
            this.InnerStatusCode = info.GetInt32("InnerStatusCode");
        }

        /// <summary>
        /// Sets the System.Runtime.Serialization.SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("ErrorCode", this.ErrorCode);
            info.AddValue("InnerStatusCode", this.InnerStatusCode);

            base.GetObjectData(info, context);
        }
#endif
    }
}
