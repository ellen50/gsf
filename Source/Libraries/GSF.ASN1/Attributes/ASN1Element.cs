//******************************************************************************************************
//  ASN1Element.cs - Gbtc
//
//  Copyright � 2013, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  09/24/2013 - J. Ritchie Carroll
//       Derived original version of source code from BinaryNotes (http://bnotes.sourceforge.net).
//
//******************************************************************************************************

#region [ Contributor License Agreements ]

/*
    Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
    Original sources are available at www.latestbit.com

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

            http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

#endregion

using System;
using GSF.ASN1.Coders;

namespace GSF.ASN1.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ASN1Element : Attribute
    {
        private bool hasDefaultValue;
        private bool hasTag;
        private bool isImplicitTag = true;
        private bool isOptional = true;
        private string name = "";
        private int tag;
        private int tagClass = TagClasses.ContextSpecific;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public bool IsOptional
        {
            get
            {
                return isOptional;
            }
            set
            {
                isOptional = value;
            }
        }

        public bool HasTag
        {
            get
            {
                return hasTag;
            }
            set
            {
                hasTag = value;
            }
        }

        public bool IsImplicitTag
        {
            get
            {
                return isImplicitTag;
            }
            set
            {
                isImplicitTag = value;
            }
        }

        public int TagClass
        {
            get
            {
                return tagClass;
            }
            set
            {
                tagClass = value;
            }
        }

        public int Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public bool HasDefaultValue
        {
            get
            {
                return hasDefaultValue;
            }
            set
            {
                hasDefaultValue = value;
            }
        }
    }
}