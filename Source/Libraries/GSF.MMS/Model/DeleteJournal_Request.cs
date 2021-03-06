//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System.Runtime.CompilerServices;
using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "DeleteJournal_Request", IsSet = false)]
    public class DeleteJournal_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DeleteJournal_Request));
        private ObjectName journalName_;

        [ASN1Element(Name = "journalName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ObjectName JournalName
        {
            get
            {
                return journalName_;
            }
            set
            {
                journalName_ = value;
            }
        }


        public void initWithDefaults()
        {
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }
    }
}