//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "ObtainFile_Request", IsSet = false)]
    public class ObtainFile_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ObtainFile_Request));
        private FileName destinationFile_;
        private ApplicationReference sourceFileServer_;

        private bool sourceFileServer_present;


        private FileName sourceFile_;

        [ASN1Element(Name = "sourceFileServer", IsOptional = true, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ApplicationReference SourceFileServer
        {
            get
            {
                return sourceFileServer_;
            }
            set
            {
                sourceFileServer_ = value;
                sourceFileServer_present = true;
            }
        }

        [ASN1Element(Name = "sourceFile", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public FileName SourceFile
        {
            get
            {
                return sourceFile_;
            }
            set
            {
                sourceFile_ = value;
            }
        }


        [ASN1Element(Name = "destinationFile", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public FileName DestinationFile
        {
            get
            {
                return destinationFile_;
            }
            set
            {
                destinationFile_ = value;
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

        public bool isSourceFileServerPresent()
        {
            return sourceFileServer_present;
        }
    }
}