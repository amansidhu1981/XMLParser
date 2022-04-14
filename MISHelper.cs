using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISXMLParse
{
    public static class MISHelper
    {
        public const string KP_RELATIONSHIP_CODE = "*";
        public const string SPOUSE_RELATIONSHIP_CODE = "SPO";
        public const string BUS_PASS_REDUCTION_CODE = "BP"; //DO NOT DISPLAY THIS CODE
        public const string TSA_CODE = "24";
        public const string TTS_CODE = "63";
        public const string MSO_CLASS_CODE = "08";

        public static XDocument LoadUserData()
        {
            XDocument misDocument = new XDocument();
            //string misPersonID = ProfileHelper.GetUserMISPersonID();
            //misPersonID = "00000645"; /** TEST USER IF NEEDED **/

           misDocument = XDocument.Load(@"C:\Users\amansidhu\source\MISXMLParse\XML\mcp-no shelter costs.xml");

            return misDocument;
        }

//        public static XDocument LoadT5Data()
//        {
//            XDocument misDocument = new XDocument();
//            string misPersonID = ProfileHelper.GetUserMISPersonID();

//            if (HttpContext.Current.Session["T5Data"] == null)
//            {
//                string sessionData = "";
//                try
//                {
//                    if (String.IsNullOrEmpty(misPersonID) == false)
//                    {
//                        using (MCP_MC_ServicesClient mcpService = new MCP_MC_ServicesClient())
//                        {
//                            string username, password;
//                            Utility.GetMCPClientCredentials(out username, out password);
//                            mcpService.ClientCredentials.UserName.UserName = username;
//                            mcpService.ClientCredentials.UserName.Password = password;
//                            string misData;
//#if USE_T5_TEST_DATA
//                            misData = getT5TestData();
//#else
//                            misData = mcpService.LoadT5Data(misPersonID);
//#endif

//                            if (String.IsNullOrEmpty(misData) == false)
//                            {
//                                sessionData = misData;

//                                try
//                                {
//                                    misDocument = XDocument.Parse(misData);
//                                }
//                                catch (Exception)
//                                {
//                                    //unable to parse
//                                }
//                            }
//                        };

//                        HttpContext.Current.Session.Add("T5Data", sessionData);
//                    }
//                }
//                catch (Exception)
//                {
//                    //add logging here if required
//                    throw;
//                }
//            }
//            else if (String.IsNullOrEmpty(HttpContext.Current.Session["T5Data"].ToString()) == false)
//            {
//                try
//                {
//                    misDocument = XDocument.Parse(HttpContext.Current.Session["T5Data"].ToString());
//                }
//                catch (Exception)
//                {
//                    //add logging here
//                }
//            }

//            return misDocument;
//        }

        public static bool FlushMISData()
        {
            return true;
        }

        public static bool RenderMyAEE()
        {
            bool myAEEData = false;

            XDocument misData = LoadUserData();
            XElement annualizedEarnings = misData.Descendants("AnnualizedEarningExemptions").FirstOrDefault();

            if (annualizedEarnings != null && annualizedEarnings.Descendants("LimitRemaining").FirstOrDefault() != null)
            {
                myAEEData = true;
            }

            return myAEEData;
        }

        public static string GetPersonIDByRelCode(XDocument misData, string relationshipCode)
        {
            XElement caseInformation = misData.Descendants("CaseInformation").FirstOrDefault();

            if (caseInformation != null)
            {
                foreach (XElement element in caseInformation.Descendants("Person"))
                {
                    XElement personType = element.Descendants("Type").FirstOrDefault();
                    if (personType != null && personType.Value == relationshipCode)
                    {
                        XElement personID = element.Descendants("Id").FirstOrDefault();
                        if (personID != null)
                        {
                            return personID.Value;
                        }
                    }
                }
            }

            return null;
        }

        public static string GetBusPassIndByRelCode(XDocument misData, string relationshipCode)
        {
            XElement caseInformation = misData.Descendants("CaseInformation").FirstOrDefault();

            if (caseInformation != null)
            {
                foreach (XElement element in caseInformation.Descendants("Person"))
                {
                    XElement personType = element.Descendants("Type").FirstOrDefault();
                    if (personType != null && personType.Value == relationshipCode)
                    {
                        XElement busPassInd = element.Descendants("BusPassIndicator").FirstOrDefault();
                        if (busPassInd != null)
                        {
                            return busPassInd.Value;
                        }
                    }
                }
            }

            return null;
        }

        public static bool IsReceivingDisabilityAssistance(XDocument misData)
        {
            if (IsMSOCase(misData) == false)
            {
                XElement caseInformation = misData.Descendants("CaseInformation").FirstOrDefault();

                if (caseInformation != null)
                {
                    bool pwdIndFound = false;
                    foreach (XElement element in caseInformation.Descendants("Person"))
                    {
                        XElement personType = element.Descendants("Type").FirstOrDefault();
                        if (personType != null && (personType.Value == KP_RELATIONSHIP_CODE || personType.Value == SPOUSE_RELATIONSHIP_CODE))
                        {
                            XElement pwdInd = element.Descendants("PwdIndicator").FirstOrDefault();
                            if (pwdInd != null)
                            {
                                pwdIndFound = true;
                                if (pwdInd.Value == "1")
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    //Back-up plan: if PwdIndicator is not found, use old logic
                    if (pwdIndFound == false)
                    {
                        return RenderMyAEE();
                    }
                }
            }

            return false;
        }

        public static bool IsMSOCase(XDocument misData)
        {
            XElement caseInformation = misData.Descendants("CaseInformation").FirstOrDefault();

            if (caseInformation != null)
            {
                XElement classCode = caseInformation.Descendants("ClassCd").FirstOrDefault();

                if (classCode != null)
                {
                    if (classCode.Value == MSO_CLASS_CODE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //private static string getT5TestData()
        //{
        //    string xmlOutput = String.Empty;
        //    Encoding utf8noBOM = new UTF8Encoding(false);
        //    XmlWriterSettings settings = new XmlWriterSettings();
        //    settings.Encoding = utf8noBOM;
        //    settings.Indent = true;
        //    settings.IndentChars = "    ";

        //    using (MemoryStream output = new MemoryStream())
        //    {
        //        using (XmlWriter writer = XmlWriter.Create(output, settings))
        //        {
        //            writer.WriteStartDocument();
        //            writer.WriteStartElement("T5Information");
        //            writer.WriteElementString("PaymentFileId", "GA00000001");
        //            writer.WriteElementString("PaymentAmount", "12,925.99");
        //            writer.WriteElementString("PaymentYear", "2014");
        //            writer.WriteElementString("PaymentSIN", "12345782");
        //            writer.WriteElementString("PaymentReportCode", "0");
        //            writer.WriteElementString("PaymentFileId", "GA00000001");
        //            writer.WriteElementString("Surname", "DUCK");
        //            writer.WriteElementString("GivenNames", "DONALD DOUGLAS");
        //            //End for T5Information
        //            writer.WriteEndElement();

        //            //EOF
        //            writer.WriteEndDocument();
        //            writer.Flush();
        //        }

        //        xmlOutput = Encoding.Default.GetString(output.ToArray());
        //    }

        //    return xmlOutput;
        //}
    }
}
