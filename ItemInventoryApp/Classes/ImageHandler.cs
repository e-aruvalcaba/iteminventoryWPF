using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Classes
{
    class ImageHandler
    {
        public ImageHandler()
        {

        }

        public string SaveImageToLocal(int id, byte[] imageInfo)
        {
            string path = "Images/" + id + ".jpg";

            if (!Directory.Exists("Images/"))
            {
                Directory.CreateDirectory("Images/");
            }

            //Guardar la imagen en la carpeta images
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    //path = "Images/" + id +"_"+ ".jpg";
                }
                File.WriteAllBytes(path, imageInfo);

                var imagen = File.Open("Images/" + id + ".jpg", FileMode.Open);
                string newPath = imagen.Name;
                imagen.Close();

                return newPath;
            }
            catch (Exception ex)
            {

                return "";
            }
            
        }
    }
}
