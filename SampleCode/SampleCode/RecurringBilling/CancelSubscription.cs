﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNET.Api.Controllers;
using AuthorizeNET.Api.Contracts.V1;
using AuthorizeNET.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class CancelSubscription
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string subscriptionId)
        {
            Console.WriteLine("Cancel Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNET.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            //Please change the subscriptionId according to your request
            var request = new ARBCancelSubscriptionRequest { subscriptionId = subscriptionId };
            var controller = new ARBCancelSubscriptionController(request);                          // instantiate the contoller that will call the service
            controller.Execute();

            ARBCancelSubscriptionResponse response = controller.GetApiResponse();                   // get the response from the service (errors contained if any)

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, Subscription Cancelled With RefID : " + response.refId);
                }
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
