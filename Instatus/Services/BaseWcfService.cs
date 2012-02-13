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
using Instatus.Web;

namespace Instatus.Services
{
    // http://stackoverflow.com/questions/895901/exception-logging-for-wcf-services-using-elmah
    // http://codeidol.com/csharp/wcf/Faults/Error-Handling-Extensions/
    // http://stackoverflow.com/questions/1287802/access-request-body-in-a-wcf-restful-service
    public class BaseWcfService : IServiceBehavior, IErrorHandler, IDispatchMessageInspector
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {           
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers.Cast<ChannelDispatcher>())
            {
                channelDispatcher.ErrorHandlers.Add(this);  
            
                foreach(EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints) {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public virtual bool HandleError(Exception error)
        {
            using (var db = WebApp.GetService<IBaseDataContext>())
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

        public virtual string GetAuthorizationPolicy(Message request)
        {
            return HttpUtility.UrlDecode(request.Properties.Via.AbsoluteUri);
        }

        public virtual string GetAuthorizationHash(Message request)
        {
            return HttpUtility.UrlDecode(request.HttpRequestMessageProperty().Headers[HttpRequestHeader.Authorization]);
        }

        public virtual string GetAuthorizationSecret()
        {
            return null;
        }

        public virtual string GenerateHash(string policy, string secret)
        {
            return policy.ToEncrypted(secret);
        }

        private static LimitedQueue<string> hashes = new LimitedQueue<string>(10000);

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var secret = GetAuthorizationSecret();
            var policy = GetAuthorizationPolicy(request);
            
            if (!secret.IsEmpty() && !request.HttpRequestMessageProperty().Method.Match("GET"))
            {
                var hash = GetAuthorizationHash(request);
                
                if (hashes.Contains(hash))
                    throw new Exception("Duplicate hash");

                hashes.Enqueue(hash);

                if (!(hash == GenerateHash(policy, secret)))
                {
                    throw new Exception("Invalid hash");
                }
            }            
            
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            
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

        public static HttpRequestMessageProperty HttpRequestMessageProperty(this Message message)
        {
            return message.Properties["httpRequest"] as HttpRequestMessageProperty;
        }
    }
}