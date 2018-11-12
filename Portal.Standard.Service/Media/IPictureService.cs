using Portal.Standard.Core.Media;

namespace Portal.Standard.Service.Media
{
    public interface IPictureService
    {
        Picture GetPictureById(long pictureId);
        string GetPictureUrl(object picture);
        Picture InsertPicture(byte[] binaryFormatFile, string contentType, bool isNew = true,
            bool validateBinary = true);
    }
}
