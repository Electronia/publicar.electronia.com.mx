using publicar.electronia.com.mx.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace publicar.electronia.com.mx.Services
{
    public class pictureManagerService
    {
        // todo lo que usa el sistema de fotos
        MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
        HttpClient client = new HttpClient();
        string urlApiBase = "http://detocho.azurewebsites.net";
        
        //---------------
        public Picture CrearFoto(string identificador,  string nombre, int numFoto, int ancho, int alto, Directorio dir)
        {
            
            Picture foto = new Picture();

            foto.path = dir.path;
            foto.alto = alto;
            foto.ancho = ancho;
            foto.principal = true;
            foto.nombre = nombre;
            foto.url = dir.url;
            foto.titulo = ProcesaTitulo(foto.nombre);
            foto.numFoto = numFoto;
            foto.imgOriginal = System.Drawing.Image.FromFile(dir.path + nombre);
           // foto.imgOriginal.Dispose();

            return foto;

        }

        public void CreaPaquetes(Picture foto)
        {
            // debo crear cuantas fotos=
            /*
               [ ancXalt]		descripcion del lugar								Nombre designado
                [100x75]		para los anuncios patrocinados						_100.jpg
                [140X100] 		para el home y los listados 						_140.jpg
                [300X200] 		para la foto mediana de la galeria					_300.jpg
                [50X35] 		para los tmbails de la galeria						_50.jpg
                [60X60] 		para los tumbails de la carga de fotografias		_60.jpg
                [original]		para el zoom 										_zoom.jpg
             */

            for (int i = 0; i < 6; i++)
            {
                int W = int.Parse(foto.tamanios[i].Split(',')[0]);
                int H = int.Parse(foto.tamanios[i].Split(',')[1]);
                Redimensiona(foto,W, H, 100, foto.nombre.ToLower().Replace("_original.jpg", "") + foto.nombres[i], foto.Mascara[i]);
            }

            foto.imgOriginal.Dispose();
        }
        private void Redimensiona(Picture foto,int targetW, int targetH, int resolucion, string name, int agregarMascara)
        {

            // generamos un memorystream de la imagen origina
            MemoryStream imagen = new MemoryStream();
            foto.imgOriginal.Save(imagen, ImageFormat.Bmp); // salvamos en el memory la imagen original
            System.Drawing.Image original = System.Drawing.Image.FromStream(imagen);
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(imagen);

            // No vamos a permitir que la imagen final sea más grande que la inicial
            targetH = targetH <= original.Height ? targetH : original.Height;
            targetW = targetW <= original.Width ? targetW : original.Width;

            // se genera una foto con la dimension pedida
            Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);

            // No vamos a permitir dar una resolución mayor de la que tiene
            resolucion = resolucion <= Convert.ToInt32(bmPhoto.HorizontalResolution) ? resolucion : Convert.ToInt32(bmPhoto.HorizontalResolution);
            resolucion = resolucion <= Convert.ToInt32(bmPhoto.VerticalResolution) ? resolucion : Convert.ToInt32(bmPhoto.VerticalResolution);

            // se le da la resolucion a la foto creada
            bmPhoto.SetResolution(resolucion, resolucion);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

            // geenramos un memorystream de la foto 
            MemoryStream mm = new MemoryStream();
            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);

            // aqui colocamos el codigo para agregara mascara enviando la imagen a laa que se debe de tener mascara (mm)
            //Mezcla(stream, mascara, 300, 75, 10, 10, false, _path, _nombre, titulo);

            if (agregarMascara == 1)
            {
                Stream imagenProcesada = new MemoryStream();
                imagenProcesada = addMascara(mm, targetW, targetH, 10, 10, false, foto.titulo);
                System.Drawing.Image ImagenSave = System.Drawing.Image.FromStream(imagenProcesada);
                Rectangle rec = new Rectangle(0, 0, ImagenSave.Width, ImagenSave.Height);
                grPhoto.DrawImage(ImagenSave, rec, rec, GraphicsUnit.Pixel);
            }



            bmPhoto.Save(foto.path + "\\" + name);

            original.Dispose();
            imgPhoto.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();
            mm.Dispose();
            //foto.imgOriginal.Dispose();


            //return mm;
        }
        // revisar bien la difinicion de esto
        private Stream addMascara(Stream imagenOrigen, double anchorelativo, double altorelativo, double posrelativaX, double posrelativaY, bool relativo, string titulo)
        {
            // genero la mascara que utilizare
            // depnde del ancho relativo entonces se coloca la mascara

            string rutaMarca = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\archivos\\marcaagua2.png"; // obtener del config ver si puede ser una URL
            switch (int.Parse(anchorelativo.ToString()))
            {
                case 640:
                    rutaMarca = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\archivos\\marca_640.png"; // obtener del config ver si puede ser una URL
                    break;
                case 140:
                    rutaMarca = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\archivos\\marca_140.png"; // obtener del config ver si puede ser una URL
                    break;
                case 300:
                    rutaMarca = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\archivos\\marca_300.png"; // obtener del config ver si puede ser una URL
                    break;
                default:
                    rutaMarca = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\archivos\\marca_640.png"; // obtener del config ver si puede ser una URL
                    break;

            }

            System.Drawing.Image marcaAgua = System.Drawing.Image.FromFile(rutaMarca);
            MemoryStream imagenSuperpuesta = new MemoryStream();
            marcaAgua.Save(imagenSuperpuesta, ImageFormat.Png);

            System.Drawing.Image original = System.Drawing.Image.FromStream(imagenOrigen);
            System.Drawing.Image superpuesta = System.Drawing.Image.FromStream(imagenSuperpuesta);

            double posicionX;
            double posicionY;
            double ancho;
            double alto;

            if (relativo)
            {
                posicionX = posrelativaX * original.Width;
                posicionY = posrelativaY * original.Height;
                ancho = anchorelativo * original.Width;
                alto = altorelativo * original.Height;
            }
            else
            {
                posicionX = posrelativaX;
                posicionY = posrelativaY;
                ancho = anchorelativo;
                alto = altorelativo;
            }


            System.Drawing.Image imgPhoto = System.Drawing.Image.FromStream(imagenOrigen);
            Bitmap bmPhoto = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rec = new Rectangle(0, 0, original.Width, original.Height);
            grPhoto.DrawImage(imgPhoto, rec, rec, GraphicsUnit.Pixel);
            grPhoto.DrawImage(superpuesta, new Rectangle((int)posicionX, (int)posicionY, (int)ancho, (int)alto), new Rectangle(0, 0, superpuesta.Width, superpuesta.Height), GraphicsUnit.Pixel);

            // rotulamos solo si es la zoom y la de 300
            if (anchorelativo == 640 || anchorelativo == 300)
            {
                int t = 11;  // tamaño de letra en puntos  Equivalencia es  12pts  = 16 px
                int tpix = (t * 16) / 12; //tamaño de letra en pixeles
                while (0 == getTamanioLetraPuntoInicial(t, anchorelativo, (titulo.Length * tpix)))
                {
                    t = t - 1;
                    tpix = (t * 16) / 12;
                }
                if (t < 0) { t = 1; }
                tpix = (t * 16) / 12; //tamaño de letra en pixeles
                // procesamos el tamaño de letra segun sea el caso de la imagen
                int L;
                L = titulo.Length * tpix;
                float x, y;
                x = float.Parse(((anchorelativo / 2) - (L / 2)).ToString());
                y = float.Parse((altorelativo - tpix - 10).ToString());

                Font drawFont = new Font("Arial", t);
                SolidBrush drawBrush = new SolidBrush(Color.White);

                StringFormat drawFormat = new StringFormat();
                drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                grPhoto.DrawString(titulo, drawFont, drawBrush, x, y, drawFormat);
            }
            // terminamos de rotular

            MemoryStream mm = new MemoryStream();
            bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);

            //bmPhoto.Save(_path + "\\preubasmarcas_"+anchorelativo.ToString() + _nombre);

            original.Dispose();
            imgPhoto.Dispose();
            bmPhoto.Dispose();
            grPhoto.Dispose();

            return mm;

        }
        private int getTamanioLetraPuntoInicial(int t, double anchorelativo, int L)
        {
            int datos;
            double x;
            x = (anchorelativo / 2) - (L / 2);
            if (L > (anchorelativo - x))  // si la longitud del texto  es mayor a tres cuartos de la imagen entonces reduce los puntos de letras
            {
                datos = 0;
            }
            else
            {
                datos = 1;
            }
            return datos;
        }
        public void eliminarFoto(string path_foto)
        {

            string archivoEliminar;
            Picture pic =  new Picture();
            for (int i = 0; i < 6; i++)
            {

                archivoEliminar = path_foto.ToLower().Replace("_original.jpg", "") + pic.nombres[i];
                if (System.IO.File.Exists(archivoEliminar))
                {
                    try
                    {
                        System.IO.File.Delete(archivoEliminar);
                    }

                    catch (System.IO.IOException e)
                    {
                        // guardamos un log de  por que no se logro la eliminacion 
                    }
                }
            }
            archivoEliminar = (path_foto);
            if (System.IO.File.Exists(path_foto))
            {
                try
                {
                    System.IO.File.Delete(path_foto);
                }

                catch (System.IO.IOException e)
                {
                    // guardamos un log de  por que no se logro la eliminacion 
                }
            }

            // por cada nombre hacemos el borrado


        }

        private string ProcesaTitulo(string nombre)
        {
            string[] tituloA;
            tituloA = nombre.Split('_');
            string tituloB = tituloA[0];
            string titulo;
            titulo = tituloB.Replace("--D", " [D");
            titulo = titulo + " ]";
            return titulo;

        }

        //---------------
        public Directorio CreaDirectorio(string item_id)
        {

            Directorio directorio = new Directorio();
            string anioRegistro;
            string mesRegistro;
            string dominio_raiz = "http://localhost/pics/";
            string path_raiz = "D:\\websites\\Electronia\\web.electronia\\web.electronia\\pics\\";
            //"D:\\websites\\fotos.electronia.com.mx\\pics\\";
            
            DateTime dt = DateTime.Now;
            anioRegistro = dt.Year.ToString();
            if (dt.Month < 10) { mesRegistro = "0" + dt.Month; } else { mesRegistro = dt.Month.ToString(); }
            
            string newPath = System.IO.Path.Combine(path_raiz, anioRegistro);
            newPath = System.IO.Path.Combine(newPath, mesRegistro);
            newPath = System.IO.Path.Combine(newPath, item_id);

            if (!System.IO.Directory.Exists(newPath))
            {
                System.IO.Directory.CreateDirectory(newPath);
            }

            directorio.url = dominio_raiz + anioRegistro + "/" + mesRegistro + "/" + item_id + "/";
            directorio.path = newPath+"\\";

            return directorio;
        }


        public void EliminaDirectorio(string nombre)
        {
            // debemos investigar  como eliminar un directorio
        }


        public int getUltimaFoto(string item_id)
        {
            //List<Picture> categories = new List<Picture>();
            // hacemos el get y obtenemos los datos

            int max_photo = 0;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage resp = client.GetAsync("/Picture/" + item_id).Result;
            if (resp.IsSuccessStatusCode)
            {
                var data_categories = resp.Content.ReadAsAsync<List<Category>>().Result;

                foreach (var category in data_categories)
                {
                    max_photo = max_photo + 1;
                }
            }

            return max_photo;
        }

        public Foto postFoto(string item_id, Foto new_photo)
        {
            Foto foto = new Foto();
            // haces post
            HttpContent content = new ObjectContent<Foto>(new_photo, jsonFormatter);
            client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!

            var resp = client.PostAsync("/Picture/" + item_id, content).Result;

            if (resp.IsSuccessStatusCode)
            {
               var result = resp.Content.ReadAsAsync<Foto>().Result;
               foto = new Foto
               {
                   id = result.id,
                   item_id = result.item_id,
                   num_photo = result.num_photo,
                   url_general = result.url_general
               };
            }

            return foto;

        }

        public List<Foto> getFotos(string item_id)
        {
            List<Foto> fotos = new List<Foto>();
            client = new HttpClient();
            // haces get
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage respPost = client.GetAsync("/Picture/" + item_id).Result;
            if (respPost.IsSuccessStatusCode)
            {
                var data_fotos = respPost.Content.ReadAsAsync<List<Foto>>().Result;

                foreach (var array_photos in data_fotos)
                {
                    fotos.Add(
                      new Foto
                      {
                          id = array_photos.id,
                          item_id = array_photos.item_id,
                          num_photo = array_photos.num_photo,
                          url_general = array_photos.url_general,
                          path_general = array_photos.path_general

                      });
                }
            }

            return fotos;

        }

        public void delFoto(string item_id, int num_photo)
        {
            //aqui va la logica del delete de la foto 
            //HttpContent content = new ObjectContent<Foto>(new_photo, jsonFormatter);
            client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            var resp = client.DeleteAsync("/Picture/" + item_id + "/?num_photo="+num_photo);
            //var resp = client.PostAsync("/Picture/" + item_id, content).Result;

          
        }


      /*
        //--------------
        public Archivo CrearArchivo(string path, string nombre, string extension)
        {

            Archivo archivo = new Archivo();
            archivo.path = path;
            archivo.nombre = nombre;
            archivo.extension = extension;
            archivo.contenido = "";
            //nombre = nombre + "." + extension;
            if (!File.Exists(archivo.path + archivo.nombre + "." + archivo.extension))
            {

                try
                {
                    string fileName = archivo.path + archivo.nombre + "." + archivo.extension;
                    // esto inserta texto en un archivo existente, si el archivo no existe lo crea
                    File.Create(fileName);
                    
                  //  StreamWriter writer = File.AppendText(fileName);
                    //writer.WriteLine(_contenido);
                  //  writer.Close();
                }
                catch
                {

                }
            }
            
            return archivo;
        }

        public void EliminarArchivo(Archivo archivo)
        {

            //archivonombre = nombre + "." + extension;
            File.Delete(archivo.path + archivo.nombre +"."+archivo.extension);
        }
       
        public string LeerArchivo(Archivo archivo)
        {
            string lectura = "";
            string fileName = archivo.path + archivo.nombre + "." + archivo.extension;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            while (reader.Peek() > -1) lectura += reader.ReadLine();
            reader.Dispose();
            stream.Dispose();
            reader.Close();
            stream.Close();
            
            return lectura;
        }

       

        public void InsertarContenidoArchivo(Archivo archivo)
        {
           
            try
            {
                string fileName = archivo.path + archivo.nombre + "." + archivo.extension;
                // esto inserta texto en un archivo existente, si el archivo no existe lo crea
                StreamWriter writer = File.AppendText(fileName);
                writer.WriteLine(archivo.contenido);
                writer.Flush();
                writer.Dispose();
                writer.Close();
            }
            catch
            {

            }
        }
        public void ActualizaContenidoArchivo(Archivo archivo)
        {
          
            try
            {
                string fileName = archivo.path + archivo.nombre + "." + archivo.extension;

                StreamWriter sw = new StreamWriter(fileName, false, Encoding.ASCII);
                sw.Write(archivo.contenido);
                //close the file
                sw.Flush();
                sw.Dispose();
                sw.Close();
            }
            catch
            {

            }
        }

        */
    }
}