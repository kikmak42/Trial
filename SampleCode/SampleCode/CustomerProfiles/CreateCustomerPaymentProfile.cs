﻿using System;
using System.Collections.Generic;
using AuthorizeNET.Api.Controllers;
using AuthorizeNET.Api.Contracts.V1;
using AuthorizeNET.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class CreateCustomerPaymentProfile
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId)
        {
            Console.WriteLine("CreateCustomerPaymentProfile Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNET.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var bankAccount = new bankAccountType
            {
                accountNumber = "01245524321",
                routingNumber = "000000204",
                accountType = bankAccountTypeEnum.checking,
                echeckType = echeckTypeEnum.WEB,
                nameOnAccount = "test",
                bankName = "Bank Of America"
            };

            paymentType echeck = new paymentType {Item = bankAccount};

            var billTo = new customerAddressType
            {
                firstName = "John",
                lastName = "Snow"
            };
            customerPaymentProfileType echeckPaymentProfile = new customerPaymentProfileType();
            echeckPaymentProfile.payment = echeck;
            echeckPaymentProfile.billTo = billTo;

            var request = new createCustomerPaymentProfileRequest
            {
                customerProfileId = customerProfileId,
                paymentProfile = echeckPaymentProfile,
                validationMode = validationModeEnum.none
            };

            //Prepare Request
            var controller = new createCustomerPaymentProfileController(request);
            controller.Execute();

             //Send Request to EndPoint
            createCustomerPaymentProfileResponse response = controller.GetApiResponse(); 
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, createCustomerPaymentProfileID : " + response.customerPaymentProfileId);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                if (response.messages.message[0].code == "E00039")
                {
                    Console.WriteLine("Duplicate ID: " + response.customerPaymentProfileId);
                }
            }

            return response;

        }
    }
}
