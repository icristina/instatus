using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Instatus.Data;
using System.ComponentModel.Composition;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;

namespace Instatus.Services
{
    // http://stackoverflow.com/questions/895901/exception-logging-for-wcf-services-using-elmah
    // http://codeidol.com/csharp/wcf/Faults/Error-Handling-Extensions/
    public class BaseWcfService : IServiceBehavior, IErrorHandler
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ((ChannelDispatcher)channelDispatcherBase).ErrorHandlers.Add(this);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public virtual bool HandleError(Exception error)
        {
            using (var db = BaseDataContext.Instance())
            {
                db.LogError(error);
                db.SaveChanges();
            }
            return false;
        }

        public virtual void ProvideFault(Exception error, MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {

        }

        public virtual Message CreateMessage<T>(MessageVersion version, T graph)
        {
            return Message.CreateMessage(version, string.Empty, graph, new DataContractJsonSerializer(graph.GetType()));
        }
    }

    public static class MessageExtensions
    {
        public static Message AsJson(this Message message, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            message.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            var response = WebOperationContext.Current.OutgoingResponse;
            response.ContentType = "application/json";
            response.StatusCode = httpStatusCode;

            return message;
        }
    }
}