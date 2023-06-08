namespace ITSTILoopLibrary.UtilityServices.Interfaces
{
    public interface IHttpPostClient
    {
        Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, string endpoint, string clientName = "");
        Task<HttpPostClientResponse<TResponseType>> PostAsync<TPostType, TResponseType>(TPostType postContent, Uri endpoint, string clientName = "");
    }
}