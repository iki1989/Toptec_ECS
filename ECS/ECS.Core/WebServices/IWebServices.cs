using ECS.Model.Restfuls;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace ECS.Core.WebServices
{
    #region Default
    [ServiceContract]
    public interface IDefaultRestfulServerService
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/")]
        string GetServiceAlive();
    }
    #endregion

    [ServiceContract]
    public interface IWcsRestfulServerService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "order")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> OrderPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "orderCancel")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> OrderCancelPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "orderDelete")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> OrderDeletePostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "operatorWeightResult")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> OperatorWeightResultPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "skuMaster")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> SkuMasterPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "container/mapping")]
        [return: MessageParameter(Name = "Json")]
        Task<WcsResponse> ContainerMappingPostAsync(Stream body);
    }

    [ServiceContract]
    public interface IRicpRestfulServerService
    {
        //[OperationContract]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "container/mapping")]
        //[return: MessageParameter(Name = "Json")]
        //Task<RicpResponse> ContainerMappingPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "location/point/push/callback")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> LocationPointPushCallbackPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rolltainer/schedule/setting")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> RolltainerScheduleSettingPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "pickingResultsImport")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> PickingResultsImportPostAsync(Stream body);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rms/status/setting/callback")]
        [return: MessageParameter(Name = "Json")]
        Task<RicpResponse> RmsStatusSettingCallbackPostAsync(Stream body);
    }
}
