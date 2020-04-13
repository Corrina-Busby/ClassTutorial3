using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gallery3Selfhost 
{
    public class GalleryController : System.Web.Http.ApiController
    {
        public List<string> GetArtistNames()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();
            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }
    

    }
}
