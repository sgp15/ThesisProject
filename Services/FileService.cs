using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThesisProject.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;
    using System.IO;

    namespace EmailOrganizer.Services
    {
        public class JSONService<T>
        {
            /// <summary>
            /// Reads a JSON file and returns the object
            /// </summary>
            /// <returns>The deserialized result.</returns>
            public T ConvertJSONToObject(string jsonFileName)
            {
                try
                {

                    string json = File.ReadAllText(jsonFileName);

                    T deserializedJsonOutput = JsonConvert.DeserializeObject<T>(json);

                    return deserializedJsonOutput;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Writes the object to the specified by converting into JSON format
            /// </summary>
            /// <param name="obj"></param>
            public void WriteToJSONFile(T obj, string jsonFileName)
            {
                try
                {
                    string jsonString = JsonConvert.SerializeObject(obj);
                    File.WriteAllText(jsonFileName, jsonString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
