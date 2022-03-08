using System.ComponentModel;
using System.Net;

namespace ECS.Model.Restfuls
{
    public static class ErrorCode
    {
        [Description("성공")]
        public static HttpStatusCode Success { get; } = HttpStatusCode.OK;

        [Description("조회된 데이터 없음")]
        public static HttpStatusCode DataNotFound { get; } = HttpStatusCode.NoContent;

        [Description("Request 오류")]
        public static HttpStatusCode BadRequset { get; } = HttpStatusCode.BadRequest;

        [Description("생성시 데이터가 이미 존재하는 경우")]
        public static HttpStatusCode AlreadyExists { get; } = HttpStatusCode.Conflict;

        [Description("내부 서버 오류")]
        public static HttpStatusCode InternalServerError { get; } = HttpStatusCode.InternalServerError;
    }
}
