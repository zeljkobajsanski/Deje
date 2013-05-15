using System.IO;

namespace Deje.Core.Services
{
    public interface IPictureStorageService
    {
        string SacuvajSlikuArtikla(Stream stream, string contentType, string type);

        string SacuvajSlikuDobavljaca(Stream fileContent, string contentType, string type);

        byte[] VratiSlikuDobavljaca(string putanja);

        byte[] VratiSlikuArtikla(string putanja);
    }
}