using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// In this multi-project scenario I am a little more careful with public/private: the keyword internal in the header 
/// line means the method is accessible for classes in this project, but invisible to other projects 
/// (unlike public items that are potentially visible to other projects if they are referenced).
/// This small piece of code creates a HttpClient object which requests a JSON string 
/// from the specified URL and deserialises it into a .Net object i.e. a List<string>.
/// </summary>
namespace Gallery3WinForm
{
    class ServiceClient
    {
        internal async static Task<List<string>> GetArtistNamesAsync()
        {
            using (HttpClient lcHttpClient = new HttpClient())
                return JsonConvert.DeserializeObject<List<string>>
            (await lcHttpClient.GetStringAsync("http://localhost:60064/api/gallery/GetArtistNames/"));
        }

    }
}
