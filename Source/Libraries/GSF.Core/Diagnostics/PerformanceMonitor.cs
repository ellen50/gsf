//*******************************************************************************************************
//  PerformanceMonitor.cs - Gbtc
//
//  Tennessee Valley Authority, 2009
//  No copyright is claimed pursuant to 17 USC � 105.  All Other Rights Reserved.
//
//  This software is made freely available under the TVA Open Source Agreement (see below).
//  Code in this file licensed to GSF under one or more contributor license agreements listed below.
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  06/01/2007 - Pinal C. Patel
//       Generated original version of source code.
//  08/08/2007 - J. Ritchie Carroll
//       Added lock contention rate and datagram / sec performance counters.
//  09/04/2007 - Pinal C. Patel
//       Added Status property.
//  09/22/2008 - J. Ritchie Carroll
//       Converted to C#.
//  10/01/2008 - Pinal C. Patel
//       Entered code comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  06/21/2010 - Stephen C. Wills
//       Fixed issue with monitor not disposing of counters properly.
//  01/03/2011 - J. Ritchie Carroll
//       Added counters for CLR memory consumption, IPv6 incoming / outgoing rates and lifetime status.
//  01/04/2011 - J. Ritchie Carroll
//       Made addition of default counters optional in case user wants a custom monitor.
//       Added new and reorganized default counters.
//
//*******************************************************************************************************

