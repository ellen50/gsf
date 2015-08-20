//******************************************************************************************************
//  CInputAdapter.h - Gbtc
//
//  Copyright � 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/26/2012 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::ComponentModel;
using namespace TimeSeriesFramework;
using namespace TimeSeriesFramework::Adapters;
using namespace TVA;

namespace CSampleAdapters
{
    /// <summary>
    /// Represents a C++ input adapter implementation sample.
    /// </summary>
    [Description("Represents a C++ input adapter implementation sample.")]
    public ref class CInputAdapter : public InputAdapterBase
    {
        #pragma region [ Properties ]

    public:

        /// <summary>
        /// Gets the flag indicating if this <see cref="CInputAdapter"/> supports temporal processing.
        /// </summary>
        property bool SupportsTemporalProcessing
        {
            virtual bool get() override;
        };

    protected:
   
        /// <summary>
        /// Gets flag that determines if this <see cref="CInputAdapter"/> uses an asynchronous connection.
        /// </summary>
        property bool UseAsyncConnect
        {
            virtual bool get() override;
        };

        #pragma endregion

        #pragma region [ Methods ]

    public:

        /// <summary>
        /// Initializes <see cref="CInputAdapter"/>.
        /// </summary>
        virtual void Initialize() override;

        /// <summary>
        /// Gets a short one-line status of this <see cref="CInputAdapter"/>.
        /// </summary>
        virtual String^ GetShortStatus(int maxLength) override;

    protected:

        /// <summary>
        /// Attempts to connect this <see cref="CInputAdapter"/>.
        /// </summary>
        virtual void AttemptConnection() override;

        /// <summary>
        /// Attempts to disconnect this <see cref="CInputAdapter"/>.
        /// </summary>
        virtual void AttemptDisconnection() override;

        #pragma endregion
    };
}