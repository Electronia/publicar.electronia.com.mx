using publicar.electronia.com.mx.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace publicar.electronia.com.mx.Services
{
    public class PublicationService 
    {
        MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
        HttpClient client = new HttpClient();
        string urlApiBase = "http://detocho.azurewebsites.net";

        public List<Category> getCategory(string id){
            
            List<Category> categories = new List<Category>();
            // hacemos el get y obtenemos los datos


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage resp = client.GetAsync("/category/" + id).Result;
            if (resp.IsSuccessStatusCode)
            {
                var data_categories = resp.Content.ReadAsAsync<List<Category>>().Result;

                foreach (var category in data_categories)
                {
                    categories.Add(
                      new Category
                      {
                          category_id = category.category_id,
                          category_name = category.category_name

                      });
                }
            }

            return categories;

        }

        public Item postItem(Item item, string userid)
        {
            Item itemNew = new Item();
            HttpContent content = new ObjectContent<Item>(item, jsonFormatter);

            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!

            var resp = client.PostAsync("/item/?userid="+userid, content).Result;

            if (resp.IsSuccessStatusCode)
            {
                var result = resp.Content.ReadAsAsync<Item>().Result;
                itemNew =
                  new Item
                  {
                       itemId = result.itemId,
                       title = result.title, 
                       categoryId = result.categoryId,
                       state = result.state,
                       price  = result.price,
                       typeCurrency = result.typeCurrency,
                       status = result.status,
                       typeItem  = result.typeItem,
                       dateRegistration = result.dateRegistration,
                       dateExpired  = result.dateExpired,
                       dateActivation = result.dateActivation,
                       dateRenovation = result.dateRenovation,
                       description = result.description,
                       usage  = result.usage
                  };
            }
            
            return itemNew;
        }

        public Item getItem(string id)
        {
            Item itemNew = new Item();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage resp = client.GetAsync("/item/" + id).Result;


            if (resp.IsSuccessStatusCode)
            {
                var result = resp.Content.ReadAsAsync<Item>().Result;
                itemNew =
                  new Item
                  {
                      itemId = result.itemId,
                      title = result.title,
                      categoryId = result.categoryId,
                      state = result.state,
                      price = result.price,
                      typeCurrency = result.typeCurrency,
                      status = result.status,
                      typeItem = result.typeItem,
                      dateRegistration = result.dateRegistration,
                      dateExpired = result.dateExpired,
                      dateActivation = result.dateActivation,
                      dateRenovation = result.dateRenovation,
                      description = result.description,
                      usage = result.usage,
                      pictures = result.pictures
                  };
            }

            return itemNew;
        }


        /*

        public RegistroModel singUp(RegistroModel newPerson)
        {


            RegistroModel registerPerson = new RegistroModel();
            HttpContent content = new ObjectContent<RegistroModel>(newPerson, jsonFormatter);

            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            var resp = client.PostAsync("/user/", content).Result;

            if (resp.IsSuccessStatusCode)
            {
                var users = resp.Content.ReadAsAsync<RegistroModel>().Result;
                registerPerson =
                  new RegistroModel
                  {
                      userID = users.userID,
                      nombre = users.nombre,
                      email = users.email

                  };
            }
            //if(!sendMail(registerPerson.email, registerPerson.nombre, int.Parse(registerPerson.userID.ToString())
            //{
                
            //}

            bool envio = sendMail(registerPerson.email, registerPerson.nombre, (int)(registerPerson.userID));

            return registerPerson;
        }

        public bool activeAccount(string id, string token)
        {
            bool actived = false;

            // proceso que manda llamar la api de activacion con token y todo

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            token = HttpUtility.UrlEncode(token);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage resp = client.GetAsync("/User/?id=usr_" + id + "&token=" + token + "").Result;


            if (resp.IsSuccessStatusCode)
            {
                var users = resp.Content.ReadAsAsync<ResponseMessageAPI>().Result;

                if (users.status == "actived")
                {
                    actived = true;
                }

            }

            return actived;
        }

        // Servicio de envio de mail

        public bool sendMail(string email, string nombre, int userID)
        {
            bool enviado;

            // generamos un objeto Mail

            RegistroMail myMailRegistration = new RegistroMail
            {
                emailDestino = email,
                subject = "Bienvenido a electronia.com.mx",
                body = "datos " + email + " - " + nombre + "-" + userID + "...",
                idService = "eniaRegistration"
            };

            // depndiendo del servicio seleccionamos la plantilla HTML

            Encriptador enc = new Encriptador();
            string data_token = enc.Encriptar(userID.ToString());

            myMailRegistration.body = "	<div style=\"border:1px solid #e1e1e1;  width: 700px; height:auto;\"> " +
         "<div style=\"height: 40px; background-color:#27709b;background-image:-webkit-linear-gradient(top, #27709b, #24688f); " +
         "font-family: Tahoma, Arial; color:#fff; font-size: 30px; font-weight: bold;" +
         "padding-left: 10px; padding-top: 5px;\" >" +
         "		Electronia" +
         "</div>" +
         "<div style=\" font-family: Arial; font-size: 12px; padding: 10px; \">" +
         "	<p style=\"font-family: Arial;font-size: 20px;font-weight: bold;width: auto;color: #0B4D7D;" +
         "	margin-left: 15px;\">" +
         "		Bienvenido " + nombre + "</p>" +
         "	<p>" +
         "		Una vez activada tu cuenta estara todo listo para que puedas comenzar a difrutar de los beneficios " +
         "				que <a   style=\"color:#27709b;\"href=\"http://www.electronia.com.mx\">electronia.com.mx</a> tiene para ti.<br><br><br>" +
         "		Para activar tu cuenta da <a  style=\"color:#27709b; font-size: 13px;\" href=\"http://electronia.azurewebsites.net/club/ActivaCuenta/?id=usr_" + userID + "&token=" + HttpUtility.UrlEncode(data_token) + "\">clik aqui [ACTIVAR CUENTA]</a>  " +
         "	</p>" +
         " </div>" +
         "<div style=\"height:30px; border-top:1px solid #e1e1e1; background: #f1f1f1; text-align: center;" +
         "font-family: Arial; font-size: 10px; color:#555; \">" +
         "	<a style=\"color:#27709b;\" href=\"http://www.electronia.com.mx\">www.electronia.com.mx</a> <br> Todos los derechos reservados" +
         "</div>	" +
     "</div>";

            HttpContent contentMail = new ObjectContent<RegistroMail>(myMailRegistration, jsonFormatter);
            client = new HttpClient();

            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            var resp = client.PostAsync("/Mail/", contentMail).Result;
            if (resp.IsSuccessStatusCode)
            {

                enviado = true;

            }
            else
            {
                enviado = false;
            }

            return enviado;
        }

        // generar el metodo para login
        public AccesToken Authenticate(string email, string password)
        {
            AccesToken response = new AccesToken();


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(urlApiBase);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send an HTTP GET request. Blocking!
            HttpResponseMessage resp = client.GetAsync("/access/?login=" + email + "&password=" + password + "").Result;


            if (resp.IsSuccessStatusCode)
            {
                var users = resp.Content.ReadAsAsync<AccesToken>().Result;

                response = new AccesToken
                {
                    token = users.token,
                    codigo = users.codigo
                };

            }

            return response;
        }

        */
    }
}