#region [ TVA Open Source Agreement ]
/*

 THIS OPEN SOURCE AGREEMENT ("AGREEMENT") DEFINES THE RIGHTS OF USE,REPRODUCTION, DISTRIBUTION,
 MODIFICATION AND REDISTRIBUTION OF CERTAIN COMPUTER SOFTWARE ORIGINALLY RELEASED BY THE
 TENNESSEE VALLEY AUTHORITY, A CORPORATE AGENCY AND INSTRUMENTALITY OF THE UNITED STATES GOVERNMENT
 ("GOVERNMENT AGENCY"). GOVERNMENT AGENCY IS AN INTENDED THIRD-PARTY BENEFICIARY OF ALL SUBSEQUENT
 DISTRIBUTIONS OR REDISTRIBUTIONS OF THE SUBJECT SOFTWARE. ANYONE WHO USES, REPRODUCES, DISTRIBUTES,
 MODIFIES OR REDISTRIBUTES THE SUBJECT SOFTWARE, AS DEFINED HEREIN, OR ANY PART THEREOF, IS, BY THAT
 ACTION, ACCEPTING IN FULL THE RESPONSIBILITIES AND OBLIGATIONS CONTAINED IN THIS AGREEMENT.

 Original Software Designation: openPDC
 Original Software Title: The GSF Open Source Phasor Data Concentrator
 User Registration Requested. Please Visit https://naspi.tva.com/Registration/
 Point of Contact for Original Software: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>

 1. DEFINITIONS

 A. "Contributor" means Government Agency, as the developer of the Original Software, and any entity
 that makes a Modification.

 B. "Covered Patents" mean patent claims licensable by a Contributor that are necessarily infringed by
 the use or sale of its Modification alone or when combined with the Subject Software.

 C. "Display" means the showing of a copy of the Subject Software, either directly or by means of an
 image, or any other device.

 D. "Distribution" means conveyance or transfer of the Subject Software, regardless of means, to
 another.

 E. "Larger Work" means computer software that combines Subject Software, or portions thereof, with
 software separate from the Subject Software that is not governed by the terms of this Agreement.

 F. "Modification" means any alteration of, including addition to or deletion from, the substance or
 structure of either the Original Software or Subject Software, and includes derivative works, as that
 term is defined in the Copyright Statute, 17 USC � 101. However, the act of including Subject Software
 as part of a Larger Work does not in and of itself constitute a Modification.

 G. "Original Software" means the computer software first released under this Agreement by Government
 Agency entitled openPDC, including source code, object code and accompanying documentation, if any.

 H. "Recipient" means anyone who acquires the Subject Software under this Agreement, including all
 Contributors.

 I. "Redistribution" means Distribution of the Subject Software after a Modification has been made.

 J. "Reproduction" means the making of a counterpart, image or copy of the Subject Software.

 K. "Sale" means the exchange of the Subject Software for money or equivalent value.

 L. "Subject Software" means the Original Software, Modifications, or any respective parts thereof.

 M. "Use" means the application or employment of the Subject Software for any purpose.

 2. GRANT OF RIGHTS

 A. Under Non-Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor,
 with respect to its own contribution to the Subject Software, hereby grants to each Recipient a
 non-exclusive, world-wide, royalty-free license to engage in the following activities pertaining to
 the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Modification

 5. Redistribution

 6. Display

 B. Under Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor, with
 respect to its own contribution to the Subject Software, hereby grants to each Recipient under Covered
 Patents a non-exclusive, world-wide, royalty-free license to engage in the following activities
 pertaining to the Subject Software:

 1. Use

 2. Distribution

 3. Reproduction

 4. Sale

 5. Offer for Sale

 C. The rights granted under Paragraph B. also apply to the combination of a Contributor's Modification
 and the Subject Software if, at the time the Modification is added by the Contributor, the addition of
 such Modification causes the combination to be covered by the Covered Patents. It does not apply to
 any other combinations that include a Modification. 

 D. The rights granted in Paragraphs A. and B. allow the Recipient to sublicense those same rights.
 Such sublicense must be under the same terms and conditions of this Agreement.

 3. OBLIGATIONS OF RECIPIENT

 A. Distribution or Redistribution of the Subject Software must be made under this Agreement except for
 additions covered under paragraph 3H. 

 1. Whenever a Recipient distributes or redistributes the Subject Software, a copy of this Agreement
 must be included with each copy of the Subject Software; and

 2. If Recipient distributes or redistributes the Subject Software in any form other than source code,
 Recipient must also make the source code freely available, and must provide with each copy of the
 Subject Software information on how to obtain the source code in a reasonable manner on or through a
 medium customarily used for software exchange.

 B. Each Recipient must ensure that the following copyright notice appears prominently in the Subject
 Software:

          No copyright is claimed pursuant to 17 USC � 105.  All Other Rights Reserved.

 C. Each Contributor must characterize its alteration of the Subject Software as a Modification and
 must identify itself as the originator of its Modification in a manner that reasonably allows
 subsequent Recipients to identify the originator of the Modification. In fulfillment of these
 requirements, Contributor must include a file (e.g., a change log file) that describes the alterations
 made and the date of the alterations, identifies Contributor as originator of the alterations, and
 consents to characterization of the alterations as a Modification, for example, by including a
 statement that the Modification is derived, directly or indirectly, from Original Software provided by
 Government Agency. Once consent is granted, it may not thereafter be revoked.

 D. A Contributor may add its own copyright notice to the Subject Software. Once a copyright notice has
 been added to the Subject Software, a Recipient may not remove it without the express permission of
 the Contributor who added the notice.

 E. A Recipient may not make any representation in the Subject Software or in any promotional,
 advertising or other material that may be construed as an endorsement by Government Agency or by any
 prior Recipient of any product or service provided by Recipient, or that may seek to obtain commercial
 advantage by the fact of Government Agency's or a prior Recipient's participation in this Agreement.

 F. In an effort to track usage and maintain accurate records of the Subject Software, each Recipient,
 upon receipt of the Subject Software, is requested to register with Government Agency by visiting the
 following website: https://naspi.tva.com/Registration/. Recipient's name and personal information
 shall be used for statistical purposes only. Once a Recipient makes a Modification available, it is
 requested that the Recipient inform Government Agency at the web site provided above how to access the
 Modification.

 G. Each Contributor represents that that its Modification does not violate any existing agreements,
 regulations, statutes or rules, and further that Contributor has sufficient rights to grant the rights
 conveyed by this Agreement.

 H. A Recipient may choose to offer, and to charge a fee for, warranty, support, indemnity and/or
 liability obligations to one or more other Recipients of the Subject Software. A Recipient may do so,
 however, only on its own behalf and not on behalf of Government Agency or any other Recipient. Such a
 Recipient must make it absolutely clear that any such warranty, support, indemnity and/or liability
 obligation is offered by that Recipient alone. Further, such Recipient agrees to indemnify Government
 Agency and every other Recipient for any liability incurred by them as a result of warranty, support,
 indemnity and/or liability offered by such Recipient.

 I. A Recipient may create a Larger Work by combining Subject Software with separate software not
 governed by the terms of this agreement and distribute the Larger Work as a single product. In such
 case, the Recipient must make sure Subject Software, or portions thereof, included in the Larger Work
 is subject to this Agreement.

 J. Notwithstanding any provisions contained herein, Recipient is hereby put on notice that export of
 any goods or technical data from the United States may require some form of export license from the
 U.S. Government. Failure to obtain necessary export licenses may result in criminal liability under
 U.S. laws. Government Agency neither represents that a license shall not be required nor that, if
 required, it shall be issued. Nothing granted herein provides any such export license.

 4. DISCLAIMER OF WARRANTIES AND LIABILITIES; WAIVER AND INDEMNIFICATION

 A. No Warranty: THE SUBJECT SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTY OF ANY KIND, EITHER
 EXPRESSED, IMPLIED, OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, ANY WARRANTY THAT THE SUBJECT
 SOFTWARE WILL CONFORM TO SPECIFICATIONS, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 PARTICULAR PURPOSE, OR FREEDOM FROM INFRINGEMENT, ANY WARRANTY THAT THE SUBJECT SOFTWARE WILL BE ERROR
 FREE, OR ANY WARRANTY THAT DOCUMENTATION, IF PROVIDED, WILL CONFORM TO THE SUBJECT SOFTWARE. THIS
 AGREEMENT DOES NOT, IN ANY MANNER, CONSTITUTE AN ENDORSEMENT BY GOVERNMENT AGENCY OR ANY PRIOR
 RECIPIENT OF ANY RESULTS, RESULTING DESIGNS, HARDWARE, SOFTWARE PRODUCTS OR ANY OTHER APPLICATIONS
 RESULTING FROM USE OF THE SUBJECT SOFTWARE. FURTHER, GOVERNMENT AGENCY DISCLAIMS ALL WARRANTIES AND
 LIABILITIES REGARDING THIRD-PARTY SOFTWARE, IF PRESENT IN THE ORIGINAL SOFTWARE, AND DISTRIBUTES IT
 "AS IS."

 B. Waiver and Indemnity: RECIPIENT AGREES TO WAIVE ANY AND ALL CLAIMS AGAINST GOVERNMENT AGENCY, ITS
 AGENTS, EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT. IF RECIPIENT'S USE
 OF THE SUBJECT SOFTWARE RESULTS IN ANY LIABILITIES, DEMANDS, DAMAGES, EXPENSES OR LOSSES ARISING FROM
 SUCH USE, INCLUDING ANY DAMAGES FROM PRODUCTS BASED ON, OR RESULTING FROM, RECIPIENT'S USE OF THE
 SUBJECT SOFTWARE, RECIPIENT SHALL INDEMNIFY AND HOLD HARMLESS  GOVERNMENT AGENCY, ITS AGENTS,
 EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT, TO THE EXTENT PERMITTED BY
 LAW.  THE FOREGOING RELEASE AND INDEMNIFICATION SHALL APPLY EVEN IF THE LIABILITIES, DEMANDS, DAMAGES,
 EXPENSES OR LOSSES ARE CAUSED, OCCASIONED, OR CONTRIBUTED TO BY THE NEGLIGENCE, SOLE OR CONCURRENT, OF
 GOVERNMENT AGENCY OR ANY PRIOR RECIPIENT.  RECIPIENT'S SOLE REMEDY FOR ANY SUCH MATTER SHALL BE THE
 IMMEDIATE, UNILATERAL TERMINATION OF THIS AGREEMENT.

 5. GENERAL TERMS

 A. Termination: This Agreement and the rights granted hereunder will terminate automatically if a
 Recipient fails to comply with these terms and conditions, and fails to cure such noncompliance within
 thirty (30) days of becoming aware of such noncompliance. Upon termination, a Recipient agrees to
 immediately cease use and distribution of the Subject Software. All sublicenses to the Subject
 Software properly granted by the breaching Recipient shall survive any such termination of this
 Agreement.

 B. Severability: If any provision of this Agreement is invalid or unenforceable under applicable law,
 it shall not affect the validity or enforceability of the remainder of the terms of this Agreement.

 C. Applicable Law: This Agreement shall be subject to United States federal law only for all purposes,
 including, but not limited to, determining the validity of this Agreement, the meaning of its
 provisions and the rights, obligations and remedies of the parties.

 D. Entire Understanding: This Agreement constitutes the entire understanding and agreement of the
 parties relating to release of the Subject Software and may not be superseded, modified or amended
 except by further written agreement duly executed by the parties.

 E. Binding Authority: By accepting and using the Subject Software under this Agreement, a Recipient
 affirms its authority to bind the Recipient to all terms and conditions of this Agreement and that
 Recipient hereby agrees to all terms and conditions herein.

 F. Point of Contact: Any Recipient contact with Government Agency is to be directed to the designated
 representative as follows: J. Ritchie Carroll <mailto:jrcarrol@tva.gov>.

*/
#endregion

