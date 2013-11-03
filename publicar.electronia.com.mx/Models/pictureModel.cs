using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace publicar.electronia.com.mx.Models
{
    public class Picture
    {
        public string nombre {get; set;}
        public int ancho {get; set;}
        public int alto {get; set;}
        public bool principal{get; set;}
        public string path {get; set;}
        public string url {get; set;}
        public System.Drawing.Image imgOriginal {get; set;}
        public int numFoto {get; set;}
        public string titulo { get; set; }
        public int[] Mascara = new int[6] { 1, 0, 0, 1, 1, 1 };
        public string[] tamanios = new string[6] { "640,480", "60,60", "50,35", "300,200", "140,100", "100,75" };
        public string[] nombres = new string[6] { "_zoom.jpg", "_60.jpg", "_50.jpg", "_300.jpg", "_140.jpg", "_100.jpg" };
    }

    public class Directorio
    {
        public string path { get; set; }
        public string url { get; set; }
    }

    public class Archivo
    {
        public string path { get; set; }
        public string nombre { get; set; }
        public string extension { get; set; }
        public string contenido { get; set; }
        public string url { get; set; }
    }

}