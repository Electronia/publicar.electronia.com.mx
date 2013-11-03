using publicar.electronia.com.mx.Models;
using publicar.electronia.com.mx.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace publicar.electronia.com.mx.Controllers
{
    public class HomeController : Controller
    {

        

        public ActionResult Index()
        {
            ViewBag.Message = "Selecciona el tipo de artículo";

            return View();
        }

        public ActionResult Categoria(string id)
        {
           

            //consumir el servicio qu epoblas categorias 
            PublicationService publicationService = new PublicationService();
            List<Category> categorias = new List<Category>();
            categorias = publicationService.getCategory(id);
            ViewBag.Categories = categorias;           
            //ViewBag.Message = ""+categorias[0].category_name;

            return View();
        }

        public void CategoriaAjax(string id)
        {


            //consumir el servicio qu epoblas categorias 
            PublicationService publicationService = new PublicationService();
            List<Category> categorias = new List<Category>();
            categorias = publicationService.getCategory(id);
           // JsonResult data_category = new JsonResult();
           // Response.Write( categorias);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(categorias);
            Response.Write(json);

  
        }

        public ActionResult datosArticulo(string id)
        {
            ViewBag.Message = id;
            return View();
        }

        public ActionResult Fotos (string id)
        {
         
            PublicationService publicationService = new PublicationService();
            if (string.IsNullOrEmpty(id))
            {
               
                string _categoryID = Request.Form["category_id"];
                string _title = Request.Form["item_title"];
                string _price = Request.Form["item_price"];
                string _typeCurrency = Request.Form["item_typeCurrency"];
                string _typeSeller = Request.Form["item_type_seller"];
                string _condition = Request.Form["item_condition"];
                string _description = Request.Form["description"];


                // generamos el item, procesamos el userid y consumimos el post
                Item item = new Item
                {
                    title = _title,
                    categoryId = _categoryID,
                    state = 1,
                    price = decimal.Parse(_price),
                    typeCurrency = _typeCurrency,
                    status = "active",
                    typeItem = "free",
                    description = _description,
                    usage = _condition
                };
                // obtenemos el id del item generado 

                var userid = "mdzRREFZxGmgCw8omPbYGQ==";
                item = publicationService.postItem(item, userid);
                id = item.itemId;
            }
           
            
           // una vez con el item generado comenzamos el procesado de fotos
            //    Response.Write ("El item generado es"+id);

            ViewBag.Message = id; //"DTM2013100081"
            return View();
        }

        

        public void process_picture(string id)
        {

            
            // generamos toda la logica del procesamiento de fotos

            string item_id;
            string option;
            string photo;
            try
            {

                item_id = id;
            }
            catch
            {
                item_id = "0";
            }

            try
            {
                option = Request.QueryString["option"];
                photo = Request.QueryString["photo"];
            }
            catch
            {
                option = "";
                photo = "";
            }

            string identificador = item_id;

            if (option == "delete")
            {
                //Response.Write("Entro a eliminar la foto" + photo + "con el id "+identificador);
                pictureProcessService processPicture = new pictureProcessService();
                Response.Write( processPicture.eliminaFoto(identificador, int.Parse(photo)));
            }

            else
            {
                pictureProcessService processPicture = new pictureProcessService();
                HttpFileCollectionBase Files;
                Files = Request.Files;

                Response.Write(processPicture.creaFotos(identificador, Files));
            }


        }


        public ActionResult Preview(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Response.Write("EL ideintificador no existe");
            }
            PublicationService publicationService = new PublicationService();
            Item item = new Item();

            item = publicationService.getItem(id);

            ViewBag.Message = "El id del articulo es:"+item.itemId;
            ViewBag.Category = item.categoryId;
            ViewBag.Title = item.title;
            ViewBag.Price = item.price;
            ViewBag.Currency = item.typeCurrency;
            ViewBag.Condition = item.usage;
            ViewBag.Description = item.description;
            if (item.pictures.Count() > 0)
            {
                ViewBag.FotoMain = item.pictures[0].url_general.Replace("_original.","_300.");
            }
            else
            {
                ViewBag.FotoMain = "http://usados.autoplaza.com.mx/css/images/logo.png";
            }
            

            return View();
        }

    }

    
}
