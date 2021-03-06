﻿//******************************************************************************************************
//  WebServer.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/10/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.IO;
using GSF.IO.Checksums;
using GSF.Reflection;
using GSF.Web.Model;

namespace GSF.Web.Hosting
{
    /// <summary>
    /// Represents the web server engine for <see cref="WebPageController"/> instances.
    /// </summary>
    public class WebServer
    {
        #region [ Members ]

        // Events

        /// <summary>
        /// Exposes execution exceptions that occur in <see cref="WebPageController"/> instances.
        /// </summary>
        public event EventHandler<EventArgs<Exception>> ExecutionException;

        /// <summary>
        /// Exposes status messages that get reported from <see cref="WebPageController"/> instances.
        /// </summary>
        public event EventHandler<EventArgs<string>> StatusMessage;

        // Fields
        private readonly string m_webRootPath;
        private readonly IRazorEngine m_razorEngineCS;
        private readonly IRazorEngine m_razorEngineVB;
        private readonly ConcurrentDictionary<string, uint> m_etagCache;
        private readonly ConcurrentDictionary<string, Tuple<Type, Type>> m_pagedViewModelTypes;
        private readonly SafeFileWatcher m_fileWatcher;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="WebServer"/>.
        /// </summary>
        /// <param name="webRootPath">Root path for web server; defaults to template path for <paramref name="razorEngineCS"/>.</param>
        /// <param name="razorEngineCS">Razor engine instance for .cshtml templates; uses default instance if not provided.</param>
        /// <param name="razorEngineVB">Razor engine instance for .vbhtml templates; uses default instance if not provided.</param>
        public WebServer(string webRootPath = null, IRazorEngine razorEngineCS = null, IRazorEngine razorEngineVB = null)
        {
            bool releaseMode = !AssemblyInfo.EntryAssembly.Debuggable;
            m_razorEngineCS = razorEngineCS ?? (releaseMode ? RazorEngine<CSharp>.Default : RazorEngine<CSharpDebug>.Default as IRazorEngine);
            m_razorEngineVB = razorEngineVB ?? (releaseMode ? RazorEngine<VisualBasic>.Default : RazorEngine<VisualBasicDebug>.Default as IRazorEngine);
            m_webRootPath = FilePath.AddPathSuffix(webRootPath ?? m_razorEngineCS.TemplatePath);
            m_etagCache = new ConcurrentDictionary<string, uint>(StringComparer.InvariantCultureIgnoreCase);
            m_pagedViewModelTypes = new ConcurrentDictionary<string, Tuple<Type, Type>>(StringComparer.InvariantCultureIgnoreCase);

            m_fileWatcher = new SafeFileWatcher(m_webRootPath)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            m_fileWatcher.Changed += m_fileWatcher_FileChange;
            m_fileWatcher.Deleted += m_fileWatcher_FileChange;
            m_fileWatcher.Renamed += m_fileWatcher_FileChange;

            ClientCacheEnabled = true;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets flag that determines if cache control is enabled for browser clients; default to <c>true</c>.
        /// </summary>
        public bool ClientCacheEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets root path defined for this <see cref="WebServer"/>.
        /// </summary>
        public string WebRootPath => m_webRootPath;

        /// <summary>
        /// Gets the C# Razor engine instance used by this <see cref="WebServer"/>.
        /// </summary>
        public IRazorEngine RazorEngineCS => m_razorEngineCS;

        /// <summary>
        /// Gets the Visual Basic Razor engine instance used by this <see cref="WebServer"/>.
        /// </summary>
        public IRazorEngine RazorEngineVB => m_razorEngineVB;

        /// <summary>
        /// Defines associated page view model types and data hub types for Razor pages, if any.
        /// </summary>
        /// <remarks>
        /// This dictionary associates Razor views based on a paged view model with associated <see cref="Type"/> values.
        /// </remarks>
        public ConcurrentDictionary<string, Tuple<Type, Type>> PagedViewModelTypes => m_pagedViewModelTypes;

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="WebServer"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="WebServer"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                        m_fileWatcher?.Dispose();
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Renders an HTTP response for a given request.
        /// </summary>
        /// <param name="request">HTTP request instance.</param>
        /// <param name="pageName">Name of page to render.</param>
        /// <param name="model">Reference to model to use when rendering Razor templates, if any.</param>
        /// <param name="modelType">Type of <paramref name="model"/>, if any.</param>
        /// <param name="database"><see cref="AdoDataConnection"/> to use, if any.</param>
        /// <param name="postData">Any posted data if request is a POST type.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RenderResponse(HttpRequestMessage request, string pageName, object model = null, Type modelType = null, AdoDataConnection database = null, dynamic postData = null)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            string content, fileExtension = FilePath.GetExtension(pageName).ToLowerInvariant();
            Tuple<Type, Type> pagedViewModelTypes;

            switch (fileExtension)
            {
                case ".cshtml":
                    m_pagedViewModelTypes.TryGetValue(pageName, out pagedViewModelTypes);
                    content = await new RazorView(m_razorEngineCS, pageName, model, modelType, pagedViewModelTypes?.Item1, pagedViewModelTypes?.Item2, database, OnExecutionException).ExecuteAsync(request, postData);
                    response.Content = new StringContent(content, Encoding.UTF8, "text/html");
                    break;
                case ".vbhtml":
                    m_pagedViewModelTypes.TryGetValue(pageName, out pagedViewModelTypes);
                    content = await new RazorView(m_razorEngineVB, pageName, model, modelType, pagedViewModelTypes?.Item1, pagedViewModelTypes?.Item2, database, OnExecutionException).ExecuteAsync(request, postData);
                    response.Content = new StringContent(content, Encoding.UTF8, "text/html");
                    break;
                default:
                    string fileName = FilePath.GetAbsolutePath($"{m_webRootPath}{pageName.Replace('/', Path.DirectorySeparatorChar)}");

                    if (File.Exists(fileName))
                    {
                        FileStream fileData = null;
                        uint responseHash = 0;

                        if (ClientCacheEnabled && !m_etagCache.TryGetValue(fileName, out responseHash))
                        {
                            // Calculate check-sum for file
                            await Task.Run(() =>
                            {
                                const int BufferSize = 32768;
                                byte[] buffer = new byte[BufferSize];
                                Crc32 calculatedHash = new Crc32();

                                fileData = File.OpenRead(fileName);
                                int bytesRead = fileData.Read(buffer, 0, BufferSize);

                                while (bytesRead > 0)
                                {
                                    calculatedHash.Update(buffer, 0, bytesRead);
                                    bytesRead = fileData.Read(buffer, 0, BufferSize);
                                }

                                responseHash = calculatedHash.Value;
                                m_etagCache.TryAdd(fileName, responseHash);
                                fileData.Seek(0, SeekOrigin.Begin);

                                OnStatusMessage($"Cache [{responseHash}] added for file \"{fileName}\"");
                            });
                        }

                        if (PublishResponseContent(request, response, responseHash))
                        {
                            if (fileData == null)
                                fileData = File.OpenRead(fileName);

                            response.Content = await Task.Run(() => new StreamContent(fileData));
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(pageName));
                        }
                        else
                        {
                            fileData?.Dispose();
                        }
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    break;
            }

            return response;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool PublishResponseContent(HttpRequestMessage request, HttpResponseMessage response, long responseHash)
        {
            if (!ClientCacheEnabled)
                return true;

            // See if client's version of cached resource is up to date
            foreach (EntityTagHeaderValue headerValue in request.Headers.IfNoneMatch)
            {
                long requestHash;

                if (long.TryParse(headerValue.Tag?.Substring(1, headerValue.Tag.Length - 2), out requestHash) && responseHash == requestHash)
                {
                    response.StatusCode = HttpStatusCode.NotModified;
                    return false;
                }
            }

            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = new TimeSpan(31536000 * TimeSpan.TicksPerSecond)
            };

            response.Headers.ETag = new EntityTagHeaderValue($"\"{responseHash}\"");
            return true;
        }

