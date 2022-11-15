
namespace ITSTILoopSampleFSP.Services
{
    public interface IHttpPostClient
    {
        Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, string endpoint, string clientName = "");        
    }
}