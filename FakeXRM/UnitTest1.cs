using System;
using System.Collections.Generic;
using System.Reflection;
using FakeXrmEasy;
using LucasWorkflowTools;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace FakeXRM
{
    [TestClass]
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {

            var context = new XrmFakedContext();
            context.ProxyTypesAssembly = Assembly.GetExecutingAssembly();
            //    context.CallerId = new EntityReference() { Id = Guid.NewGuid(), Name = "Super Faked User" };

            var service = context.GetFakedOrganizationService();
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='asyncoperation'>
    <attribute name='asyncoperationid' />
    <attribute name='name' />
    <attribute name='regardingobjectid' />
    <attribute name='operationtype' />
    <attribute name='statuscode' />
    <attribute name='ownerid' />
    <attribute name='startedon' />
    <attribute name='statecode' />
    <order attribute='startedon' descending='true' />
    <filter type='and'>
      <condition attribute='statuscode' operator='eq' value='31' />
    </filter>
  </entity>
</fetch>";

            var target = new Entity("workflow")
            {
               Id = new Guid("7B11BECF-EBD8-4962-8DCF-94E8CF42FD2B")
            };



            var inputs = new Dictionary<string, object>() {
                
                {"FetchXMLQuery",fetchXml },
                { "Workflow", target.ToEntityReference() }
            };



            var result = context.ExecuteCodeActivity<StartScheduledWorkflows>(inputs);


            WhoAmIRequest req = new WhoAmIRequest();

            var response = service.Execute(req) as WhoAmIResponse;
            Xunit.Assert.Equal(response.UserId, context.CallerId.Id);

        }
    }
}
