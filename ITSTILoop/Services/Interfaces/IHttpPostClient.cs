
using ITSTILoopDTOLibrary;

namespace ITSTILoop.Services.Interfaces
{
    public interface IHttpPostClient
    { 
        Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, Uri endpoint, string clientName = "");
    }
}