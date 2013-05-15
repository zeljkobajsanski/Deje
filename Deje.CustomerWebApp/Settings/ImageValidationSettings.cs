using DevExpress.Web.ASPxUploadControl;

namespace Deje.CustomerWebApp.Settings
{
    public class ImageValidationSettings : ValidationSettings
    {
        public ImageValidationSettings()
        {
            AllowedFileExtensions = new string[]{".jpeg", ".jpg", ".png", ".bmp"};
            ShowErrors = true;
            NotAllowedFileExtensionErrorText = "Izabrana datoteka nije dozvoljena";
            MaxFileSize = 500*1024;
            MaxFileSizeErrorText = "Veličina datoteka je ograničena na 500kb";
        }
    }
    
}