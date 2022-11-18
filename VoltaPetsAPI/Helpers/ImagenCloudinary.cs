using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using VoltaPetsAPI.Models;

namespace VoltaPetsAPI.Helpers
{
    public class ImagenCloudinary
    {
        public static void EliminarImagenHosting(Cloudinary cloudinary, Imagen imagen)
        {
            if(imagen != null)
            {
                var deletionParams = new DeletionParams(imagen.Public_Id);
                cloudinary.Destroy(deletionParams);
            }
            
        }

    }
}
