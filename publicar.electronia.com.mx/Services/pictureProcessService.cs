using publicar.electronia.com.mx.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace publicar.electronia.com.mx.Services
{
    public class pictureProcessService
    {
        // todo lo que usa el procesafoto

            

        public void test(string name)
        {
            StreamWriter writer = File.AppendText("D:\\electroniaLogs\\fotosTest\\" + name + ".txt");
            //writer.WriteLine("mi itme creado = " + _query);
            writer.Close();
        }

        public  string creaFotos(string identificador, HttpFileCollectionBase FileCollection)
        {

            pictureManagerService pictureService = new pictureManagerService();
            Directorio dir = new Directorio();
            string nameFile = "electronia--" + identificador;
            string result = "";

            

            if (FileCollection.Count > 0)
            {
                
                dir = new Directorio();
                dir = pictureService.CreaDirectorio(identificador);


                for (int i = 0; i < FileCollection.Count; i++)
                {

                    if (FileCollection[i].FileName.Length > 0)
                    {
                        
                        pictureService = new pictureManagerService();
                        Foto new_photo = new Foto();
                        new_photo.item_id = identificador;
                        new_photo.url_general = dir.url + nameFile;
                        new_photo.path_general = dir.path + nameFile;

                        new_photo = pictureService.postFoto(identificador,  new_photo);

                        if (new_photo.num_photo > 0)
                        {
                            Picture pic = new Picture();
                            string nombre = nameFile + "_" + new_photo.num_photo + "_original.jpg";
                            FileCollection[i].SaveAs(dir.path + nombre);
                            string contenido = "";

                            try
                            {

                                pic = pictureService.CrearFoto(identificador, nombre, new_photo.num_photo, 640, 480, dir);
                                pictureService.CreaPaquetes(pic);

                                //string urlfotos = new_photo.url_general;
                                /*
                                contenidoGallery = "{\"icono\":\"" + urlfotos.Replace("_original.", "_50.") +
                                                          "\",\"individual\":\"" + urlfotos.Replace("_original.", "_300.") +
                                                          "\",\"zoom\":\"" + urlfotos.Replace("_original.", "_zoom.") + "\"}";
                                */
                                List<Foto> lista_photos = new List<Foto>();

                                lista_photos = pictureService.getFotos(identificador);

                                foreach (var photos in lista_photos)
                                {
                                    string urlfotos = photos.url_general;
                                    contenido = "{" +
                                                                     "\"name\":\"" + photos.item_id + "_" + photos.num_photo + "_50.jpg\"," +
                                                                     "\"size\":902604," +
                                                                     "\"url\":\"" + urlfotos.Replace("_original.", "_zoom.") + "\"," +
                                                                     "\"thumbnail_url\":\"" + urlfotos.Replace("_original.", "_60.") + "\"," +
                                                                     "\"delete_url\":\"/home/process_picture/" + photos.item_id + "/?option=delete&photo=" + photos.num_photo + "\"," +
                                                                     "\"delete_type\":\"GET\"" +
                                                                     "},";
                                }

                                if (contenido.Length > 0)
                                {
                                    contenido = "[" + contenido + "]";
                                    contenido = contenido.Replace(",]", "]");
                                }




                            }
                            catch
                            {
                                // cuando no subio la foto
                            }

                            result = contenido;
                        }
                    }
                }
                 
            }
            else
            {
                // obtenemos el get para generar el admin de fotos

                List<Foto> lista_photos = new List<Foto>();
                string contenido = "";

                lista_photos = pictureService.getFotos(identificador);

                foreach (var photos in lista_photos)
                {
                    string urlfotos = photos.url_general;
                    contenido = contenido + "{" +
                                                     "\"name\":\"" + photos.item_id + "_" + photos.num_photo + "_50.jpg\"," +
                                                     "\"size\":902604," +
                                                     "\"url\":\"" + urlfotos.Replace("_original.", "_zoom.") + "\"," +
                                                     "\"thumbnail_url\":\"" + urlfotos.Replace("_original.", "_60.") + "\"," +
                                                     "\"delete_url\":\"/home/process_picture/" + photos.item_id + "/?option=delete&photo=" + photos.num_photo + "\"," +
                                                     "\"delete_type\":\"GET\"" +
                                                     "},";
                }

                if (contenido.Length > 0)
                {
                    contenido = "[" + contenido + "]";
                    contenido = contenido.Replace(",]", "]");
                }

                /*
                Directorio dir = new Directorio();
                dir = pictureService.CreaDirectorio(identificador, anioRegistro, mesRegistro);
                string ruta = dir.path;
                
                Archivo archivoAdmin = new Archivo();
                archivoAdmin = pictureService.CrearArchivo(ruta, identificador + "_admin", "json");
                
                string fotos = pictureService.LeerArchivo(archivoAdmin);
                result = fotos;
                */
                result = contenido;
                
            }




            return result;
        }




        public string  eliminaFoto( string identificador, int photo)
        {
            pictureManagerService pictureService = new pictureManagerService();
            

            List<Foto> lista_photos = new List<Foto>();
            string contenido = "";
            string result = "";

            lista_photos = pictureService.getFotos(identificador);
           

            foreach (var photos in lista_photos)
            {
                string urlfotos = photos.url_general;
                if (photos.id == photo)
                {
                    pictureService.delFoto(identificador, photo);
                    pictureService.eliminarFoto(photos.path_general);
                }
                else
                {
                contenido = contenido + "{" +
                                                 "\"name\":\"" + photos.item_id + "_" + photos.num_photo + "_50.jpg\"," +
                                                 "\"size\":902604," +
                                                 "\"url\":\"" + urlfotos.Replace("_original.", "_zoom.") + "\"," +
                                                 "\"thumbnail_url\":\"" + urlfotos.Replace("_original.", "_60.") + "\"," +
                                                 "\"delete_url\":\"/home/process_picture/" + photos.item_id + "/?option=delete&photo=" + photos.num_photo + "\"," +
                                                 "\"delete_type\":\"GET\"" +
                                                 "},";

                }
                
               

            }

            if (contenido.Length > 0)
            {
                contenido = "[" + contenido + "]";
                contenido = contenido.Replace(",]", "]");
            }

            result = contenido;
            return result;
        }

       

    }
}