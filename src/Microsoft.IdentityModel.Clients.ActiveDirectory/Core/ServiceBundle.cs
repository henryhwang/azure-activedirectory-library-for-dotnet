﻿// ------------------------------------------------------------------------------
// 
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// 
// This code is licensed under the MIT License.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ------------------------------------------------------------------------------

using System;
using System.Net.Http;
using Microsoft.Identity.Core.Http;
using Microsoft.Identity.Core.WsTrust;

namespace Microsoft.Identity.Core
{
    internal class ServiceBundle : IServiceBundle
    {
        internal ServiceBundle(
            IHttpClientFactory httpClientFactory = null,
            IHttpManager httpManager = null,
            IWsTrustWebRequestManager wsTrustWebRequestManager = null,
            bool shouldClearCaches = false)
        {
            HttpManager = httpManager ?? new HttpManager(httpClientFactory);
            WsTrustWebRequestManager = wsTrustWebRequestManager ?? new WsTrustWebRequestManager(HttpManager);
            PlatformProxy = PlatformProxyFactory.GetPlatformProxy();
        }

        /// <inheritdoc />
        public IHttpManager HttpManager { get; }

        /// <inheritdoc />
        public IWsTrustWebRequestManager WsTrustWebRequestManager { get; }

        /// <inheritdoc />
        public IPlatformProxy PlatformProxy { get; }

        public static ServiceBundle CreateWithCustomHttpManager(IHttpManager httpManager)
        {
            return new ServiceBundle(httpManager: httpManager, shouldClearCaches: true);
        }

        internal static IServiceBundle CreateWithHttpClient(HttpClient httpClient)
        {
            if (httpClient != null)
            {
                return new ServiceBundle(new HttpClientFactory(httpClient));
            }
            else
            {
                return new ServiceBundle();
            }
        }
    }
}