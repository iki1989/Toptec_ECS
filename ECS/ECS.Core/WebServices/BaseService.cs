using System;
using System.IO;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net;
using Urcis.SmartCode.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;

namespace ECS.Core.WebServices
{
    public class BaseService : IHaveLogger
    {
        protected string senderIp;

        public Logger Logger { get; protected set; }

        protected async Task<string> GetStreamtoStringAsync(Stream body)
        {
            string bodyText = string.Empty;
            using (StreamReader sr = new StreamReader(body))
            {
                try
                {
                    bodyText = await sr.ReadToEndAsync();

                    //Test용
                    //bodyText = File.ReadAllText("D://project//picking.txt");
                }
                catch { }
            }

            return bodyText;
        }

        protected Task<T> JsonDeserializeAsync<T>(string strBody)
        {
            return Task.Run(() =>
            {
                return this.JsonDeserialize<T>(strBody);
            });
        }

        protected T JsonDeserialize<T>(string strBody)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<T>(strBody);
                return (T)Convert.ChangeType(deserialized, typeof(T));
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return default;
            }
        }

        protected Task<string> JsonConvertAsync(object value) => Task.Run(() => JsonConvert.SerializeObject(value));

        protected string GetSenderIp()
        {
            try
            {
                if (OperationContext.Current != null)
                {
                    MessageProperties oMessageProperties = OperationContext.Current.IncomingMessageProperties;
                    RemoteEndpointMessageProperty oRemoteEndpointMessageProperty = (RemoteEndpointMessageProperty)oMessageProperties[RemoteEndpointMessageProperty.Name];
                    //int nPort = oRemoteEndpointMessageProperty.Port;

                    this.senderIp = oRemoteEndpointMessageProperty.Address;
                }

                
            }
            catch { }

            return this.senderIp;
        }

        protected void WrtieLog(string text, Type type, string strBody)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{text}(Sender : {this.senderIp}) : {type}");
            sb.Append($"{strBody})");

            this.Logger?.Write(sb.ToString());
        }

        protected void WrtieLog(string text, string strBody)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{text}(Sender : {this.senderIp})");
            sb.Append($"{strBody})");

            this.Logger?.Write(sb.ToString());
        }

        protected async Task<T> ParseBody<T>(Stream body)
        {
            this.senderIp = this.GetSenderIp();
            try
            {
                var type = typeof(T);

                if (body == null)
                {
                    this.WrtieLog("[Recived]",type, "Stream Body is null");
                    return default;
                }

                string strBody = await this.GetStreamtoStringAsync(body);
                if (string.IsNullOrEmpty(strBody))
                {
                    this.WrtieLog("[Recived]", type, "Body is null or Empty");
                    return default;
                }
                this.WrtieLog("[Recived]", type, strBody);

                strBody = strBody.Replace('(', ' ').Replace(')', ' '); //소괄호가 있어서 파싱이 안됨..
                var deserialize = await this.JsonDeserializeAsync<T>(strBody);
                if (deserialize == null)
                {
                    this.WrtieLog("[Recived]", type, "JsonDeserialize is null");
                    return default;
                }

                return (T)Convert.ChangeType(deserialize, typeof(T));
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return default;
            }
        }

        protected async Task<string> GetStrBody(Stream body)
        {
            if (body == null)
            {
                this.WrtieLog("[Recived]", "Stream Body is null");
                return string.Empty;
            }

            string strBody = await this.GetStreamtoStringAsync(body);
            if (string.IsNullOrEmpty(strBody))
            {
                this.WrtieLog("[Recived]", "Body is null or Empty");
                return string.Empty;
            }
            this.WrtieLog("[Recived]", strBody);

            return strBody;
        }
    }

    public enum ServiceTypeEnum
    {
        WCS,
        RICP,
    }
}