        private void OnExecutionException(Exception exception)
        {
            ExecutionException?.Invoke(this, new EventArgs<Exception>(exception));
        }

        private void OnStatusMessage(string message)
        {
            StatusMessage?.Invoke(this, new EventArgs<string>(message));
        }

        private void m_fileWatcher_FileChange(object sender, FileSystemEventArgs e)
        {
            uint responseHash;

            if (m_etagCache.TryRemove(e.FullPath, out responseHash))
                OnStatusMessage($"Cache [{responseHash}] cleared for file \"{e.FullPath}\"");
        }

        #endregion

        #region [ Static ]

        // Static Fields
        private static WebServer s_defaultServer;
        private static readonly Dictionary<string, WebServer> s_configuredServers;

        // Static Properties

        /// <summary>
        /// Gets default configured web server instance.
        /// </summary>
        public static WebServer Default => s_defaultServer ?? (s_defaultServer = GetConfiguredServer());

        // Static Constructor
        static WebServer()
        {
            s_configuredServers = new Dictionary<string, WebServer>(StringComparer.OrdinalIgnoreCase);
        }

        // Static Methods

        /// <summary>
        /// Gets a shared <see cref="WebServer"/> instance created based on configured options defined in the specified <paramref name="settingsCategory"/>.
        /// </summary>
        /// <param name="settingsCategory">Settings category to use for web server options; defaults to "systemSettings".</param>
        /// <param name="razorEngineCS">Razor engine instance for .cshtml templates; uses default instance if not provided.</param>
        /// <param name="razorEngineVB">Razor engine instance for .vbhtml templates; uses default instance if not provided.</param>
        /// <returns>Shared <see cref="WebServer"/> instance created based on configured options.</returns>
        public static WebServer GetConfiguredServer(string settingsCategory = null, IRazorEngine razorEngineCS = null, IRazorEngine razorEngineVB = null)
        {
            lock (typeof(WebServer))
            {
                if (string.IsNullOrWhiteSpace(settingsCategory))
                    settingsCategory = "systemSettings";

                return s_configuredServers.GetOrAdd(settingsCategory, category =>
                {
                    // Get configured template path
                    CategorizedSettingsElementCollection settings = ConfigurationFile.Current.Settings[category];
                    settings.Add("WebRootPath", "wwwroot", "The root path for the hosted web server files. Location will be relative to install folder if full path is not specified.");
                    settings.Add("ClientCacheEnabled", "true", "Determines if cache control is enabled for web server when rendering content to browser clients.");

                    return new WebServer(FilePath.GetAbsolutePath(settings["WebRootPath"].Value), razorEngineCS, razorEngineVB)
                    {
                        ClientCacheEnabled = settings["ClientCacheEnabled"].Value.ParseBoolean()
                    };
                });
            }
        }

        #endregion
    }
}
