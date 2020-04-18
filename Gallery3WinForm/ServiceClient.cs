using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Using a DTO to tranfer data from the client back to the Server 
/// </summary>
namespace Gallery3WinForm
{
    public static class ServiceClient
    {
        internal async static Task<List<string>> GetArtistNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
            (await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtistNames/"));
        }

        internal async static Task<clsArtist> GetArtistAsync(string prArtistName)
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<clsArtist>
            (await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtist?Name=" + prArtistName));
        }

        internal async static Task<string> InsertArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PostArtist", "POST");
        }

        internal async static Task<string> InsertWorkAsync(clsAllWork prWork)
        {
            return await InsertOrUpdateAsync(prWork, "http://localhost:60064/api/gallery/PostArtWork", "POST");
        }

        internal async static Task<string> UpdateWorkAsync(clsAllWork prWork)
        {
            return await InsertOrUpdateAsync(prWork, "http://localhost:60064/api/gallery/PutArtWork", "PUT");
        }

        internal async static Task<string> UpdateArtistAsync(clsArtist prArtist)
        {
            return await InsertOrUpdateAsync(prArtist, "http://localhost:60064/api/gallery/PutArtist", "PUT");
        }

        /// <summary>
        /// Client talks to a ReST server and attaches data in JSON format for a PUT or POST request
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="prItem"></param>
        /// <param name="prUrl"></param>
        /// <param name="prRequest"></param>
        /// <returns></returns>

        private async static Task<string> InsertOrUpdateAsync<TItem>(TItem prItem, string prUrl, string prRequest)
        {
            using (HttpRequestMessage lcReqMessage = new HttpRequestMessage(new HttpMethod(prRequest), prUrl))
            using (lcReqMessage.Content = new StringContent(JsonConvert.SerializeObject(prItem), Encoding.UTF8, "application/json"))
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.SendAsync(lcReqMessage);
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Requesting that the selfhost DELETEs an artist 
        /// </summary>
        /// <param name="lcKey"> The ID for the artist in the selfhost</param>
        /// <returns></returns>
        internal async static Task<string> DeleteArtist(string lcKey)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                ($"http://localhost:60064/api/gallery/DeleteArtist?ArtistName={lcKey}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }

        internal async static Task<string> DeleteArtworkAsync(clsAllWork prWork)
        {
            using (HttpClient lcHttpClient = new HttpClient())
            {
                HttpResponseMessage lcRespMessage = await lcHttpClient.DeleteAsync
                ($"http://localhost:60064/api/gallery/DeleteArtWork?WorkName={prWork.Name}&ArtistName={prWork.ArtistName}");
                return await lcRespMessage.Content.ReadAsStringAsync();
            }
        }
    }
}
