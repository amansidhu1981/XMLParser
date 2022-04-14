// See https://aka.ms/new-console-template for more information
using MISXMLParse;
using System.Xml.Linq;

Console.WriteLine("Hello, World!");

var Model = new ChequeInfoViewModel();


if (Model.misData != null)
{

    XElement benefits = Model.misData.Descendants("CurrentBenefits").FirstOrDefault();

    if (benefits != null &&
        (benefits.Descendants("Support").Any()
         || benefits.Descendants("Shelter").Any()
         || benefits.Descendants("OtherBenefits").Any()
         || benefits.Descendants("Deductions").FirstOrDefault() != null))
    {


        if (Model.receivingDisability)
        {
            Console.WriteLine("Disability Assistance");
        }
        else
        {
            Console.WriteLine("Assistance");
        }

        foreach (XElement element in benefits.Descendants("Support"))
        {
            Console.WriteLine(Model.SantizeXMLElementValue(element, "Amount"));
            foreach (XElement element1 in benefits.Descendants("Shelter"))
            {
                Console.WriteLine(Model.GetShelter(Model.SantizeXMLElementValue(element1, "Type")));
                Console.WriteLine(Model.SantizeXMLElementValue(element1, "Amount"));
            }
            if (Model.kpHasBusPass)
            {

            }
            if (Model.spouseHasBusPass)
            {
                // Bus Pass(Spouse) </ td >


            }

            foreach (XElement element2 in benefits.Descendants("OtherBenefits").Descendants("OtherBenefit"))
            {
                if (Model.DisplayBenefit(Model.SantizeXMLElementValue(element2, "Name"), Model.SantizeXMLElementValue(element2, "PersonId")))
                {
                    Model.GetOtherBenefit(Model.SantizeXMLElementValue(element2, "Name"), Model.SantizeXMLElementValue(element2, "PersonId"));
                    Model.SantizeXMLElementValue(element2, "Amount");


                    if (element2.Element("ExpiryDate") != null) ;
                    {
                        DateTime expiryTime;
                        var xElement = element2.Element("ExpiryDate");
                        if (xElement != null && DateTime.TryParse(xElement.Value, out expiryTime))
                        {
                            //expiryTime.ToString(ConfigHelper.DATE_FORMAT);
                        }
                    }

                }
            }


            XElement deductions = benefits.Descendants("Deductions").FirstOrDefault();
            if (deductions != null && deductions.Descendants("Deduction").Any())
            {
                // Deductions 

                foreach (XElement element3 in deductions.Descendants("Deduction"))
                {
                    if (Model.DisplayDeduction(Model.SantizeXMLElementValue(element3, "Name")))
                    {
                        Model.GetDeduction(Model.SantizeXMLElementValue(element3, "Name"));
                        Model.SantizeXMLElementValue(element3, "Amount");



                    }
                }

            }

            XElement serviceProviders = Model.misData.Descendants("ServiceProviders").FirstOrDefault();
            if (serviceProviders != null && serviceProviders.Descendants("ServiceProvider").Any())
            {
                //Service Provider Payments
                foreach (XElement element4 in serviceProviders.Descendants("ServiceProvider"))
                {
                    Model.SantizeXMLElementValue(element4, "Name");
                    Model.SantizeXMLElementValue(element4, "Amount");
                    Model.SantizeXMLElementValue(element4, "Reference");



                }

            }

            XElement pensionData = Model.misData.Descendants("Pensions").FirstOrDefault();
            XElement childCareBenefits = Model.misData.Descendants("ChildCareTaxBenefits").FirstOrDefault();
            if ((pensionData != null && pensionData.Descendants("Pension").Any()) || (childCareBenefits != null && childCareBenefits.Descendants("ChildCareTaxBenefit").Any()))
            {
                // Pension Payments & Canada Child Tax Benefits (CCTB)
                if (pensionData != null)
                {
                    foreach (XElement element5 in pensionData.Descendants("Pension"))
                    {
                        Model.GetPensionChildCareValue(Model.SantizeXMLElementValue(element5, "Type"));
                        Model.SantizeXMLElementValue(element5, "Amount");



                    }
                }
                if (childCareBenefits != null)
                {
                    foreach (XElement element6 in childCareBenefits.Descendants("ChildCareTaxBenefit"))
                    {
                        Model.GetPensionChildCareValue(Model.SantizeXMLElementValue(element6, "Type"));
                        Model.SantizeXMLElementValue(element6, "Amount");


                        ;
                    }
                }

            }
        }
    }

    XElement clientCheques = Model.misData.Descendants("ClientCheques").FirstOrDefault();
    if (Model.benefitDate != DateTime.MinValue && clientCheques != null && clientCheques.Descendants("ClientCheque").Any())
    {
        // Previous assistance issued on @
       // Model.previousChequeIssueDate.ToString(ConfigHelper.DATE_FORMAT);

        /** first order by C and D, then show the rest **/
        var orderedData = clientCheques.Descendants("ClientCheque").OrderBy(x =>
        {
            var xElement = x.Element("PaymentIssueCode");
            var element = x.Element("PaymentIssueCode");
            return element != null && element.Value == "C" ? 1 :
                    xElement != null && xElement.Value == "B" ? 1 : 3;
        });

        foreach (XElement clientCheque in orderedData)
        {
            string bankName = "", bankAccountNumber = "", bankAccountHolder = "";

            XElement bankInformation = clientCheque.Descendants("BankInformation").FirstOrDefault();
            if (bankInformation != null)
            {
                bankName = Model.SantizeXMLElementValue(bankInformation, "BankName");
                bankAccountNumber = Model.SantizeXMLElementValue(bankInformation, "AccountNumber");
                bankAccountHolder = Model.SantizeXMLElementValue(bankInformation, "AccountHolder");
            }

            string paymentIssueCode = Model.SantizeXMLElementValue(clientCheque, "PaymentIssueCode");
            string distributionText = Model.GetPaymentDistribution(Model.SantizeXMLElementValue(clientCheque, "PaymentDistributionCode"));
            if (distributionText.ToUpper() == "MAILED")
            {
                if (paymentIssueCode == "B" || paymentIssueCode == "G" || paymentIssueCode == "E")
                {
                    distributionText = "DIRECT DEPOSIT";
                }
                else if (paymentIssueCode == "C")
                {
                    distributionText = "MAILED";
                }
            }
            //Paid to:
            Model.SantizeXMLElementValue(clientCheque, "PayeeName");


            // Payment Method: 
            Model.GetPaymentIssueCode(paymentIssueCode);


            // Amount: $
            Model.SantizeXMLElementValue(clientCheque, "ChequeAmount");


            //Name of Bank: @bankName </ td >


            //Cheque Number:
            Model.SantizeXMLElementValue(clientCheque, "ChequeNumber");


            //Bank Account Number: @bankAccountNumber </ td >





            // Payment Distribution: @distributionText </ td >


            //Bank Account Name: @bankAccountHolder </ td >



        }
    }
}
        
    
