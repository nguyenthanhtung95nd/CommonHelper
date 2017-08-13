using System.IO;
using System.Xml;

namespace Utility
{
    public class XmlHelper
    {
        private const string XMLTEMPLATE_TAG = "XMLTemplateList";
        private const string TEMPLATE_TAG = "Template";
        private const string ID_ATR = "Id";
        private const string ID_SUBJECT = "Subject";

        public static string ReadEmailTemplate(string urlSource, string idTemplate, out string eSubject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string rtnValue = string.Empty;
            eSubject = string.Empty;

            if (!File.Exists(urlSource))
                return rtnValue;

            xmlDoc.Load(urlSource);

            // Read Report Note by Id
            XmlNode rptNode = xmlDoc.SelectSingleNode(XMLTEMPLATE_TAG + "/" +
                                                   TEMPLATE_TAG + "[@" + ID_ATR + "='" + idTemplate + "']");
            if (rptNode != null)
            {
                rtnValue = rptNode.InnerText;
                eSubject = rptNode.Attributes[ID_SUBJECT].Value;
            }

            return rtnValue;
        }
    }
}