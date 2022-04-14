using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISXMLParse
{
    public class ChequeInfoViewModel
    {
        public XDocument misData;
        public Dictionary<string, string> ChequeStatus = new Dictionary<string, string>();
        public Dictionary<string, string> PaymentMethod = new Dictionary<string, string>();
        public Dictionary<string, string> PaymentIssueCode = new Dictionary<string, string>();
        public Dictionary<string, string> PaymentDistribution = new Dictionary<string, string>();
        public Dictionary<string, string> SD81Status = new Dictionary<string, string>();
        public Dictionary<string, string> Deductions = new Dictionary<string, string>();
        public Dictionary<string, string> Shelter = new Dictionary<string, string>();
        public Dictionary<string, string> OtherBenefits = new Dictionary<string, string>();
        public Dictionary<string, string> Pensions = new Dictionary<string, string>();

        public string missingLanguage = "MISSING_KEY";

        public DateTime benefitDate = DateTime.MinValue;
        public DateTime chequeIssueDate = DateTime.MinValue;
        public DateTime previousChequeIssueDate = DateTime.MinValue;

        public string kpPersonID = null;
        public string spousePersonID = null;
        public bool kpHasBusPass = false;
        public bool spouseHasBusPass = false;
        public bool receivingDisability = false;
        public bool hasT5Info = false;

        public List<string> SuppressableBenefits = new List<string>();
        public List<string> SuppressableDeductions = new List<string>();

        public ChequeInfoViewModel()
        {
            this.misData = MISHelper.LoadUserData();

            //load cheque schedule data
            LoadCurrentChequeSchedule();

            //load key player and spouse info
            ExtractPersonalInfo();

            //load cheque status language
            ChequeStatus.Add("C", "CASHED");
            ChequeStatus.Add("O", "OUTSTANDING");
            ChequeStatus.Add("R", "REVERSED");
            ChequeStatus.Add("S", "PAYMENT STOPPED");
            ChequeStatus.Add("U", "UNKNOWN");
            ChequeStatus.Add("W", "WRITTEN OFF");
            ChequeStatus.Add("X", "CANCELLED");
            ChequeStatus.Add("Y", "OLD CONVERSION");
            ChequeStatus.Add("Z", "CONVERSION - OUTSTANDING");

            //load payment method language
            PaymentMethod.Add("B", "DIRECT DEPOSIT");
            PaymentMethod.Add("C", "CHEQUE");
            PaymentMethod.Add("G", "DIRECT PURCHASE");
            PaymentMethod.Add("I", "CHEQUE");

            //load payment issue code
            PaymentIssueCode.Add("B", "DIRECT DEPOSIT");
            PaymentIssueCode.Add("C", "CHEQUE");
            PaymentIssueCode.Add("G", "DIRECT DEPOSIT");
            PaymentIssueCode.Add("I", "CHEQUE");
            PaymentIssueCode.Add("E", "DIRECT DEPOSIT");
            PaymentIssueCode.Add("O", "CHEQUE");

            //load payment distribution
            PaymentDistribution.Add("A", "HELD");
            PaymentDistribution.Add("B", "MAILED");
            PaymentDistribution.Add("C", "N/A");
            PaymentDistribution.Add("D", "DELIVER");
            PaymentDistribution.Add("E", "EMAIL");
            PaymentDistribution.Add("F", "HELD");
            PaymentDistribution.Add("H", "HOLD");
            PaymentDistribution.Add("I", "ISSUE DAY");
            PaymentDistribution.Add("M", "MAIL");
            PaymentDistribution.Add("N", "NONE");
            PaymentDistribution.Add("O", "OFFICE");
            PaymentDistribution.Add("P", "HELD");
            PaymentDistribution.Add("R", "MY SELF SERVE");
            PaymentDistribution.Add("W", "WAITING CLIENT");
            PaymentDistribution.Add("Z", "DIRECT DEPOSIT");
            PaymentDistribution.Add("0", "HELD");
            PaymentDistribution.Add("1", "HELD");
            PaymentDistribution.Add("2", "MAILED");
            PaymentDistribution.Add("3", "N/A");
            PaymentDistribution.Add("4", "HELD");
            PaymentDistribution.Add("5", "N/A");
            PaymentDistribution.Add("6", "N/A");
            PaymentDistribution.Add("7", "HELD");
            PaymentDistribution.Add("8", "HELD");
            PaymentDistribution.Add("9", "HELD");

            SD81Status.Add("1", "CHEQUE SIGNALLED");
            SD81Status.Add("2", "PICKUP CHEQUE");
            SD81Status.Add("F", "FUTURE SIGNAL");

            //load deduction language
            Deductions.Add("AC", "SERVICE PROVIDER (THIRD PARTY PAYMENT)");
            Deductions.Add("AM", "MANUAL DEDUCTION");
            Deductions.Add("DE", "EXCESS EARNED INCOME DEDUCTION");
            Deductions.Add("DI", "EXCESS UNEARNED INCOME DEDUCTION");
            Deductions.Add("RC", "REPAYMENT DEDUCTION");
            Deductions.Add("TL", "TIME LIMITS REDUCTION");
            Deductions.Add("88", "SANCTION REDUCTION");
            Deductions.Add("89", "TIME LIMITS REDUCTION");
            Deductions.Add("90", "MANUAL DEDUCTION");
            Deductions.Add("BP", "BUS PASS REDUCTION");

            //load shelter langauge
            Shelter.Add("1", "SHELTER: ROOM / BOARD PRIVATE");
            Shelter.Add("2", "SHELTER: ROOM / BOARD PARENT OR CHILD");
            Shelter.Add("3", "SHELTER: MORTGAGE PAYMENT / PROPERTY TAXES");
            //Shelter.Add("64", "PROPERTY TAXES");
            Shelter.Add("4", "SHELTER: SHARED RENT");
            Shelter.Add("5", "SHELTER: RENT");
            Shelter.Add("8", "SHELTER: UTILITIES");

            //other benefits
            OtherBenefits.Add("01", "FAMILY BONUS TEMPORARY TOP UP");
            OtherBenefits.Add("02", "DIET - NATAL ALLOWANCE");
            OtherBenefits.Add("03", "DIETARY ALLOWANCE");
            OtherBenefits.Add("04", "TRAVEL BENEFIT");
            OtherBenefits.Add("05", "COMFORTS ALLOWANCE");
            OtherBenefits.Add("06", "2010NOV01 - ALLOWANCE INACTIVE");
            OtherBenefits.Add("07", "MEDICAL TRANSPORTATION - ESCORT");
            OtherBenefits.Add("08", "CONFIRMED JOB - HEALTH AND SAFETY SUPPLIES");
            OtherBenefits.Add("09", "GUIDE ANIMAL SUPPLEMENT");
            OtherBenefits.Add("10", "MEDICAL TRANSPORTATION - LOCAL");
            OtherBenefits.Add("11", "COMMUNITY VOLUNTEER PROGRAM");
            OtherBenefits.Add("12", "ADMINISTRATION FEE");
            OtherBenefits.Add("13", "BENEFITS UNDER APPEAL");
            OtherBenefits.Add("14", "TRAINING INIT FOR HC");
            OtherBenefits.Add("15", "OUTSTANDING WARRANT SUPPLEMENT");
            OtherBenefits.Add("16", "PRE-NATAL SHELTER SUPPLEMENT");
            OtherBenefits.Add("17", "VOLUNTEER INCENTIVE PROGRAM -PROGRAM SUPPORTS");
            OtherBenefits.Add("18", "MEDICAL TRANSPORTATION - EXCEPTIONAL RATE/KM");
            OtherBenefits.Add("19", "AUTOMATIC ESCALATING TRUSTEE");
            OtherBenefits.Add("20", "REGULAR SUPPLIER TRUSTEE");
            OtherBenefits.Add("21", "EI ASSIGNMENT RECOVERY");
            OtherBenefits.Add("22", "NUTRITIONAL SUPPLEMENT - DIETARY ITEMS ($165)");
            OtherBenefits.Add("23", "LOST FAMILY BONUS");
            OtherBenefits.Add("25", "O/S WARRANT TRANSPORTATION SUPPLEMENT");
            OtherBenefits.Add("26", "CPP ADJUSTMENT BENEFIT");
            OtherBenefits.Add("27", "SPECIAL TRANSPORTATION SUBSIDY");
            OtherBenefits.Add("28", "NUTRITIONAL SUPPLEMENT - VITAM/MINERALS ($40)");
            OtherBenefits.Add("29", "GRANDPARENTED SCHEDULE C APPEAL AWARD");
            OtherBenefits.Add("30", "DIRECT PURCHASE - ABE/UPGRADING");
            OtherBenefits.Add("31", "DIRECT PURCHASE - LITERACY");
            OtherBenefits.Add("32", "DIRECT PURCHASE - ESL");
            OtherBenefits.Add("33", "DIRECT PURCHASE - CERT(SHORT TERM COURSE)");
            OtherBenefits.Add("34", "DIRECT PURCHASE - FOREIGN CREDENTIALS");
            OtherBenefits.Add("35", "DIRECT PURCHASE - TRANS/BOOKS SUPPLIES");
            OtherBenefits.Add("36", "SHARED PARENTING ALLOWANCE");
            OtherBenefits.Add("37", "DECEASED ADJUSTMENT SUPPLEMENT");
            OtherBenefits.Add("38", "FMP EXPENSE ISO NOTARY COURT PATERNITY TRAVEL");
            OtherBenefits.Add("39", "ALCOHOL AND DRUG SERVICES");
            OtherBenefits.Add("40", "FAMILY BONUS AUTOMATIC TOP UP");
            OtherBenefits.Add("41", "SUPPORT");
            OtherBenefits.Add("42", "SHELTER");
            OtherBenefits.Add("43", "CHRISTMAS BENEFIT");
            OtherBenefits.Add("44", "SCHOOL START-UP ALLOWANCE FOR FAMILY UNITS");
            OtherBenefits.Add("45", "DIRECT PURCHASE - VOCATIONAL ASSESSMENTS");
            OtherBenefits.Add("46", "CIC TEMPORARY ABSENCE ASSISTANCE");
            OtherBenefits.Add("47", "TRIBUNAL AWARD");
            OtherBenefits.Add("48", "ID LETTER");
            OtherBenefits.Add("49", "TRANSPORTATION SUPPLEMENT");
            OtherBenefits.Add("50", "CRISIS BENEFIT-OTHER");
            OtherBenefits.Add("51", "TEMPORARY CIHR SPECIAL NEEDS ASSISTANCE");
            OtherBenefits.Add("52", "CRISIS BENEFIT-FOOD");
            OtherBenefits.Add("53", "EMERG MOVE AND TRANS");
            OtherBenefits.Add("54", "CONFIRMED JOB - CLOTHING /ESSENTIALS");
            OtherBenefits.Add("55", "EMERGENCY MOVING/IMMINENT DANGER");
            OtherBenefits.Add("56", "MOVE OUT OF PROV");
            OtherBenefits.Add("57", "CAMP FEES");
            OtherBenefits.Add("58", "CONFIRMED JOB - TRANSPORTATION");
            OtherBenefits.Add("59", "CO-OP SHARES - WAS CIVIL DISASTER");
            OtherBenefits.Add("60", "SECURITY DEPOSIT");
            OtherBenefits.Add("61", "TCLOTH SPEC CARE FACILITY");
            OtherBenefits.Add("62", "MEDICAL TRANSPORTATION - TRAVEL");
            OtherBenefits.Add("64", "FEES FOR ID/SIN DOCUMENT");
            OtherBenefits.Add("65", "CONFIRMED JOB - MOVE");
            OtherBenefits.Add("66", "UTILITY SECURITY DEPOSIT");
            OtherBenefits.Add("67", "TRANS RESIDNTL ALC / DRUG TREATMENT FACILITY");
            OtherBenefits.Add("68", "LOST/STOLEN CHEQUE");
            OtherBenefits.Add("69", "CJ-PART TIME WORK PILOT");
            OtherBenefits.Add("70", "HARDSHIP SUPPORT");
            OtherBenefits.Add("71", "HARDSHIP SHELTER");
            OtherBenefits.Add("72", "FAMILY BONUS HARDSHIP TOP UP");
            //OtherBenefits.Add("73", "2002APR02 - ALLOWANCE INACTIVE");
            OtherBenefits.Add("73", "HARDSHIP COMFORTS");
            OtherBenefits.Add("74", "CRISIS BENEFIT-SHELTER");
            OtherBenefits.Add("75", "CRISIS BENEFIT-CLOTHING");
            OtherBenefits.Add("76", "EMERGENCY DISASTER SUPPLEMENT");
            OtherBenefits.Add("77", "CRISIS BENEFIT-UTILITIES");
            OtherBenefits.Add("78", "CRISIS BENEFIT-FURNITURE");
            OtherBenefits.Add("79", "CRISIS BENEFIT - HOME REPAIR");
            OtherBenefits.Add("80", "CRISIS - ESSENTIAL UTILITIES");
            OtherBenefits.Add("81", "SEP - BCEA REGS ORIENTATN , 12 MTHLY INC RVWS");
            OtherBenefits.Add("82", "SEP - BCEA ONGOING PERIODIC REVIEWS");
            OtherBenefits.Add("83", "MEDICAL TRANSPORTATION - SHELTER");
            OtherBenefits.Add("84", "MEDICAL TRANSPORTATION - FOOD");
            OtherBenefits.Add("88", "SANCTION REDUCTION");
            OtherBenefits.Add("89", "TIME LIMITS REDUCTION");
            OtherBenefits.Add("90", "MANUAL DEDUCTION");
            OtherBenefits.Add("91", "MANUAL DEDUCTION");
            OtherBenefits.Add("92", "CHEQUE TOTAL");

            //pensions and child care language
            Pensions.Add("01", "NET EARNINGS - EARNED INCOME");
            Pensions.Add("02", "ONE TIME NET EARNINGS");
            Pensions.Add("03", "ROOMER");
            Pensions.Add("04", "BOARDER");
            Pensions.Add("05", "MAINTENANCE (ALIMONY,SUPPORT)");
            Pensions.Add("06", "ONE TIME MAINTENANCE");
            Pensions.Add("07", "TRAINING - EARNED INCOME");
            Pensions.Add("08", "FOSTER CARE-EARNED INCOME");
            Pensions.Add("09", "OTHER EARNED INCOME");
            Pensions.Add("11", "CANADA PENSION PLAN");
            Pensions.Add("12", "WAR VETERANS ALLOWANCE");
            Pensions.Add("13", "TRAINING ALLOWANCE");
            Pensions.Add("14", "WORKERS' COMPENSATION BOARD");
            Pensions.Add("15", "EMPLOYMENT INSURANCE BENEFIT");
            Pensions.Add("16", "CANADIAN PENSION COMMISSION");
            Pensions.Add("17", "OLD AGE SECURITY/GUAR.INC SUPP");
            Pensions.Add("18", "GAIN FOR SENIOR SUPPLEMENT");
            Pensions.Add("19", "SHELTER AID FOR ELDERLY RENTER");
            Pensions.Add("20", "WAR VETERANS -WIDOW'S PENSION");
            Pensions.Add("21", "EXTENDED SPOUSES ALLOWANCE");
            Pensions.Add("22", "PRIVATE RETIREMENT PENSION");
            Pensions.Add("23", "PRIVATE DISABILITY PENSION");
            Pensions.Add("24", "INCOME TAX REFUND");
            Pensions.Add("26", "1-TIME SPONSORSHIP INCOME");
            Pensions.Add("27", "1-TIME MEDICAL NEEDS (TRUST)");
            Pensions.Add("28", "1-TIME INDEP. PURPOSES (TRUST)");
            Pensions.Add("29", "OTHER UNEARNED INCOME");
            Pensions.Add("3A", "UNIVERSAL CHILD CARE BENEFIT");
            Pensions.Add("31", "HST/GST/CLIM ACT TAX CREDITS");
            Pensions.Add("32", "BASIC CHILD TAX BENEFIT");
            Pensions.Add("33", "WORKING INCOME TAX BENEFIT");
            Pensions.Add("34", "MOH HIV PAYMENT");
            Pensions.Add("35", "FAMILY BONUS/NATIONAL CHILD BENEFIT SUPPLEMENT");
            Pensions.Add("36", "BC EARNED INCOME BENEFIT");
            Pensions.Add("37", "HEPATITIS C VIRUS PAYMENT");
            Pensions.Add("38", "EXEMPT FOSTER CARE");
            Pensions.Add("39", "FAMILY BONUS FROM OTHER PARENT");
            Pensions.Add("4A", "CHILD DISABILITY BENEFIT");
            Pensions.Add("4B", "THERAPEUTIC VOLUNTEER SUPPLEMENT");
            Pensions.Add("4C", "OTHER MH VOLUNTEER PAYMENTS");
            Pensions.Add("4D", "EDUCATION GRANT");
            Pensions.Add("4E", "REGISTERED DISABILITY SAVINGS PLAN");

            //Benefits that won't be displayed to the user
            SuppressableBenefits.Add("24");
            SuppressableBenefits.Add("63");

            //Deductions that won't be displayed to the user
            SuppressableDeductions.Add("BP");
        }

        public string GetChequeStatus(string statusKey)
        {
            if (String.IsNullOrEmpty(statusKey) == false)
            {
                return LanguageHelper.GetDictionaryKey(ChequeStatus, statusKey);
            }

            return String.Empty;
        }

        public string GetPaymentDistribution(string key)
        {
            return LanguageHelper.GetDictionaryKey(PaymentDistribution, key);
        }

        public string GetPaymentMethod(string paymentKey)
        {
            return LanguageHelper.GetDictionaryKey(PaymentMethod, paymentKey);
        }

        public string GetPaymentIssueCode(string key)
        {
            return LanguageHelper.GetDictionaryKey(PaymentIssueCode, key);
        }

        public string GetSD81Status(string statusKey)
        {
            return LanguageHelper.GetDictionaryKey(SD81Status, statusKey);
        }

        public string GetDeduction(string deductionKey)
        {
            return LanguageHelper.GetDictionaryKey(Deductions, deductionKey);
        }

        public string GetShelter(string shelterKey)
        {
            return LanguageHelper.GetDictionaryKey(Shelter, shelterKey);
        }

        public string GetOtherBenefit(string benefitKey, string personID)
        {
            return AddBenefactorDescription(LanguageHelper.GetDictionaryKey(OtherBenefits, benefitKey), personID);
        }

        public string GetPensionChildCareValue(string pccKey)
        {
            return LanguageHelper.GetDictionaryKey(Pensions, pccKey);
        }

        public string LoadXMLValue(string xmlNode, string defaultValue)
        {
            string rtnString = defaultValue;
            XElement xmlElement = this.misData.Descendants(xmlNode).FirstOrDefault();

            if (xmlElement != null && String.IsNullOrWhiteSpace(xmlElement.Value) == false)
            {
                rtnString = xmlElement.Value;
            }

            return rtnString;

        }

        public string SantizeXMLElementValue(XElement xmlElement, string keyName)
        {
            string rtnVal = "";

            if (xmlElement.Element(keyName) != null)
            {
                rtnVal = xmlElement.Element(keyName).Value;
            }

            return rtnVal;
        }

        public bool DisplayBenefit(string benefitKey, string personID)
        {
            if (SuppressableBenefits.Contains(benefitKey) == true)
            {
                if (benefitKey == MISHelper.TSA_CODE || benefitKey == MISHelper.TTS_CODE)
                {
                    if ((personID == kpPersonID && kpHasBusPass == true) || (personID == spousePersonID && spouseHasBusPass == true))
                    {
                        return false;
                    }
                }
                else
                {
                    if (personID == kpPersonID || personID == spousePersonID)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool DisplayDeduction(string deductionKey)
        {
            if (SuppressableDeductions.Contains(deductionKey) == true)
            {
                return false;
            }

            return true;
        }


        private void LoadCurrentChequeSchedule()
        {
            //using (MySelfServe.MCP_MC_Services.MCP_MC_ServicesClient remote = new MySelfServe.MCP_MC_Services.MCP_MC_ServicesClient())
            //{
            //    string username, password;
            //    Utility.GetMCPClientCredentials(out username, out password);
            //    remote.ClientCredentials.UserName.UserName = username;
            //    remote.ClientCredentials.UserName.Password = password;
            //    DataSet currentPeroidSchedule = remote.GetChequeScheduleWindow(DateTime.Today.ToString());

            //    if (currentPeroidSchedule.Tables[0].Rows.Count > 0)
            //    {
            //        benefitDate = Convert.ToDateTime(currentPeroidSchedule.Tables[0].Rows[0]["BENEFIT_DT"]);
            //        chequeIssueDate = Convert.ToDateTime(currentPeroidSchedule.Tables[0].Rows[0]["CHEQUE_ISSUE_DT"]);
            //        previousChequeIssueDate = Convert.ToDateTime(currentPeroidSchedule.Tables[0].Rows[0]["PREVIOUS_CHEQ_ISSUE_DT"]);
            //    }
            //}
        }

     

        private void ExtractPersonalInfo()
        {
            if (this.misData != null)
            {
                this.kpPersonID = MISHelper.GetPersonIDByRelCode(this.misData, MISHelper.KP_RELATIONSHIP_CODE);
                this.kpHasBusPass = (MISHelper.GetBusPassIndByRelCode(this.misData, MISHelper.KP_RELATIONSHIP_CODE) == "1") ? true : false;
                this.spousePersonID = MISHelper.GetPersonIDByRelCode(this.misData, MISHelper.SPOUSE_RELATIONSHIP_CODE);
                this.spouseHasBusPass = (MISHelper.GetBusPassIndByRelCode(this.misData, MISHelper.SPOUSE_RELATIONSHIP_CODE) == "1") ? true : false;
                this.receivingDisability = MISHelper.IsReceivingDisabilityAssistance(this.misData);
            }
        }

        private string AddBenefactorDescription(string benefit, string personID)
        {
            string result = benefit;
            //Currenly only used for the Spouse
            if (String.IsNullOrEmpty(personID) == false && personID == this.spousePersonID)
            {
                result += " (SPOUSE)";
            }

            return result;
        }
    }
}
