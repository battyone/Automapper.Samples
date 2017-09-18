using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Labo.ServiceModel.Core.Utils.Reflection;
using Labo.ServiceModel.DynamicProxy;
using Labo.WcfTestClient.Win.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automapper.Test
{
    [TestClass]
    public class AutomapperTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var mapper = GetMapper();
            var wsdl = "http://www.dneonline.com/calculator.asmx";
            var serviceInfo = GetServiceInfo(wsdl);
            var endpoint = serviceInfo.EndPoints[0];
            var method = endpoint.ContractInfo.Operations.FirstOrDefault(x => x.Method.Name == "Add")?.Method;
            if (method != null)
            {
                // Return Value
                var returnValue = method.ReturnValue;
                var members = new List<Labo.ServiceModel.Core.Utils.Reflection.Member> {
                    new Labo.ServiceModel.Core.Utils.Reflection.Property { Name = "(return)", Definition = returnValue.Definition, Type = returnValue.Type }
                };
                var returnValues = mapper.Map<List<FSX.Member>>(members);
                // Parameters
                var parameterValue = method.Parameters;
                members = parameterValue.Cast<Labo.ServiceModel.Core.Utils.Reflection.Member>().ToList();
                var parameters = mapper.Map<List<FSX.Member>>(members);

                foreach (var member in parameters)
                {
                    // In Automapper 5.0.2, member.Definition is converting to Class, which is fine
                    // In Automapper 6.1.1, member.Definition is converting to Instance, and nested levels are effected in this object, which is wrong
                    Assert.AreEqual(member.IsClass, true);
                }
            }
        }

        private ServiceInfo GetServiceInfo(string wsdl)
        {
            var proxyFactoryGenerator = new ServiceClientProxyFactoryGenerator(
                new ServiceMetadataDownloader(),
                new ServiceMetadataImporter(
                    new CSharpCodeDomProviderFactory()),
                new ServiceClientProxyCompiler());
            var proxyFactory = proxyFactoryGenerator.GenerateProxyFactory(wsdl);
            var serviceInfo = new ServiceInfo { Wsdl = wsdl, Config = proxyFactory.Config };
            var serviceEndpoints = proxyFactory.Endpoints;

            foreach (var serviceEndpoint in serviceEndpoints)
            {
                var contractDescription = serviceEndpoint.Contract;
                var contractName = contractDescription.Name;

                var proxy = proxyFactory.CreateProxy(serviceEndpoint);

                var operationNames = contractDescription.Operations.Select(x => x.Name).ToArray();
                var contractInfo = new ContractInfo { Proxy = proxy, ContractName = contractName };
                var endPointInfo = new EndPointInfo { BindingName = serviceEndpoint.Binding.Name, ContractInfo = contractInfo };

                foreach (var operationName in operationNames)
                {
                    var instance = proxy.CreateInstance();
                    using (instance as IDisposable)
                    {
                        var method = ReflectionUtils.GetMethodDefinition(instance, operationName);
                        contractInfo.Operations.Add(new OperationInfo { Contract = contractInfo, Method = method });
                    }
                }
                serviceInfo.EndPoints.Add(endPointInfo);
            }
            return serviceInfo;
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<object, FSX.Instance>()
                    .ForMember(d => d.Definition, o => o.Ignore())
                    .ForMember(d => d.Type, o => o.Ignore())
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Instance, FSX.Instance>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Member, FSX.Member>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Property, FSX.Property>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Parameter, FSX.Parameter>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Field, FSX.Field>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Class, FSX.Class>()
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Member, FSX.Member>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Property, FSX.Property>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Parameter, FSX.Parameter>()
                    .Include<Labo.ServiceModel.Core.Utils.Reflection.Field, FSX.Field>()
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Class, FSX.Class>()
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Property, FSX.Property>()
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Parameter, FSX.Parameter>()
                    .PreserveReferences();

                cfg.CreateMap<Labo.ServiceModel.Core.Utils.Reflection.Field, FSX.Field>()
                    .PreserveReferences();
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }
    }
}
