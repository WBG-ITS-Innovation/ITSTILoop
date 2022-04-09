
namespace ITSTILoopDTOLibrary
{
    public interface IHttpPostClient
    {
        Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, Uri endpoint);
    }
}