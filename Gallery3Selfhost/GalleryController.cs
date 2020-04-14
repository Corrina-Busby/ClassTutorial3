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
        #region ### Get ALL Artist Names ###
        public List<string> GetArtistNames()
        {
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT Name FROM Artist", null);
            List<string> lcNames = new List<string>();

            foreach (DataRow dr in lcResult.Rows)
                lcNames.Add((string)dr[0]);
            return lcNames;
        }
        #endregion

        #region ### Get ONE Artist ###
        public clsArtist GetArtist(string Name)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(1);
            par.Add("Name", Name);
            DataTable lcResult = clsDbConnection.GetDataTable("SELECT * FROM Artist WHERE Name = @Name", par);
            // unpacking the data-table and sticks the values from the db into a new clsArtist DTO
            if (lcResult.Rows.Count > 0)
                return new clsArtist()
                {
                    Name = (string)lcResult.Rows[0]["Name"],
                    Speciality = (string)lcResult.Rows[0]["Speciality"],
                    Phone = (string)lcResult.Rows[0]["Phone"]
                };
            else
                return null;
        }
        #endregion

        #region ### UPDATE/PUT an Artist ###
        public string PutArtist(clsArtist prArtist)
        {   // update
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "UPDATE Artist SET Speciality = @Speciality, Phone = @Phone WHERE Name = @Name",
                    prepareArtistParameters(prArtist));

                if (lcRecCount == 1)
                    return "One artist updated";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        #endregion

        #region ### POST/INSERT/ADD an Artist ###
        public string PostArtist(clsArtist prArtist)
        {   // Insert/ADDED
            try
            {
                int lcRecCount = clsDbConnection.Execute(
                    "INSERT INTO Artist (Name, Speciality, Phone) VALUES (@Name, @Speciality, @Phone)",
                    prepareArtistParameters(prArtist));

                if (lcRecCount == 1)
                    return "One artist Inserted";
                else
                    return "Unexpected artist update count: " + lcRecCount;
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }
        #endregion


        private Dictionary<string, object> prepareArtistParameters(clsArtist prArtist)
        {
            Dictionary<string, object> par = new Dictionary<string, object>(3);
            par.Add("Name", prArtist.Name);
            par.Add("Speciality", prArtist.Speciality);
            par.Add("Phone", prArtist.Phone);
            return par;
        }

    }
}