#region [ Contributor License Agreements ]

//******************************************************************************************************
//
//  Copyright � 2011, Grid Protection Alliance.  All Rights Reserved.
//
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//******************************************************************************************************

#endregion

using GSF.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace GSF.Diagnostics
{
    /// <summary>
    /// Represents a process performance monitor that operates similar to the Windows Performance Monitor utility
    /// that can be used to monitor system performance.
    /// </summary>
    /// <example>
    /// This example shows how to use <see cref="PerformanceMonitor"/> for monitoring application performance:
    /// <code>
    /// using System;
    /// using System.Threading;
    /// using GSF.Diagnostics;
    ///
    /// class Program
    /// {
    ///     static void Main(string[] args)
    ///     {
    ///         PerformanceMonitor perfMon = new PerformanceMonitor();
    ///         while (true)
    ///         {
    ///             // Display process performance.
    ///             Thread.Sleep(5000);
    ///             Console.WriteLine("");
    ///             Console.Write(perfMon.Status);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="PerformanceCounter"/>
    public class PerformanceMonitor : IDisposable, IProvideStatus
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// Name of the custom thread pool counters category.
        /// </summary>
        public const string ThreadPoolCountersCategoryName = "TVA Thread Pool Counters";

        /// <summary>
        /// Default interval for sampling the <see cref="Counters"/>.
        /// </summary>
        public const double DefaultSamplingInterval = 1000.0D;

        // Fields
        private string m_processName;
        private List<PerformanceCounter> m_counters;
        private System.Timers.Timer m_samplingTimer;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class.
        /// </summary>
        public PerformanceMonitor()
            : this(DefaultSamplingInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class.
        /// </summary>
        /// <param name="samplingInterval">Interval, in milliseconds, at which the <see cref="Counters"/> are to be sampled.</param>
        /// <param name="addDefaultCounters">Set to <c>true</c> to add default counters; otherwise <c>false</c>.</param>
        public PerformanceMonitor(double samplingInterval, bool addDefaultCounters = true)
            : this(Process.GetCurrentProcess().ProcessName, samplingInterval, addDefaultCounters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class.
        /// </summary>
        /// <param name="processName">Name of the <see cref="Process"/> whose performance is to be monitored.</param>
        /// <param name="addDefaultCounters">Set to <c>true</c> to add default counters; otherwise <c>false</c>.</param>
        public PerformanceMonitor(string processName, bool addDefaultCounters = true)
            : this(processName, DefaultSamplingInterval, addDefaultCounters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceMonitor"/> class.
        /// </summary>
        /// <param name="processName">Name of the <see cref="Process"/> whose performance is to be monitored.</param>
        /// <param name="samplingInterval">Interval, in milliseconds, at which the <see cref="Counters"/> are to be sampled.</param>
        /// <param name="addDefaultCounters">Set to <c>true</c> to add default counters; otherwise <c>false</c>.</param>
        public PerformanceMonitor(string processName, double samplingInterval, bool addDefaultCounters = true)
        {
            if ((object)processName == null)
                throw new ArgumentNullException("processName");

            m_processName = processName;
            m_counters = new List<PerformanceCounter>();

            if (addDefaultCounters)
            {
                // Add default process and .NET counters
                AddCounter("Process", "% Processor Time", m_processName, "CPU Utilization", "Average % / CPU", Environment.ProcessorCount);
                AddCounter("Process", "IO Data Bytes/sec", m_processName, "I/O Data Rate", "Kilobytes / sec", SI2.Kilo);
                AddCounter("Process", "IO Data Operations/sec", m_processName, "I/O Activity Rate", "Operations / sec", 1);
                AddCounter("Process", "Handle Count", m_processName, "Process Handle Count", "Total Handles", 1);
                AddCounter("Process", "Thread Count", m_processName, "Process Thread Count", "System Threads", 1);
                AddCounter(".NET CLR LocksAndThreads", "# of current logical Threads", m_processName, "CLR Thread Count", "Managed Threads", 1);

                if (PerformanceCounterCategory.Exists(ThreadPoolCountersCategoryName))
                {
                    // Add custom thread pool counters                                                             1234567890123456
                    AddCounter(ThreadPoolCountersCategoryName, "Worker Threads", m_processName, "Worker Threads", "Active in Pool", 1, false);
                    //                                                                                   12345678901234567890
                    AddCounter(ThreadPoolCountersCategoryName, "Completion Port Threads", m_processName, "I/O Port Threads", "Active in Pool", 1, false);
                }

                AddCounter(".NET CLR LocksAndThreads", "Current Queue Length", m_processName, "Thread Queue Size", "Waiting Threads", 1);
                AddCounter(".NET CLR LocksAndThreads", "Contention Rate / sec", m_processName, "Lock Contention Rate", "Attempts / sec", 1);
                AddCounter("Process", "Working Set", m_processName, "Process Memory Usage", "Megabytes", SI2.Mega);
                AddCounter(".NET CLR Memory", "# Bytes in all Heaps", m_processName, "CLR Memory Usage", "Megabytes", SI2.Mega);
                AddCounter(".NET CLR Memory", "Large Object Heap size", m_processName, "Large Object Heap", "Megabytes", SI2.Mega);
                //                                                                                         1234567890123456
                AddCounter(".NET CLR Exceptions", "# of Exceps Thrown", m_processName, "Exception Count", "Total Exceptions", 1);
                AddCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", m_processName, "Exception Rate", "Exceptions / sec", 1);

                // Add default networking counters
                if (PerformanceCounterCategory.Exists("IPv4"))
                {
                    //                                            12345678901234567890
                    AddCounter("IPv4", "Datagrams Sent/sec", "", "IPv4 Outgoing Rate", "Datagrams / sec", 1);
                    //                                                12345678901234567890
                    AddCounter("IPv4", "Datagrams Received/sec", "", "IPv4 Incoming Rate", "Datagrams / sec", 1);
                }
                else if (PerformanceCounterCategory.Exists("IP"))
                {
                    AddCounter("IP", "Datagrams Sent/sec", "", "IP Outgoing Rate", "Datagrams / sec", 1);
                    AddCounter("IP", "Datagrams Received/sec", "", "IP Incoming Rate", "Datagrams / sec", 1);
                }

                if (PerformanceCounterCategory.Exists("IPv6"))
                {
                    AddCounter("IPv6", "Datagrams Sent/sec", "", "IPv6 Outgoing Rate", "Datagrams / sec", 1);
                    AddCounter("IPv6", "Datagrams Received/sec", "", "IPv6 Incoming Rate", "Datagrams / sec", 1);
                }

                // Perform initial sample for counters since in case timer interval is large
                SampleCounters();
            }

            m_samplingTimer = new System.Timers.Timer(samplingInterval);
            m_samplingTimer.Elapsed += m_samplingTimer_Elapsed;
            m_samplingTimer.Start();
        }

        /// <summary>
        /// Releases the unmanaged resources before the <see cref="PerformanceMonitor"/> object is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~PerformanceMonitor()
        {
            Dispose(false);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the name of the <see cref="Process"/> to be monitored.
        /// </summary>
        public string ProcessName
        {
            get
            {
                return m_processName;
            }
            set
            {
                m_processName = value;
                lock (m_counters)
                {
                    foreach (PerformanceCounter counter in m_counters)
                    {
                        // Only update the InstanceName for counters that had it set.
                        if (!string.IsNullOrEmpty(counter.BaseCounter.InstanceName))
                            counter.BaseCounter.InstanceName = m_processName;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the interval, in milliseconds, at which the <see cref="Counters"/> are to be sampled.
        /// </summary>
        public double SamplingInterval
        {
            get
            {
                return m_samplingTimer.Interval;
            }
            set
            {
                m_samplingTimer.Interval = value;
            }
        }

        /// <summary>
        /// Gets a read-only list of the <see cref="PerformanceCounter"/> objects monitored by the <see cref="PerformanceMonitor"/> object.
        /// </summary>
        public ReadOnlyCollection<PerformanceCounter> Counters
        {
            get
            {
                lock (m_counters)
                {
                    return new ReadOnlyCollection<PerformanceCounter>(m_counters);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the processor utilization of the monitored process.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter CPUUsage
        {
            get
            {
                return FindCounter("% Processor Time");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the IP based datagrams sent / second of the system.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter DatagramSendRate
        {
            get
            {
                return FindCounter("Datagrams Sent/sec");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the IP based datagrams received / second of the system.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter DatagramReceiveRate
        {
            get
            {
                return FindCounter("Datagrams Received/sec");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the .NET threading contention rate / second of the process.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter ThreadingContentionRate
        {
            get
            {
                return FindCounter("Contention Rate / sec");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the memory utilization of the monitored process.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter MemoryUsage
        {
            get
            {
                return FindCounter("Working Set");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the rate at which the monitored process is 
        /// issuing bytes to I/O operations that do not involve data such as control operations.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter IOUsage
        {
            get
            {
                return FindCounter("IO Data Bytes/sec");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the rate at which the monitored process is 
        /// issuing read and write I/O operations.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter IOActivity
        {
            get
            {
                return FindCounter("IO Data Operations/sec");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the total number of handles currently open by 
        /// the monitored process.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter HandleCount
        {
            get
            {
                return FindCounter("Handle Count");
            }
        }

        /// <summary>
        /// Gets the <see cref="PerformanceCounter"/> that monitors the number of threads currently active in the 
        /// monitored process.
        /// </summary>
        /// <remarks>This <see cref="PerformanceCounter"/> is added by default.</remarks>
        public PerformanceCounter ThreadCount
        {
            get
            {
                return FindCounter("Thread Count");
            }
        }

        /// <summary>
        /// Gets the friendly name of the <see cref="PerformanceMonitor"/> object.
        /// </summary>
        public string Name
        {
            get
            {
                return string.Format("{0}.{1}", this.GetType().Name, m_processName);
            }
        }

        /// <summary>
        /// Gets the current status of the <see cref="PerformanceMonitor"/> object.
        /// </summary>
        public string Status
        {
            get
            {
                StringBuilder status = new StringBuilder();

                // Status header.
                status.Append("Counter".PadRight(20));
                status.Append(' ');
                status.Append("Last".CenterText(13));
                status.Append(' ');
                status.Append("Average".CenterText(13));
                status.Append(' ');
                status.Append("Maximum".CenterText(13));
                status.Append(' ');
                status.Append("Units".CenterText(16));
                status.AppendLine();
                status.Append(new string('-', 20));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 16));
                status.AppendLine();

                lock (m_counters)
                {
                    foreach (PerformanceCounter counter in m_counters)
                    {
                        // Counter status.
                        status.Append(counter.AliasName.PadLeft(20));
                        status.Append(' ');
                        status.Append(counter.LastValue.ToString("0.00").CenterText(13));
                        status.Append(' ');
                        status.Append(counter.AverageValue.ToString("0.00").CenterText(13));
                        status.Append(' ');
                        status.Append(counter.MaximumValue.ToString("0.00").CenterText(13));
                        status.Append(' ');
                        status.Append(counter.ValueUnit);
                        status.AppendLine();
                    }
                }

                //          1         2         3         4         5         6         7         8
                // 12345678901234567890123456789012345678901234567890123456789012345678901234567890
                // Statistics calculated using last 500 counter values sampled every 12.5 seconds.

                string samplingInterval = "second";

                if (m_samplingTimer.Interval != 1000.0D)
                    samplingInterval = (m_samplingTimer.Interval / 1000.0D).ToString("0.0") + " seconds";

                status.AppendFormat("{0}Statistics calculated using last {1} counter values sampled every {2}.{3}", Environment.NewLine, PerformanceCounter.DefaultSamplingWindow, samplingInterval, Environment.NewLine);

                return status.ToString();
            }
        }

        /// <summary>
        /// Gets the lifetime status statistics of the <see cref="PerformanceMonitor"/> object.
        /// </summary>
        public string LifetimeStatus
        {
            get
            {
                StringBuilder status = new StringBuilder();
                long sampleCount = 0;

                // Status header.
                status.Append("Counter".PadRight(20));
                status.Append(' ');
                //             1234567890123
                status.Append("Lifetime Max.".CenterText(13));
                status.Append(' ');
                //             1234567890123
                status.Append("Lifetime Avg.".CenterText(13));
                status.Append(' ');
                //             1234567890123
                status.Append("Inv(Scalar)".CenterText(13));
                status.Append(' ');
                status.Append("Units".CenterText(16));
                status.AppendLine();
                status.Append(new string('-', 20));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 13));
                status.Append(' ');
                status.Append(new string('-', 16));
                status.AppendLine();

                lock (m_counters)
                {
                    foreach (PerformanceCounter counter in m_counters)
                    {
                        // Counter status.
                        status.Append(counter.AliasName.PadLeft(20));
                        status.Append(' ');
                        status.Append(counter.LifetimeMaximumValue.ToString("0.00").CenterText(13));
                        status.Append(' ');
                        status.Append(counter.LifetimeAverageValue.ToString("0.00").CenterText(13));
                        status.Append(' ');
                        status.Append(counter.ValueDivisor.ToString().CenterText(13));
                        status.Append(' ');
                        status.Append(counter.ValueUnit);
                        status.AppendLine();

                        if (sampleCount == 0)
                            sampleCount = counter.LifetimeSampleCount;
                    }
                }

                //          1         2         3         4         5         6         7         8
                // 12345678901234567890123456789012345678901234567890123456789012345678901234567890
                // Statistics calculated using 121878905 counter values sampled every 5.0 seconds.

                string samplingInterval = "second";

                if (m_samplingTimer.Interval != 1000.0D)
                    samplingInterval = (m_samplingTimer.Interval / 1000.0D).ToString("0.0") + " seconds";

                status.AppendFormat("{0}Statistics calculated using {1} counter values sampled every {2}.{3}", Environment.NewLine, sampleCount, samplingInterval, Environment.NewLine);

                return status.ToString();
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="PerformanceMonitor"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="PerformanceMonitor"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    // This will be done regardless of whether the object is finalized or disposed.
                    if (disposing)
                    {
                        // This will be done only when the object is disposed by calling Dispose().
                        if ((object)m_samplingTimer != null)
                        {
                            m_samplingTimer.Elapsed -= m_samplingTimer_Elapsed;
                            m_samplingTimer.Dispose();
                        }

                        lock (m_counters)
                        {
                            foreach (PerformanceCounter counter in m_counters)
                            {
                                counter.Dispose();
                            }

                            m_counters.Clear();
                        }
                    }
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Adds a <see cref="PerformanceCounter"/> to be monitored.
        /// </summary>
        /// <param name="categoryName">The name of the performance counter category (performance object) with which this performance counter is associated.</param>
        /// <param name="counterName">The name of the performance counter.</param>
        /// <param name="instanceName">The name of the performance counter category instance, or an empty string (""), if the category contains a single instance.</param>
        public void AddCounter(string categoryName, string counterName, string instanceName)
        {
            AddCounter(categoryName, counterName, instanceName, counterName);
        }

        /// <summary>
        /// Adds a <see cref="PerformanceCounter"/> to be monitored.
        /// </summary>
        /// <param name="categoryName">The name of the performance counter category (performance object) with which this performance counter is associated.</param>
        /// <param name="counterName">The name of the performance counter.</param>
        /// <param name="instanceName">The name of the performance counter category instance, or an empty string (""), if the category contains a single instance.</param>
        /// <param name="aliasName">The alias name for the <see cref="PerformanceCounter"/> object.</param>
        public void AddCounter(string categoryName, string counterName, string instanceName, string aliasName)
        {
            AddCounter(categoryName, counterName, instanceName, aliasName, PerformanceCounter.DefaultValueUnit);
        }

        /// <summary>
        /// Adds a <see cref="PerformanceCounter"/> to be monitored.
        /// </summary>
        /// <param name="categoryName">The name of the performance counter category (performance object) with which this performance counter is associated.</param>
        /// <param name="counterName">The name of the performance counter.</param>
        /// <param name="instanceName">The name of the performance counter category instance, or an empty string (""), if the category contains a single instance.</param>
        /// <param name="aliasName">The alias name for the <see cref="PerformanceCounter"/> object.</param>
        /// <param name="valueUnit">The measurement unit for the statistical values of the <see cref="PerformanceCounter"/> object.</param>
        public void AddCounter(string categoryName, string counterName, string instanceName, string aliasName, string valueUnit)
        {
            AddCounter(categoryName, counterName, instanceName, aliasName, valueUnit, PerformanceCounter.DefaultValueDivisor);
        }

        /// <summary>
        /// Adds a <see cref="PerformanceCounter"/> to be monitored.
        /// </summary>
        /// <param name="categoryName">The name of the performance counter category (performance object) with which this performance counter is associated.</param>
        /// <param name="counterName">The name of the performance counter.</param>
        /// <param name="instanceName">The name of the performance counter category instance, or an empty string (""), if the category contains a single instance.</param>
        /// <param name="aliasName">The alias name for the <see cref="PerformanceCounter"/> object.</param>
        /// <param name="valueUnit">The measurement unit for the statistical values of the <see cref="PerformanceCounter"/> object.</param>
        /// <param name="valueDivisor">The divisor to be applied to the statistical values of the <see cref="PerformanceCounter"/> object.</param>
        /// <param name="readOnly">Flag that determines if this counter is read-only.</param>
        public void AddCounter(string categoryName, string counterName, string instanceName, string aliasName, string valueUnit, float valueDivisor, bool readOnly = true)
        {
            try
            {
                AddCounter(new PerformanceCounter(categoryName, counterName, instanceName, aliasName, valueUnit, valueDivisor, readOnly));
            }
            catch
            {
                // Performance counter may not exist...
            }
        }

        /// <summary>
        /// Adds a <see cref="PerformanceCounter"/> to be monitored.
        /// </summary>
        /// <param name="counter">The <see cref="PerformanceCounter"/> object to be monitored.</param>
        public void AddCounter(PerformanceCounter counter)
        {
            lock (m_counters)
            {
                m_counters.Add(counter);
            }
        }

        /// <summary>
        /// Removes a <see cref="PerformanceCounter"/> being monitored.
        /// </summary>
        /// <param name="counter">The <see cref="PerformanceCounter"/> object to be unmonitored.</param>
        public void RemoveCounter(PerformanceCounter counter)
        {
            lock (m_counters)
            {
                m_counters.Remove(counter);
            }
        }

        /// <summary>
        /// Returns a <see cref="PerformanceCounter"/> object matching the specified counter name.
        /// </summary>
        /// <param name="counterName">Name of the <see cref="PerformanceCounter"/> to be retrieved.</param>
        /// <returns>A <see cref="PerformanceCounter"/> object if a match is found; otherwise null.</returns>
        /// <remarks>
        /// First <see cref="PerformanceCounter"/> with matching name is returned. If same name exists within
        /// muliple monitored categories, use <see cref="FindCounter(string,string)"/> overload instead.
        /// </remarks>
        public PerformanceCounter FindCounter(string counterName)
        {
            lock (m_counters)
            {
                foreach (PerformanceCounter counter in m_counters)
                {
                    if (string.Compare(counter.BaseCounter.CounterName, counterName, true) == 0)
                        return counter; // Return the match.
                }
            }

            return null;    // No match found.
        }

        /// <summary>
        /// Returns a <see cref="PerformanceCounter"/> object matching the specified counter name.
        /// </summary>
        /// <param name="categoryName">Category of the <see cref="PerformanceCounter"/> to be retrieved.</param>
        /// <param name="counterName">Name of the <see cref="PerformanceCounter"/> to be retrieved.</param>
        /// <returns>A <see cref="PerformanceCounter"/> object if a match is found; otherwise null.</returns>
        public PerformanceCounter FindCounter(string categoryName, string counterName)
        {
            lock (m_counters)
            {
                foreach (PerformanceCounter counter in m_counters)
                {
                    if (string.Compare(counter.BaseCounter.CategoryName, categoryName, true) == 0 && string.Compare(counter.BaseCounter.CounterName, counterName, true) == 0)
                        return counter; // Return the match.
                }
            }

            return null;    // No match found.
        }

        /// <summary>
        /// Sample all defined counters.
        /// </summary>
        public void SampleCounters()
        {
            lock (m_counters)
            {
                // Sample custom thread pool counters
                PerformanceCounter workerThreadsCounter = FindCounter(ThreadPoolCountersCategoryName, "Worker Threads");
                PerformanceCounter completionPortThreadsCounter = FindCounter(ThreadPoolCountersCategoryName, "Completion Port Threads");

                if (workerThreadsCounter != null && completionPortThreadsCounter != null)
                {
                    System.Diagnostics.PerformanceCounter workerThreads = workerThreadsCounter.BaseCounter;
                    System.Diagnostics.PerformanceCounter completionPortThreads = completionPortThreadsCounter.BaseCounter;

                    if (workerThreads != null && completionPortThreads != null)
                    {
                        int maximumWorkerThreads, maximumCompletionPortThreads, availableWorkerThreads, availableCompletionPortThreads;

                        ThreadPool.GetMaxThreads(out maximumWorkerThreads, out maximumCompletionPortThreads);
                        ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);

                        workerThreads.RawValue = maximumWorkerThreads - availableWorkerThreads;
                        completionPortThreads.RawValue = maximumCompletionPortThreads - availableCompletionPortThreads;
                    }
                }                

                foreach (PerformanceCounter counter in m_counters)
                {
                    counter.Sample();
                }
            }
        }

        private void m_samplingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SampleCounters();
        }

        #endregion

        #region [ Static ]

        // Static Constructor
        static PerformanceMonitor()
        {
            try
            {
                if (!PerformanceCounterCategory.Exists(ThreadPoolCountersCategoryName))
                {
                    CounterCreationDataCollection customPerformanceCounters = new CounterCreationDataCollection();

                    // Create custom counter objects for thread pool monitoring
                    CounterCreationData workerThreadCounter = new CounterCreationData();
                    workerThreadCounter.CounterName = "Worker Threads";
                    workerThreadCounter.CounterHelp = "Active worker threads in the thread pool";
                    workerThreadCounter.CounterType = PerformanceCounterType.NumberOfItems32;

                    CounterCreationData completionPortThreadCounter = new CounterCreationData();
                    completionPortThreadCounter.CounterName = "Completion Port Threads";
                    completionPortThreadCounter.CounterHelp = "Active completion port threads in the thread pool";
                    completionPortThreadCounter.CounterType = PerformanceCounterType.NumberOfItems32;

                    // Add custom counter objects to CounterCreationDataCollection
                    customPerformanceCounters.Add(workerThreadCounter);
                    customPerformanceCounters.Add(completionPortThreadCounter);

                    // Bind the counters to the PerformanceCounterCategory
                    PerformanceCounterCategory category = PerformanceCounterCategory.Create(ThreadPoolCountersCategoryName, "Application thread pool counters", PerformanceCounterCategoryType.MultiInstance, customPerformanceCounters);
                }
            }
            catch
            {
                // Not failing if custom counters cannot be created
            }
        }

        #endregion
    }
}